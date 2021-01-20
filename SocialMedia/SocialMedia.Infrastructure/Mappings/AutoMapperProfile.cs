using AutoMapper;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;

namespace SocialMedia.Infrastructure.Mappings
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      CreateMap<PostDto, Post>();
      CreateMap<Post, PostDto>();

      CreateMap<SecurityDto, Security>().ReverseMap();
    }
  }
}