using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_store.Models;
using online_store.DTOs;

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
            //if (p.VendorId != null)
            //{
            //    return true;
            //}
            //return false;


             return true;
        }
        public async Task<List<ProductReadDto>> GetProductsss(Func<Product, bool> c)
        {
            
           
            List<ProductReadDto> products = _context.Products
                .Include(x => x.Category)
                .Include(x => x.Images)
                .AsSplitQuery()
                .AsNoTracking()
                .Where(c)
                .Select(p => new ProductReadDto
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                    VendorId = p.VendorId,
                    Description = p.Description,
                    ProductName = p.ProductName,
                    MainImageUrl = p.MainImageUrl,
                    Price = p.Price,
                    Category = p.Category.CategoryName,
                    CategoryId = p.CategoryId,
                    imagesUrl = p.Images
                                    .Select(x => new ImageDto
                                    {
                                        ImageUrl = x.ImageUrl,
                                        ProductId = x.ProductId,
                                        ImageId = x.Id
                                    }).ToList()
                })
                .ToList();

            return products;
        }

        [HttpGet("getproduct")]
        public async Task<IActionResult> getElement()
        {
            // var t = _context.Carts.Where(null).to; -----------> this wrong  
            //var temp = _context.Products.Where(t).ToList();
             var res = await GetProductsss(x=>x.ProductId != null);
            return Ok(res);



        }

    }
}
