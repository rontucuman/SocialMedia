﻿using System.Collections.Generic;
using System.Linq;
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
    protected readonly DbSet<T> _entities;

    public BaseRepository(SocialMediaContext context)
    {
      _context = context;
      _entities = _context.Set<T>();
    }

    public IEnumerable<T> GetAll()
    {
      return _entities.AsEnumerable();
    }

    public async Task<T> GetByIdAsync(int id)
    {
      return await _entities.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
      await _entities.AddAsync(entity);
    }

    public void Update(T entity)
    {
      _entities.Update(entity);
    }

    public async Task DeleteAsync(int id)
    {
      T entity = await GetByIdAsync(id);
      _entities.Remove(entity);
    }
  }
}