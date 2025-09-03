using MediatR;
using Shop.Domain.Results;
using System.Text.Json.Serialization;

namespace Shop.Application.Requests.OrderDetails
{
    public class CreateOrderDetailRequest : IRequest<CommandResult>
    {
        public int OrderId { get; set; }
        public int VariantId { get; set; }
        public int Quantity { get; set; }
        [JsonIgnore]
        public int? PriceAtOrder { get; set; }
        [JsonIgnore]
        public int? TotalPrice { get; set; }
        [JsonIgnore]
        public bool IsReview {  get; set; } = false;
    }
}
