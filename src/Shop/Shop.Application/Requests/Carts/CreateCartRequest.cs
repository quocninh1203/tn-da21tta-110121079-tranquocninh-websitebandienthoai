using MediatR;
using Shop.Domain.Results;
using System.Text.Json.Serialization;

namespace Shop.Application.Requests.Carts
{
    public class CreateCartRequest : IRequest<CommandResult>
    {
        public int UserId { get; set; }
        public int VariantId { get; set; }
        public int Quantity { get; set; }
        [JsonIgnore]
        public DateTime AddedDate { get; set; } = DateTime.Now;
    }
}
