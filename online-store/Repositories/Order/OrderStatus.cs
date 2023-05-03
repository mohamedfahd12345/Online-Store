namespace online_store.Repositories.ORDER;

public enum OrderStatus
{
    PendingPayment,
    Paid,
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}