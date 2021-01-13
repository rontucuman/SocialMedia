using System;
using FluentValidation;
using SocialMedia.Core.DTOs;

namespace SocialMedia.Infrastructure.Validators
{
  public class PostValidator : AbstractValidator<PostDto>
  {
    public PostValidator()
    {
      RuleFor(post => post.Description)
        .NotEmpty()
        .Length(5, 10);

      RuleFor(post => post.Date)
        .NotEmpty()
        .LessThan(DateTime.Now);
    }
  }
}