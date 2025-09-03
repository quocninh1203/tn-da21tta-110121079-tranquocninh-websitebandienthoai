using MediatR;
using Shop.Domain.Results;
using System.Text.Json.Serialization;

namespace Shop.Application.Requests.Orders
{
    public class ChangeStatusRequest : IRequest<CommandResult>
    {
        [JsonIgnore]
        public int OrderId { get; set; }
        public int Status { get; set; }
    }
}
