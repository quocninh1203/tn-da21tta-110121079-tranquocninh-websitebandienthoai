using MediatR;
using Microsoft.AspNetCore.Http;
using Shop.Application.DTOs.Users;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Users;
using Shop.Application.Services.Jwt;
using Shop.Domain.Methods;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;
using System.Security.Claims;

namespace Shop.Application.Handlers.Users
{
    public class LoginHandler : IRequestHandler<LoginRequest, QueryResult<AuthDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly HashPassWord _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        IHttpContextAccessor _httpContextAccessor;
        public LoginHandler(
            IUserRepository userRepository, 
            HashPassWord hashPassWord, 
            IJwtService jwtService, 
            IRefreshTokenRepository refreshTokenRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _passwordHasher = hashPassWord;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<QueryResult<AuthDTO>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            // Retrieve user from the database based on the username
            var user = await _userRepository.GetSingleAsync(u => u.UserName == request.UserName);

            var response = new QueryResult<AuthDTO>();

            // Check if the user exists
            if (user == null)
            {
                response.Success = false;
                response.Message = "Tên đăng nhập không tồn tại";
                response.Code = StatusCode.BadRequest;
                response.Model = null;
                return response;
            }

            // Verify the password by comparing it with the stored hashed password
            bool isPasswordValid = _passwordHasher.VerifyPassword(user.PassWord, request.PassWord);
            if (!isPasswordValid)
            {
                response.Success = false;
                response.Message = "Sai mật khẩu";
                response.Code = StatusCode.BadRequest;
                response.Model = null;
                return response;
            }

            // If login is successful, generate claims for JWT
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("role", user.Role.ToString()),
                new Claim("email", user.Email),
            };

            // Generate access token
            var accessToken = _jwtService.GenerateAccessToken(claims);

            // Generate refresh token
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Set refreshToken vào cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

            // Prepare login response DTO
            AuthDTO login = new AuthDTO()
            {
                AccessToken = accessToken,
            };
            response.Message = "Đăng nhập thành công";
            response.Model = login;

            // Save refresh token
            await _refreshTokenRepository.SaveRefreshTokenAsync(user.Id, refreshToken);

            return response;
        }
    }
}
