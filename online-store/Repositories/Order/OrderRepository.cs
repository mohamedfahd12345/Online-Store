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
                    PricePerItem = ci.Product.Price ,
                    OrderId = order.OrderId
                }
            ).ToList();

       
        await _context.OrderProducts.AddRangeAsync(orderItems);

        await cartRepository.DeleteAsync(cartExists);

        await _context.SaveChangesAsync();


        return GenerateResponseDetails(201, "created successfully");
    }

    public async Task<List<OrderReadDto>?> GetAllOrders(int customerId)
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

    public async Task<OrderDetailsReadDto?> GetOrderDetails(int customerId, int orderId)
    {
        var targetUser = await authRepository.IsUserExist(customerId);
        if (!targetUser)
        {
            return null;
        }

        var targetOrder = await _context.Orders
            .Include(a => a.Adderss)
            .Include(o => o.OrderProducts)
            .ThenInclude(p=>p.Product)
            .AsSplitQuery()
            .AsNoTracking()
            .Where(o => o.OrderId == orderId)
            .FirstOrDefaultAsync();

        if (targetOrder == null || targetOrder.CustomerId != customerId)
        {
            return null;
        }

        return MappingOrderToOrderDto(targetOrder);

    }

    public OrderDetailsReadDto MappingOrderToOrderDto(Order order)
    {
        var orderDetails = new OrderDetailsReadDto();

        orderDetails.AddressId = order.Adderss.AddressId;
        orderDetails.StreetAddress = order.Adderss.StreetAddress;
        orderDetails.StreetNumber = order.Adderss.StreetNumber;
        orderDetails.State = order.Adderss.State;
        orderDetails.City = order.Adderss.City;
        orderDetails.Country = order.Adderss.Country;
        orderDetails.ZipCode = order.Adderss.ZipCode;

        orderDetails.OrderId = order.OrderId;
        orderDetails.OrderStatus = order.OrderStatus;
        orderDetails.PhoneNumber = order.PhoneNumber;
        orderDetails.ShippedDate = order.ShippedDate;
        orderDetails.ShippingCost = order.ShippingCost;
        orderDetails.OrderDate = order.OrderDate;
        orderDetails.DeliveredDate = order.DeliveredDate;
        orderDetails.TotalAmount = order.TotalAmount;
        orderDetails.PaymentMethod = order.PaymentMethod;

        orderDetails.orderProducts = order.OrderProducts
                                                        .Select(o => new CartReadDto
                                                        {
                                                            Price = o.PricePerItem,
                                                            ProductId = o.ProductId,
                                                            Quantity = o.Quantity,
                                                            MainImageUrl = o.Product.MainImageUrl,
                                                            ProductName = o.Product.ProductName
                                                        }).ToList();
        return orderDetails;
    }



}
