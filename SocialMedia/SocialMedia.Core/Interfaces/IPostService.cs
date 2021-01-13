using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Interfaces
{
  public interface IPostService
  {
    Task<IEnumerable<Post>> GetPostsAsync();
    Task<Post> GetPostAsync(int postId);
    Task InsertPostAsync(Post post);
    Task<bool> UpdatePostAsync(Post post);
    Task<bool> DeletePostAsync(int postId);
  }
}