using Microsoft.AspNetCore.Identity;

namespace WebServer.Data.Identity
{
    public class SiteIdentityRole : IdentityRole<Guid>
    {
        public SiteIdentityRole():base(){
        }
    }
}
