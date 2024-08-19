using WebReactApp.Server.ModelObjects.Identity;

namespace WebReactApp.Server.Services.IdentityService
{
    public class IdentityManagerAccountInformation
    {
        public Guid AccountID { get; set; }
        public string UserName { get; set; }
        public ICollection<RoleType> RoleTags { get; set; }
    }
    
}
