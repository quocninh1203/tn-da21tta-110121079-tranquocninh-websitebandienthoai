using Shop.Application.DTOs.Users;

namespace Shop.Application.DTOs.Orders
{
    public class OrderDetailDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalAmount { get; set; }
        public int OrderStatus { get; set; }
        public string Method { get; set; }
        public string Ship { get; set; }
        public int Cost { get; set; }
        public string ShippingAddress { get; set; }
        public string OrderCode { get; set; }
        public bool IsPrepaid { get; set; }
        public UserDTO User { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }
}
