using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;

namespace SocialMedia.Core.Interfaces
{
  public interface IPostService
  {
    PagedList<Post> GetPosts(PostQueryFilter postQueryFilter);
    Task<Post> GetPostAsync(int postId);
    Task InsertPostAsync(Post post);
    Task<bool> UpdatePostAsync(Post post);
    Task<bool> DeletePostAsync(int postId);
  }
}