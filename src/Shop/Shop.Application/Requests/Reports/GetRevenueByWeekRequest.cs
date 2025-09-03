using MediatR;
using Shop.Application.DTOs.Report;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Reports
{
    public class GetRevenueByWeekRequest : IRequest<QueryResult<RevenueReportDTO>>
    {
        public int Week { get; set; }
        public int Year { get; set; }
    }
}
