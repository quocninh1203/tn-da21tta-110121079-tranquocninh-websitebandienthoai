using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Configurations
{
    public class PhoneImageConfig : IEntityTypeConfiguration<PhoneImage>
    {
        public void Configure(EntityTypeBuilder<PhoneImage> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("img_id");
            builder.Property(x => x.PhoneId).HasColumnName("phone_id");
            builder.Property(x => x.ImageUrl).HasColumnName("img_image_url");

            builder.ToTable("PhoneImages");

            builder.HasOne(pv => pv.Phone)
                .WithMany(p => p.PhoneImages)
                .HasForeignKey(pv => pv.PhoneId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
