using System.Text.Json.Serialization;

namespace Shop.Domain.Entities
{
    public class Phone : BaseEntities<int>
    {
        public string? Name { get; set; }
        public int BrandId { get; set; }
        public string? ImageUrl { get; set; }
        public string? Screen { get; set; }
        public string? Chip { get; set; }
        public string? Battery { get; set; }
        public string? Description { get; set; }
        public string? Slug { get; set; }
        public bool IsActive { get; set; }
        public List<PhoneVariant>? PhoneVariants { get; set; }
        public List<PhoneImage>? PhoneImages { get; set; }
    }
}
