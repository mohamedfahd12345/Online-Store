using AutoMapper;
using online_store.DTOs;
using Microsoft.EntityFrameworkCore;
namespace online_store.Repositories.PRODUCTS
{
    public class ProductRepository : IProductRepository
    {
        private readonly OnlineStoreContext _context;
        private readonly ICacheService _cacheService;
        private readonly IMapper mapper;
        public ProductRepository(OnlineStoreContext context, ICacheService cacheService , IMapper mapper)
        {
            this._context = context;
            _cacheService = cacheService;
            this.mapper = mapper;
        }

        /*private async Task RemoveProductsCachingAsync()
        {
            await _cacheService.RemoveAsync("Products");
        }*/

        public async Task<List<ProductsReadDto>> GetProductsWithDelegate(Func<Product, bool> condition, string cacheKey)
        {
            var productResponse = await _cacheService
               .GetAsync<List<ProductsReadDto>>(cacheKey);

            if (productResponse is not null)
            {
                return productResponse;
            }


            List<ProductsReadDto> products = _context.Products
                .Include(x => x.Category)
                .AsSplitQuery()
                .AsNoTracking()
                .Where(condition)
                .Select(p => new ProductsReadDto
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                    VendorId = p.VendorId,
                    Description = p.Description,
                    ProductName = p.ProductName,
                    MainImageUrl = p.MainImageUrl,
                    Price = p.Price,
                    Category = p.Category != null ? p.Category.CategoryName : null,
                    CategoryId = p.CategoryId
                    
                })
                .ToList();

            await _cacheService.SetAsync<List<ProductsReadDto>>(cacheKey, products);
           
         
            return products;
        }


        public async Task<VerifyOfRequest> AddProduct(Product product, List<string> imgesUrl)
        {
            var VerifyOfRequest = new VerifyOfRequest();

            var ExistCategory = await _context.Categories.AnyAsync(x => x.CategoryId == product.CategoryId);
            if (ExistCategory == false)
            {
                VerifyOfRequest.ErrorDetails = "This Category Does not exist ";
                VerifyOfRequest.Errorexisting = true;
                return VerifyOfRequest;
            }

            var ExistVendor = await _context.Users.AnyAsync(x => x.UserId == product.VendorId);
            if (ExistVendor == false)
            {
                VerifyOfRequest.ErrorDetails = "This Vendor Does not exist ";
                VerifyOfRequest.Errorexisting = true;
                return VerifyOfRequest;
            }

            try
            {
                
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                var ImagesUrl = new List<Image>();
                foreach (var it in imgesUrl)
                {
                    ImagesUrl.Add(new Image { ImageUrl = it, ProductId = product.ProductId });
                }

                await _context.Images
                    .AddRangeAsync(ImagesUrl);
                await _context.SaveChangesAsync();
               
            }
            catch (Exception ex)
            {
                VerifyOfRequest.ErrorDetails = ex.Message;
                VerifyOfRequest.Errorexisting = true;
                return VerifyOfRequest;
            }

            return VerifyOfRequest;

        }

        public async Task<VerifyOfRequest> Deleteproduct(int productId, int userId)
        {
            var VerifyOfRequest = new VerifyOfRequest();
            try
            {
                var TargetProduct = await _context.Products
                    .Where(x => x.ProductId == productId)
                    .FirstOrDefaultAsync();
                if (TargetProduct.VendorId != userId)
                {
                    VerifyOfRequest.ErrorDetails = "403";
                    VerifyOfRequest.Errorexisting = true;
                    return VerifyOfRequest;
                }
                _context.Products.Remove(TargetProduct);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                VerifyOfRequest.ErrorDetails = ex.Message;
                VerifyOfRequest.Errorexisting = true;
            }
            return VerifyOfRequest;
        }

        public async Task<List<ProductsReadDto>> GetAllProducts()
        {
            return await GetProductsWithDelegate(x => x.ProductId > 0 , "Products");
        }

