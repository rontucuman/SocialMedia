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
        .NotEmpty().WithMessage("Descripcion no puede ser vacia.")
        .Length(10, 1000);

      RuleFor(post => post.Date)
        .NotEmpty()
        .LessThan(DateTime.Now);
    }
  }
}