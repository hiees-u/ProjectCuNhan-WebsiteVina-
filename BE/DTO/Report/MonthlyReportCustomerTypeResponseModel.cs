namespace DTO.Report
{
    public class MonthlyReportCustomerTypeResponseModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string CustomerType { get; set; }
        public decimal Revenue { get; set; }
    }
}
