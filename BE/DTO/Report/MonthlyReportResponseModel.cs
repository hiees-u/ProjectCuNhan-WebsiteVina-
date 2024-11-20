﻿namespace DTO.Report
{
    public class MonthlyReportResponseModel
    {
        public int Month { get; set; }              
        public int Year { get; set; }               
        public string ProductId { get; set; }       
        public string ProductName { get; set; }     
        public int TotalSales { get; set; }         
        public decimal TotalRevenue { get; set; }   
    }
}
