using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.PhoneVariants
{
    public class CreatePhoneVariantRequest : IRequest<CommandResult>
    {
        public int PhoneId { get; set; }
        public int ColorId { get; set; }
        public int RamId { get; set; }
        public int StorageId { get; set; }
        public int Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
