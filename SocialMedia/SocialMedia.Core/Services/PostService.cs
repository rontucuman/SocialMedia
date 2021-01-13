using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Core.Services
{
  public class PostService : IPostService
  {
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public PostService(IPostRepository postRepository, IUserRepository userRepository)
    {
      _postRepository = postRepository;
      _userRepository = userRepository;
    }

    public async Task<IEnumerable<Post>> GetPostsAsync()
    {
      return await _postRepository.GetPostsAsync();
    }

    public async Task<Post> GetPostAsync(int postId)
    {
      return await _postRepository.GetPostAsync(postId);
    }

    public async Task InsertPostAsync(Post post)
    {
      User user = await _userRepository.GetUserAsync(post.UserId);

      if (user == null)
      {
        throw new Exception("User does not exist");
      }

      if (post.Description.Contains("sex", StringComparison.InvariantCultureIgnoreCase))
      {
        throw new Exception("Content not allowed");
      }

      await _postRepository.InsertPostAsync(post);
    }

    public async Task<bool> UpdatePostAsync(Post post)
    {
      return await _postRepository.UpdatePostAsync(post);
    }

    public async Task<bool> DeletePostAsync(int postId)
    {
      return await _postRepository.DeletePostAsync(postId);
    }
  }
}