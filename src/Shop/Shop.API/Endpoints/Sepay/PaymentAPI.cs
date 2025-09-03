using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.OnlinePayment.Sepay;
using Shop.Application.DTOs.Orders;
using Shop.Application.Interfaces;
using Shop.Application.Services.Email;
using Shop.Application.Services.Sepay;

namespace Shop.API.Endpoints.Sepay
{
    [Route("api/[Controller]")]
    [ApiController]
    public class PaymentAPI : ControllerBase
    {
        private readonly ISePayService _paymentService;
        private readonly IConfiguration _configuration;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IPhoneVariantRepository _phoneVariantRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly IShipperRepository _shipperRepository;
        private readonly IUserProductInteractionRepository _userProductInteractionRepository;
        private readonly IEmailService _emailService;

        public PaymentAPI(ISePayService paymentService, 
                          IConfiguration configuration, 
                          IOrderRepository orderRepository, 
                          IUserRepository userRepository,
                          IPhoneVariantRepository phoneVariantRepository,
                          IOrderDetailRepository orderDetailRepository,
                          IPaymentMethodRepository paymentMethodRepository,
                          IShipperRepository shipperRepository,
                          IUserProductInteractionRepository userProductInteractionRepository,
                          IEmailService emailService)
        {
            _paymentService = paymentService;
            _configuration = configuration;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _phoneVariantRepository = phoneVariantRepository;
            _orderDetailRepository = orderDetailRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _shipperRepository = shipperRepository;
            _userProductInteractionRepository = userProductInteractionRepository;
            _emailService = emailService;
        }

        // [1] FE gọi API này để tạo thanh toán
        //[HttpPost("create")]
        //public async Task<IActionResult> Create([FromBody] SepayPaymentRequest request)
        //{
        //    try
        //    {
        //        var paymentUrl = await _paymentService.CreatePaymentAsync(request);
        //        return Ok(new { paymentUrl });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        // [5] SePay gọi lại webhook tại đây
        //[HttpPost("callback")]
        //public async Task<IActionResult> Callback([FromBody] SepayCallbackDto data)
        //{
        //    try
        //    {
        //        await _paymentService.HandleCallbackAsync(data);
        //        return Ok(new { success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { success = false, message = ex.Message });
        //    }
        //}

        [HttpPost("payment")]
        public async Task<IActionResult> ReceiveWebhookAsync([FromBody] SePayWebhookPayload payload)
        {
            // 1. Xác thực API Key từ Header
            if (!Request.Headers.TryGetValue("Authorization", out var authHeader) ||
                !authHeader.ToString().Equals($"Apikey {_configuration["SePay:ApiKey"]}"))
            {
                return Unauthorized(new { success = false, message = "Invalid API Key" });
            }

            // 2. Kiểm tra dữ liệu đầu vào
            if (payload == null || string.IsNullOrWhiteSpace(payload.Content))
            {
                return BadRequest(new { success = false, message = "Invalid or missing payload data." });
            }

            try
            {
                // ✅ Cắt mã đơn hàng: lấy từ cuối cùng của chuỗi Content
                var tokens = payload.Content.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 0)
                    return BadRequest(new { success = false, message = "Không tìm thấy mã đơn hàng trong nội dung." });

                var orderCode = tokens[^1]; // phần tử cuối cùng

                // 3. Tìm đơn hàng theo mã Code
                var order = await _orderRepository.GetSingleAsync(o => o.OrderCode == orderCode);
                if (order == null)
                {
                    return NotFound(new { success = false, message = "Order not found." });
                }

                // 4. Cập nhật trạng thái thanh toán
                order.IsPrepaid = true;
                order.Status = 3;
                await _orderRepository.Update(order);

                // Gửi thông tin đơn hàng qua tới khách hàng
                var user = await _userRepository.GetByIdAsync(order.UserId);
                var details = await _orderDetailRepository.GetAsync(d => d.OrderId == order.Id);

                var items = new List<OrderItemDTO>();
                foreach (var d in details)
                {
                    var variant = await _phoneVariantRepository.GetSingleAsync(
                        v => v.Id == d.VariantId,
                        v => v.Phone,
                        v => v.Color,
                        v => v.Ram,
                        v => v.Storage
                    );

                    items.Add(new OrderItemDTO
                    {
                        Quantity = d.Quantity,
                        PriceAtOrder = d.PriceAtOrder,
                        Price = variant?.Price ?? 0,
                        Color = variant?.Color?.Name ?? "N/A",
                        Ram = variant?.Ram?.Size ?? "N/A",
                        Storage = variant?.Storage?.Size ?? "N/A",
                        PhoneName = variant?.Phone?.Name ?? "N/A",
                        PhoneImageUrl = variant?.Phone?.ImageUrl ?? ""
                    });

                    var userProductInteraction = await _userProductInteractionRepository.GetSingleAsync(u => u.UserId == order.UserId && u.ProductId == variant.PhoneId);

                    userProductInteraction.Label = true;
                    await _userProductInteractionRepository.Update(userProductInteraction);
                }

                var paymentMethod = await _paymentMethodRepository.GetByIdAsync(order.MethodId);
                var shippingCost = await _shipperRepository.GetByIdAsync(order.ShipperId);

                await _emailService.SendOrderConfirmationAsync(user.Email, user.FullName, user.PhoneNumber,
                                                               order.ShippingAddress, order.OrderCode, order.OrderDate,
                                                               order.TotalPrice, items, paymentMethod.Name, shippingCost.Cost, order.IsPrepaid);

                return Ok(new { success = true, message = "Webhook processed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi server khi xử lý webhook." });
            }
        }


    }
}
