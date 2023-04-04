using online_store.Models;


namespace online_store.test;

public class tempclass
{
    private readonly OnlineStoreContext _context;
    public tempclass(OnlineStoreContext c)
    {
        _context = c;
    }

    public  async Task<List<ProductReadDto>> GetProducts(Func<Product, bool> cond)
    {
        
        List<ProductReadDto> products = _context.Products

            .Include(x => x.Category)
            .Include(x => x.Images)
            .AsSplitQuery()
            .AsNoTracking()
            .Where(cond)
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
}
