using MediatR;
using Shop.Application.DTOs.Users;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Users
{
    public class GetAllUserRequest : IRequest<QueryResult<List<UserDTO>>>
    {
    }
}
