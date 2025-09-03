using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shop.Application.DTOs.OnlinePayment.Sepay;
using Shop.Application.Interfaces;
using System.Net.Http.Json;

namespace Shop.Application.Services.Sepay
{
    public class SePayService : ISePayService
    {
        private readonly IConfiguration _config;
        private readonly IOrderRepository _orderRepository;
        private readonly HttpClient _httpClient;

        public SePayService(HttpClient httpClient, IConfiguration config, IOrderRepository orderRepository)
        {
            _config = config;
            _orderRepository = orderRepository;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _config["Sepay:Token"]);
        }

        public async Task<string> CreatePaymentAsync(SepayPaymentRequest request)
        {
            var payload = new
            {
                orderCode = request.OrderCode,
                amount = request.Amount,
                description = request.Description,
                returnUrl = request.ReturnUrl ?? _config["Sepay:ReturnUrl"],
                notifyUrl = request.NotifyUrl ?? _config["Sepay:NotifyUrl"]
            };

            var response = await _httpClient.PostAsJsonAsync(_config["Sepay:BaseUrl"] + "payment", payload);
            var url = _config["Sepay:BaseUrl"] + "payment";
            Console.WriteLine("Calling SePay URL: " + url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"SePay API error: {response} - {(int)response.StatusCode} - {content}");

            dynamic result = JsonConvert.DeserializeObject(content);
            return result.paymentUrl;
        }

        public async Task HandleCallbackAsync(SepayCallbackDto data)
        {
            if (data.Status == "success")
            {
                var order = await _orderRepository.GetSingleAsync(o => o.OrderCode == data.OrderCode);
                if (order == null)
                    throw new Exception("Order not found");

                order.IsPrepaid = true;
                //order.PaymentTransactionId = data.TransactionId;
                await _orderRepository.Update(order);
            }

            // You could log or handle other statuses if needed
        }
    }
}
