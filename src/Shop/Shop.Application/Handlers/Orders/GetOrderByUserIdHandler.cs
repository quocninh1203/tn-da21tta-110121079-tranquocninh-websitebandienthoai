using MediatR;
using Microsoft.AspNetCore.Http;
using Shop.Application.DTOs.Orders;
using Shop.Application.DTOs.Users;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Orders;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Orders
{
    public class GetOrderByUserIdHandler : IRequestHandler<GetOrdersByUserIdRequest, QueryResult<List<OrderDetailDTO>>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IPhoneVariantRepository _phoneVariantRepository;
        private readonly IUserRepository _userRepository; 
        private readonly IShipperRepository _shipperRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly IHttpContextAccessor _httpContext;

        public GetOrderByUserIdHandler(IOrderRepository orderRepository,
                                       IOrderDetailRepository orderDetailRepository,
                                       IPhoneVariantRepository phoneVariantRepository,
                                       IUserRepository userRepository,
                                       IShipperRepository shipperRepository,
                                       IPaymentMethodRepository paymentMethodRepository,
                                       IHttpContextAccessor httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _phoneVariantRepository = phoneVariantRepository;
            _userRepository = userRepository;
            _shipperRepository = shipperRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _httpContext = httpContextAccessor;
        }

        public async Task<QueryResult<List<OrderDetailDTO>>> Handle(GetOrdersByUserIdRequest request, CancellationToken cancellationToken)
        {
            var result = new QueryResult<List<OrderDetailDTO>>();

            // Lấy tất cả các đơn hàng của người dùng
            var orders = await _orderRepository.GetAsync(o => o.UserId == request.UserId);
            if (orders == null || orders.Count == 0)
            {
                result.Success = false;
                result.Message = "Không có đơn hàng nào cho người dùng này.";
                result.Code = StatusCode.NotFound;
                result.Model = new List<OrderDetailDTO>();
                return result;
            }

            // Danh sách chứa thông tin chi tiết đơn hàng
            var orderDetailsList = new List<OrderDetailDTO>();

            foreach (var order in orders)
            {
                // Lấy thông tin người dùng (UserName)
                var user = await _userRepository.GetByIdAsync(order.UserId); // Lấy thông tin người dùng qua UserId
                var profile = new UserDTO
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                };
                var userName = user?.FullName ?? "Unknown"; // Nếu không có người dùng, trả về "Unknown"

                var shipper = await _shipperRepository.GetByIdAsync(order.ShipperId);
                var shipName = shipper?.Name ?? "Unknown";

                var method = await _paymentMethodRepository.GetByIdAsync(order.MethodId);
                var methodName = method?.Name ?? "Unknown";

                // Lấy các chi tiết đơn hàng cho mỗi đơn hàng
                var orderDetails = await _orderDetailRepository.GetAsync(od => od.OrderId == order.Id);

                // Tạo danh sách các sản phẩm trong đơn hàng
                var orderItems = new List<OrderItemDTO>();

                foreach (var orderDetail in orderDetails)
                {
                    // Lấy PhoneVariant qua VariantId
                    var phoneVariant = await _phoneVariantRepository.GetSingleAsync(
                        pv => pv.Id == orderDetail.VariantId,
                        pv => pv.Ram,
                        pv => pv.Storage,
                        pv => pv.Color,
                        pv => pv.Phone  // Nếu cần thông tin Phone
                    );

                    // Tạo DTO cho sản phẩm trong đơn hàng
                    var orderItemDTO = new OrderItemDTO
                    {
                        OrderDetailId = orderDetail.Id,
                        Quantity = orderDetail.Quantity,
                        PriceAtOrder = orderDetail.PriceAtOrder,
                        Price = phoneVariant?.Price ?? 0,  // Lấy giá từ PhoneVariant
                        Color = phoneVariant?.Color?.Name ?? "N/A",  // Màu từ PhoneVariant
                        Ram = phoneVariant?.Ram?.Size ?? "N/A",  // RAM từ PhoneVariant
                        Storage = phoneVariant?.Storage?.Size ?? "N/A",  // Dung lượng lưu trữ từ PhoneVariant
                        PhoneName = phoneVariant?.Phone?.Name ?? "N/A",  // Tên sản phẩm từ Phone
                        PhoneImageUrl = GetFullImageUrl(phoneVariant?.Phone?.ImageUrl),  // Lấy URL ảnh đầy đủ
                        IsReview = orderDetail.IsReview,
                    };

                    orderItems.Add(orderItemDTO);
                }

                // Tạo DTO cho từng đơn hàng và thêm UserName
                var orderDetailDTO = new OrderDetailDTO
                {
                    OrderId = order.Id,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalPrice,
                    OrderStatus = order.Status,
                    Method = methodName,
                    Ship = shipName,
                    Cost = shipper.Cost,
                    ShippingAddress = order.ShippingAddress,
                    OrderCode = order.OrderCode,
                    IsPrepaid = order.IsPrepaid,
                    OrderItems = orderItems,
                    User = profile  // Gán tên người dùng vào OrderDetailDTO
                };

                orderDetailsList.Add(orderDetailDTO);
            }

            result.Success = true;
            result.Code = StatusCode.Ok;
            result.Model = orderDetailsList;

            return result;
        }

        // Hàm để thêm đuôi server vào URL ảnh
        private string GetFullImageUrl(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return string.Empty;
            }

            // Lấy Scheme (http hoặc https) và Host (tên miền hoặc IP)
            var baseUrl = _httpContext.HttpContext != null
                ? $"{_httpContext.HttpContext.Request.Scheme}://{_httpContext.HttpContext.Request.Host}"
                : string.Empty;

            // Nếu URL ảnh đã là đầy đủ (bắt đầu với http:// hoặc https://), không cần thêm
            if (imageUrl.StartsWith("http://") || imageUrl.StartsWith("https://"))
            {
                return imageUrl;
            }

            // Thêm đường dẫn server vào URL ảnh
            return $"{baseUrl}/{imageUrl.TrimStart('/')}";
        }
    }
}
