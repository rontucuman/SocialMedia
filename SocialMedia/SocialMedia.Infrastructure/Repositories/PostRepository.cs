﻿using System.Collections.Generic;
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

    public async Task InsertPostAsync(Post post)
    {
      _context.Posts.Add(post);
      await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdatePostAsync(Post post)
    {
      Post savedPost = await GetPostAsync(post.PostId);
      savedPost.Date = post.Date;
      savedPost.Description = post.Description;
      savedPost.Image = post.Image;

      int rowsAffected = await _context.SaveChangesAsync();
      return rowsAffected > 0;
    }

    public async Task<bool> DeletePostAsync(int postId)
    {
      Post savedPost = await GetPostAsync(postId);
      _context.Posts.Remove(savedPost);
      
      int rowsAffected = await _context.SaveChangesAsync();
      return rowsAffected > 0;
    }
  }
}