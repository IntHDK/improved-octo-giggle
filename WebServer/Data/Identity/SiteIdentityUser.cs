using Microsoft.AspNetCore.Identity;

namespace WebServer.Data.Identity
{
    public class SiteIdentityUser : IdentityUser<Guid>
    {
        public SiteIdentityUser() : base()
        {
        }
        public string? Name { get; set; }
    }
}
