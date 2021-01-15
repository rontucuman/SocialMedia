using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Interfaces
{
  public interface IPostRepository : IRepository<Post>
  {
    Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId);
  }
}