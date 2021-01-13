using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Repositories;

namespace SocialMedia.Api.Controllers
{
  [Route( "api/[controller]" )]
  [ApiController]
  public class PostController : ControllerBase
  {
    private readonly IPostRepository _postRepository;

    public PostController(IPostRepository postRepository)
    {
      _postRepository = postRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetPostsAsync()
    {
      var posts = await _postRepository.GetPostsAsync();
      return Ok(posts);
    }

    [HttpGet("{postId}")]
    public async Task<IActionResult> GetPostAsync(int postId)
    {
      var post = await _postRepository.GetPostAsync(postId);
      return Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> InsertPostAsync(Post post)
    {
      await _postRepository.InsertPostAsync(post);
      return Ok(post);
    }
  }
}
