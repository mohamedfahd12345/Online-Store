namespace online_store.Repositories.Order;

public interface IOrderRepository
{
    Task<ResponseDetails> CheckoutAsync(int customerId , CheckoutDto checkoutDto);
}
