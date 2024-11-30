using BLL.Interface;
using DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReport report;

        public ReportController(IReport report)
        {
            this.report = report;
        }
        [HttpGet("Daily")]
        [Authorize(Roles = "Moderator")]
        public IActionResult GetDailySalesReport(DateTime InputDate)
        {
            BaseResponseModel res = this.report.GetDailySalesReport(InputDate);
            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }

        [HttpGet("Weekly")]
        [Authorize(Roles = "Moderator")]
        public IActionResult GetWeeklySalesReport(DateTime StartDate, DateTime EndDate) 
        {
            var res = this.report.GetWeeklySalesReport(StartDate, EndDate);
            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }

        [HttpGet("Monthly")]
        [Authorize(Roles = "Moderator")]
        public IActionResult GetMonthlySalesReport(int Month, int Year)
        {
            var res = this.report.GetMonthlySalesReport(Month, Year);
            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }

        [HttpGet("Yearly")]
        [Authorize(Roles = "Moderator")]
        public IActionResult GetYearlySalesReport(int Year)
        {
            var res = this.report.GetYearlySalesReport(Year);
            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }
    }
}
