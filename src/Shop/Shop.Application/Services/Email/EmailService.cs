using Microsoft.Extensions.Configuration;
using Shop.Application.DTOs.Orders;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Globalization;

namespace Shop.Application.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(string toEmail, string otpCode)
        {
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(
                    _config["Email:Username"],
                    _config["Email:Password"]),
                EnableSsl = true
            };

            var fromAddress = new MailAddress(
                _config["Email:Username"], "NNHShop" // ✅ Tên hiển thị khi gửi email
            );

            // Đọc template HTML
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "OtpEmail.html");
            var htmlBody = await File.ReadAllTextAsync(templatePath);

            // Thay thế nội dung động
            htmlBody = htmlBody
                //.Replace("{{CustomerName}}", customerName)
                .Replace("{{OTP}}", otpCode);

            var mail = new MailMessage
            {
                From = fromAddress,
                Subject = "Mã xác thực OTP",
                Body = htmlBody,
                IsBodyHtml = true
            };

            mail.To.Add(toEmail);
            await smtp.SendMailAsync(mail);
        }

        /// <summary>
        /// Gửi email xác nhận đơn hàng cho khách hàng
        /// </summary>
        public async Task SendOrderConfirmationAsync(
            string toEmail,
            string customerName,
            string phoneNumber,
            string address,
            string orderCode,
            DateTime orderDate,
            int totalPrice,
            List<OrderItemDTO> items,
            string paymentMethod,     // 🆕 thêm hình thức thanh toán
            int shippingFee,          // 🆕 thêm phí giao hàng
            bool isPaid               // 🆕 thêm trạng thái thanh toán
        )
        {
            // 1) cấu hình SMTP
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(
                    _config["Email:Username"],
                    _config["Email:Password"]),
                EnableSsl = true
            };
            var fromAddress = new MailAddress(
                _config["Email:Username"], "NNHShop");

            // 2) đọc template
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "OrderConfirmation.html");
            var htmlBody = await File.ReadAllTextAsync(templatePath);

            // 3) build các dòng <tr> cho mỗi sản phẩm
            var sb = new StringBuilder();
            foreach (var it in items)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine($"  <td>{it.PhoneName}</td>");
                sb.AppendLine($"  <td style=\"text-align:center;\">{it.Quantity}</td>");
                sb.AppendLine($"  <td style=\"text-align:right;\">{it.Price.ToString("C0", CultureInfo.GetCultureInfo("vi-VN"))}</td>");
                sb.AppendLine($"  <td style=\"text-align:right;\">{it.PriceAtOrder.ToString("C0", CultureInfo.GetCultureInfo("vi-VN"))}</td>");
                sb.AppendLine("</tr>");
            }
            var itemsRows = sb.ToString();

            // 4) chuẩn bị dữ liệu thêm
            var orderTotal = items.Sum(x => x.PriceAtOrder);  // tổng tiền hàng
            var paymentStatus = isPaid ? "Đã thanh toán" : "Chưa thanh toán";

            // 5) thay thế placeholder
            htmlBody = htmlBody
                .Replace("{{CustomerName}}", customerName)
                .Replace("{{PhoneNumber}}", phoneNumber)
                .Replace("{{Address}}", address)
                .Replace("{{OrderCode}}", orderCode)
                .Replace("{{OrderDate}}", orderDate.ToString("yyyy-MM-dd HH:mm"))
                .Replace("{{PaymentMethod}}", paymentMethod)
                .Replace("{{PaymentStatus}}", paymentStatus)
                .Replace("{{OrderTotal}}", orderTotal.ToString("C0", CultureInfo.GetCultureInfo("vi-VN")))
                .Replace("{{ShippingFee}}", shippingFee.ToString("C0", CultureInfo.GetCultureInfo("vi-VN")))
                .Replace("{{TotalPrice}}", totalPrice.ToString("C0", CultureInfo.GetCultureInfo("vi-VN")))
                .Replace("{{ItemsRows}}", itemsRows);

            // 6) tạo và gửi mail
            var mail = new MailMessage
            {
                From = fromAddress,
                Subject = $"[NNHShop] Xác nhận đơn hàng {orderCode}",
                Body = htmlBody,
                IsBodyHtml = true
            };
            mail.To.Add(toEmail);

            await smtp.SendMailAsync(mail);
        }
    }
}
