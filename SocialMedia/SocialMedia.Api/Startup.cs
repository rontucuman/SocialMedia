using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SocialMedia.Infrastructure.Extensions;
using SocialMedia.Infrastructure.Filters;

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

      services.AddOptions(Configuration);
      services.AddDbContexts(Configuration);
      services.AddServices();
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
      services.AddSwagger();

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
        options.SwaggerEndpoint("../swagger/v1/swagger.json", "Social Media API v1");
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
