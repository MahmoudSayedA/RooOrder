using Domain.Entities.Restaurants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
internal class RestaurantConfigurations : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(r => r.Description)
            .HasMaxLength(1000);

        builder.HasOne(r => r.Owner)
            .WithOne(o => o.Restaurant)
            .HasPrincipalKey<Restaurant>(r => r.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => r.Name);
        builder.HasIndex(r => r.OwnerId);
        builder.HasIndex(r => r.IsActive);
        builder.HasIndex(r => new {r.City, r.Region});
    }
}
