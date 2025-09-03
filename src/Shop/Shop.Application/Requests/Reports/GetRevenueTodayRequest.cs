using MediatR;
using Shop.Application.DTOs.Report;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Reports
{
    public class GetRevenueTodayRequest : IRequest<QueryResult<RevenueReportDTO>>
    {
    }
}
