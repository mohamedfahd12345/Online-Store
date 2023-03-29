using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_store.Authentication_Services;
using online_store.Repositories.Auth;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using online_store.Helper;

namespace online_store.Controllers
{
    
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepository;
        private readonly TokenServices TokenServices;
        private readonly HashServices hashServices;
        public AuthController(IAuthRepository authRepository , TokenServices tokenServices 
            , HashServices hashServices)
        {
            this.TokenServices = tokenServices;
            this.authRepository = authRepository;
            this.hashServices = hashServices;
        }

        [HttpPost , Route("/customer/register")]
        public async Task<IActionResult> Register([FromBody]CustomerDTO customerDTO)
        {


            if (!ModelState.IsValid) { return BadRequest(new { error = "Ivaild Input" }); }

            var result =  await authRepository.Register(customerDTO);

            if (result.Errorexisting) { return BadRequest(result.ErrorDetails); }
           
            return Ok();
        }


        [HttpPost , Route("/login")]
        public async Task<IActionResult> Login([FromBody] CustomerLoginDTO customerLoginDTO)
        {
           

            if (!ModelState.IsValid) { return BadRequest(new {error = "Ivaild Input"}); }


            var user = await authRepository.GetUser(customerLoginDTO.Email);

            if (user is null  )
            {
                return BadRequest(new { error = "Invalid Email or Password" });
            }
            if(!hashServices.VerifyPasswordHash(customerLoginDTO.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest(new { error = "Invalid Email or Password" });
            }

           
            var token =await TokenServices.CreateToken(user);
            return Ok(token);
            
        }


        [HttpPost, Route("/admin/register")]
        public async Task<IActionResult> AdminRegister([FromBody] CustomerDTO customerDTO)
        {


            if (!ModelState.IsValid) { return BadRequest(new { error = "Ivaild Input" }); }

            var result = await authRepository.Register(customerDTO,"admin");

            if (result.Errorexisting) { return BadRequest(result.ErrorDetails); }

            return Ok();
        }

        [HttpPost, Route("/vendor/register")]
        public async Task<IActionResult> VendorRegister([FromBody] CustomerDTO customerDTO)
        {


            if (!ModelState.IsValid) { return BadRequest(new { error = "Ivaild Input" }); }

            var result = await authRepository.Register(customerDTO , "vendor");

            if (result.Errorexisting) { return BadRequest(result.ErrorDetails); }

            return Ok();
        }

        [HttpPost("/refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RequestToken requestToken)
        {
            if (!ModelState.IsValid) { return BadRequest(new { error = "Ivaild Input" }); }
            var token = await TokenServices.RefreshToken(requestToken);
            if(token is null)
            {
                return BadRequest(new { error = "Invaild Token" });
            }
            return Ok(token);
        }

        [HttpPost("/revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeToken model)
        {
            if (!ModelState.IsValid) { return BadRequest(new { error = "Ivaild Input" }); }

            var result = await TokenServices.RevokeToken(model.Token);

            if(result == false)
            {
                return BadRequest(new { error = "Token is invalid!" });
            }

            return Ok();
        } 
    }
}
