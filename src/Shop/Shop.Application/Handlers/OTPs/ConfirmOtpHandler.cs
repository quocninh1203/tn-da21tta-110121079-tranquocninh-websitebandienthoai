using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.OTPs;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.OTPs
{
    public class ConfirmOtpHandler : IRequestHandler<ConfirmOtpRequest, CommandResult>
    {
        private readonly IOtpRepository _otpRepository;
        private readonly IUserRepository _userRepository;

        public ConfirmOtpHandler(IOtpRepository otpRepository, IUserRepository userRepository)
        {
            _otpRepository = otpRepository;
            _userRepository = userRepository;
        }

        public async Task<CommandResult> Handle(ConfirmOtpRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var otp = await _otpRepository.GetSingleAsync(
                x => x.Email == request.Email
                  && x.OtpCode == request.OtpCode
                  && x.OtpExpired > DateTime.UtcNow
            );

            if (otp == null)
            {
                result.Success = false;
                result.Code = StatusCode.BadRequest;
                result.Message = "Mã OTP không hợp lệ hoặc đã hết hạn.";
                return result;
            }

            // Cập nhật trạng thái IsVerify của người dùng
            var user = await _userRepository.GetSingleAsync(x => x.Email == request.Email);
            if (user != null)
            {
                user.IsVerify = true;  // Đánh dấu người dùng là đã xác thực
                await _userRepository.Update(user);  // Lưu cập nhật vào cơ sở dữ liệu 
            }

            await _otpRepository.Delete(otp);

            result.Success = true;
            result.Code = StatusCode.Ok;
            result.Message = "Tạo tài khoản thành công.";
            return result;
        }
    }
}
