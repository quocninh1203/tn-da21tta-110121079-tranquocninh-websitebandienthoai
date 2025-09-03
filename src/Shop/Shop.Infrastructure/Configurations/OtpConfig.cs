using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Configurations
{
    public class OtpConfig : IEntityTypeConfiguration<OTP>
    {
        public void Configure(EntityTypeBuilder<OTP> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("otp_id");
            builder.Property(x => x.Email).HasColumnName("email");
            builder.Property(x => x.OtpCode).HasColumnName("otp_code");
            builder.Property(x => x.OtpExpired).HasColumnName("otp_expired");

            builder.ToTable("OTP");
        }
    }
}
