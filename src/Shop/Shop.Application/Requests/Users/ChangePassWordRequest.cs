using MediatR;
using Shop.Domain.Results;
using System.Text.Json.Serialization;

namespace Shop.Application.Requests.Users
{
    public class ChangePassWordRequest : IRequest<CommandResult>
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public string? PassWord { get; set; }
        public string? NewPassWord { get; set; }
    }
}
