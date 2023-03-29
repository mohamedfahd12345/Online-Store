using AutoMapper;
using online_store.Helper;

namespace online_store.Repositories.category
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly OnlineStoreContext _context;
        
        public CategoryRepository(OnlineStoreContext context)
        {
            _context = context;
            
        }

        public async Task<VerifyOfRequest> CreateCategory(Category category)
        {
            var verifyOfRequest = new VerifyOfRequest();
            try
            {
                
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

            }
            catch(Exception ex)
            {
                verifyOfRequest.Errorexisting = true;
                verifyOfRequest.ErrorDetails = ex.Message;
                return verifyOfRequest;
            }
            return verifyOfRequest;

        }

        public async Task<VerifyOfRequest> DeleteCategory(int categoryId)
        {
            var verifyOfRequest = new VerifyOfRequest();

            try
            {
                
                var targetCategory= await _context.Categories.Where(x=>x.CategoryId == categoryId).FirstOrDefaultAsync();
                
                if(targetCategory is null)
                {
                    verifyOfRequest.Errorexisting = true;
                    verifyOfRequest.ErrorDetails = "This Category does not exit";
                    return verifyOfRequest;
                }

                _context.Categories.Remove(targetCategory);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                verifyOfRequest.Errorexisting = true;
                verifyOfRequest.ErrorDetails = ex.Message;
                return verifyOfRequest;
            }
            return verifyOfRequest;
        }

        public async Task <List<Category>> GetAllCategory()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            try
            {
                 _context.Categories.Update(category);
                 await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
            
        }

        public async Task<Category> GetById(int categoryId)
        {
            var targetCategory = await _context.Categories
                .Where(x => x.CategoryId == categoryId)
                .FirstOrDefaultAsync();
            return targetCategory;
        }

        public async Task<bool> IsCategoryExist(int categoryId)
        {
            var targetCategory = await _context.Categories
                .Where(x => x.CategoryId == categoryId)
                .Select(x => x.CategoryId)
                .FirstOrDefaultAsync();

            if (targetCategory == 0) return false;

            return true;
        }
        
        public async Task<int> GetCountOfCategories()
        {
            int count = await _context.Categories.CountAsync();

            return count;
        }
    }
}
