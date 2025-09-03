using MediatR;
using Shop.Application.DTOs.Users;
using Shop.Domain.Entities;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Users
{
    public class GetProfileRequest : IRequest<QueryResult<ProfileDTO>>
    {
        public int UserId { get; set; }
    }
}
