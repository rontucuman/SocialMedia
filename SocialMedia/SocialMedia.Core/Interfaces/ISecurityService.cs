using System.Threading.Tasks;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Interfaces
{
  public interface ISecurityService
  {
    Task<Security> GetLoginByCredentials( UserLoginDto userLogin );
    Task RegisterUser( Security security );
  }
}