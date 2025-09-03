using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("order_id");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.OrderDate).HasColumnName("order_date");
            builder.Property(x => x.Status).HasColumnName("status");
            builder.Property(x => x.MethodId).HasColumnName("method_id");
            builder.Property(x => x.ShipperId).HasColumnName("shipper_id");
            builder.Property(x => x.ShippingAddress).HasColumnName("shipping_address");
            builder.Property(x => x.OrderCode).HasColumnName("order_code");
            builder.Property(x => x.IsPrepaid).HasColumnName("is_prepaid");
            builder.Property(x => x.TotalPrice).HasColumnName("total_price");

            builder.ToTable("Orders");

            builder.HasMany(p => p.OrderDetails)
                .WithOne(v => v.Order)
                .HasForeignKey(v => v.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
