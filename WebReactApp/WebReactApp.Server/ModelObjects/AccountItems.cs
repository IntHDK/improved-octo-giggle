using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebReactApp.Server.ModelObjects.Identity;

namespace WebReactApp.Server.ModelObjects
{
    public class AccountPost
    {
        public Guid ID { get; set; }
        public DateTime CreatedTime { get; set; }
        public Guid AccountID { get; set; }
        [JsonIgnore] public Account Account { get; set; }
        public string Context { get; set; }
        public DateTime ExpireAt { get; set; }
        public List<AccountPostEnclosure> AccountPostenclosure { get; set; }
        public bool IsRead { get; set; }
        public bool IsEnclosureTaken { get; set; }
    }
    [Index(nameof(ItemMetaName))]
    public class AccountPostEnclosure
    {
        public Guid ID { get; set; }
        public DateTime CreatedTime { get; set; }
        public AccountItemType Type { get; set; }
        public string ItemMetaName { get; set; }
        public DateTime ExpireAt { get; set; }
        public int Quantity { get; set; }
        [JsonIgnore] public List<AccountPost> AccountPost { get; set; }
        public List<AccountPostEnclosureItemParameter> Parameters { get; set; }
    }
    [Index(nameof(ParamName))]
    public class AccountPostEnclosureItemParameter
    {
        public Guid ID { get; set; }
        public Guid AccountPostEnclosureID { get; set; }
        [JsonIgnore] public AccountPostEnclosure AccountPostEnclosure { get; set; }
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
    [Index(nameof(ItemMetaName))]
    public class AccountItem
    {
        public Guid ID { get; set; }
        public DateTime CreatedTime { get; set; }
        public Guid? AccountID { get; set; }
        [JsonIgnore] public Account? Account { get; set; }
        public AccountItemType Type { get; set; }
        public string ItemMetaName { get; set; }
        public DateTime ExpireAt { get; set; }
        public int Quantity { get; set; }
        public List<AccountItemParameters> Parameters { get; set; }
    }
    [Index(nameof(ParamName))]
    public class AccountItemParameters
    {
        public Guid ID { get; set; }
        public Guid AccountItemID { get; set; }
        [JsonIgnore] public AccountItem AccountItem { get; set; }
        public string ParamName { get; set; }
        public int Index { get; set; }
        public int NumberValue { get; set; }
        public string StringValue { get; set; }
    }
}
