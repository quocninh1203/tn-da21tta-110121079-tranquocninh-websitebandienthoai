namespace Shop.Domain.Entities
{
    public class PhoneVariant : BaseEntities<int>
    {
        public int PhoneId { get; set; }
        public int ColorId { get; set; }
        public int RamId { get; set; }
        public int StorageId { get; set; }
        public int Price { get; set; }
        public int StockQuantity { get; set; }

        public Phone Phone { get; set; }
        public Color Color { get; set; }
        public Ram Ram { get; set; }
        public Storage Storage { get; set; }
        public List<Cart>? Carts { get; set; }
    }
}
