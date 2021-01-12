using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
  public class PostRepository : IPostRepository
  {
    private readonly SocialMediaContext _context;

    public PostRepository(SocialMediaContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Post>> GetPostsAsync()
    {
      var posts = await _context.Posts.ToListAsync();
      
      return posts;
    }

    public async Task<Post> GetPostAsync(int postId)
    {
      var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == postId);
      return post;
    }
  }
}