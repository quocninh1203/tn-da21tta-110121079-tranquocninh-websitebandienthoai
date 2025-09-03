using MediatR;
using Shop.Domain.Entities;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Storages
{
    public class GetAllStorageRequest : IRequest<QueryResult<List<Storage>>>
    {
    }
}
