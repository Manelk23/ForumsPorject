using ForumsPorject.Repository.ClassesRepository;
using ForumsPorject.Repository.Entites;

namespace ForumsPorject.Services
{
    
    
        public class CategoryService
        {
            private readonly CategoryRepository _categoryRepository;

            public CategoryService(CategoryRepository categoryRepository)
            {
                _categoryRepository = categoryRepository;
            }


            public async Task<IEnumerable<Category>> GetAll()
            {
               return await _categoryRepository.GetAllAsync();
            }

        }
    }


