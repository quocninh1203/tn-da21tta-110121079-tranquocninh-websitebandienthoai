using Shop.Application.DTOs.Phones;

namespace Shop.Application.DTOs.Brands
{
    public class BrandDetailDTO
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string Slug { get; set; }
        public List<PhoneDTO>? Phones { get; set; }
    }
}
