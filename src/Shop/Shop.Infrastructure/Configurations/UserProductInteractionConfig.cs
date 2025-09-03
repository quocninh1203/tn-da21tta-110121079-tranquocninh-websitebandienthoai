using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Configurations
{
    public class UserProductInteractionConfig : IEntityTypeConfiguration<UserProductInteraction>
    {
        public void Configure(EntityTypeBuilder<UserProductInteraction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("u_pro_inter_id");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.ProductId).HasColumnName("product_id");
            builder.Property(x => x.Label).HasColumnName("label");

            builder.ToTable("UserProductInteractions");
        }
    }
}
