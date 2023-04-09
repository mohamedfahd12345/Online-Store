namespace online_store.Repositories.CART
{
    public class CartRepository : ICartRepository
    {
        private readonly OnlineStoreContext _context;

        public CartRepository(OnlineStoreContext context)
        {
            _context = context;
        }

        private async Task<Cart?> GetCart(int customerId)
        {
            return await _context.Carts
                .Where(c => c.UserId == customerId)
                .FirstOrDefaultAsync();
        }

        private async Task<CartItem?> GetProductInCart(int cartId, int productId)
        {
            return await _context.CartItems
                .Where(c => c.ProductId == productId && c.CartId == cartId)
                .FirstOrDefaultAsync();
        }

        public Task<bool> AddOneToCart(int customerId, int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddToCartWithQuantity(int customerId, int productId, int quantity)
        {
            //var customerCart = await GetCart(customerId);

            //if (customerCart is null)
            //{

            //}
            throw new Exception();
        }

        public async Task<bool> ClearCart(int customerId)
        {
            var customerCart = await _context.Carts
                .Where(c => c.UserId == customerId)
                .FirstOrDefaultAsync();

            if (customerCart is null) return false;

            _context.Carts.Remove(customerCart);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> RemoveFromCart(int customerId, int productId)
        {
            var customerCart = await GetCart(customerId);

            if (customerCart is null) return false;

            var customerCartItems = await GetProductInCart(customerCart.CartId, productId);

            if (customerCartItems is null) return false;

            _context.CartItems.Remove(customerCartItems);
            await _context.SaveChangesAsync();
            return true;

            
        }

        public async Task<bool> RmoveOneFromCart(int customerId ,int productId)
        {
            var customerCart = await GetCart(customerId);

            if (customerCart is null) return false;

            var customerCartItem = await GetProductInCart(customerCart.CartId, productId);

            if (customerCartItem is null) return false;

            if(customerCartItem.Quantity > 1)
            {
                customerCartItem.Quantity--;
                _context.CartItems.Update(customerCartItem);
            }

            else if(customerCartItem.Quantity == 1)
            {
                _context.CartItems.Remove(customerCartItem);
            }

            await _context.SaveChangesAsync();

            return true;

        }
    }
}
