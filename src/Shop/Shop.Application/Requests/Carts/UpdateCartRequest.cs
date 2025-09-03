using MediatR;
using Shop.Domain.Results;
using System.Text.Json.Serialization;

namespace Shop.Application.Requests.Carts
{
    public class UpdateCartRequest : IRequest<CommandResult>
    {
        [JsonIgnore]
        public int CartId { get; set; }
        [JsonIgnore]
        public int? UserId { get; set; }
        [JsonIgnore]
        public int? VariantId { get; set; }
        public int? Quantity { get; set; }
        [JsonIgnore]
        public DateTime AddedDate { get; set; }
    }
}
