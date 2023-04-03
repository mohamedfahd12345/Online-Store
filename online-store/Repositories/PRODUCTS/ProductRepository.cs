using AutoMapper;
using online_store.DTOs;
using Microsoft.EntityFrameworkCore;
namespace online_store.Repositories.PRODUCTS
{
    public class ProductRepository : IProductRepository
    {
        private readonly OnlineStoreContext _context;
        private readonly IMapper mapper;
        public ProductRepository(OnlineStoreContext context , IMapper mapper)
        {
            this._context = context;
            this.mapper = mapper;   
        }


        public async Task<VerifyOfRequest> AddProduct(Product product , List<string> imgesUrl)
        {
            var VerifyOfRequest = new VerifyOfRequest();

            var ExistCategory =await  _context.Categories.AnyAsync(x => x.CategoryId == product.CategoryId);
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

        public async Task<VerifyOfRequest> Deleteproduct(int productId)
        {
            var VerifyOfRequest = new VerifyOfRequest();
            try
            {
                var TargetProduct = await _context.Products.Where(x => x.ProductId == productId).FirstOrDefaultAsync();
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
            
            var Products = await _context.Products
                .Include(x => x.Category)
                .Include(x=>x.Images)
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
                    Category = p.Category.CategoryName,
                    CategoryId = p.CategoryId ,
                    imagesUrl = p.Images
                                 .Select(x=>new ImageDto
                                 {
                                     ImageUrl =x.ImageUrl,
                                     ProductId = x.ProductId,
                                     ImageId = x.Id
                                 }).ToList()
                })
                .ToListAsync();

            return Products;
           
        }

        public async Task<ProductReadDto> GetProductById(int productid)
        {
            

             var Products = await _context.Products
                .Where(p=>p.ProductId == productid)
                .Include(x => x.Category)
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
                    CategoryId = p.CategoryId
                })
                .FirstOrDefaultAsync();

            return Products;
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

           
            var Products = await _context.Products
                .Include(x => x.Category)
                .Select(p => new ProductReadDto
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                    VendorId = p.VendorId,
                    Description = p.Description,
                    ProductName = p.ProductName,
                    MainImageUrl = p.MainImageUrl,
                    Price = p.Price,
                    Category = p.Category.CategoryName , 
                    CategoryId = p.CategoryId
                })
                .Skip(skip)
                .Take(productsPerPage)
                .ToListAsync();


            return Products;

        }

        public async Task<List<ProductReadDto>> GetProductsByCategoryID(int categoryId)
        {
            var Products = await _context.Products
                .Where(x => x.CategoryId == categoryId)
                .Include(x => x.Category)
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
                    CategoryId = p.CategoryId
                })
                .ToListAsync();
            return Products;
        }

        public async Task<List<ProductReadDto>> GetProductsByName(string name)
        {
            var productName = name.Trim().ToLower();

            
            var Products = await _context.Products
                .Where(p=>p.ProductName.ToLower().Contains(productName))
                .Include(x => x.Category)
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
                    CategoryId = p.CategoryId
                })
                .ToListAsync();
            return Products;
        }

        public async Task<bool> IsProductExist(int productId)
        {
            return await _context.Products
                .AnyAsync(x=>x.ProductId == productId);
        }

        public async Task<VerifyOfRequest> UpdateProduct(ProductReadDto updatedProduct)
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

            var ExistVendor = await _context.Users
                .AnyAsync(x => x.UserId == updatedProduct.VendorId);

            if (ExistVendor == false)
            {
                VerifyOfRequest.ErrorDetails = "This Vendor Does not exist ";
                VerifyOfRequest.Errorexisting = true;
                return VerifyOfRequest;
            }

            try
            {
                var CurrentProduct = await _context.Products.Where(x => x.ProductId == updatedProduct.ProductId).FirstOrDefaultAsync();
                
                CurrentProduct.Description = updatedProduct.Description;
                CurrentProduct.CategoryId = updatedProduct.CategoryId;
                CurrentProduct.MainImageUrl = updatedProduct.MainImageUrl;
                CurrentProduct.ProductName = updatedProduct.ProductName;
                CurrentProduct.Quantity = updatedProduct.Quantity;
                CurrentProduct.Price = updatedProduct.Price;
               

                _context.Products.Update(CurrentProduct);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
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
