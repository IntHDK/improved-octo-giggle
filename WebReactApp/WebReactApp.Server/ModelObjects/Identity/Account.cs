using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebReactApp.Server.ModelObjects.Identity.LoginMethod;

namespace WebReactApp.Server.ModelObjects.Identity
{
    [Index(nameof(Email))]
    public class Account
    {
        public Guid ID { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastLoginTime { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public bool IsConfirmed { get; set; }
        public List<AccountRole> Roles { get; set; }

        public UsernamePasswordMethod UsernamePasswordMethod { get; set; }

        public List<AccountItem> AccountItems { get; set; }
        public List<AccountPost> AccountPosts { get; set; }

        public int Currency_Point { get; set; }
        public int Currency_Cash { get; set; }
    }

    public class AccountConfirmTicket
    {
        public Guid ID { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ExpireAt { get; set; }
        public bool IsUsed { get; set; }
        public Guid AccountID { get; set; }
        public Account Account { get; set; }
    }

    public class AccountRole
    {
        public Guid ID { get; set; }
        public DateTime CreatedTime { get; set; }
        public RoleType Role { get; set; }
    }
    public enum RoleType
    {
        None,
        Suspended,
        Admin,
        Master
    }

}
