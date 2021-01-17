using System.Threading.Tasks;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly SocialMediaContext _context;
    private IPostRepository _postRepository;
    private IRepository<User> _userRepository;
    private IRepository<Comment> _commentRepository;

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