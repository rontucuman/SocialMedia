using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using SocialMedia.Api.Responses;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastructure.Interfaces;
using SocialMedia.Infrastructure.Repositories;

namespace SocialMedia.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PostController : ControllerBase
  {
    private readonly IPostService _postService;
    private readonly IMapper _mapper;
    private readonly IUriService _uriService;

    public PostController(IPostService postService, IMapper mapper, IUriService uriService)
    {
      _postService = postService;
      _mapper = mapper;
      _uriService = uriService;
    }

    [HttpGet(Name = nameof(GetPosts))]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PostDto>>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<IEnumerable<PostDto>>))]
    public IActionResult GetPosts([FromQuery]PostQueryFilter filters)
    {
      PagedList<Post> posts = _postService.GetPosts(filters);
      IEnumerable<PostDto> postDtos = _mapper.Map<IEnumerable<PostDto>>(posts);

      Metadata metadata = new Metadata
      {
        PageSize = posts.PageSize,
        CurrentPage = posts.CurrentPage,
        TotalCount = posts.TotalCount,
        TotalPages = posts.TotalPages,
        HasNextPage = posts.HasNextPage,
        HasPreviousPage = posts.HasPreviousPage,
        NextPageUrl = _uriService.GetPostPaginationUri(filters, Url.RouteUrl(nameof(GetPosts))).ToString(),
        PreviousPageUrl = _uriService.GetPostPaginationUri(filters, Url.RouteUrl(nameof(GetPosts))).ToString()
      };

      ApiResponse<IEnumerable<PostDto>> response = new ApiResponse<IEnumerable<PostDto>>(postDtos)
      {
        Meta = metadata
      };

      Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

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
