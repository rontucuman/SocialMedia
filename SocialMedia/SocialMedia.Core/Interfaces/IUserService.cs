using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Interfaces
{
  public interface IUserService
  {
    Task<User> GetUserAsync( int userId );
    Task<IEnumerable<User>> GetUsersAsync();
  }
}