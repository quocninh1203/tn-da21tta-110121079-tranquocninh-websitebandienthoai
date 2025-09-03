using MediatR;
using Shop.Domain.Results;
using System.Text.Json.Serialization;

namespace Shop.Application.Requests.Storages
{
    public class UpdateStorageRequest : IRequest<CommandResult>
    {
        [JsonIgnore]
        public int StorageId { get; set; }
        public string? Size { get; set; }
    }
}
