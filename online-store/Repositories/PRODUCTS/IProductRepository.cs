namespace online_store.Repositories.PRODUCTS
{
    public interface IProductRepository
    {
        public Task<List<ProductReadDto>> GetProductsByCategoryID(int categoryId);
        public Task<List<ProductReadDto>> GetProductsByName(string name);
        
        public Task<List<ProductReadDto>> GetAllProducts();
        public Task<List<ProductReadDto>> GetProducts(int productsPerPage , int pageNumber);

        public Task<ProductReadDto> GetProductById(int productid);

        public Task<VerifyOfRequest> AddProduct(Product product , List<string>imgesUrl);
        public Task<VerifyOfRequest> UpdateProduct(ProductReadDto updatedProduct, int userId);
        public Task<VerifyOfRequest> Deleteproduct(int productId , int userId);

        public Task<bool> IsProductExist(int productId);

        public  Task<int> GetCountOfProducts();

    }
}
