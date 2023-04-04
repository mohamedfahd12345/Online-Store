using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using online_store.Helper;
using online_store.Models;

namespace online_store.Authentication_Services
{
    public class TokenServices
    {
        private readonly OnlineStoreContext _context;
        private readonly IConfiguration _configuration;
        
        public TokenServices(OnlineStoreContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
        public async Task<RequestToken> CreateToken(User user)
        {
            
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email , user.Email) ,
                new Claim(ClaimTypes.NameIdentifier , user.UserId.ToString())

            };
            if(user.Role == "admin")
            {
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
                
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
                expires: DateTime.UtcNow.AddMinutes(3),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            var RefreshToken = new RefreshToken()
            {
                IsExpired = false,
                IsRevoked = false,
                IsUsed = false,
                UserId = user.UserId,
                AccessToken = jwt,
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddDays(1),
                RefreshToken1 = CreateRefreshToken()
            };

            await _context.RefreshTokens.AddAsync(RefreshToken);
            await _context.SaveChangesAsync();

            return new RequestToken()
            {
                jwtToken = jwt ,
                refreshToken = RefreshToken.RefreshToken1
            };
        }
        private string CreateRefreshToken()
        {
            string resultRefreshToken = "";
            for(int i = 0; i < 5; i++)
            {
                resultRefreshToken += Guid.NewGuid().ToString();
            }
            return resultRefreshToken;
        }

        public async Task<RequestToken?> RefreshToken(RequestToken requestToken) 
        {
            var targetRefreshToken =await _context.RefreshTokens
                .Where(t => t.RefreshToken1 == requestToken.refreshToken)
                .FirstOrDefaultAsync();

            if(targetRefreshToken is null)
            {
                return null;
            }

            if(targetRefreshToken.AccessToken != requestToken.jwtToken)
            {
                return null;
            }

            if (targetRefreshToken.IsUsed == true || targetRefreshToken.IsRevoked == true)
            {
                return null;
            }

            if (targetRefreshToken.IsExpired == true)
            {
                return null;
            }

            if(targetRefreshToken.ExpirationDate < DateTime.UtcNow)
            {
                targetRefreshToken.IsExpired = true;
                targetRefreshToken.IsRevoked = true;
                targetRefreshToken.IsUsed = true;

                _context.RefreshTokens
                    .Update(targetRefreshToken);

                await _context.SaveChangesAsync();

                return null;
            }


            targetRefreshToken.IsExpired = true;
            targetRefreshToken.IsRevoked = true;
            targetRefreshToken.IsUsed = true;

            _context.RefreshTokens
                .Update(targetRefreshToken);

            await _context.SaveChangesAsync();

            var tokenOwner =await _context.Users
                .Where(x => x.UserId == targetRefreshToken.UserId)
                .FirstOrDefaultAsync();

            var Token = await CreateToken(tokenOwner);

            return Token;
        }


        public async Task<bool> RevokeToken(string token)
        {
            var targetRefreshToken = await _context.RefreshTokens
                .Where(t => t.RefreshToken1 == token)
                .FirstOrDefaultAsync();

            if (targetRefreshToken is null)
            {
                return false;
            }

           

            if (targetRefreshToken.IsUsed == true || targetRefreshToken.IsRevoked == true)
            {
                return false;
            }


            if (targetRefreshToken.IsExpired == true)
            {
                return false;
            }

            if (targetRefreshToken.ExpirationDate < DateTime.UtcNow)
            {
                targetRefreshToken.IsExpired = true;
                targetRefreshToken.IsRevoked = true;
                targetRefreshToken.IsUsed = true;

                _context.RefreshTokens
                    .Update(targetRefreshToken);

                await _context.SaveChangesAsync();

                return false;
            }



            targetRefreshToken.IsExpired = true;
            targetRefreshToken.IsRevoked = true;
            targetRefreshToken.IsUsed = true;

            _context.RefreshTokens
                .Update(targetRefreshToken);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
