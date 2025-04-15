using Microsoft.AspNetCore.Mvc;
using EFinnance.API.Services;

namespace EFinnance.API.Controllers
{
    public static class ControllerExtensions
    {
        public static ActionResult UnauthorizedIfNoUser(this ControllerBase controller)
        {
            var currentUserService = controller.HttpContext.RequestServices.GetService(typeof(ICurrentUserService)) as ICurrentUserService;
            if (currentUserService == null || currentUserService.GetUserId() == null)
            {
                return controller.Unauthorized();
            }

            return null;
        }
    }
}