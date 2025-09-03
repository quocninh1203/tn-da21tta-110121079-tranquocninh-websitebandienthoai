using Shop.Application.DTOs.Reviews;

namespace Shop.Application.DTOs.Phones
{
    public class PhoneDetailDTO
    {
        public int PhoneId { get; set; }
        public string PhoneName { get; set; }
        public string Screen { get; set; }
        public string Chip { get; set; }
        public string Battery { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; }
        public string BrandName { get; set; }
        public List<string> PhoneImages { get; set; } = new();
        public List<VariantDTO> PhoneVariants { get; set; }
        public double Rating { get; set; }
        public List<ReviewDTO> Reviews { get; set; }
    }

    public class VariantDTO
    {
        public int PhoneVariantId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ColorName { get; set; }
        public string RamSize { get; set; }
        public string StorageSize { get; set; }
    }
}
