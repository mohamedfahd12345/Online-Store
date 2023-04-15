using AutoMapper;
using online_store.DTOs;
using Microsoft.EntityFrameworkCore;
namespace online_store.Repositories.PRODUCTS
{
    public class ProductRepository : IProductRepository
    {
        private readonly OnlineStoreContext _context;
        private readonly IMapper mapper;
        public ProductRepository(OnlineStoreContext context, IMapper mapper)
        {
            this._context = context;
            this.mapper = mapper;
        }

        public async Task<List<ProductReadDto>> GetProductsWithDelegate(Func<Product, bool> condition)
        {

            List<ProductReadDto> products = _context.Products
                .Include(x => x.Category)
                .Include(x => x.Images)
                .AsSplitQuery()
                .AsNoTracking()
                .Where(condition)
                .Select(p => new ProductReadDto
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
                .ToList();

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

        public async Task<List<ProductReadDto>> GetAllProducts()
        {
            return await GetProductsWithDelegate(x => x.ProductId > 0);
        }

        public async Task<ProductReadDto?> GetProductById(int productid)
        {
            var targetProduct = await _context.Products
            .Include(x => x.Category)
            .Include(x => x.Images)
            .AsSplitQuery()
            .AsNoTracking()
            .Where(x => x.ProductId == productid)
            .Select(p => new ProductReadDto
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

        public async Task<List<ProductReadDto>> GetProducts(int productsPerPage, int pageNumber)
        {
            if (productsPerPage < 1 || pageNumber < 1)
                return null;

            var skip = (pageNumber - 1) * productsPerPage;
            var TotalProducts = await _context.Products.CountAsync();

            if (skip >= TotalProducts)
            {
                return null;
            }


            List<ProductReadDto> products = await _context.Products
                .Include(x => x.Category)
                .Include(x => x.Images)
                .AsSplitQuery()
                .AsNoTracking()
                .Select(p => new ProductReadDto
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
                .OrderBy(x => x.ProductId)
                .Skip(skip)
                .Take(productsPerPage)
                .ToListAsync();

            return products;

        }

        public async Task<List<ProductReadDto>> GetProductsByCategoryID(int categoryId)
        {
            return await GetProductsWithDelegate(x => x.CategoryId == categoryId);
        }

        public async Task<List<ProductReadDto>> GetProductsByName(string name)
        {

            var productName = name.Trim().ToLower();
            return await GetProductsWithDelegate(p => p.ProductName.ToLower().Contains(productName));

        }

        public async Task<bool> IsProductExist(int productId)
        {
            return await _context.Products
                .AnyAsync(x => x.ProductId == productId);
        }

        public async Task<VerifyOfRequest> UpdateProduct(ProductReadDto updatedProduct, int userId)
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



            try
            {
                var CurrentProduct = await _context.Products.Where(x => x.ProductId == updatedProduct.ProductId).FirstOrDefaultAsync();
                if (CurrentProduct.VendorId != userId)
                {
                    VerifyOfRequest.ErrorDetails = "403";
                    VerifyOfRequest.Errorexisting = true;
                    return VerifyOfRequest;
                }
                CurrentProduct.Description = updatedProduct.Description;
                CurrentProduct.CategoryId = updatedProduct.CategoryId;
                CurrentProduct.MainImageUrl = updatedProduct.MainImageUrl;
                CurrentProduct.ProductName = updatedProduct.ProductName;
                CurrentProduct.Quantity = updatedProduct.Quantity;
                CurrentProduct.Price = updatedProduct.Price;




                var images = updatedProduct.imagesUrl
                    .Select(x => new Image
                    {
                        Id = x.ImageId,
                        ImageUrl = x.ImageUrl,
                        ProductId = x.ProductId
                    }).ToList();

                _context.Products.Update(CurrentProduct);
                _context.Images.UpdateRange(images);
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
