using MediatR;
using Shop.Application.DTOs.Users;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Users
{
    public class LoginRequest : IRequest<QueryResult<AuthDTO>>
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
}
