using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace WebReactApp.Server.ModelObjects.Identity.LoginMethod
{
    public enum PasswordHashMethod
    {
        None, //TODO: 개발용으로만 (해시 안함)
        PBKDF2
    }
    [Index(nameof(UserName), IsUnique = true)]
    public class UsernamePasswordMethod
    {
        public Guid ID { get; set; }
        public DateTime CreatedTime { get; set; }
        public string UserName { get; set; }
        public PasswordHashMethod PasswordMethod { get; set; }
        public KeyDerivationPrf PasswordPrf { get; set; }
        public int PasswordSaltLength { get; set; }
        public int PasswordItr { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public Guid AccountID { get; set; }
        public Account Account { get; set; }
    }
}
