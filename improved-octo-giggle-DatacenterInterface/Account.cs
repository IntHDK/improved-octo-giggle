namespace improved_octo_giggle_DatacenterInterface
{
    public partial interface DataCenter
    {
        public string HashPassword(string password);
        
        //Base Account CRUD
        public class GetAccount_Result : DataActionResultBase
        {
            public Account? Account { get; set; }
        }
        public class AddAccount_Result : DataActionResultBase { }
        public class UpdateAccount_Result : DataActionResultBase { }
        public class DeleteAccount_Result : DataActionResultBase { }
        public GetAccount_Result GetAccountByID(string ID);
        public AddAccount_Result AddAccount(Account account);
        public UpdateAccount_Result UpdateAccount(Account account);
        public DeleteAccount_Result DeleteAccount(string ID);

        //Base AccountLoginMethod_IDPW CRUD
        public class GetAccountLoginMethod_IDPW_Result : DataActionResultBase
        {
            public AccountLoginMethod_IDPW AccountLoginMethod_IDPW { get; set; }
        }
        public class AddAccountLoginMethod_IDPW_Result : DataActionResultBase { }
        public class UpdateAccountLoginMethod_IDPW_Result : DataActionResultBase { }
        public class DeleteAccountLoginMethod_IDPW_Result : DataActionResultBase { }
        public GetAccountLoginMethod_IDPW_Result GetAccountLoginMethod_IDPWByUserName(string userName);
        public GetAccountLoginMethod_IDPW_Result GetAccountLoginMethod_IDPWByID(string ID);
        public AddAccountLoginMethod_IDPW_Result AddAccountLoginMethod_IDPW(AccountLoginMethod_IDPW accountLoginMethod_IDPW);
        public UpdateAccountLoginMethod_IDPW_Result UpdateAccountLoginMethod_IDPW(AccountLoginMethod_IDPW accountLoginMethod_IDPW);
        public DeleteAccountLoginMethod_IDPW_Result DeleteAccountLoginMethod_IDPW(string ID);

        //Base AccountLoginMethod_Google CRUD
        public class GetAccountLoginMethod_Google_Result : DataActionResultBase
        {
            public AccountLoginMethod_Google AccountLoginMethod_Google { get; set; }
        }
        public class AddAccountLoginMethod_Google_Result : DataActionResultBase { }
        public class UpdateAccountLoginMethod_Google_Result : DataActionResultBase { }
        public class DeleteAccountLoginMethod_Google_Result : DataActionResultBase { }
        public GetAccountLoginMethod_Google_Result GetAccountLoginMethod_GoogleByEmail(string email);
        public GetAccountLoginMethod_Google_Result GetAccountLoginMethod_GoogleByID(string ID);
        public AddAccountLoginMethod_Google_Result AddAccountLoginMethod_Google(AccountLoginMethod_Google accountLoginMethod_Google);
        public UpdateAccountLoginMethod_Google_Result UpdateAccountLoginMethod_Google(AccountLoginMethod_Google accountLoginMethod_Google);
        public DeleteAccountLoginMethod_Google_Result DeleteAccountLoginMethod_Google(string ID);

        //Base AccountLoginMethod_Apple CRUD
        public class GetAccountLoginMethod_Apple_Result : DataActionResultBase
        {
            public AccountLoginMethod_Apple AccountLoginMethod_Apple { get; set; }
        }
        public class AddAccountLoginMethod_Apple_Result : DataActionResultBase { }
        public class UpdateAccountLoginMethod_Apple_Result : DataActionResultBase { }
        public class DeleteAccountLoginMethod_Apple_Result : DataActionResultBase { }
        public GetAccountLoginMethod_Apple_Result GetAccountLoginMethod_AppleByEmail(string email);
        public GetAccountLoginMethod_Apple_Result GetAccountLoginMethod_AppleByID(string ID);
        public AddAccountLoginMethod_Apple_Result AddAccountLoginMethod_Apple(AccountLoginMethod_Apple accountLoginMethod_Apple);
        public UpdateAccountLoginMethod_Apple_Result UpdateAccountLoginMethod_Apple(AccountLoginMethod_Apple accountLoginMethod_Apple);
        public DeleteAccountLoginMethod_Apple_Result DeleteAccountLoginMethod_Apple(string ID);
    }
}
