using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_store.Models;
namespace online_store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class testController : ControllerBase
    {
        private readonly OnlineStoreContext _context;
        public testController(OnlineStoreContext context)
        {
            _context = context;
        }
        public static bool t(Product p) {
            if (p.VendorId != null)
            {
                return true;
            }
            return false;


            // return true;
        }

        [HttpGet("getproduct")]
        public async Task<IActionResult> getElement()
        {
           
            var tt = _context.Products.Where(t).ToList();
            return Ok(tt);

            

        }

    }
}
