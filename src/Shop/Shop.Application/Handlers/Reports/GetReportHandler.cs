using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shop.Application.DTOs.Report;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Reports;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Reports
{
    public class GetReportHandler : IRequestHandler<GetReportRequest, QueryResult<ReportDTO>>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IPhoneVariantRepository _variantRepo;
        private readonly IUserRepository _userRepo;
        private readonly IPaymentMethodRepository _paymentMethodRepo;
        private readonly IHttpContextAccessor _httpContext;

        public GetReportHandler(
            IOrderRepository orderRepo,
            IPhoneVariantRepository variantRepo,
            IUserRepository userRepo,
            IPaymentMethodRepository paymentMethodRepo,
            IHttpContextAccessor httpContextAccessor)
        {
            _orderRepo = orderRepo;
            _variantRepo = variantRepo;
            _userRepo = userRepo;
            _paymentMethodRepo = paymentMethodRepo;
            _httpContext = httpContextAccessor;
        }

        public async Task<QueryResult<ReportDTO>> Handle(GetReportRequest req, CancellationToken ct)
        {
            var result = new QueryResult<ReportDTO>();

            var baseUrl = _httpContext.HttpContext != null
                ? $"{_httpContext.HttpContext.Request.Scheme}://{_httpContext.HttpContext.Request.Host}"
            : "";

            // 1) Load orders kèm OrderDetails
            var orders = await _orderRepo.GetAsync(
                predicate: o => o.OrderDate >= req.StartDate && o.OrderDate <= req.EndDate,
                include: q => q.Include(o => o.OrderDetails)
            );

            // 2) RevenueReport
            long totalRevenue = orders.Where(o => o.Status == 3 || o.Status == 4).Sum(o => (long)o.TotalPrice);
            long totalCancelled = orders.Where(o => o.Status == 0).Sum(o => (long)o.TotalPrice);

            var revDto = new RevenueReportDTO
            {
                TotalRevenue = totalRevenue,
                TotalCancelledOrdersAmount = totalCancelled,
                TotalOrders = orders.Count,
                CompletedOrders = orders.Count(o => o.Status == 3 || o.Status == 4),
                CancelledOrders = orders.Count(o => o.Status == 0),
                UndeliveredOrders = orders.Count(o => o.Status == 2),
                PendingOrders = orders.Count(o => o.Status == 1)
            };

            // 3) ProductReport
            var details = orders
                .Where(o => o.Status == 3 || o.Status == 4) // Lọc các Order có Status là 3 hoặc 4
                .SelectMany(o => o.OrderDetails)            // Lấy tất cả OrderDetail từ các Order đã lọc
                .ToList();

            var prodGroups = details
                .GroupBy(d => d.VariantId)
                .Select(g => new {
                    VariantId = g.Key,
                    Qty = g.Sum(x => (long)x.Quantity),
                    Rev = g.Sum(x => (long)x.Quantity * x.PriceAtOrder)
                })
                .ToList();

            var bestQty = prodGroups.OrderByDescending(x => x.Qty).FirstOrDefault();
            var bestRev = prodGroups.OrderByDescending(x => x.Rev).FirstOrDefault();

            var prodDto = new ProductReportDTO();
            if (bestQty != null)
            {
                var v = await _variantRepo.GetSingleAsync(v => v.Id == bestQty.VariantId, v => v.Phone);

                var image1Url = v.Phone.ImageUrl;
                if (!string.IsNullOrWhiteSpace(image1Url) && !image1Url.StartsWith("http"))
                {
                    image1Url = baseUrl + image1Url;
                }

                prodDto.MostSoldProductName = v.Phone?.Name ?? "N/A";
                prodDto.MostSoldProductImageUrl = image1Url;
                prodDto.TotalSoldQuantity = bestQty.Qty;
            }
            if (bestRev != null)
            {
                var v = await _variantRepo.GetSingleAsync(v => v.Id == bestRev.VariantId, v => v.Phone);

                var image2Url = v.Phone.ImageUrl;
                if (!string.IsNullOrWhiteSpace(image2Url) && !image2Url.StartsWith("http"))
                {
                    image2Url = baseUrl + image2Url;
                }

                prodDto.TopRevenueProductName = v.Phone?.Name ?? "N/A";
                prodDto.TopRevenueProductImageUrl = image2Url;
                prodDto.TotalRevenue = bestRev.Rev;
            }

            // 4) CustomerReport
            var custGroups = orders
                .Where(o => o.Status == 3 || o.Status == 4)
                .GroupBy(o => o.UserId)
                .Select(g => new {
                    UserId = g.Key,
                    Items = g.Sum(o => o.OrderDetails.Sum(d => (long)d.Quantity)),
                    Spent = g.Sum(o => (long)o.TotalPrice)
                })
                .OrderByDescending(x => x.Items)
                .FirstOrDefault();

            var custDto = new CustomerReportDTO();
            if (custGroups != null)
            {
                var u = await _userRepo.GetByIdAsync(custGroups.UserId);
                custDto.CustomerName = u?.FullName ?? "N/A";
                custDto.PhoneNumber = u?.PhoneNumber ?? "";
                custDto.Email = u?.Email ?? "";
                custDto.TotalItemsPurchased = custGroups.Items;
                custDto.TotalPrice = custGroups.Spent;
            }

            // 5) PaymentMethodReport
            var payGroup = orders
                .GroupBy(o => o.MethodId)
                .Select(g => new {
                    MethodId = g.Key,
                    Count = g.Count(),
                    Rev = g.Sum(o => (long)o.TotalPrice)
                })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault();

            var payDto = new PaymentMethodReportDTO();
            if (payGroup != null)
            {
                var pm = await _paymentMethodRepo.GetByIdAsync(payGroup.MethodId);
                payDto.PaymentMethod = pm?.Name ?? "Unknown";
                payDto.TotalOrders = payGroup.Count;
                payDto.TotalRevenue = payGroup.Rev;
            }

            // 6) Kết quả
            result.Model = new ReportDTO
            {
                Revenue = revDto,
                Products = prodDto,
                Customers = custDto,
                PaymentMethods = payDto
            };
            result.Success = true;
            result.Code = StatusCode.Ok;
            return result;
        }
    }
}
