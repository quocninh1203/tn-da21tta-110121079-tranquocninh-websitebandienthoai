using MediatR;
using Microsoft.AspNetCore.Http;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Users;
using Shop.Domain.Results;

namespace Shop.Application.Handlers.Users
{
    public class LogoutHandler : IRequestHandler<LogoutRequest, CommandResult>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LogoutHandler(IRefreshTokenRepository refreshTokenRepository, IHttpContextAccessor httpContextAccessor)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CommandResult> Handle(LogoutRequest request, CancellationToken cancellationToken)
        {
            var response = new CommandResult();
            await _refreshTokenRepository.DeleteRefreshTokenAsync(request.UserId);

            _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/"
            });

            response.Message = "Đăng xuất thành công.";
            return response;
        }
    }
}
