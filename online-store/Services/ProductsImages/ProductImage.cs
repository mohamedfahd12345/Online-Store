using Supabase;
using Supabase.Storage;
using FileOptions = Supabase.Storage.FileOptions;
namespace online_store.Services.ProductsImages;

public class ProductImage : IProductImage
{
    private readonly Supabase.Client client;
    public async Task<string> UploadImage(int customerId, IFormFile file)
    {
        using var memoryStream = new MemoryStream();

        await file.CopyToAsync(memoryStream);

        var lastIndexOfDot = file.FileName.LastIndexOf('.');

        string extension = file.FileName.Substring(lastIndexOfDot + 1);

        

        var r = await client.Storage.From("Products").Upload(
            memoryStream.ToArray(),
            $"Product-{customerId}.{extension}");

       

        throw new NotImplementedException();
    }
}
