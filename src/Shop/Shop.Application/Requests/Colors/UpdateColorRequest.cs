using MediatR;
using Shop.Domain.Results;
using System.Text.Json.Serialization;

namespace Shop.Application.Requests.Colors
{
    public class UpdateColorRequest : IRequest<CommandResult>
    {
        [JsonIgnore]
        public int ColorId { get; set; }
        public string? Name { get; set; }
    }
}
