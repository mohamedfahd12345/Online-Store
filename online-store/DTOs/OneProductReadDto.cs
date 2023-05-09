namespace online_store.DTOs;

public class OneProductReadDto : ProductsReadDto
{
    public List<ImageDto> imagesUrl { get; set; } = new List<ImageDto>();
}
