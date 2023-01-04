using online_store.Repositories.Auth;
using online_store.Repositories.category;
using online_store.Repositories.PRODUCTS;

namespace online_store.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAuthRepository authRepository { get; }
        ICategoryRepository categoryRepository { get; }
        IProductRepository productRepository { get; }

    }
}
