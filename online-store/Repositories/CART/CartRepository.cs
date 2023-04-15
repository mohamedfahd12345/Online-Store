using online_store.Repositories.PRODUCTS;

namespace online_store.Repositories.CART
{
    public class CartRepository : ICartRepository
    {
        private readonly OnlineStoreContext _context;
        private readonly IProductRepository _ProductRepository;

        public CartRepository(OnlineStoreContext context, IProductRepository ProductRepository)
        {
            _context = context;
            _ProductRepository = ProductRepository;
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

        public async Task<bool> AddOneToCart(int customerId, int productId)
        {
            if (await _ProductRepository.GetProductById(productId) is null)
            {
                return false;
            }

            var customerCart = await GetCart(customerId);
            if (customerCart is null)
            {
                var cart = new Cart
                {
                    UserId = customerId
                };
                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();

                var cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = 1
                };
                await _context.CartItems.AddAsync(cartItem);
                await _context.SaveChangesAsync();
                return true;
            }

            var customerCartItem = await GetProductInCart(customerCart.CartId, productId);
            if (customerCartItem is null)
            {
                var cartItem = new CartItem
                {
                    CartId = customerCart.CartId,
                    ProductId = productId,
                    Quantity = 1
                };
                await _context.CartItems.AddAsync(cartItem);
                await _context.SaveChangesAsync();

            }
            else
            {
                customerCartItem.Quantity++;
                _context.CartItems.Update(customerCartItem);
                await _context.SaveChangesAsync();
            }

            return true;

        }

        public async Task<bool> AddToCartWithQuantity(int customerId, int productId, int quantity)
        {
            var product = await _ProductRepository.GetProductById(productId);
            if (product is null)
            {
                return false;
            }
            
            if(product.Quantity < quantity){
                quantity =(int)product.Quantity ;
            }

            var customerCart = await GetCart(customerId);
            if (customerCart is null)
            {
                var cart = new Cart
                {
                    UserId = customerId
                };
                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();

                var cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = quantity
                };
                await _context.CartItems.AddAsync(cartItem);
                await _context.SaveChangesAsync();
                return true;
            }

            var customerCartItem = await GetProductInCart(customerCart.CartId, productId);
            if (customerCartItem is null)
            {
                var cartItem = new CartItem
                {
                    CartId = customerCart.CartId,
                    ProductId = productId,
                    Quantity = quantity
                };
                await _context.CartItems.AddAsync(cartItem);
                await _context.SaveChangesAsync();

            }
            else
            {
                customerCartItem.Quantity = quantity;
                _context.CartItems.Update(customerCartItem);
                await _context.SaveChangesAsync();
            }

            return true;

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

        public async Task<bool> RmoveOneFromCart(int customerId, int productId)
        {
            var customerCart = await GetCart(customerId);

            if (customerCart is null) return false;

            var customerCartItem = await GetProductInCart(customerCart.CartId, productId);

            if (customerCartItem is null) return false;

            if (customerCartItem.Quantity > 1)
            {
                customerCartItem.Quantity--;
                _context.CartItems.Update(customerCartItem);
            }

            else if (customerCartItem.Quantity == 1)
            {
                _context.CartItems.Remove(customerCartItem);
            }

            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<CartInfoReadDTO?> GetAllInCart(int customerId)
        {
            var customCart = await GetCart(customerId);
            if (customCart is null) return null;

            var CartInfoDetails = new CartInfoReadDTO();

            CartInfoDetails.cartItems = await _context.CartItems
                .Include(p => p.Product)
                .AsNoTracking()
                .AsSplitQuery()
                .Where(c => c.CartId == customCart.CartId)
                .Select(i => new CartReadDto
                {
                    MainImageUrl = i.Product != null ? i.Product.MainImageUrl : null ,
                    Price = i.Product != null ? i.Product.Price : null,
                    ProductId = i.Product != null ? i.Product.ProductId : null ,
                    Quantity = i.Quantity  ,
                    ProductName = i.Product != null ? i.Product.ProductName : null
                    
                })
                .ToListAsync();


            if (CartInfoDetails is null) return null;

            CartInfoDetails.CartCount = await _context.CartItems
                .Where(c => c.CartId == customCart.CartId)
                .SumAsync(x => x.Quantity);

            CartInfoDetails.CartTotalPrice = await _context.CartItems
                .Include(c=>c.Product)
                .Where(c => c.CartId == customCart.CartId)
                .SumAsync(x => x.Quantity * (x.Product !=null ? x.Product.Price : 0));
            
            return CartInfoDetails;
        }
    }
}
