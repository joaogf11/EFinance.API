using EFinnance.API.ViewModels.Expense;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace EFinnance.API.Controllers.Expense
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
            private readonly AppDbContext _context;

        public ExpenseController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseViewModel>>> GetExpenses()
        {
            return await _context.Expenses.Include(e => e.Category).Include(e => e.User).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseViewModel>> CreateExpense(ExpenseViewModel expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetExpenses), new { id = expense.Id }, expense);
        }
    }
}
