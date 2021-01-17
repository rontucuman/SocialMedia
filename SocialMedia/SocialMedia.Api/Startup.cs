using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Options;
using SocialMedia.Core.Services;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Filters;
using SocialMedia.Infrastructure.Interfaces;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Infrastructure.Services;

namespace SocialMedia.Api
{
  public class Startup
  {
    public Startup( IConfiguration configuration )
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices( IServiceCollection services )
    {
      services.AddControllers(options =>
        {
          options.Filters.Add<GlobalExceptionFilter>();
        })
        .AddNewtonsoftJson(options =>
        {
          options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
          options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        });

      services.Configure<PaginationOptions>(Configuration.GetSection("Pagination"));
      services.Configure<AuthenticationOptions>(Configuration.GetSection("Authentication"));

      services.AddDbContext<SocialMediaContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("SocialMedia")));
      
      services.AddTransient<IPostService, PostService>();
      services.AddTransient<IUnitOfWork, UnitOfWork>();
      services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
      services.AddSingleton<IUriService>(provider =>
      { 
        IHttpContextAccessor accessor = provider.GetRequiredService<IHttpContextAccessor>();
        HttpRequest request = accessor.HttpContext.Request;
        string absoluteUri = $"{request.Scheme}://{request.Host.ToUriComponent()}";
        return new UriService(absoluteUri);
      });
      
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
      services.AddSwaggerGen(options =>
      {
        options.SwaggerDoc("v1", new OpenApiInfo {Title = "Social Media API", Version = "v1"});
        
        DirectoryInfo baseDirectory = new DirectoryInfo(AppContext.BaseDirectory);

        foreach (FileInfo fileInfo in baseDirectory.EnumerateFiles("*.xml"))
        {
          options.IncludeXmlComments(fileInfo.FullName);
        }
      });

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = Configuration["Authentication:Issuer"],
          ValidAudience = Configuration.GetSection("Authentication").GetSection("Audience").Value,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]))
        };
      });

      services.AddMvc().AddFluentValidation(options =>
      {
        options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
    {
      if ( env.IsDevelopment() )
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseSwagger();
      app.UseSwaggerUI(options =>
      {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Social Media API v1");
        options.RoutePrefix = string.Empty;
      });

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();
      
      app.UseEndpoints( endpoints =>
       {
         endpoints.MapControllers();
       } );
    }
  }
}
