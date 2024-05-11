using improved_octo_giggle_server.Data.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace improved_octo_giggle_server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Account> accounts { get; set; }
        public DbSet<Inventory> inventory { get; set; }
        public DbSet<Item> items { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
