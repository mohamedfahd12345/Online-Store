using online_store.Repositories.CART;

namespace online_store.Repositories.Order;

public class OrderRepository : IOrderRepository
{
    private readonly OnlineStoreContext _context;
    private readonly ICartRepository cartRepository;
    public OrderRepository(OnlineStoreContext context , ICartRepository cartRepository)
    {
        _context = context;
        this.cartRepository = cartRepository;
    }
    public Task<ResponseDetails> CheckoutAsync(int customerId, CheckoutDto checkoutDto)
    {
        
        throw new Exception();
    }
}
