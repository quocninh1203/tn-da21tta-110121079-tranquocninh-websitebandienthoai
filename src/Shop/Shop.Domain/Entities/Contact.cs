namespace Shop.Domain.Entities
{
    public class Contact : BaseEntities<int>
    {
        public string Reason { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
    }
}
