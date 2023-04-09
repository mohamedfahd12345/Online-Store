namespace online_store.Repositories.CART
{
    public interface ICartRepository
    {
        Task<bool> AddOneToCart(int customerId, int productId);
        Task<bool> RemoveFromCart(int customerId, int productId);
        Task<bool> RmoveOneFromCart(int customerId, int productId);

        Task<bool> AddToCartWithQuantity(int customerId, int productId, int quantity);
        Task<bool> ClearCart(int customerId);

    }
}
