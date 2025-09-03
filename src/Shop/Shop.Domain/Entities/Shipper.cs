namespace Shop.Domain.Entities
{
    public class Shipper : BaseEntities<int>
    {
        public string Name { get; set; }
        public int Cost { get; set; }
    }
}
