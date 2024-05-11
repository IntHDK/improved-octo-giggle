namespace improved_octo_giggle_server.Data.Entity
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
        public Account Owner { get; set; }
    }

    public class AccountLoginMethod_Google
    {
        public string ID { get; set; }
        public string Email { get; set; }
        public DateTime CreateTime { get; set; }
        public Account Owner { get; set; }
    }
    public class AccountLoginMethod_Apple
    {
        public string ID;
        public string Email;
        public DateTime CreateTime;
        public Account Owner { get; set; }
    }
}
