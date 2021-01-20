using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using SocialMedia.Infrastructure.Interfaces;
using SocialMedia.Infrastructure.Options;

namespace SocialMedia.Infrastructure.Services
{
  public class PasswordService : IPasswordService
  {
    private readonly PasswordOptions _passwordOptions;

    public PasswordService(IOptions<PasswordOptions> passwordOptions)
    {
      _passwordOptions = passwordOptions.Value;
    }

    public string Hash(string password)
    {
      using (Rfc2898DeriveBytes algorithm = new Rfc2898DeriveBytes(password, _passwordOptions.SaltSize,
        _passwordOptions.Iterations, HashAlgorithmName.SHA512))
      {
        string key = Convert.ToBase64String(algorithm.GetBytes(_passwordOptions.KeySize));
        string salt = Convert.ToBase64String(algorithm.Salt);

        return $"{_passwordOptions.Iterations}.{salt}.{key}";
      }
    }

    public bool Check(string hash, string password)
    {
      string[] parts = hash.Split('.');

      if (parts.Length != 3)
      {
        throw new FormatException("Unexpected hash format");
      }

      int iterations = Convert.ToInt32(parts[0]);
      byte[] salt = Convert.FromBase64String(parts[1]);
      byte[] key = Convert.FromBase64String(parts[2]);

      using var algorithm = new Rfc2898DeriveBytes(password, salt, iterations);
      byte[] keyToCheck = algorithm.GetBytes(_passwordOptions.KeySize);
      return keyToCheck.SequenceEqual(key);
    }
  }
}