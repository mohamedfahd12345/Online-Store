using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_store.Repositories.PRODUCTS;
namespace online_store.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        [HttpGet , Route("Products")]
        public  async Task<IActionResult> GetAllProducts()
        {
            return Ok(await productRepository.GetAllProducts());
        }

        
        [HttpGet, Route("Products/{pageNumber:int}/{pagesize:int}")]
        public async Task<IActionResult> GetProducts([FromRoute] int pageNumber, [FromRoute] int pagesize)
        {
               return Ok(await productRepository.GetProducts(pagesize, pageNumber));
        }


        [Authorize(Roles = "vendor")]
        [HttpPost , Route("Products")]
        public async Task<IActionResult> CreateProduct([FromBody]ProductWriteDto productDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Invaild Input");
            }

             var VerifyOfRequest = new VerifyOfRequest();

             
             var new_Product = mapper.Map<Product>(productDto);
            

             var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
             int vendorId_Int = Int32.Parse(vendorId);

             new_Product.VendorId = vendorId_Int;

            VerifyOfRequest = await productRepository.AddProduct(new_Product , productDto.imagesUrl);

            if (VerifyOfRequest.Errorexisting == true)
            {
                return BadRequest(VerifyOfRequest.ErrorDetails);
            }

            return StatusCode(201, "Added Product successfully");
        }


        [HttpGet , Route("Count-Products")]
        public async Task<IActionResult> GetCountOfProducts()
        {
            return Ok( await productRepository.GetCountOfProducts());
        }


        [HttpGet, Route("Products/{ProductId:int}")]
        public async Task<IActionResult> GetProductById([FromRoute]int ProductId)
        {
            
            var product = await productRepository.GetProductById(ProductId);
            if(product is null)
            {
                return NotFound("This Product not exist to get it");
            }
            return Ok(product);
        }


        [HttpGet, Route("Product/{ProductName:alpha}")]
        public async Task<IActionResult> SearchByName([FromRoute]string ProductName)
        {
            return Ok(await productRepository.GetProductsByName(ProductName));
        }


        [Authorize(Roles = "vendor")]
        [HttpPut, Route("Products/{productId:int}")]
        public async Task<IActionResult> UpdateProduct([FromRoute]int productId ,[FromBody] ProductUpdateDto updatedProduct)
        {
            if(updatedProduct.ProductId != productId)
            {
                return BadRequest(new {error =  "id in object doesn't match with id in Parameters" });
            }
            if(await  productRepository.IsProductExist(productId) == false)
            {
                return NotFound($"can't found product with id {productId}");
            }

            int  userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var VerifyOfRequest = await  productRepository.UpdateProduct(updatedProduct , userId);

            if(VerifyOfRequest.Errorexisting == true)
            {
                if(VerifyOfRequest.ErrorDetails == "403")
                {
                    return Forbid();
                }
                return BadRequest(VerifyOfRequest.ErrorDetails);
            }

            return Ok("Product Updated Successfully");
        }


        [Authorize(Roles = "vendor")]
        [HttpDelete, Route("Products/{productId:int}")]
        public async Task<IActionResult> DeleteProduct([FromRoute]int productId)
        {
            if (await  productRepository.IsProductExist(productId) == false)
            {
                return NotFound($"can't found product with id {productId}");
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var VerifyOfRequest = await productRepository.Deleteproduct(productId , userId);

            if(VerifyOfRequest.Errorexisting == true)
            {
                if (VerifyOfRequest.ErrorDetails == "403")
                {
                    return Forbid();
                }
                return BadRequest(VerifyOfRequest.ErrorDetails);
            }

            return Ok();


        }

        [HttpGet("products-category /{categoryId:int}")]
        public async Task<IActionResult> GetProductsByCategoryID([FromRoute] int categoryId)
        {
            return Ok(await productRepository.GetProductsByCategoryID(categoryId));
        }
    }
}
