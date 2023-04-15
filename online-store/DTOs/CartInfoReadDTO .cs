namespace online_store.DTOs
{
    public class CartInfoReadDTO
    {
        public List<CartReadDto> cartItems = new List<CartReadDto>();
        public int CartCount { get; set; }
        public decimal CartTotalPrice { get; set; }
        
    }
}
