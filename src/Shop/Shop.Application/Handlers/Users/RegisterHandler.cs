using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Users;
using Shop.Domain.Entities;
using Shop.Domain.Methods;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Users
{
    public class RegisterHandler : IRequestHandler<RegisterRequest, CommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly HashPassWord _passwordHasher;

        public RegisterHandler(IUserRepository userRepository, HashPassWord passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<CommandResult> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            // Kiểm tra username hoặc email đã tồn tại
            var existingUserName = await _userRepository.GetSingleAsync(
                u => u.UserName == request.UserName
            );

            if (existingUserName != null)
            {
                result.Success = false;
                result.Code = StatusCode.Conflict;
                result.Message = "Tên đăng nhập đã được sử dụng.";
                return result;
            }
            var existingEmail = await _userRepository.GetSingleAsync(
                u => u.Email == request.Email
            );

            if (existingUserName != null)
            {
                result.Success = false;
                result.Code = StatusCode.Conflict;
                result.Message = "Email đã tồn tại.";
                return result;
            }

            // Hash password (giả sử không dùng Identity)
            string hashedPassword = _passwordHasher.HashPassword(request.PassWord);

            var newUser = new User
            {
                FullName = request.FullName,
                UserName = request.UserName,
                Email = request.Email,
                PassWord = hashedPassword,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                ImageUrl = request.ImageBase64, // nên xử lý ảnh nếu cần
                CreatedAt = request.CreatedAt,
                IsVerify = request.IsVerify,
                Role = request.Role,
                RefreshToken = request.RefreshToken
            };

            await _userRepository.Add(newUser);
            return result;
        }
    }
}
