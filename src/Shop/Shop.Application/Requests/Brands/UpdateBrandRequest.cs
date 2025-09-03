using MediatR;
using Shop.Domain.Results;
using System.Text.Json.Serialization;

namespace Shop.Application.Requests.Brands
{
    public class UpdateBrandRequest : IRequest<CommandResult>
    {
        [JsonIgnore]
        public int BrandId { get; set; }
        public string? Name { get; set; }
        [JsonIgnore]
        public string? Slug { get; set; }
        public string? ImageBase64 { get; set; }
    }
}
