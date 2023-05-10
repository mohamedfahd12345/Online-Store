using System.Reflection;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Supabase;

using Supabase.Storage;
namespace online_store.Controllers;

//[Authorize("vendor")]
[Route("api/[controller]")]
[ApiController]
public class ProductsImages : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly Supabase.Client _clinet;
    public ProductsImages(IConfiguration config, Supabase.Client clinet)
    {
        _config = config;
        _clinet = clinet;
    }

    [HttpPost("image")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {


        using var memoryStream = new MemoryStream();

        await file.CopyToAsync(memoryStream);

        var lastIndexOfDot = file.FileName.LastIndexOf('.');

        string extension = file.FileName.Substring(lastIndexOfDot + 1);
        Guid newGuid = Guid.NewGuid();
        var imagePath = newGuid.ToString() + extension;

          await _clinet.Storage.From("Products").Upload(
            memoryStream.ToArray(),
            imagePath);
        var imageUrl = _clinet.Storage.From("Products").GetPublicUrl("mo.jpg");

        return Ok(imageUrl);
        
    }


     


}
