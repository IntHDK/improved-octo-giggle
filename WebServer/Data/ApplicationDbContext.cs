using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebServer.Data.Identity;

namespace WebServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<SiteIdentityUser, SiteIdentityRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
