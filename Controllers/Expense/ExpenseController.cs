using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using EFinnance.API;
using EFinnance.API.ViewModels.Expense;


namespace EFinnance.API.Controllers.Expense
{
    [Route("api/expense")]
    [ApiController]
    [Authorize] 
    public class ExpenseController : ControllerBase
    {
            private readonly AppDbContext _context;

        public ExpenseController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-expense")]
        public async Task<ActionResult<IEnumerable<ExpenseViewModel>>> GetExpenses()
        {
            return await _context.Expenses.Include(e => e.Category).Include(e => e.User).ToListAsync();
        }

        [HttpPost("new-expense")]
        public async Task<ActionResult<ExpenseViewModel>> CreateExpense(ExpenseViewModel expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return Ok(new{expense.Id});
        }
    }
}
