using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SocialMedia.Api.Responses;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Repositories;

namespace SocialMedia.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PostController : ControllerBase
  {
    private readonly IPostService _postService;
    private readonly IMapper _mapper;

    public PostController(IPostService postService, IMapper mapper)
    {
      _postService = postService;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetPostsAsync()
    {
      IEnumerable<Post> posts = await _postService.GetPostsAsync();
      IEnumerable<PostDto> postDtos = _mapper.Map<IEnumerable<PostDto>>(posts);
      ApiResponse<IEnumerable<PostDto>> response = new ApiResponse<IEnumerable<PostDto>>(postDtos);

      return Ok(response);
    }

    [HttpGet("{postId}")]
    public async Task<IActionResult> GetPostAsync(int postId)
    {
      Post post = await _postService.GetPostAsync(postId);
      PostDto postDto = _mapper.Map<PostDto>(post);
      ApiResponse<PostDto> response = new ApiResponse<PostDto>(postDto);
      
      return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> InsertPostAsync(PostDto postDto)
    {
      Post post = _mapper.Map<Post>(postDto);
      await _postService.InsertPostAsync(post);
      PostDto postResultDto = _mapper.Map<PostDto>(post);
      ApiResponse<PostDto> response = new ApiResponse<PostDto>(postResultDto);

      return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> PutPostAsync(int postId, PostDto postDto)
    {
      Post post = _mapper.Map<Post>(postDto);
      post.Id = postId;
      bool result = await _postService.UpdatePostAsync(post);
      ApiResponse<bool> response = new ApiResponse<bool>(result);

      return Ok(response);
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> DeletePostAsync(int postId)
    {
      bool result = await _postService.DeletePostAsync(postId);
      ApiResponse<bool> response = new ApiResponse<bool>(result);

      return Ok(response);
    }
  }
}
