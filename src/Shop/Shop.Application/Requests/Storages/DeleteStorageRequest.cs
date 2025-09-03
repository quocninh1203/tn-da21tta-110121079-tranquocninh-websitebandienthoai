using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Storages
{
    public class DeleteStorageRequest : IRequest<CommandResult>
    {
        public int StorageId { get; set; }
    }
}
