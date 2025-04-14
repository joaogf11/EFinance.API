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

            var user = await _currentUserService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Usuario não encontrado");
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
                    UserId = userId,
                    User = user
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

            var user = await _currentUserService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Usuario não encontrado");
            }

            var expenseEntity = new ExpenseViewModel
            {
                Value = viewModel.Value,
                DueDate = viewModel.DueDate,
                CategoryId = viewModel.CategoryId,
                UserId = userId,
                User = user
            };

            _context.Expenses.Add(expenseEntity);
            await _context.SaveChangesAsync();

            viewModel.Id = expenseEntity.Id;
            return CreatedAtAction(nameof(GetExpenses), new { id = viewModel.Id }, viewModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(string id, [FromBody] ExpenseViewModel expense)
        {
            var unauthorizedResult = this.UnauthorizedIfNoUser();
            if (unauthorizedResult != null) return unauthorizedResult;

            if (id != expense.Id)
            {
                return BadRequest();
            }

            _context.Entry(expense).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(string id)
        {
            var unauthorizedResult = this.UnauthorizedIfNoUser();
            if (unauthorizedResult != null) return unauthorizedResult;

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
                return NotFound();

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}