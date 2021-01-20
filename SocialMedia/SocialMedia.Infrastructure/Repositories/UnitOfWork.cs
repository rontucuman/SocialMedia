﻿using System.Threading.Tasks;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly SocialMediaContext _context;
    private readonly IPostRepository _postRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Comment> _commentRepository;
    private readonly ISecurityRepository _securityRepository;

    public UnitOfWork(SocialMediaContext context)
    {
      _context = context;
    }

    public async void Dispose()
    {
      if (_context != null)
      {
        await _context.DisposeAsync();
      }
    }

    public IPostRepository PostRepository => _postRepository ?? new PostRepository(_context);
    public IRepository<User> UserRepository => _userRepository ?? new BaseRepository<User>(_context);
    public IRepository<Comment> CommentRepository => _commentRepository ?? new BaseRepository<Comment>(_context);
    public ISecurityRepository SecurityRepository => _securityRepository ?? new SecurityRepository(_context);

    public void SaveChanges()
    {
      _context.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
      await _context.SaveChangesAsync();
    }
  }
}