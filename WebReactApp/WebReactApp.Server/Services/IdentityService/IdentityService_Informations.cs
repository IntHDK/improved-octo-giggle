using WebReactApp.Server.ModelObjects.Identity;

namespace WebReactApp.Server.Services.IdentityService
{
    //TODO: (차후 사용) 계정 정보 반환시 일부 정보만 노출할 때
    public class IdentityManagerAccountInformation
    {
        public Guid AccountID { get; set; }
        public string NickName { get; set; }
        public ICollection<RoleType> RoleTags { get; set; }
    }
    
}
