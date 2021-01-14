using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
  public class BaseRepository<T> : IRepository<T> where T : BaseEntity
  {
    private readonly SocialMediaContext _context;
    private readonly DbSet<T> _entities;

    public BaseRepository(SocialMediaContext context)
    {
      _context = context;
      _entities = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
      return await _entities.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
      return await _entities.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
      _entities.Add(entity);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
      _entities.Update(entity);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
      T entity = await GetByIdAsync(id);
      _entities.Remove(entity);
      await _context.SaveChangesAsync();
    }
  }
}