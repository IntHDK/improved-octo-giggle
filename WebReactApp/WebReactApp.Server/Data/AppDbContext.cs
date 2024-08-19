using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebReactApp.Server.ModelObjects.Identity.LoginMethod;
using WebReactApp.Server.ModelObjects.Identity;

namespace WebReactApp.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountConfirmTicket> AccountConfirmTickets { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<UsernamePasswordMethod> UsernamePasswordMethods { get; set; }
    }
}
