using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebReactApp.Server.ModelObjects.Identity.LoginMethod;
using WebReactApp.Server.ModelObjects.Identity;
using WebReactApp.Server.ModelObjects;

namespace WebReactApp.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountConfirmTicket> AccountConfirmTickets { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }

        public DbSet<AccountItem> AccountItems { get; set; }
        public DbSet<AccountPost> AccountPosts { get; set; }
        public DbSet<AccountPostEnclosure> AccountPostEnclosures { get; set; }
        public DbSet<AccountPostEnclosureItemParameter> AccountPostEnclosuresItemParameters { get; set; }
        public DbSet<AccountItemParameters> AccountItemsParameters { get; set; }

        public DbSet<UsernamePasswordMethod> UsernamePasswordMethods { get; set; }
    }
}
