using online_store.Repositories.Auth;
using online_store.Repositories.CART;
using  online_store.Models;
using AutoMapper;

namespace online_store.Repositories.ORDER;

public class OrderRepository : IOrderRepository
{
    private readonly OnlineStoreContext _context;
    private readonly ICartRepository cartRepository;
    private readonly IAuthRepository authRepository;
    private readonly IMapper mapper;
    public OrderRepository(OnlineStoreContext context 
        , ICartRepository cartRepository
        , IAuthRepository authRepository
        , IMapper mapper
        )
    {
        _context = context;
        this.cartRepository = cartRepository;
        this.authRepository = authRepository;
        this.mapper = mapper;
    }

    public ResponseDetails GenerateResponseDetails(int statusCode , string message)
    {
        return new ResponseDetails
        {
            statusCode = statusCode,
            ResponseMessage = message
        };

    }
    public async Task<ResponseDetails> CheckoutAsync(int customerId, CheckoutDto checkoutDto)
    {

        var targetUser = await authRepository.IsUserExist(customerId);

        if (!targetUser)
        {
            return  GenerateResponseDetails(401, "buyer is not found");
        }

        var cartExists = await cartRepository.GetFullCart(customerId);

        if (cartExists is null)
        {
            return  GenerateResponseDetails(404, "cart not found");
        }


        if (!cartExists.CartItems.Any() || cartExists.CartItems is null)
        {
            return GenerateResponseDetails(404, "cart was empty");
        }

        if(customerId != cartExists.UserId)
        {
            return GenerateResponseDetails(403, "this is not your cart ");
        }

        var Address = mapper.Map<Address>(checkoutDto);
        await _context.Addresses.AddAsync(Address);
        await _context.SaveChangesAsync();

        decimal shippingCost = 0;
        var order = new Order
        {
           OrderDate = DateTime.UtcNow,
           OrderStatus = OrderStatus.Pending.ToString() ,
           PaymentMethod = checkoutDto.paymentMethod ,
           CustomerId = customerId ,
           PhoneNumber  = checkoutDto.PhoneNumber ,
           ShippingCost = shippingCost,
           AdderssId = Address.AddressId

        };

        order.TotalAmount = cartExists.CartItems
            .Sum(ci => ci.Product.Price * ci.Quantity) + shippingCost;

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        var orderItems =  cartExists.CartItems
            .Select(
                ci => new OrderProduct
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    PricePerItem = ci.Product.Price,
                    OrderId = order.OrderId
                }
            ).ToList();

        await _context.OrderProducts.AddRangeAsync(orderItems);
        await _context.SaveChangesAsync();


        return GenerateResponseDetails(201, "created successfully");
    }

    public async Task<List<OrderReadDto>> GetAllOrders(int customerId)
    {
       var targetUser = await authRepository.IsUserExist(customerId);
        if (!targetUser)
        {
            return null;
        }

        return await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .Select(or => new OrderReadDto
            {
                OrderDate = or.OrderDate ,
                OrderId = or.OrderId ,
                OrderStatus = or.OrderStatus 
            })
            .ToListAsync();


    }

    public async Task<OrderDetailsReadDto> GetOrderDetail(int customerId, int orderId)
    {
        var targetUser = await authRepository.IsUserExist(customerId);
        if (!targetUser)
        {
            return null;
        }

        var targetOrder = await _context.Orders
            .Where(o => o.OrderId == orderId)
            .Select(or => new 
            {
                or.OrderId ,
                or.CustomerId ,
                or.OrderStatus ,
                or.DeliveredDate
            })
            .FirstOrDefaultAsync();

        throw new Exception();

    }
}
