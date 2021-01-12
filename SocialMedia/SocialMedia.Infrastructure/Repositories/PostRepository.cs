using System;
using System.Collections.Generic;
using System.Linq;
using SocialMedia.Core.Entities;

namespace SocialMedia.Infrastructure.Repositories
{
  public class PostRepository
  {
    public IEnumerable<Post> GetPosts()
    {
      var posts = Enumerable.Range(1, 10).Select(x => new Post
      {
        PostId = x,
        UserId = x,
        Date = DateTime.Now,
        Description = $"Description Post {x}",
        Image = $"Image Post {x} URL"
      });

      return posts;
    }
  }
}