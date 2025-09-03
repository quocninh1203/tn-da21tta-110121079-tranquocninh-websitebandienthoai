using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Reviews;
using Shop.Application.Services.FileUpload;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Reviews
{
    public class CreateReviewHandler : IRequestHandler<CreateReviewRequest, CommandResult>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPhoneRepository _phoneRepository;
        private readonly IPhoneVariantRepository _variantRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IFileUploadService _uploadService;
        public CreateReviewHandler(
            IReviewRepository reviewRepository,
            IUserRepository userRepository,
            IPhoneRepository phoneRepository,
            IPhoneVariantRepository phoneVariantRepository,
            IOrderDetailRepository orderDetailRepository,
            IFileUploadService fileUploadService)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _phoneRepository = phoneRepository;
            _variantRepository = phoneVariantRepository;
            _orderDetailRepository = orderDetailRepository;
            _uploadService = fileUploadService;
        }
        public async Task<CommandResult> Handle(CreateReviewRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(User));
                result.Code = StatusCode.NotFound;
                return result;
            }
            var phone = await _phoneRepository.GetByIdAsync(request.PhoneId);
            if (phone == null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Phone));
                result.Code = StatusCode.NotFound;
                return result;
            }
            var variant = await _variantRepository.GetByIdAsync(request.VariantId);
            if (variant == null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(PhoneVariant));
                result.Code = StatusCode.NotFound;
                return result;
            }
            var detail = await _orderDetailRepository.GetByIdAsync(request.OrderDetailId);
            if (detail == null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Order));
                result.Code = StatusCode.NotFound;
                return result;
            }
            detail.IsReview = true;

            var review = new Review
            {
                UserId = user.Id,
                PhoneId = phone.Id,
                VariantId = variant.Id,
                Rating = request.Rating,
                Text = request.Text,
                CreateDate = request.CreateDate,
            };

            await _reviewRepository.Add(review);

            if (!string.IsNullOrWhiteSpace(request.ImageBase64))
            {
                review.ImageUrl = _uploadService.SaveImage(request.ImageBase64, nameof(Review), review.Id);
                await _reviewRepository.Update(review);
            }

            await _orderDetailRepository.Update(detail);

            return result;
        }
    }
}
