namespace Shop.Application.DTOs.Phones
{
    public class PhoneDTO
    {
        public int PhoneId { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public int? PriceReview { get; set; }
        public string ImageUrl { get; set; }
        public string Screen { get; set; }
        public string Chip { get; set; }
        public string Battery { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public double Rating { get; set; } = 5;
        public bool IsActive { get; set; }
    }
}
