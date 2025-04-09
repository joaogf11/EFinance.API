using EFinnance.API.Services;
using EFinnance.API.ViewModels.Revenue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFinnance.API.Controllers
{
    [Route("api/revenue")]
    [ApiController]
    [Authorize]
    public class RevenueController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public RevenueController(
            AppDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        [HttpGet("get-revenue")]
        public async Task<ActionResult<IEnumerable<RevenueViewModel>>> GetRevenues()
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
                .Include(r => r.Category)
                .Select(r => new RevenueViewModel
                {
                    Id = r.Id,
                    Salary = r.Salary,
                    OtherIncomes = r.OtherIncomes,
                    Description = r.Description,
                    IncomeDate = r.IncomeDate,
                    CategoryId = r.CategoryId,
                    UserId = r.UserId
                })
                .ToListAsync();

            return Ok(revenues);
        }

        [HttpPost("new-revenue")]
        public async Task<ActionResult<RevenueViewModel>> CreateRevenue([FromBody] RevenueViewModel viewModel)
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

            var revenueEntity = new RevenueViewModel
            {
                Salary = viewModel.Salary,
                OtherIncomes = viewModel.OtherIncomes,
                Description = viewModel.Description,
                IncomeDate = viewModel.IncomeDate,
                CategoryId = viewModel.CategoryId,
                UserId = userId,
                User = user
            };

            _context.Revenues.Add(revenueEntity);
            await _context.SaveChangesAsync();

            viewModel.Id = revenueEntity.Id;
            return CreatedAtAction(nameof(GetRevenues), new { id = viewModel.Id }, viewModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRevenue(string id, [FromBody] RevenueViewModel revenue)
        {
            var unauthorizedResult = this.UnauthorizedIfNoUser();
            if (unauthorizedResult != null) return unauthorizedResult;

            if (id != revenue.Id)
            {
                return BadRequest();
            }

            _context.Entry(revenue).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRevenue(string id)
        {
            var unauthorizedResult = this.UnauthorizedIfNoUser();
            if (unauthorizedResult != null) return unauthorizedResult;

            var revenue = await _context.Revenues.FindAsync(id);
            if (revenue == null)
                return NotFound();

            _context.Revenues.Remove(revenue);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}