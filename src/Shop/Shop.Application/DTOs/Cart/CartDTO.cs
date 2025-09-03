namespace Shop.Application.DTOs.Cart
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public int PhoneId { get; set; }
        public int VariantId { get; set; }
        public string PhoneName { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public string Color { get; set; }
        public string Ram { get; set; }
        public string Storage { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
