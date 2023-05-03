using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_store.Repositories.ORDER;

namespace online_store.Controllers;
[Authorize(Roles = "user")]
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    public OrdersController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    
    [HttpPost("cart/checkout")]
    public async Task<IActionResult> BuyersCheckout([FromBody] CheckoutDto checkoutDto)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _orderRepository.CheckoutAsync(userId , checkoutDto);
        return StatusCode(result.statusCode, result.ResponseMessage);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        return Ok(await _orderRepository.GetAllOrders(userId));
    }

    [HttpGet("orderDetails/{orderId:int}")]
    public async Task<IActionResult> GetOrderDetils([FromRoute]int orderId)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        return Ok(await _orderRepository.GetOrderDetail(userId , orderId));
    }
}
