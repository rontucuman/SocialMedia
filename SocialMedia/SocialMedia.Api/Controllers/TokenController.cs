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
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Options;
using SocialMedia.Infrastructure.Interfaces;

namespace SocialMedia.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TokenController : ControllerBase
  {
    private readonly AuthenticationOptions _authenticationOptions;
    private readonly ISecurityService _securityService;
    private readonly IPasswordService _passwordService;

    public TokenController(IOptions<AuthenticationOptions> authenticationOptions, ISecurityService securityService,
      IPasswordService passwordService)
    {
      _authenticationOptions = authenticationOptions.Value;
      _securityService = securityService;
      _passwordService = passwordService;
    }

    [HttpPost]
    public async Task<IActionResult> Authentication(UserLoginDto userLogin)
    {
      // if it is a valid user
      (bool, Security) validation = await IsValidUser(userLogin);

      if (validation.Item1)
      {
        string token = GenerateToken(validation.Item2);
        return Ok(new {token});
      }

      return NotFound();
    }

    private string GenerateToken(Security security)
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
        new Claim(ClaimTypes.Name, security.UserName),
        new Claim("User", security.User),
        new Claim(ClaimTypes.Role, security.Role.ToString())
      };

      // Payload
      JwtPayload payload = new JwtPayload(_authenticationOptions.Issuer, _authenticationOptions.Audience, claims,
        DateTime.Now,
        DateTime.Now.AddMinutes(10));

      JwtSecurityToken token = new JwtSecurityToken(header, payload);

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<(bool, Security)> IsValidUser(UserLoginDto userLogin)
    {
      Security user = await _securityService.GetLoginByCredentials(userLogin);
      var isValid = _passwordService.Check(user.Password, userLogin.Password);
      return (isValid, user);
    }
  }
}
