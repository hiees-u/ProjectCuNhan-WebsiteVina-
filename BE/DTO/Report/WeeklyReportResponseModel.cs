namespace DTO.Report
{
    public class WeeklyReportResponseModel
    {
        public DateTime? StartDate { get; set; }    
        public DateTime? EndDate { get; set; }  
        public string ProductId { get; set; } 
        public string ProductName { get; set; } 
        public int TotalSales { get; set; }     
        public decimal TotalRevenue { get; set; }  
    }
}
