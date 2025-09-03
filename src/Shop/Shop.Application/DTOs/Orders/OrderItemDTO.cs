namespace Shop.Application.DTOs.Orders
{
    public class OrderItemDTO
    {
        public int OrderDetailId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtOrder { get; set; }
        public decimal Price { get; set; }
        public string Color { get; set; }
        public string Ram { get; set; }
        public string Storage { get; set; }
        public int? PhoneId { get; set; }
        public int? VariantId { get; set; }
        public string PhoneName { get; set; }
        public string PhoneImageUrl { get; set; }
        public bool IsReview { get; set; }
    }
}
