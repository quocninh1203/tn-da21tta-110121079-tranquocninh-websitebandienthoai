using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Configurations
{
    public class PhoneConfig : IEntityTypeConfiguration<Phone>
    {
        public void Configure(EntityTypeBuilder<Phone> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("phone_id");
            builder.Property(x => x.Name).HasColumnName("phone_name");
            builder.Property(x => x.BrandId).HasColumnName("brand_id");
            builder.Property(x => x.ImageUrl).HasColumnName("phone_image_url");
            builder.Property(x => x.Screen).HasColumnName("phone_screen");
            builder.Property(x => x.Chip).HasColumnName("phone_chip");
            builder.Property(x => x.Battery).HasColumnName("phone_battery");
            builder.Property(x => x.Description).HasColumnName("description");
            builder.Property(x => x.Slug).HasColumnName("phone_slug");
            builder.Property(x => x.IsActive).HasColumnName("is_active");

            builder.ToTable("Phones");

            builder.HasMany(p => p.PhoneVariants)
               .WithOne(v => v.Phone)
               .HasForeignKey(v => v.PhoneId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.PhoneImages)
                .WithOne(v => v.Phone)
                .HasForeignKey(v => v.PhoneId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
