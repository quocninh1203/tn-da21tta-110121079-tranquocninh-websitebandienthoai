using MediatR;
using Shop.Domain.Entities;
using Shop.Domain.Results;
using System.Text.Json.Serialization;

namespace Shop.Application.Requests.Orders
{
    public class CreateOrderRequest : IRequest<QueryResult<Order>>
    {
        public int UserId { get; set; }
        [JsonIgnore]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [JsonIgnore]
        public int Status { get; set; } = 1;
        public int MethodId { get; set; }
        public int ShipperId { get; set; }
        public string ShippingAddress { get; set; }
        [JsonIgnore]
        public string? OrderCode { get; set; }
        [JsonIgnore]
        public bool IsPrepaid { get; set; } = false;  
    }
}
