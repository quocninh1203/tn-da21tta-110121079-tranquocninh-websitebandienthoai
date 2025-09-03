using MediatR;
using Shop.Domain.Results;
using System.Text.Json.Serialization;

namespace Shop.Application.Requests.Users
{
    public class RegisterRequest : IRequest<CommandResult>
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        [JsonIgnore]
        public string? ImageBase64 { get; set; }
        [JsonIgnore]
        public bool IsVerify { get; set; } = false;
        [JsonIgnore]
        public int Role { get; set; } = 1;
        [JsonIgnore]
        public string? RefreshToken { get; set; }
    }
}
