using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Configurations
{
    public class CartConfig : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("cart_id");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.VariantId).HasColumnName("var_id");
            builder.Property(x => x.Quantity).HasColumnName("quantity");
            builder.Property(x => x.AddedDate).HasColumnName("added_date");

            builder.ToTable("Carts");

            builder.HasOne(pv => pv.PhoneVariant)
                .WithMany(p => p.Carts)
                .HasForeignKey(pv => pv.VariantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
