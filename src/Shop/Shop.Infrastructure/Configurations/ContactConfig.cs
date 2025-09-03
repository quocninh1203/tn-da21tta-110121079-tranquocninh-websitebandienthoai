using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Configurations
{
    public class ContactConfig : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("contact_id");
            builder.Property(x => x.Reason).HasColumnName("reason");
            builder.Property(x => x.OrderId).HasColumnName("order_id");
            builder.Property(x => x.UserId).HasColumnName("user_id");

            builder.ToTable("Contacts");
        }
    }
}
