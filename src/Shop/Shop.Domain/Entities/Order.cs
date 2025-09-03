namespace Shop.Domain.Entities
{
    public class Order : BaseEntities<int>
    {
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Status { get; set; }
        public int MethodId { get; set; }
        public int ShipperId { get; set; }  
        public string ShippingAddress { get; set; }
        public string OrderCode { get; set; }
        public bool IsPrepaid { get; set; }
        public int TotalPrice { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
