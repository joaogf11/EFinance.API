using EFinnance.API.ViewModels.Category;
using Microsoft.EntityFrameworkCore;

namespace EFinnance.API.Services
{
    public interface ICategoryService
    {
        Task<CategoryViewModel?> GetByIdAsync(string id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public CategoryService(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<CategoryViewModel?> GetByIdAsync(string id)
        {
            return await _context.Categories.FirstOrDefaultAsync(u => u.Title == id);
        }
    }
    
 }
