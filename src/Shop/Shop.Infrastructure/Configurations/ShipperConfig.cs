using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Configurations
{
    public class ShipperConfig : IEntityTypeConfiguration<Shipper>
    {
        public void Configure(EntityTypeBuilder<Shipper> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("ship_id");
            builder.Property(x => x.Name).HasColumnName("ship_name");
            builder.Property(x => x.Cost).HasColumnName("ship_cost");

            builder.ToTable("Shippers");
        }
    }
}
