using MediatR;
using Shop.Application.DTOs.Report;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Reports
{
    public class GetRevenueByMonthRequest : IRequest<QueryResult<RevenueReportDTO>>
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
