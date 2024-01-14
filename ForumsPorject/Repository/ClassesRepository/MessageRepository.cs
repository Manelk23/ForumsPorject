
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ForumsPorject.Repository.Entites;
using ForumsPorject.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumsPorject.Repository.ClassesRepository { 
    public class MessageRepository : IRepository<Message>
    {
        private readonly DB_ForumsDbContext _context;

        public MessageRepository(DB_ForumsDbContext context)
        {
            _context = context;
        }


        public List<Message> GetMessagesByDiscId(int discussionId)
        {
            return _context.Messages
                .Where(m => m.Discussionid == discussionId)
                .ToList();
        }





        public async Task<Message?> GetByIdAsync(int id)
        {
            return await _context.Set<Message>().FindAsync(id);
        }


        public async Task<IEnumerable<Message>> GetAllAsync()
        {
            return await _context.Set<Message>().ToListAsync();
        }

        public IQueryable<Message> Find(Expression<Func<Message, bool>> predicate)
        {
            return _context.Set<Message>().Where(predicate);
        }


       

        public async Task AddRangeAsync(IEnumerable<Message> entities)
        {
            await _context.Set<Message>().AddRangeAsync(entities);
        }

        public void Update(Message entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            
        }

        public async Task UpdateAsync(Message entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public void UpdateRange(IEnumerable<Message> entities)
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
        }

        public void Remove(Message entity)
        {
            _context.Set<Message>().Remove(entity);
        }

        public async Task RemoveAsyn(Message entity)
        {
            _context.Set<Message>().Remove(entity);
            await _context.SaveChangesAsync();
        }


        public void RemoveRange(IEnumerable<Message> entities)
        {
            _context.Set<Message>().RemoveRange(entities);
        }

        public async Task AddAsync(Message entity)
        {
            await _context.Set<Message>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
