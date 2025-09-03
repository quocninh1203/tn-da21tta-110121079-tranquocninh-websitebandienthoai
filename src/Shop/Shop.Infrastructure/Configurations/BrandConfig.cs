using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Configurations
{
    public class BrandConfig : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("brand_id");
            builder.Property(x => x.Name).HasColumnName("brand_name");
            builder.Property(x => x.Slug).HasColumnName("brand_slug");
            builder.Property(x => x.ImageUrl).HasColumnName("brand_image_url");

            builder.ToTable("Brands");
        }
    }
}
