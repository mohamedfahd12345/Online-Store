using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_store.Repositories.CART;

namespace online_store.Controllers;
[Authorize(Roles = "user")]
[Route("api/carts")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartRepository _cartRepository;
    public CartController(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    [HttpGet("items")]
    public async Task<IActionResult> GetCartItems()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        return Ok(await _cartRepository.GetCartItemsAsync(userId));
    }

    [HttpGet("item-count")]
    public async Task<IActionResult> GetCartItemCount()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        return Ok(await _cartRepository.GetCartItemsAsync(userId));
    }


    [HttpGet("total-price")]
    public async Task<IActionResult> GetCartTotalPrice()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        return Ok(await _cartRepository.GetCartTotalPriceAsync(userId));
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddCartItems([FromQuery][Required] int productId , [FromQuery][Required] int quntity)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result =await _cartRepository.AddToCartWithQuantity(userId,productId,quntity);

        if(result == false)return BadRequest(new {error = "Failed to add"});
        return StatusCode(201);
    }

    [HttpPost("one-item")]
    public async Task<IActionResult> AddOneCartItem([FromQuery][Required] int productId)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _cartRepository.AddOneToCart(userId, productId);

        if (result == false) return BadRequest(new { error = "Failed to add" });
        return StatusCode(201);
    }

    [HttpDelete("items/{productId:int}")]
    public async Task<IActionResult> DeleteCartItems([FromRoute]int productId){
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _cartRepository.RemoveFromCart(userId, productId);

        if (result == false) return BadRequest(new { error = "Failed to Remove" });
        return NoContent();
    }

    [HttpDelete("item/{productId:int}")]
    public async Task<IActionResult> DeleteOneCartItem([FromRoute] int productId)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _cartRepository.RmoveOneFromCart(userId, productId);

        if (result == false) return BadRequest(new { error = "Failed to Remove" });
        return NoContent();
    }

    [HttpDelete("cart")]
    public async Task<IActionResult> ClearCart()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _cartRepository.ClearCart(userId);

        if (result == false) return BadRequest(new { error = "Failed to Clear the Cart" });
        return NoContent();
    }


}
