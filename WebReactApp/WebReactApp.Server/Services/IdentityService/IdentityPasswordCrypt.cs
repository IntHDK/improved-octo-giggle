using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;
using WebReactApp.Server.ModelObjects.Identity.LoginMethod;

namespace WebReactApp.Server.Services.IdentityService
{
    public class IdentityPasswordCrypt
    {
        /*
            * PBKDF2 with HMAC-SHA512, 128-bit salt, 256-bit subkey, 100000 iterations.
        */
        private const int _saltsize = 128 / 8;
        private const int _itercount = 100000;
        private const int _numbytesrequested = 256 / 8;
        private static readonly KeyDerivationPrf _prf = KeyDerivationPrf.HMACSHA512;
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        public static bool HashPassword(string password,
            out PasswordHashMethod passwordHashMethod,
            out byte[] result,
            out int itercount,
            out byte[] salt,
            out int saltsize,
            out KeyDerivationPrf prf
            )
        {
#if DEBUG
            //passwordHashMethod = PasswordHashMethod.None;
            passwordHashMethod = PasswordHashMethod.PBKDF2;
#else
            passwordHashMethod = PasswordHashMethod.PBKDF2;
#endif

            switch (passwordHashMethod)
            {
                case PasswordHashMethod.PBKDF2:
                    salt = new byte[_saltsize];
                    itercount = _itercount;
                    saltsize = _saltsize;
                    prf = _prf;

                    // Produce a version 3 (see comment above) text hash.
                    _rng.GetBytes(salt);
                    result = KeyDerivation.Pbkdf2(password, salt, _prf, _itercount, _numbytesrequested);
                    saltsize = _saltsize;
                    break;

                case PasswordHashMethod.None:
                default:
                    salt = new byte[0];
                    itercount = 0;
                    saltsize = 0;
                    result = Encoding.UTF8.GetBytes(password);
                    prf = _prf;
                    break;
            }

            return true;
        }
        public static bool VerifyPassword(string password,
            byte[] hashorigin,
            PasswordHashMethod passwordHashMethod,
            int itercount,
            byte[] salt,
            int saltsize,
            KeyDerivationPrf prf
            )
        {
            switch (passwordHashMethod)
            {
                case PasswordHashMethod.PBKDF2:
                    try
                    {
                        if (saltsize < 128 / 8)
                        {
                            return false;
                        }
                        if (hashorigin.Length < 128 / 8)
                        {
                            return false;
                        }
                        byte[] actualHash = KeyDerivation.Pbkdf2(password, salt, prf, itercount, hashorigin.Length);
                        return CryptographicOperations.FixedTimeEquals(actualHash, hashorigin);
                    }
                    catch
                    {
                        return false;
                    }

                case PasswordHashMethod.None:
                default:
                    var inputbytes = Encoding.UTF8.GetBytes(password);
                    return CryptographicOperations.FixedTimeEquals(inputbytes, hashorigin);
            }
        }
    }
}
