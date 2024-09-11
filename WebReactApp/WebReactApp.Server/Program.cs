
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebReactApp.Server.Data;
using WebReactApp.Server.Services.Authentication;
using WebReactApp.Server.Services.IdentityService;
using WebReactApp.Server.Services.MessageChannel;
using WebReactApp.Server.Services.TimerTaskService;

namespace WebReactApp.Server
{
    public class Program
    {
        private class Configuration_Database
        {
            public string ConnectionString { get; set; }
        }
        private class Configuration_InitialMaster
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public static void Main(string[] args)
        {
#if DEBUG
            Console.WriteLine("�ڡڡڡڡڡڡ� I am on DEBUG �ڡڡڡڡڡڡ�");
#else
#endif
            var builder = WebApplication.CreateBuilder(args);
            var dbconf = builder.Configuration.GetSection("Database").Get<Configuration_Database>();
            var initmasterconf = builder.Configuration.GetSection("InitialMaster").Get<Configuration_InitialMaster>();
            if (dbconf == null)
            {
                dbconf = new Configuration_Database()
                {
                    ConnectionString = "Server=localhost;Database='improved_octo_giggle';Uid='octo-giggle';Pwd='octo-giggle';"
                };
            }

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(dbconf.ConnectionString, ServerVersion.AutoDetect(dbconf.ConnectionString));
            });
            builder.Services.AddAuthentication(AppAuthenticationOptions.DefaultScheme).
                AddScheme<AppAuthenticationOptions, AppAuthenticationHandler>(AppAuthenticationOptions.DefaultScheme, options =>
                {
                });
            builder.Services.AddOptions<BearerTokenOptions>(IdentityConstants.BearerScheme).Configure(configure =>
            {
                configure.BearerTokenExpiration = TimeSpan.FromDays(1);
            });
            builder.Services.AddSingleton<IdentityTokenSingleton>();
            builder.Services.AddSingleton<MessageChannelSingleton>();
            builder.Services.AddSingleton<TimerOnOneMinuteTaskSingleton>();
            builder.Services.AddScoped<IdentityService>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<ClaimsPrincipal>(s => s.GetService<IHttpContextAccessor>().HttpContext.User);

            var app = builder.Build();

            //app.UseDefaultFiles();
            //app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromDays(1)
            });
            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            //initialize
            app.Services.GetService<TimerOnOneMinuteTaskSingleton>().Start();
            using (var scope = app.Services.CreateScope())
            {

                var dbcontext = scope.ServiceProvider.GetService<AppDbContext>();
                dbcontext?.Database.Migrate();

                var idservice = scope.ServiceProvider.GetService<IdentityService>();
                if (idservice != null)
                {
                    if (idservice.IsNeedInitialize && initmasterconf != null)
                    {
                        idservice.InitializeMasterAccount(initmasterconf.Username, initmasterconf.Email, initmasterconf.Password);
                    }
                }
            }

            app.Run();
        }
    }
}
