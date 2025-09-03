namespace Shop.Domain.Entities
{
    public class OrderDetail : BaseEntities<int>
    {
        public int OrderId { get; set; }
        public int VariantId { get; set; }
        public int Quantity { get; set; }
        public int PriceAtOrder { get; set; }
        public bool IsReview { get; set; }
        public Order Order { get; set; }
    }
}
