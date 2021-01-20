using System.Threading.Tasks;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Interfaces
{
  public interface ISecurityRepository : IRepository<Security>
  {
    Task<Security> GetLoginByCredentials( UserLoginDto userLogin );
  }
}