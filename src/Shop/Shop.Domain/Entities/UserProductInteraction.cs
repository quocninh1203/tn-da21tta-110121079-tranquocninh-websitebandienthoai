namespace Shop.Domain.Entities
{
    public class UserProductInteraction : BaseEntities<int>
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public bool Label { get; set; }
    }
}
