using EFinnance.API.Services;
using EFinnance.API.ViewModels.Expense;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFinnance.API.Controllers
{
    [Route("api/expense")]
    [ApiController]
    [Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public ExpenseController(
            AppDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        [HttpGet("get-expense")]
        public async Task<ActionResult<IEnumerable<ExpenseViewModel>>> GetExpenses()
        {
            var unauthorizedResult = this.UnauthorizedIfNoUser();
            if (unauthorizedResult != null) return unauthorizedResult;

            var userId = _currentUserService.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId)
                .Include(e => e.Category)
                .Select(e => new ExpenseViewModel
                {
                    Id = e.Id,
                    Value = e.Value,
                    DueDate = e.DueDate,
                    CategoryId = e.CategoryId,
                    UserId = e.UserId
                })
                .ToListAsync();

            return Ok(expenses);
        }

        [HttpPost("new-expense")]
        public async Task<ActionResult<ExpenseViewModel>> CreateExpense([FromBody] ExpenseViewModel viewModel)
        {
            var unauthorizedResult = this.UnauthorizedIfNoUser();
            if (unauthorizedResult != null) return unauthorizedResult;

            var userId = _currentUserService.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var expenseEntity = new ExpenseViewModel
            {
                Value = viewModel.Value,
                DueDate = viewModel.DueDate,
                CategoryId = viewModel.CategoryId,
                UserId = userId
            };

            _context.Expenses.Add(expenseEntity);
            await _context.SaveChangesAsync();

            viewModel.Id = expenseEntity.Id;
            return CreatedAtAction(nameof(GetExpenses), new { id = viewModel.Id }, viewModel);
        }
    }
}