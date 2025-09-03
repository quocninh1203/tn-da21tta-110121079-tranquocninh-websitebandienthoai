using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Configurations
{
    public class ReviewConfig : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("review_id");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.PhoneId).HasColumnName("phone_id");
            builder.Property(x => x.VariantId).HasColumnName("var_id");
            builder.Property(x => x.Rating).HasColumnName("rating");
            builder.Property(x => x.Text).HasColumnName("review_text");
            builder.Property(x => x.CreateDate).HasColumnName("review_date");
            builder.Property(x => x.ImageUrl).HasColumnName("review_image_url");

            builder.ToTable("Reviews");
        }
    }
}
