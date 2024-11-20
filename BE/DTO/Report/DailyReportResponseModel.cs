namespace DTO.Report
{
    public class DailyReportResponseModel
    {
        public DateTime? InputDate { get; set; }
        public string ProductId { get; set; }    
        public string ProductName { get; set; }  
        public int TotalSales { get; set; }      
        public decimal TotalRevenue { get; set; } 
    }
}
