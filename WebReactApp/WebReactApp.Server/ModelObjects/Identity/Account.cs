using Microsoft.EntityFrameworkCore;
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

    public class AccountPost
    {
        public Guid ID { get; set; }
        public DateTime CreatedTime { get; set; }
        public Guid AccountID { get; set; }
        public Account Account { get; set; }
        public string Context { get; set; }
        public DateTime ExpireAt { get; set; }
        public List<AccountPostEnclosure> AccountPostenclosure { get; set; }
        public bool IsRead { get; set; }
        public bool IsEnclosureTaken { get; set; }
    }
    public class AccountPostEnclosure
    {
        public Guid ID { get; set; }
        public DateTime CreatedTime { get; set; }
        public AccountItemType Type { get; set; }
        public string ItemMetaName { get; set; }
        public DateTime ExpireAt { get; set; }
        public int Quantity { get; set; }
        public List<AccountPost> AccountPost { get; set; }
        public List<AccountPostEnclosureItemParameter> Parameters { get; set; }
    }
    public class AccountPostEnclosureItemParameter
    {
        public Guid ID { get; set; }
        public Guid AccountPostEnclosureID { get; set; }
        public AccountPostEnclosure AccountPostEnclosure { get; set; }
        public string ParamName { get; set; }
        public int Index { get; set; }
        public int NumberValue { get; set; }
        public string StringValue { get; set; }
    }

    public enum AccountItemType
    {
        Currency_Point,
        Currency_Cash,
        Item,
        Item_Package,
    }
    public class AccountItem
    {
        public Guid ID { get; set; }
        public DateTime CreatedTime { get; set; }
        public Guid? AccountID { get; set; }
        public Account? Account { get; set; }
        public AccountItemType Type { get; set; }
        public string ItemMetaName { get; set; }
        public DateTime ExpireAt { get; set; }
        public int Quantity { get; set; }
        public List<AccountItemParameters> Parameters { get; set; }
    }
    public class AccountItemParameters
    {
        public Guid ID { get; set; }
        public Guid AccountItemID { get; set; }
        public AccountItem AccountItem { get; set; }
        public string ParamName { get; set; }
        public int Index { get; set; }
        public int NumberValue { get; set; }
        public string StringValue { get; set; }
    }
}
