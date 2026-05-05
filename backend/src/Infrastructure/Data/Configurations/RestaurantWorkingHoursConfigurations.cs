using Domain.Entities.Restaurants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
internal class RestaurantWorkingHoursConfigurations : IEntityTypeConfiguration<RestaurantWorkingHours>
{
    public void Configure(EntityTypeBuilder<RestaurantWorkingHours> builder)
    {
        builder.HasOne(w => w.Restaurant)
            .WithMany(r => r.WorkingHours)
            .HasForeignKey(w => w.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(w => w.RestaurantId)
            .IncludeProperties(w => new { w.DayOfWeek, w.OpeningTime, w.ClosingTime, w.IsClosed })
            .HasDatabaseName("IX_RestaurantWorkingHours_RestaurantId_DayOfWeek_OpeningTime_ClosingTime");

        builder.Property(w => w.DayOfWeek)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(w => w.OpeningTime)
            .IsRequired();

        builder.Property(w => w.ClosingTime)
            .IsRequired();

        builder.Property(w => w.IsClosed)
            .IsRequired();
    }
}
