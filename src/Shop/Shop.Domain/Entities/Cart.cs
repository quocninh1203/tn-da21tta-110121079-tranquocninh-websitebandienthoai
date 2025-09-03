namespace Shop.Domain.Entities
{
    public class Cart : BaseEntities<int>
    {
        public int UserId { get; set; }
        public int VariantId { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedDate { get; set; }
        public PhoneVariant PhoneVariant { get; set; }
    }
}
