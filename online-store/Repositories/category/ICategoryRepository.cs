using online_store.Helper;
using online_store.Models;
namespace online_store.Repositories.category
{
    public interface ICategoryRepository
    {
        public Task <List<Category>> GetAllCategory();
        public Task<VerifyOfRequest> CreateCategory(Category category);
        public Task<bool> UpdateCategory(Category category);
        public Task <VerifyOfRequest> DeleteCategory(int categoryId);
        public Task<Category> GetById(int categoryId);
        public Task<bool> IsCategoryExist(int categoryId);

        public Task<int> GetCountOfCategories();
    }
}
