using MediatR;
using Shop.Domain.Results;
using System.Text.Json.Serialization;

namespace Shop.Application.Requests.Rams
{
    public class UpdateRamRequest : IRequest<CommandResult>
    {
        [JsonIgnore]
        public int RamId { get; set; }
        public string? Size { get; set; }
    }
}
