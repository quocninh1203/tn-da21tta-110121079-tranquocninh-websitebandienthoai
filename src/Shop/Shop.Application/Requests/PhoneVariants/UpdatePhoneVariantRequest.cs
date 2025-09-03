using MediatR;
using Shop.Domain.Results;
using System.Text.Json.Serialization;

namespace Shop.Application.Requests.PhoneVariants
{
    public class UpdatePhoneVariantRequest : IRequest<CommandResult>
    {
        [JsonIgnore]
        public int VariantId { get; set; }
        public int? PhoneId { get; set; }
        public int? ColorId { get; set; }
        public int? RamId { get; set; } 
        public int? StorageId { get; set; }
        public int? Price { get; set; }
        public int? StockQuantity { get; set; }
    }
}
