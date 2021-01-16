using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Options;
using SocialMedia.Core.QueryFilters;

namespace SocialMedia.Core.Services
{
  public class PostService : IPostService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly PaginationOptions _paginationOptions;

    public PostService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions)
    {
      _unitOfWork = unitOfWork;
      _paginationOptions = paginationOptions.Value;
    }

    public PagedList<Post> GetPosts(PostQueryFilter postQueryFilter)
    {
      var posts = _unitOfWork.PostRepository.GetAll();

      postQueryFilter.PageNumber = postQueryFilter.PageNumber == 0
        ? _paginationOptions.DefaultPageNumber
        : postQueryFilter.PageNumber;
      postQueryFilter.PageSize = postQueryFilter.PageSize == 0
        ? _paginationOptions.DefaultPageSize
        : postQueryFilter.PageSize;

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

      PagedList<Post> pagedPosts = PagedList<Post>.Create(posts, postQueryFilter.PageNumber, postQueryFilter.PageSize);

      return pagedPosts;
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