using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace online_store.Authentication_Services
{
    public class TokenServices
    {

        private readonly IConfiguration _configuration;
        
        public TokenServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public  string CreateToken(User user)
        {
            string userId = user.UserId.ToString();
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email , user.Email) ,
                new Claim(ClaimTypes.NameIdentifier , userId)

            };
            if(user.Role == "admin")
            {
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
                claims.Add(new Claim(ClaimTypes.Role, "user"));
                claims.Add(new Claim(ClaimTypes.Role, "vendor"));
            }
            else if(user.Role == "vendor")
            {
               
                claims.Add(new Claim(ClaimTypes.Role, "vendor"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "user"));
              
            }

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(100),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
