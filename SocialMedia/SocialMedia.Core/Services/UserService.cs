using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Core.Services
{
  public class UserService : IUserService
  {
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }
    public async Task<User> GetUserAsync(int userId)
    {
      return await _userRepository.GetUserAsync(userId);
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
      return await _userRepository.GetUsersAsync();
    }
  }
}