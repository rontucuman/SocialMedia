using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;

namespace SocialMedia.Core.Services
{
  public class PostService : IPostService
  {
    private readonly IUnitOfWork _unitOfWork;

    public PostService(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public IEnumerable<Post> GetPosts(PostQueryFilter postQueryFilter)
    {
      var posts = _unitOfWork.PostRepository.GetAll();

      if (postQueryFilter.UserId != null)
      {
        posts = posts.Where(x => x.UserId == postQueryFilter.UserId);
      }

      if (postQueryFilter.Date != null)
      {
        posts = posts.Where(x => x.Date.ToShortDateString() == postQueryFilter.Date?.ToShortDateString());
      }

      if (postQueryFilter.Description != null)
      {
        posts = posts.Where(
          x => x.Description.Contains(postQueryFilter.Description, StringComparison.OrdinalIgnoreCase));
      }

      return posts;
    }

    public async Task<Post> GetPostAsync(int postId)
    {
      return await _unitOfWork.PostRepository.GetByIdAsync(postId);
    }

    public async Task InsertPostAsync(Post post)
    {
      User user = await _unitOfWork.UserRepository.GetByIdAsync(post.UserId);

      if (user == null)
      {
        throw new BusinessException("User does not exist");
      }

      IEnumerable<Post> userPosts = await _unitOfWork.PostRepository.GetPostsByUserIdAsync(post.UserId);

      if (userPosts.Count() < 10)
      {

        Post lastPost = userPosts.OrderByDescending(x => x.Date).FirstOrDefault();
        if ((DateTime.Now - lastPost.Date).Days < 7)
        {
          throw new BusinessException("You cannot publish more Posts");
        }
      }

      if (post.Description.Contains("sex", StringComparison.InvariantCultureIgnoreCase))
      {
        throw new BusinessException("Content not allowed");
      }

      await _unitOfWork.PostRepository.AddAsync(post);
      await _unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> UpdatePostAsync(Post post)
    {
      _unitOfWork.PostRepository.Update(post);
      await _unitOfWork.SaveChangesAsync(); 
      return true;
    }

    public async Task<bool> DeletePostAsync(int postId)
    {
      await _unitOfWork.PostRepository.DeleteAsync(postId);
      await _unitOfWork.SaveChangesAsync();
      return true;
    }
  }
}