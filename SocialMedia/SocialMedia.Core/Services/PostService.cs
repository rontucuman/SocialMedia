using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Core.Services
{
  public class PostService : IPostService
  {
    private readonly IUnitOfWork _unitOfWork;

    public PostService(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Post>> GetPostsAsync()
    {
      return await _unitOfWork.PostRepository.GetAllAsync();
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
        throw new Exception("User does not exist");
      }

      if (post.Description.Contains("sex", StringComparison.InvariantCultureIgnoreCase))
      {
        throw new Exception("Content not allowed");
      }

      await _unitOfWork.PostRepository.AddAsync(post);
    }

    public async Task<bool> UpdatePostAsync(Post post)
    {
      await _unitOfWork.PostRepository.UpdateAsync(post);
      return true;
    }

    public async Task<bool> DeletePostAsync(int postId)
    {
      await _unitOfWork.PostRepository.DeleteAsync(postId);
      return true;
    }
  }
}