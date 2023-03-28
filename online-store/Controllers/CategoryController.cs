using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_store.Repositories.category;

namespace online_store.Controllers
{
    [Authorize (Roles ="vendor")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;
        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
            
        }
        
        [HttpGet, Route("/categories")]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(mapper.Map<List<CategoryReadDTO>>(await categoryRepository.GetAllCategory())) ;
        }


        [HttpPost, Route("/categories")]
        public async Task<IActionResult> CreateCategories(CategoryDTO categoryDTO)
        {
            if(categoryDTO == null || !ModelState.IsValid)
            {
                return BadRequest("Invaild Request");
            }

            var VerifyOfRequest = new VerifyOfRequest();

            VerifyOfRequest =await categoryRepository.CreateCategory( mapper.Map<Category>(categoryDTO) );

            if (VerifyOfRequest.Errorexisting == false)
            {
                return StatusCode(201);
            }
            return BadRequest(VerifyOfRequest.ErrorDetails);
        }


        [HttpDelete, Route("/categories/{categoryId}")]
        public async Task<IActionResult> DeleteCategories(int categoryId)
        {
            if (await categoryRepository.IsCategoryExist(categoryId) == false)
            {
                return NotFound("can't found the Category to delete it");
            }

            var VerifyOfRequest = new VerifyOfRequest();

            VerifyOfRequest = await categoryRepository.DeleteCategory(categoryId);

            if(VerifyOfRequest.Errorexisting == false)
            {
                return Ok();
            }

            return BadRequest(VerifyOfRequest.ErrorDetails);
           
        }


        [HttpPut, Route("/categories/{categoryId}")]
        public async Task<IActionResult> UpdateCategories(int categoryId , CategoryReadDTO updatedCategory)
        {
            if(categoryId != updatedCategory.CategoryId)
            {
                return BadRequest($"id in object doesn't match with id in Parameters");
            }

            if (await categoryRepository.IsCategoryExist(categoryId) == false)
            {
                return NotFound($"can't found Category with id {categoryId} ");
            }

            
            bool result= await categoryRepository.UpdateCategory(mapper.Map<Category>(updatedCategory));
            if(result == true)
            {
                return Ok("Category Updated Successfully");
            }
            return BadRequest("No Category Updated, You didn't change any data");
        }


        [HttpGet, Route("/categories/{categoryId}")]
        public async Task<IActionResult> GetCategoryByID(int categoryId) 
        {
            var category =await categoryRepository.GetById(categoryId);
            if ( category is null)
            {
                 return NotFound($"can't found Category with id {categoryId} ");
            }
            return Ok(mapper.Map<CategoryReadDTO>(category));
        
        }
        

        [HttpGet, Route("/count-catergorys")]
        public async Task<IActionResult> GetCountOfCategories()
        {
            return Ok(await categoryRepository.GetCountOfCategories());
        }
    }
}
