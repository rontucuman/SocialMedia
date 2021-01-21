using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Options;
using SocialMedia.Core.Services;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Interfaces;
using SocialMedia.Infrastructure.Options;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Infrastructure.Services;

namespace SocialMedia.Infrastructure.Extensions
{
  public static class ServiceCollectionExtension
  {
    public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<SocialMediaContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("SocialMedia")));

      return services;
    }

    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
      services.Configure<PaginationOptions>(options => configuration.GetSection("Pagination").Bind(options));
      services.Configure<AuthenticationOptions>(options => configuration.GetSection("Authentication").Bind(options));
      services.Configure<PasswordOptions>(options => configuration.GetSection("Password").Bind(options));

      return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
      services.AddTransient<IPostService, PostService>();
      services.AddTransient<ISecurityService, SecurityService>();
      services.AddTransient<IUnitOfWork, UnitOfWork>();
      services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
      services.AddSingleton<IPasswordService, PasswordService>();
      services.AddSingleton<IUriService>(provider =>
      {
        IHttpContextAccessor accessor = provider.GetRequiredService<IHttpContextAccessor>();
        HttpRequest request = accessor.HttpContext.Request;
        string absoluteUri = $"{request.Scheme}://{request.Host.ToUriComponent()}";
        return new UriService(absoluteUri);
      });

      return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
      services.AddSwaggerGen(options =>
      {
        options.SwaggerDoc("v1", new OpenApiInfo {Title = "Social Media API", Version = "v1"});

        DirectoryInfo baseDirectory = new DirectoryInfo(AppContext.BaseDirectory);

        foreach (FileInfo fileInfo in baseDirectory.EnumerateFiles("*.xml"))
        {
          options.IncludeXmlComments(fileInfo.FullName);
        }
      });

      return services;
    }
  }
}