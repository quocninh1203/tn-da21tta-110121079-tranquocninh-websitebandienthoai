using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Users
{
    public class LogoutRequest : IRequest<CommandResult>
    {
        public int UserId { get; set; }
    }
}
