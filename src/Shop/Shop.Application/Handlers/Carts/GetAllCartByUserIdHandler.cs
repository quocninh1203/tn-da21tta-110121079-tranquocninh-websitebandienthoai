using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shop.Application.DTOs.Cart;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Carts;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Carts
{
    public class GetAllCartByUserIdHandler : IRequestHandler<GetAllCartByUserIdRequest, QueryResult<List<CartDTO>>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IHttpContextAccessor _httpContext;
        public GetAllCartByUserIdHandler(ICartRepository cartRepository, IHttpContextAccessor httpContextAccessor)
        {
            _cartRepository = cartRepository;
            _httpContext = httpContextAccessor;
        }
        public async Task<QueryResult<List<CartDTO>>> Handle(GetAllCartByUserIdRequest request, CancellationToken cancellationToken)
        {
            var result = new QueryResult<List<CartDTO>>();

            var cartItems = await _cartRepository.GetAsync(
                predicate: c => c.UserId == request.UserId,
                include: query => query
                    .Include(c => c.PhoneVariant)
                        .ThenInclude(v => v.Phone)
                    .Include(c => c.PhoneVariant)
                        .ThenInclude(v => v.Color)
                    .Include(c => c.PhoneVariant)
                        .ThenInclude(v => v.Ram)
                    .Include(c => c.PhoneVariant)
                        .ThenInclude(v => v.Storage)
            );

            if (cartItems == null || !cartItems.Any())
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NoDataFound, nameof(Cart));
                result.Code = StatusCode.NotFound;
                result.Model = new List<CartDTO>();
                return result;
            }

            var baseUrl = _httpContext.HttpContext != null
                ? $"{_httpContext.HttpContext.Request.Scheme}://{_httpContext.HttpContext.Request.Host}"
                : "";

            var cartDTOs = cartItems.Select(c => new CartDTO
            {
                CartId = c.Id,
                PhoneId = c.PhoneVariant?.PhoneId ?? 0,
                VariantId = c.VariantId,
                PhoneName = c.PhoneVariant?.Phone?.Name ?? "",
                ImageUrl = !string.IsNullOrEmpty(c.PhoneVariant?.Phone?.ImageUrl)
                    ? $"{baseUrl}/{c.PhoneVariant.Phone.ImageUrl.TrimStart('/')}"
                    : "",
                IsActive = c.PhoneVariant?.Phone?.IsActive ?? false,
                Color = c.PhoneVariant?.Color?.Name ?? "",
                Ram = c.PhoneVariant?.Ram?.Size ?? "",
                Storage = c.PhoneVariant?.Storage?.Size ?? "",
                Price = c.PhoneVariant?.Price ?? 0,
                Quantity = c.Quantity,
                AddedDate = c.AddedDate
            }).ToList();

            result.Success = true;
            result.Code = StatusCode.Ok;
            result.Model = cartDTOs;
            return result;
        }
    }
}
