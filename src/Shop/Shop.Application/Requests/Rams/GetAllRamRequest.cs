using MediatR;
using Shop.Domain.Entities;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Rams
{
    public class GetAllRamRequest : IRequest<QueryResult<List<Ram>>>
    {
    }
}
