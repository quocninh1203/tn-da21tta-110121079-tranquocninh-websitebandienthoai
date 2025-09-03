using Shop.Application.DTOs.Phones;
using Shop.Domain.Entities;
using Microsoft.AspNetCore.Http; // Thêm thư viện này

namespace Shop.Application.Extension
{
    public static class PhoneExtensions
    {
        public static PhoneDTO ToDto(this Phone phone, IHttpContextAccessor httpContextAccessor)
        {
            if (phone == null)
            {
                return null;
            }

            // Lấy Base URL từ HttpContextAccessor
            var baseUrl = httpContextAccessor.HttpContext != null
                ? $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}"
                : "";

            // Xử lý ImageUrl tương tự như trong GetPhoneActivateHandler
            var imageUrl = phone.ImageUrl;
            if (!string.IsNullOrWhiteSpace(imageUrl) && !imageUrl.StartsWith("http"))
            {
                imageUrl = baseUrl + imageUrl;
            }

            return new PhoneDTO
            {
                PhoneId = phone.Id,
                BrandId = phone.BrandId,
                Name = phone.Name,
                ImageUrl = imageUrl, // Gán URL đã xử lý
                Screen = phone.Screen,
                Chip = phone.Chip,
                Battery = phone.Battery,
                Description = phone.Description,
                Slug = phone.Slug,
                IsActive = phone.IsActive,
                // Các thuộc tính khác cần được tính toán ở nơi gọi phương thức ToDto
            };
        }
    }
}