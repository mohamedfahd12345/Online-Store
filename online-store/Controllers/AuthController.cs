using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_store.Authentication_Service;
using online_store.Authentication_Services;
using online_store.Repositories.Auth;
using online_store.Repositories.UnitOfWork;

using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
namespace online_store.Controllers
{
    
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork  _UnitOfWork;
        private readonly IConfiguration _configuration;
        public AuthController(IUnitOfWork  UnitOfWork , IConfiguration configuration )
        {
            _UnitOfWork =  UnitOfWork;
            _configuration = configuration;
        }

        [HttpPost , Route("/customer/register")]
        public async Task<IActionResult> Register([FromBody]CustomerDTO customerDTO)
        {
            if (customerDTO == null) { return BadRequest(); }

            if (!ModelState.IsValid) { return BadRequest(); }

            var result =  await _UnitOfWork.authRepository.Register(customerDTO);

            if (result.Errorexisting) { return BadRequest(result.ErrorDetails); }
           
            return Ok();
        }


        [HttpPost , Route("/login")]
        public async Task<IActionResult> Login([FromBody] CustomerLoginDTO customerLoginDTO)
        {
            if (customerLoginDTO == null) { return BadRequest(); }

            if (!ModelState.IsValid) { return BadRequest(); }


            var user = new User();
            user = await _UnitOfWork.authRepository.GetUser(customerLoginDTO.Email);

            if (user is null || !HashServices.VerifyPasswordHash(customerLoginDTO.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Invalid Email or Password ");
            }

         
            try
            {
                var TokenServices = new TokenServices(_configuration);

                string token = TokenServices.CreateToken(user);
                return Ok(token);
            }
            catch(Exception ex)
            { return BadRequest(ex.Message); }
            
        }


        [HttpPost, Route("/admin/register")]
        public async Task<IActionResult> AdminRegister([FromBody] CustomerDTO customerDTO)
        {
            if (customerDTO == null) { return BadRequest(); }

            if (!ModelState.IsValid) { return BadRequest(); }

            var result = await _UnitOfWork.authRepository.Register(customerDTO,"admin");

            if (result.Errorexisting) { return BadRequest(result.ErrorDetails); }

            return Ok();
        }

        [HttpPost, Route("/vendor/register")]
        public async Task<IActionResult> VendorRegister([FromBody] CustomerDTO customerDTO)
        {
            if (customerDTO == null) { return BadRequest(); }

            if (!ModelState.IsValid) { return BadRequest(); }

            var result = await _UnitOfWork.authRepository.Register(customerDTO , "vendor");

            if (result.Errorexisting) { return BadRequest(result.ErrorDetails); }

            return Ok();
        }

    }
}
