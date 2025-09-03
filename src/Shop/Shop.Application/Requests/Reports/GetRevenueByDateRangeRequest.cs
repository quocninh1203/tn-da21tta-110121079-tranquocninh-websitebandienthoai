using MediatR;
using Shop.Application.DTOs.Report;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Reports
{
    public class GetRevenueByDateRangeRequest : IRequest<QueryResult<RevenueReportDTO>>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
