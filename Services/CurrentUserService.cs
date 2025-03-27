using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EFinnance.API.Services
{
    public interface ICurrentUserService
    {
        string? GetUserId();
        bool IsAuthenticated();
    }

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
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