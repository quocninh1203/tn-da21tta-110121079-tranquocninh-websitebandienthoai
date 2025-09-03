using MediatR;
using Shop.Domain.Results;
using System.Text.Json.Serialization;

namespace Shop.Application.Requests.Users
{
    public class UpdateProfileRequest : IRequest<CommandResult>
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public string? FullName { get; set; }
        [JsonIgnore]
        public string? UserName { get; set; }
        [JsonIgnore]
        public string? PassWord { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? ImageBase64 { get; set; }
        [JsonIgnore]
        public bool IsVerify { get; set; }
        [JsonIgnore]
        public int Role { get; set; }
        [JsonIgnore]
        public string? RefreshToken { get; set; }
    }
}
