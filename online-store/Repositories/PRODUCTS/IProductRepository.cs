namespace online_store.Repositories.PRODUCTS
{
    public interface IProductRepository
    {
        public  Task<List<Product>> GetProductsByCategoryID(int categoryId);
        public Task<List<ProductReadDto>> GetProductsByName(string name);
        
        public Task<List<ProductReadDto>> GetAllProducts();
        public Task<List<ProductReadDto>> GetProducts(int productsPerPage , int pageNumber);

        public Task<ProductReadDto> GetProductById(int productid);

        public Task<VerifyOfRequest> AddProduct(Product product);
        public Task<VerifyOfRequest> UpdateProduct(ProductReadDto updatedProduct);
        public Task<VerifyOfRequest> Deleteproduct(int productId);

        public Task<bool> IsProductExist(int productId);

        public  Task<int> GetCountOfProducts();

    }
}
