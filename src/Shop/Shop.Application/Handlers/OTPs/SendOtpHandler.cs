using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.OTPs;
using Shop.Application.Services.Email;
using Shop.Domain.Entities;
using Shop.Domain.Methods;
using Shop.Domain.Results;

namespace Shop.Application.Handlers.OTPs
{
    public class SendOtpHandler : IRequestHandler<SendOtpRequest, CommandResult>
    {
        private readonly IEmailService _emailService;
        private readonly IOtpRepository _otpRepository;

        public SendOtpHandler(IEmailService emailService, IOtpRepository otpRepository)
        {
            _emailService = emailService;
            _otpRepository = otpRepository;
        }

        public async Task<CommandResult> Handle(SendOtpRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            // Tạo mã OTP 
            var otp = GenerateOtpCode.GenerateOtp();

            // Gửi email
            //var subject = "Mã xác thực OTP";
            var body = $"Mã OTP của bạn là: {otp}";
            await _emailService.SendAsync(request.Email, body);

            // Lưu OTP vào DB
            var otpEntity = new OTP
            {
                Email = request.Email,
                OtpCode = otp,
                OtpExpired = DateTime.UtcNow.AddMinutes(5),
            };

            await _otpRepository.Add(otpEntity);

            return result;
        }
    }
}
