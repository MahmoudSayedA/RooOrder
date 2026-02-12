using Domain.Entities.Restaurants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
internal class MenuItemConfigurations : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(m => m.Description) 
            .HasMaxLength(1000); 
        builder.Property(m => m.Price) 
            .HasColumnType("decimal(18,2)");

        builder.HasOne(m => m.Restaurant)
            .WithMany(r => r.MenuItems)
            .HasForeignKey(m => m.RestaurantId) 
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.Category)
            .WithMany()
            .HasForeignKey(m => m.CategoryId) 
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(m => m.CategoryId);
        builder.HasIndex(m => m.Name)
            .IncludeProperties(m => m.Price);
        builder.HasIndex(m => new { m.RestaurantId, m.IsAvailable, m.IsHidden });


    }
}
