namespace DTO.Report
{
    public class RangeReportCustomerTypeResponseModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CustomerType { get; set; }
        public decimal Revenue { get; set; }
    }
}
