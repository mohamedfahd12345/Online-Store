using System.Security.Cryptography;

namespace online_store.Authentication_Services
{
    public class HashServices
    {
        public   void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = System.Text.Encoding.UTF8.GetBytes(password);
                passwordHash = passwordSalt.Concat(passwordHash).ToArray();
                passwordHash = hmac.ComputeHash(passwordHash);

            }
        }


        public   bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                byte[] computedHash;
                computedHash = System.Text.Encoding.UTF8.GetBytes(password);
                computedHash = passwordSalt.Concat(computedHash).ToArray();

                computedHash = hmac.ComputeHash(computedHash);
                return computedHash.SequenceEqual(passwordHash);
            }
        }



    }
}
