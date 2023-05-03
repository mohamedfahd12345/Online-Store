namespace online_store.Repositories.ORDER;

public interface IOrderRepository
{
    Task<ResponseDetails> CheckoutAsync(int customerId , CheckoutDto checkoutDto);
    Task<List<OrderReadDto>> GetAllOrders(int customerId);  

    Task<OrderDetailsReadDto> GetOrderDetail(int customerId , int orderId);
}
