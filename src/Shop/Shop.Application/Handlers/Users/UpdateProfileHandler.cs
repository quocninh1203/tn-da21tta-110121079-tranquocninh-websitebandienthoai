using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Users;
using Shop.Application.Services.FileUpload;
using Shop.Domain.Entities;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Users
{
    public class UpdateProfileHandler : IRequestHandler<UpdateProfileRequest, CommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileUploadService _fileUploadService;

        public UpdateProfileHandler(IUserRepository userRepository, IFileUploadService fileUploadService)
        {
            _userRepository = userRepository;
            _fileUploadService = fileUploadService;
        }

        public async Task<CommandResult> Handle(UpdateProfileRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            // Tìm người dùng theo UserId
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                result.Success = false;
                result.Message = "Người dùng không tồn tại.";
                result.Code = StatusCode.NotFound;
                return result;
            }

            // Cập nhật thông tin người dùng
            var updateEntity = new User
            {
                FullName = request.FullName,
                UserName = request.UserName,
                PassWord = request.PassWord,
                CreatedAt = user.CreatedAt,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                IsVerify = request.IsVerify,
                Role = request.Role,
                RefreshToken = request.RefreshToken,
            };

            // Nếu có ảnh mới
            if (!string.IsNullOrWhiteSpace(request.ImageBase64))
            {
                // Xoá ảnh cũ nếu có
                if (!string.IsNullOrWhiteSpace(user.ImageUrl))
                {
                    _fileUploadService.RemoveImage(user.ImageUrl);
                }

                // Lưu ảnh mới
                user.ImageUrl = _fileUploadService.SaveImage(request.ImageBase64, nameof(User), user.Id);
            }

            user.UpdateWith(updateEntity);

            // Cập nhật thông tin trong cơ sở dữ liệu
            await _userRepository.Update(user);

            // Trả kết quả thành công
            result.Success = true;
            result.Message = "Cập nhật thông tin người dùng thành công.";
            result.Code = StatusCode.Ok;

            return result;
        }
    }
}
