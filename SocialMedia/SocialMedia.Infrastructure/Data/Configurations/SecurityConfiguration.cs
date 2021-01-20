﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Enumerations;

namespace SocialMedia.Infrastructure.Data.Configurations
{
  public class SecurityConfiguration : IEntityTypeConfiguration<Security>
  {
    public void Configure(EntityTypeBuilder<Security> builder)
    {
      builder.HasKey(e => e.Id);

      builder.ToTable("Seguridad");

      builder.Property(e => e.Id)
        .HasColumnName("IdSeguridad");

      builder.Property(e => e.User)
        .HasColumnName("Usuario")
        .IsRequired()
        .HasMaxLength(50)
        .IsUnicode(false);

      builder.Property(e => e.UserName)
        .HasColumnName("NombreUsuario")
        .IsRequired()
        .HasMaxLength(100)
        .IsUnicode(false);

      builder.Property(e => e.Password)
        .HasColumnName("Contrasena")
        .HasMaxLength(200)
        .IsUnicode(false);

      builder.Property(e => e.Role)
        .HasColumnName("Rol")
        .IsRequired()
        .HasMaxLength(15)
        .IsUnicode(false)
        .HasConversion(
          x => x.ToString(),
          x => (RoleType) Enum.Parse(typeof(RoleType), x)
        );
    }
  }
}