        public async Task<OneProductReadDto?> GetProductById(int productid)
        {
            var targetProduct = await _context.Products
            .Include(x => x.Category)
            .Include(x => x.Images)
            .AsSplitQuery()
            .AsNoTracking()
            .Where(x => x.ProductId == productid)
            .Select(p => new OneProductReadDto
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity,
                VendorId = p.VendorId,
                Description = p.Description,
                ProductName = p.ProductName,
                MainImageUrl = p.MainImageUrl,
                Price = p.Price,
                Category = p.Category != null ? p.Category.CategoryName : null,
                CategoryId = p.CategoryId,
                imagesUrl = p.Images
                                .Select(x => new ImageDto
                                {
                                    ImageUrl = x.ImageUrl,
                                    ProductId = x.ProductId,
                                    ImageId = x.Id
                                }).ToList()
            })
            .FirstOrDefaultAsync();
            return targetProduct;

        }

        public async Task<List<ProductsReadDto>> GetProducts(int productsPerPage, int pageNumber)
        {
            if (productsPerPage < 1 || pageNumber < 1)
                return null;

            var skip = (pageNumber - 1) * productsPerPage;
            var TotalProducts = await _context.Products.CountAsync();

            if (skip >= TotalProducts)
            {
                return null;
            }


            List<ProductsReadDto> products = await _context.Products
                .Include(x => x.Category)
                .AsSplitQuery()
                .AsNoTracking()
                .Select(p => new ProductsReadDto
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                    VendorId = p.VendorId,
                    Description = p.Description,
                    ProductName = p.ProductName,
                    MainImageUrl = p.MainImageUrl,
                    Price = p.Price,
                    Category = p.Category != null ? p.Category.CategoryName : null,
                    CategoryId = p.CategoryId
                  
                })
                .OrderBy(x => x.ProductId)
                .Skip(skip)
                .Take(productsPerPage)
                .ToListAsync();

            return products;

        }

        public async Task<List<ProductsReadDto>> GetProductsByCategoryID(int categoryId)
        {
            string CategoryID = categoryId.ToString();
            return await GetProductsWithDelegate(x => x.CategoryId == categoryId , $"ProductsWithCategoryId{CategoryID}");
        }

        public async Task<List<ProductsReadDto>> GetProductsByName(string name)
        {

            var productName = name.Trim().ToLower();
            return await GetProductsWithDelegate(p => p.ProductName.ToLower().Contains(productName),$"ProductsWithName{productName}" );

        }

        public async Task<bool> IsProductExist(int productId)
        {
            return await _context.Products
                .AnyAsync(x => x.ProductId == productId);
        }

        public async Task<VerifyOfRequest> UpdateProduct(ProductUpdateDto updatedProduct, int userId)
        {
            var VerifyOfRequest = new VerifyOfRequest();

            var ExistCategory = await _context.Categories
                .AnyAsync(x => x.CategoryId == updatedProduct.CategoryId);

            if (ExistCategory == false)
            {
                VerifyOfRequest.ErrorDetails = "This Category Does not exist ";
                VerifyOfRequest.Errorexisting = true;
                return VerifyOfRequest;
            }

            var CurrentProduct = await _context.Products
                .Where(x => x.ProductId == updatedProduct.ProductId)
                .Include(i=>i.Images)
                .FirstOrDefaultAsync();

            if (CurrentProduct.VendorId != userId)
            {
                VerifyOfRequest.ErrorDetails = "403";
                VerifyOfRequest.Errorexisting = true;
                return VerifyOfRequest;
            }

            try
            {
               
                CurrentProduct.Description = updatedProduct.Description;
                CurrentProduct.CategoryId = updatedProduct.CategoryId;
                CurrentProduct.MainImageUrl = updatedProduct.MainImageUrl;
                CurrentProduct.ProductName = updatedProduct.ProductName;
                CurrentProduct.Quantity = updatedProduct.Quantity;
                CurrentProduct.Price = updatedProduct.Price;

                
                CurrentProduct.Images.Clear();




                foreach (var imageDto in updatedProduct.imagesUrl)
                {
                    var image = new Image
                    {
                        ProductId = CurrentProduct.ProductId,
                        ImageUrl = imageDto
                    };

                    CurrentProduct.Images.Add(image);
                }

                _context.Products.Update(CurrentProduct);
                await _context.SaveChangesAsync(); 


            }
            catch (Exception ex)
            {
                VerifyOfRequest.ErrorDetails = ex.Message;
                VerifyOfRequest.Errorexisting = true;
                return VerifyOfRequest;
            }

            return VerifyOfRequest;
        }

        public async Task<int> GetCountOfProducts()
        {
            return await _context.Products.CountAsync();
        }
    }
}
