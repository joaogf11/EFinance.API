using EFinnance.API.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;

namespace EFinnance.API.Controllers.Category
{
    [Route("api/category")]
    [ApiController]
    [Authorize] 
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-category")]
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetCategories()
        {
            var category = await _context.Categories.FindAsync();
            if (category == null)
                return NotFound();

            return await _context.Categories.ToListAsync();
        }

        [HttpPost("new-category")]
        public async Task<ActionResult<CategoryViewModel>> CreateCategory(CategoryViewModel category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(new{ category.Id });
        }
    }
}
