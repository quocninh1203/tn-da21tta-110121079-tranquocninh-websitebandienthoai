using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Users;
using Shop.Domain.Methods;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Users
{
    public class ChangePassWordHandler : IRequestHandler<ChangePassWordRequest, CommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly HashPassWord _passwordHasher;

        public ChangePassWordHandler(IUserRepository userRepository, HashPassWord hashPassWord)
        {
            _userRepository = userRepository;
            _passwordHasher = hashPassWord;
        }
        public async Task<CommandResult> Handle(ChangePassWordRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                result.Success = false;
                result.Message = "Người dùng không tồn tại.";
                result.Code = StatusCode.NotFound;
                return result;
            }

            bool isPasswordValid = _passwordHasher.VerifyPassword(user.PassWord, request.PassWord);
            if (!isPasswordValid)
            {
                result.Success = false;
                result.Message = "Mật khẩu không đúng.";
                result.Code = StatusCode.NotFound;
                return result;
            }

            string hashedPassword = _passwordHasher.HashPassword(request.NewPassWord);
            user.PassWord = hashedPassword;
            await _userRepository.Update(user);

            result.Success = true;
            result.Message = "Đổi mật khẩu thành công.";
            result.Code = StatusCode.Ok;

            return result;
        }
    }
}
