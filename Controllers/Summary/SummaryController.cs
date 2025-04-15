using EFinnance.API.Services;
using EFinnance.API.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFinnance.API.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DashboardController(
            AppDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<DashboardSummaryViewModel>> GetSummary()
        {
            var unauthorizedResult = this.UnauthorizedIfNoUser();
            if (unauthorizedResult != null) return unauthorizedResult;

            var userId = _currentUserService.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var revenues = await _context.Revenues
                .Where(r => r.UserId == userId)
                .ToListAsync();

            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId)
                .ToListAsync();

            decimal totalRevenue = revenues.Sum(r => r.Salary + r.OtherIncomes);
            decimal totalExpense = expenses.Sum(e => e.Value);
            decimal balance = totalRevenue - totalExpense;

            var summary = new DashboardSummaryViewModel
            {
                TotalRevenue = totalRevenue,
                TotalExpense = totalExpense,
                Balance = balance
            };

            return Ok(summary);
        }

        [HttpGet("recent-transactions")]
        public async Task<ActionResult<IEnumerable<TransactionViewModel>>> GetRecentTransactions()
        {
            var unauthorizedResult = this.UnauthorizedIfNoUser();
            if (unauthorizedResult != null) return unauthorizedResult;

            var userId = _currentUserService.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var recentRevenues = await _context.Revenues
                .Where(r => r.UserId == userId)
                .Include(r => r.Category)
                .OrderByDescending(r => r.IncomeDate)
                .Take(5)
                .ToListAsync();

            var revenueTransactions = recentRevenues.Select(r => new TransactionViewModel
            {
                Id = r.Id,
                Date = r.IncomeDate,
                Description = r.Description,
                Category = r.Category.Description,
                Amount = r.Salary + r.OtherIncomes,
                Type = "revenue"
            }).ToList();

            var recentExpenses = await _context.Expenses
                .Where(e => e.UserId == userId)
                .Include(e => e.Category)
                .OrderByDescending(e => e.DueDate)
                .Take(5)
                .ToListAsync();

            var expenseTransactions = recentExpenses.Select(e => new TransactionViewModel
            {
                Id = e.Id,
                Date = e.DueDate,
                Category = e.Category.Description,
                Amount = e.Value,
                Type = "expense"
            }).ToList();

            var combinedTransactions = revenueTransactions
                .Concat(expenseTransactions)
                .OrderByDescending(t => t.Date)
                .Take(10)
                .ToList();

            return Ok(combinedTransactions);
        }

        [HttpGet("monthly-data")]
        public async Task<ActionResult<IEnumerable<MonthlyDataViewModel>>> GetMonthlyData()
        {
            var unauthorizedResult = this.UnauthorizedIfNoUser();
            if (unauthorizedResult != null) return unauthorizedResult;

            var userId = _currentUserService.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var today = DateTime.Today;
            var sixMonthsAgo = today.AddMonths(-6);
            
            var revenues = await _context.Revenues
                .Where(r => r.UserId == userId && r.IncomeDate >= sixMonthsAgo)
                .ToListAsync();
                
            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId && e.DueDate >= sixMonthsAgo)
                .ToListAsync();

            var months = Enumerable.Range(0, 7)
                .Select(i => sixMonthsAgo.AddMonths(i))
                .ToList();

            var result = new List<MonthlyDataViewModel>();

            foreach (var date in months)
            {
                var year = date.Year;
                var month = date.Month;
                var monthName = date.ToString("MMM", new System.Globalization.CultureInfo("pt-BR"));
                
                var monthlyRevenue = revenues
                    .Where(r => r.IncomeDate.Year == year && r.IncomeDate.Month == month)
                    .Sum(r => r.Salary + r.OtherIncomes);
                    
                var monthlyExpenses = expenses
                    .Where(e => e.DueDate.Year == year && e.DueDate.Month == month)
                    .Sum(e => e.Value);

                result.Add(new MonthlyDataViewModel
                {
                    Month = monthName,
                    Revenue = monthlyRevenue,
                    Expenses = monthlyExpenses
                });
            }

            return Ok(result);
        }
    }
}