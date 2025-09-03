using MediatR;
using Shop.Domain.Entities;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Colors
{
    public class GetAllColorRequest : IRequest<QueryResult<List<Color>>>
    {
    }
}
