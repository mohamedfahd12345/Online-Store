namespace online_store.Services.ProductsImages;

public interface IProductImage
{
    Task<string> UploadImage(int customerId, IFormFile file );
}
