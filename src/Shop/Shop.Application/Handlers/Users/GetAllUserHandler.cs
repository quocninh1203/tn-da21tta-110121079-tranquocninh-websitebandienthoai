using MediatR;
using Shop.Application.DTOs.Users;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Users;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Users
{
    public class GetAllUserHandler : IRequestHandler<GetAllUserRequest, QueryResult<List<UserDTO>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;  // Thêm IOrderRepository để lấy danh sách đơn hàng

        public GetAllUserHandler(IUserRepository userRepository, IOrderRepository orderRepository)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
        }

        public async Task<QueryResult<List<UserDTO>>> Handle(GetAllUserRequest request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAsync(u => u.IsVerify == true);
            var response = new QueryResult<List<UserDTO>>();

            if (users.Count == 0)
            {
                response.Success = false;
                response.Message = string.Format(CommonMessages.NoDataFound, nameof(User));
                response.Code = StatusCode.NotFound;
                response.Model = new List<UserDTO>();
            }
            else
            {
                var result = new List<UserDTO>();

                // Duyệt qua tất cả người dùng
                foreach (var user in users)
                {
                    // Lấy tất cả đơn hàng của người dùng
                    var orders = await _orderRepository.GetAsync(o => o.UserId == user.Id);

                    // Chuyển đổi người dùng và các đơn hàng thành UserDTO
                    var userDTO = new UserDTO
                    {
                        UserId = user.Id,
                        FullName = user.FullName,
                        Email = user.Email,
                        Address = user.Address,
                        PhoneNumber = user.PhoneNumber,
                        Orders = orders.Select(order => new Order
                        {
                            Id = order.Id,
                            OrderDate = order.OrderDate,
                            Status = order.Status,
                            MethodId = order.MethodId,
                            ShipperId = order.ShipperId,
                            ShippingAddress = order.ShippingAddress,
                            OrderCode = order.OrderCode, 
                            IsPrepaid = order.IsPrepaid,
                            TotalPrice = order.TotalPrice,
                        }).ToList()
                    };

                    result.Add(userDTO);
                }

                response.Model = result;
            }

            return response;
        }
    }
}
