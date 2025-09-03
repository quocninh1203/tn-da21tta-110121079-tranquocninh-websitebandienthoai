using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Shop.Domain.Entities
{
    public class OTP : BaseEntities<int>
    {
        public string Email {  get; set; }
        public string OtpCode { get; set; }
        public DateTime OtpExpired {  get; set; }
    }
}
