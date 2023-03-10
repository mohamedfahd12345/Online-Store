using AutoMapper;
using online_store.DTOs;
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


        public async Task<VerifyOfRequest> AddProduct(Product product)
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
            }
            catch(Exception ex)
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
            var Product = new List<ProductReadDto>();
            Product = await (from p in _context.Products
                             join c in _context.Categories
                             on p.CategoryId equals c.CategoryId

                             select new ProductReadDto
                             {
                                 ProductId = p.ProductId,
                                 CategoryId = c.CategoryId,
                                 Category = c.CategoryName,
                                 Price = p.Price,
                                 ImageUrl = p.ImageUrl,
                                 ProductName = p.ProductName,
                                 Quantity = p.Quantity,
                                 VendorId = p.VendorId,
                                 Description = p.Description

                             }).ToListAsync();

            return Product;
           
        }

        public async Task<ProductReadDto> GetProductById(int productid)
        {
            var Product = new ProductReadDto();
            Product = await (from p in _context.Products
                             join c in _context.Categories
                             on p.CategoryId equals c.CategoryId

                             where p.ProductId == productid

                             select new ProductReadDto
                             {
                                 ProductId = p.ProductId,
                                 CategoryId = c.CategoryId,
                                 Category = c.CategoryName,
                                 Price = p.Price,
                                 ImageUrl = p.ImageUrl,
                                 ProductName = p.ProductName,
                                 Quantity = p.Quantity,
                                 VendorId = p.VendorId,
                                 Description = p.Description

                             }).FirstOrDefaultAsync();

            return Product;
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

            var Product = new List<ProductReadDto>();
            Product = await (from p in _context.Products
                             join c in _context.Categories
                             on p.CategoryId equals c.CategoryId

                             select new ProductReadDto
                             {
                                 ProductId = p.ProductId,
                                 CategoryId = c.CategoryId,
                                 Category = c.CategoryName,
                                 Price = p.Price,
                                 ImageUrl = p.ImageUrl,
                                 ProductName = p.ProductName,
                                 Quantity = p.Quantity,
                                 VendorId = p.VendorId,
                                 Description = p.Description

                             }).Skip(skip).Take(productsPerPage).ToListAsync();

            return Product;

        }

        public async Task<List<Product>> GetProductsByCategoryID(int categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductReadDto>> GetProductsByName(string name)
        {
            var productName = name.Trim().ToLower();

            var Product = new List<ProductReadDto>();

            Product = await (from p in _context.Products
                            join c in _context.Categories
                            on p.CategoryId equals c.CategoryId

                            where  p.ProductName.ToLower().Contains(productName)

                            select new ProductReadDto
                            {
                                ProductId = p.ProductId,
                                CategoryId = c.CategoryId,
                                Category = c.CategoryName,
                                Price = p.Price,
                                ImageUrl = p.ImageUrl,
                                ProductName = p.ProductName,
                                Quantity = p.Quantity,
                                VendorId = p.VendorId,
                                Description = p.Description

                            }).ToListAsync();

            return Product;
        }

        public async Task<bool> IsProductExist(int productId)
        {
            return await _context.Products.AnyAsync(x=>x.ProductId == productId);
        }

        public async Task<VerifyOfRequest> UpdateProduct(ProductReadDto updatedProduct)
        {
            var VerifyOfRequest = new VerifyOfRequest();

            var ExistCategory = await _context.Categories.AnyAsync(x => x.CategoryId == updatedProduct.CategoryId);
            if (ExistCategory == false)
            {
                VerifyOfRequest.ErrorDetails = "This Category Does not exist ";
                VerifyOfRequest.Errorexisting = true;
                return VerifyOfRequest;
            }

            var ExistVendor = await _context.Users.AnyAsync(x => x.UserId == updatedProduct.VendorId);
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
                CurrentProduct.ImageUrl = updatedProduct.ImageUrl;
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
