using Domain.Constants;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations;
internal class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasDiscriminator(u => u.MainRole)
            .HasValue<Admin>(Roles.Admin)
            .HasValue<Customer>(Roles.Customer)
            .HasValue<RestaurantOwner>(Roles.RestaurantOwner);

        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.UserName).IsUnique();
        builder.HasIndex(u => new { u.MainRole, u.IsDeleted })
            .IncludeProperties(u => new {u.Id, u.Email, u.UserName });
    }

}
