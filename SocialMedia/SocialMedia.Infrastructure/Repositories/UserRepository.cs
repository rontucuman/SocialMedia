using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly SocialMediaContext _context;

    public UserRepository( SocialMediaContext context )
    {
      _context = context;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
      return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserAsync( int userId )
    {
      return await _context.Users.FirstOrDefaultAsync( x => x.UserId == userId );
    }
  }
}