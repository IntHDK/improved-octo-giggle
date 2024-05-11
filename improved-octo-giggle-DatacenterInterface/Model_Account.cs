using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace improved_octo_giggle_DatacenterInterface
{
    public class Account
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastConnectTime { get; set; }
        public string ThumbnailURL { get; set; }
        public ICollection<AccountLoginMethod_IDPW> AccountLoginMethod_IDPWs { get; set; }
        public ICollection<AccountLoginMethod_Google> AccountLoginMethod_Googles { get; set; }
        public ICollection<AccountLoginMethod_Apple> AccountLoginMethod_Apples { get; set; }
    }

    public class AccountLoginMethod_IDPW
    {
        public string ID { get; set; }
        public DateTime CreateTime { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public bool IsUse2FA { get; set; }
        public string Information2FA { get; set; }
        public string Connected_Account_ID { get; set; }
    }

    public class AccountLoginMethod_Google
    {
        public string ID { get; set; }
        public string Email { get; set; }
        public DateTime CreateTime { get; set; }
        public string Connected_Account_ID { get; set; }
    }
    public class AccountLoginMethod_Apple
    {
        public string ID { get; set; }
        public string Email { get; set; }
        public DateTime CreateTime { get; set; }
        public string Connected_Account_ID { get; set; }
    }

    public class SessionInfo
    {
        public string Token { get; set; }
        public string Account_ID { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
