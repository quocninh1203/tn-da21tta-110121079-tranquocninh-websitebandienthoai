using MediatR;
using Microsoft.AspNetCore.Http;
using Shop.Application.DTOs.Users;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Users;
using Shop.Application.Services.Jwt;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;
using System.Security.Claims;

namespace Shop.Application.Handlers.Users
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, QueryResult<AuthDTO>>
    {
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RefreshTokenHandler(IJwtService jwtService, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<QueryResult<AuthDTO>> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = new QueryResult<AuthDTO>();

            var httpContext = _httpContextAccessor.HttpContext;
            var refreshToken = httpContext?.Request.Cookies["refreshToken"];

            // Kiểm tra RefreshToken trong cơ sở dữ liệu
            if (string.IsNullOrEmpty(refreshToken))
            {
                result.Success = false;
                result.Message = "Không tìm thấy refreshToken trong cookie.";
                result.Code = StatusCode.NotFound;
                result.Model = null;
                return result;
            }

            var user = await _userRepository.GetSingleAsync(u => u.RefreshToken == refreshToken);
            if (user == null)
            {
                result.Success = false;
                result.Message = "RefreshToken không hợp lệ.";
                result.Code = StatusCode.BadRequest;
                return result;
            }

            // Tạo AccessToken mới
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("role", user.Role.ToString()),
                new Claim("email", user.Email),
            };

            var accessToken = _jwtService.GenerateAccessToken(claims);

            // Trả về AccessToken mới
            result.Success = true;
            result.Message = "Tạo AccessToken mới thành công.";
            result.Code = StatusCode.Ok;
            result.Model = new AuthDTO()
            {
                AccessToken = accessToken,
            };

            return result;
        }
    }
}
