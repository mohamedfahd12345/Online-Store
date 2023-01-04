using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_store.Repositories.UnitOfWork;

namespace online_store.Controllers
{
    //[Authorize(Roles = "vendor")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper mapper;

        public ProductController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _UnitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet , Route("/Products")]
        public  async Task<IActionResult> GetAllProducts()
        {
          
            return Ok(await _UnitOfWork.productRepository.GetAllProducts());
        }

        [HttpGet, Route("/Products/{productsPerPage}/{pageNumber}")]
        public async Task<IActionResult> GetProducts(int productsPerPage, int pageNumber)
        {
               return Ok(await _UnitOfWork.productRepository.GetProducts(productsPerPage, pageNumber));
        }

        [HttpPost , Route("/Products")]
        public async Task<IActionResult> CreateProduct(ProductWriteDto productDto)
        {
            if(productDto == null || !ModelState.IsValid)
            {
                return BadRequest("Invaild Reqest");
            }

             var VerifyOfRequest = new VerifyOfRequest();

             var new_Product = new Product();
             new_Product = mapper.Map<Product>(productDto);

             var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
             int vendorId_Int = Int32.Parse(vendorId);

             new_Product.VendorId = vendorId_Int;

            VerifyOfRequest = await _UnitOfWork.productRepository.AddProduct(new_Product);

            if (VerifyOfRequest.Errorexisting == true)
            {
                return BadRequest(VerifyOfRequest.ErrorDetails);
            }

            return StatusCode(201, "Added Product successfully");
        }


        [HttpGet , Route("/Count-Products")]
        public async Task<IActionResult> GetCountOfProducts()
        {
            return Ok( await _UnitOfWork.productRepository.GetCountOfProducts());
        }


        [HttpGet, Route("/Products/{ProductId}")]
        public async Task<IActionResult> GetProductById(int ProductId)
        {
            var product = new ProductReadDto();
            product = await _UnitOfWork.productRepository.GetProductById(ProductId);
            if(product is null)
            {
                return NotFound("This Product not exist to get it");
            }
            return Ok(product);
        }


        [HttpGet, Route("/Product/{ProductName}")]
        public async Task<IActionResult> SearchByName(string ProductName)
        {
            return Ok(await _UnitOfWork.productRepository.GetProductsByName(ProductName));
        }



        [HttpPut, Route("/Products/{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId , ProductReadDto updatedProduct)
        {
            if(updatedProduct.ProductId != productId)
            {
                return BadRequest("id in object doesn't match with id in Parameters");
            }
            if(await _UnitOfWork.productRepository.IsProductExist(productId) == false)
            {
                return NotFound($"can't found product with id {productId}");
            }

            var VerifyOfRequest = new VerifyOfRequest();

            VerifyOfRequest = await _UnitOfWork.productRepository.UpdateProduct(updatedProduct);

            if(VerifyOfRequest.Errorexisting == true)
            {
                return BadRequest(VerifyOfRequest.ErrorDetails);
            }

            return Ok("Product Updated Successfully");
        }

        [HttpDelete, Route("/Products/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            if (await _UnitOfWork.productRepository.IsProductExist(productId) == false)
            {
                return NotFound($"can't found product with id {productId}");
            }

            var VerifyOfRequest = new VerifyOfRequest();

            VerifyOfRequest = await _UnitOfWork.productRepository.Deleteproduct(productId);

            if(VerifyOfRequest.Errorexisting == true)
            {
                return BadRequest(VerifyOfRequest.ErrorDetails);
            }

            return Ok();


        }


    }
}
