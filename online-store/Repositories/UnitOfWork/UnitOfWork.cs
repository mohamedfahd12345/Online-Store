using online_store.Repositories.Auth;
using online_store.Repositories.category;
using online_store.Repositories.PRODUCTS;

namespace online_store.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAuthRepository authRepository { get; }
        public ICategoryRepository categoryRepository { get; }
        public IProductRepository productRepository { get; }

        public UnitOfWork(
            IAuthRepository authRepository ,
            ICategoryRepository categoryRepository ,
            IProductRepository productRepository )
        {
            this.authRepository = authRepository;
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
        }


        
    }
}
