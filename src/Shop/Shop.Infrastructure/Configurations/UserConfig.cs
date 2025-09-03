using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("user_id");
            builder.Property(x => x.FullName).HasColumnName("full_name");
            builder.Property(x => x.UserName).HasColumnName("user_name");
            builder.Property(x => x.PassWord).HasColumnName("pass_word");
            builder.Property(x => x.CreatedAt).HasColumnName("create_date");
            builder.Property(x => x.Email).HasColumnName("email");
            builder.Property(x => x.PhoneNumber).HasColumnName("user_phone_number");
            builder.Property(x => x.Address).HasColumnName("user_address");
            builder.Property(x => x.ImageUrl).HasColumnName("user_image_url");
            builder.Property(x => x.IsVerify).HasColumnName("is_verify");
            builder.Property(x => x.Role).HasColumnName("user_role");
            builder.Property(x => x.RefreshToken).HasColumnName("refresh_token");

            builder.ToTable("Users");
        }
    }
}
