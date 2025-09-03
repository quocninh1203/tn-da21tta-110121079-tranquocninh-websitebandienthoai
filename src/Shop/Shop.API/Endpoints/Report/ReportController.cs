// Shop.API.Controllers/ReportController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Requests.Reports;

namespace Shop.API.Endpoints.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReportController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Báo cáo doanh thu ngày hôm nay
        /// </summary>
        [HttpGet("today")]
        public async Task<IActionResult> GetTodayReport()
        {
            var today = DateTime.Today;
            var req = new GetReportRequest
            {
                StartDate = today,
                EndDate = today.AddDays(1).AddTicks(-1)
            };
            var res = await _mediator.Send(req);
            return res.Success ? Ok(res.Model) : NotFound(res.Message);
        }

        /// <summary>
        /// Báo cáo theo tháng & năm
        /// </summary>
        [HttpGet("month")]
        public async Task<IActionResult> GetMonthlyReport([FromQuery] int month, [FromQuery] int year)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1).AddTicks(-1);
            var req = new GetReportRequest { StartDate = start, EndDate = end };
            var res = await _mediator.Send(req);
            return res.Success ? Ok(res.Model) : NotFound(res.Message);
        }

        /// <summary>
        /// Báo cáo theo năm
        /// </summary>
        [HttpGet("year")]
        public async Task<IActionResult> GetYearlyReport([FromQuery] int year)
        {
            var start = new DateTime(year, 1, 1);
            var end = new DateTime(year, 12, 31);
            var req = new GetReportRequest { StartDate = start, EndDate = end };
            var res = await _mediator.Send(req);
            return res.Success ? Ok(res.Model) : NotFound(res.Message);
        }

        /// <summary>
        /// Báo cáo theo khoảng ngày tùy chọn
        /// </summary>
        [HttpGet("range")]
        public async Task<IActionResult> GetCustomRangeReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var req = new GetReportRequest
            {
                StartDate = startDate,
                EndDate = endDate.Date.AddDays(1).AddTicks(-1)
            };
            var res = await _mediator.Send(req);
            return res.Success ? Ok(res.Model) : NotFound(res.Message);
        }
    }
}
