using System.Security.Cryptography;
using System.Text;

namespace online_store.Authentication_Services
{
    public class HashServices
    {
        public   void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                var passwordSaltString = Convert.ToBase64String(passwordSalt);
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password + passwordSaltString));

            }
        }


        public   bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var passwordSaltString = Convert.ToBase64String(passwordSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password + passwordSaltString));
                return computedHash.SequenceEqual(passwordHash);
            }
        }



    }
}
