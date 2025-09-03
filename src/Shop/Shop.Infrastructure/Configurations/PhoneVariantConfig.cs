using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Configurations
{
    public class PhoneVariantConfig : IEntityTypeConfiguration<PhoneVariant>
    {
        public void Configure(EntityTypeBuilder<PhoneVariant> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("var_id");
            builder.Property(x => x.PhoneId).HasColumnName("phone_id");
            builder.Property(x => x.ColorId).HasColumnName("color_id");
            builder.Property(x => x.RamId).HasColumnName("ram_id");
            builder.Property(x => x.StorageId).HasColumnName("storage_id");
            builder.Property(x => x.Price).HasColumnName("var_price");
            builder.Property(x => x.StockQuantity).HasColumnName("stock_quantity");

            builder.ToTable("PhoneVariants");

            builder.HasOne(pv => pv.Phone)
                   .WithMany(p => p.PhoneVariants)
                   .HasForeignKey(pv => pv.PhoneId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pv => pv.Ram)
                   .WithMany()
                   .HasForeignKey(pv => pv.RamId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pv => pv.Storage)
                   .WithMany()
                   .HasForeignKey(pv => pv.StorageId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pv => pv.Color)
                   .WithMany()
                   .HasForeignKey(pv => pv.ColorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Carts)
                .WithOne(v => v.PhoneVariant)
                .HasForeignKey(v => v.VariantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
