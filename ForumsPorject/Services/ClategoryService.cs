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

            public async Task<Category> GetById(int id)
            {
                return await _categoryRepository.GetByIdAsync(id);
            }

            public async Task<IEnumerable<Category>> GetAll()
            {
                return await _categoryRepository.GetAllAsync();
            }

            public async Task AddAsync(Category entity)
            {
                await _categoryRepository.AddAsync(entity);
            }

            public async Task UpdateAsync(Category entity)
            {
                await _categoryRepository.UpdateAsync(entity);
            }


        }
    }


