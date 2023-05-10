namespace online_store.Repositories.PRODUCTS
{
    public interface IProductRepository
    {
        public Task<List<ProductsReadDto>> GetProductsByCategoryID(int categoryId);
        public Task<List<ProductsReadDto>> GetProductsByName(string name);
        
        public Task<List<ProductsReadDto>> GetAllProducts();
        public Task<List<ProductsReadDto>> GetProducts(int productsPerPage , int pageNumber);

        public Task<OneProductReadDto> GetProductById(int productid);//-------------

        public Task<VerifyOfRequest> AddProduct(Product product , List<string>imgesUrl);
        public Task<VerifyOfRequest> UpdateProduct(ProductUpdateDto updatedProduct, int userId);//-------------
        public Task<VerifyOfRequest> Deleteproduct(int productId , int userId);

        public Task<bool> IsProductExist(int productId);

        public  Task<int> GetCountOfProducts();

    }
}
