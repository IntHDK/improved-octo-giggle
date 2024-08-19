namespace WebReactApp.Server.ModelObjects.Identity.SessionToken
{
    public class SessionToken
    {
        public string Token { get; set; }
        public DateTime ExpireAt { get; set; }
        public Guid AccountID { get; set; }
    }
}
