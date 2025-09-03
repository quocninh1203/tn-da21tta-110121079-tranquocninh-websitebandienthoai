using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Configurations
{
    public class OrderDetailConfig : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("order_detail_id");
            builder.Property(x => x.OrderId).HasColumnName("order_id");
            builder.Property(x => x.VariantId).HasColumnName("var_id");
            builder.Property(x => x.Quantity).HasColumnName("quantity");
            builder.Property(x => x.PriceAtOrder).HasColumnName("price_at_order");
            builder.Property(x => x.IsReview).HasColumnName("is_review");

            builder.ToTable("OrderDetails");

            builder.HasOne(pv => pv.Order)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(pv => pv.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
