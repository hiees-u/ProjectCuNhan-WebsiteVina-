using DTO.Report;
using DTO.Responses;

namespace BLL.Interface
{
    public interface IReport
    {
        public BaseResponseModel GetDailySalesReport(DateTime InputDate);

        public BaseResponseModel GetWeeklySalesReport(DateTime StartDate, DateTime EndDate);

        public BaseResponseModel GetMonthlySalesReport(int Month, int Year);

        public BaseResponseModel GetYearlySalesReport(int Year);

    }
}
