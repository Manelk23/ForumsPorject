
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ForumsPorject.Repository.Entites;
using ForumsPorject.Repository.Interfaces;
using ForumsPorject.Repository.ClassesRepository;
using Microsoft.EntityFrameworkCore;

namespace ForumsPorject.Repository.ClassesRepository
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly DB_ForumsDbContext _context;

        public CategoryRepository(DB_ForumsDbContext context)
        {
            _context = context;
        }


        public async Task<Category> GetByIdAsync(int id)
        {
            var category = await _context.Set<Category>().FindAsync(id);

            if (category == null)
            {
                // L'entité n'a pas été trouvée, vous pouvez choisir de lever une exception ou de retourner une valeur par défaut
                throw new EntityNotFoundException("Category not found"); // Utilisez une exception personnalisée ou System.Exception

            }

            return category;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Set<Category>().ToListAsync();
        }

        public IQueryable<Category> Find(Expression<Func<Category, bool>> predicate)
        {
            return _context.Set<Category>().Where(predicate);
        }

        public async Task AddAsync(Category entity)
        {
            await _context.Set<Category>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<Category> entities)
        {
            await _context.Set<Category>().AddRangeAsync(entities);
        }

        public void Update(Category entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateRange(IEnumerable<Category> entities)
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
        }

        public void Remove(Category entity)
        {
            _context.Set<Category>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<Category> entities)
        {
            _context.Set<Category>().RemoveRange(entities);
        }

        public async Task UpdateAsync(Category entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }

    [Serializable]
    internal class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string? message) : base(message)
        {
        }

        public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}


