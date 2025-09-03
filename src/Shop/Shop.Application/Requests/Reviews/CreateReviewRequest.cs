using MediatR;
using Shop.Domain.Results;
using System.Text.Json.Serialization;

namespace Shop.Application.Requests.Reviews
{
    public class CreateReviewRequest : IRequest<CommandResult>  
    {
        public int UserId { get; set; }
        public int PhoneId { get; set; }
        public int VariantId { get; set; }
        public int OrderDetailId { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; } 
        [JsonIgnore]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string? ImageBase64 { get; set; }
    }
}
