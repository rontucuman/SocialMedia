using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SocialMedia.Core.DTOs;
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
    private readonly IMapper _mapper;

    public PostController(IPostRepository postRepository, IMapper mapper)
    {
      _postRepository = postRepository;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetPostsAsync()
    {
      IEnumerable<Post> posts = await _postRepository.GetPostsAsync();
      IEnumerable<PostDto> postDtos = _mapper.Map<IEnumerable<PostDto>>(posts);

      return Ok(postDtos);
    }

    [HttpGet("{postId}")]
    public async Task<IActionResult> GetPostAsync(int postId)
    {
      Post post = await _postRepository.GetPostAsync(postId);
      PostDto postDto = _mapper.Map<PostDto>(post);
      
      return Ok(postDto);
    }

    [HttpPost]
    public async Task<IActionResult> InsertPostAsync(PostDto postDto)
    {
      Post post = _mapper.Map<Post>(postDto);
      await _postRepository.InsertPostAsync(post);
      PostDto postResultDto = _mapper.Map<PostDto>(post);
      
      return Ok(postResultDto);
    }
  }
}
