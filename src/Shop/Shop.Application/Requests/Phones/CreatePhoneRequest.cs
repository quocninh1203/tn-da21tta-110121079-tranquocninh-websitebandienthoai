using MediatR;
using Shop.Domain.Results;
using System.Text.Json.Serialization;

namespace Shop.Application.Requests.Phones
{
    public class CreatePhoneRequest : IRequest<CommandResult>
    {
        public string Name { get; set; }
        public int BrandId { get; set; }
        public string ImageBase64 { get; set; }
        public string Screen { get; set; }
        public string Chip { get; set; }
        public string Battery { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public string? Slug { get; set; }
        [JsonIgnore]
        public bool IsActive { get; set; } = false;
    }
}
