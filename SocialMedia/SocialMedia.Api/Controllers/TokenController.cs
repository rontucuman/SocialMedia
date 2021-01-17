using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Options;

namespace SocialMedia.Api.Controllers
{
  [Route( "api/[controller]" )]
  [ApiController]
  public class TokenController : ControllerBase
  {
    private readonly AuthenticationOptions _authenticationOptions;

    public TokenController(IOptions<AuthenticationOptions> authenticationOptions)
    {
      _authenticationOptions = authenticationOptions.Value;
    }

    [HttpPost]
    public IActionResult Authentication(UserLoginDto userLogin)
    {
      // if it is a valid user
      if (IsValidUser(userLogin))
      {
        string token = GenerateToken();
        return Ok(new {token});
      }

      return NotFound();
    }

    private string GenerateToken()
    {
      // Header
      SymmetricSecurityKey symmetricSecurityKey =
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationOptions.SecretKey));
      SigningCredentials signingCredentials =
        new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

      JwtHeader header = new JwtHeader(signingCredentials);

      // Claims
      Claim[] claims = new[]
      {
        new Claim(ClaimTypes.Name, "Ronald Tucuman"),
        new Claim(ClaimTypes.Email, "ronald.tucuman@outlook.com"),
        new Claim(ClaimTypes.Role, "Administrator")
      };

      // Payload
      JwtPayload payload = new JwtPayload(_authenticationOptions.Issuer, _authenticationOptions.Audience, claims, DateTime.Now,
        DateTime.Now.AddMinutes(1));

      JwtSecurityToken token = new JwtSecurityToken(header, payload);

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private bool IsValidUser(UserLoginDto userLogin)
    {
      return true;
    }
  }
}
