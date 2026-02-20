using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations;
internal class UserPreferenceConfigurations : IEntityTypeConfiguration<UserPreference>
{
    public void Configure(EntityTypeBuilder<UserPreference> builder)
    {
        builder.HasKey(p => new { p.UserId, p.Key });

        builder.HasOne(p => p.User)
            .WithMany(u => u.Preferences)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
