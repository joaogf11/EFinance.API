using EFinnance.API.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EFinnance.API.Services
{
    public interface ICurrentUserService
    {
        string? GetUserId();
        bool IsAuthenticated();
        Task<UserViewModel?> GetUserByIdAsync(string userId); // Novo método
    }

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public string? GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
        }

        public async Task<UserViewModel?> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }

    // Método de extensão para controllers
    public static class ControllerExtensions
    {
        public static ActionResult UnauthorizedIfNoUser(this ControllerBase controller)
        {
            var userId = controller.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(userId)
                ? controller.Unauthorized("Efetue login.")
                : null;
        }
    }
}