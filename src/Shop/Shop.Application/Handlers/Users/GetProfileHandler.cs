using MediatR;
using Microsoft.AspNetCore.Http;
using Shop.Application.DTOs.Users;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Users;
using Shop.Domain.Entities;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Users
{
    public class GetProfileHandler : IRequestHandler<GetProfileRequest, QueryResult<ProfileDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContext;
        public GetProfileHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContext = httpContextAccessor;
        }
        public async Task<QueryResult<ProfileDTO>> Handle(GetProfileRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            var response = new QueryResult<ProfileDTO>();
            if (user is null)
            {
                response.Success = false;
                response.Message = "Id not found";
                response.Code = StatusCode.NotFound;    
                response.Model = null;
            }
            else
            {

                var baseUrl = _httpContext.HttpContext != null
                    ? $"{_httpContext.HttpContext.Request.Scheme}://{_httpContext.HttpContext.Request.Host}"
                : "";

                var imageUrl = user.ImageUrl;
                if (!string.IsNullOrWhiteSpace(imageUrl) && !imageUrl.StartsWith("http"))
                {
                    imageUrl = baseUrl + imageUrl;
                }

                ProfileDTO profile = new ProfileDTO()
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                    ImageUrl = imageUrl,
                };
                response.Model = profile;
            }

            return response;
        }
    }
}