using MediatR;
using Shop.Domain.Results;
using System.Text.Json.Serialization;

namespace Shop.Application.Requests.Users
{
    public class ForgotPassWordRequest : IRequest<CommandResult>
    {
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
