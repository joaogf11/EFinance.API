using EFinnance.API.Services;
using EFinnance.API.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFinnance.API.Controllers.Category
{
    [Route("api/category")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CategoryController(
            AppDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        [HttpGet("get-category")]
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetCategories()
        {
            var unauthorizedResult = this.UnauthorizedIfNoUser();
            if (unauthorizedResult != null) return unauthorizedResult;

            var userId = _currentUserService.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var categories = await _context.Categories
                .Where(c => c.UserId == userId)
                .Include(c => c.User)
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    UserId = c.UserId
                })
                .ToListAsync();

            return Ok(categories);
        }

        [HttpPost("new-category")]
        public async Task<ActionResult<CategoryViewModel>> CreateCategory([FromBody] CategoryViewModel category)
        {
            var unauthorizedResult = this.UnauthorizedIfNoUser();
            if (unauthorizedResult != null) return unauthorizedResult;

            var userId = _currentUserService.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var categoryEntity = new CategoryViewModel
            {
                Title = category.Title,
                UserId = userId
            };

            _context.Categories.Add(categoryEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategories), new { id = categoryEntity.Id }, categoryEntity);
        }
    }
}