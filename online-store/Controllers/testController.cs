using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_store.Models;
using online_store.DTOs;

using online_store.test;
namespace online_store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class testController : ControllerBase
    {
        private readonly OnlineStoreContext _context;
        private readonly tempclass _tempclass;
        public testController(OnlineStoreContext context , tempclass tempclass)
        {
            _context = context;
            _tempclass = tempclass;
        }

        public static bool t(Product p)
        {
            return true;
        }

        [HttpGet("getproduct")]
        public async Task<IActionResult> getElement(string productName)
        {


            var res = await _tempclass.GetProducts(t);
            return Ok(res);



        }

    }
}
