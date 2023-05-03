namespace online_store.Repositories.CART;

public interface ICartRepository
{
    Task<List<CartReadDto>> GetCartItemsAsync(int customerId);
    Task<bool> AddOneToCart(int customerId, int productId);
    Task<bool> RemoveFromCart(int customerId, int productId);
    Task<bool> RmoveOneFromCart(int customerId, int productId);

    Task<bool> AddToCartWithQuantity(int customerId, int productId, int quantity);
    Task<bool> ClearCart(int customerId);

    Task<int?> GetCartItemCountAsync(int customerId);
    Task<decimal?> GetCartTotalPriceAsync(int customerId);

    Task<Cart?> GetFullCart(int customerId); 

}
