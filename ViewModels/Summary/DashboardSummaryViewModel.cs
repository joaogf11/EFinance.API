using System;

namespace EFinnance.API.ViewModels.Dashboard
{
    public class DashboardSummaryViewModel
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance { get; set; }
    }

    public class TransactionViewModel
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } 
    }

    public class MonthlyDataViewModel
    {
        public string Month { get; set; }
        public decimal Revenue { get; set; }
        public decimal Expenses { get; set; }
    }
}