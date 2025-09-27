using CareNest_Products.API.Extensions;
using CareNest_Products.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CareNest_Products.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly ICurrentUserService _currentUserService;

        protected BaseController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        protected string? CurrentUserId => _currentUserService.UserId;
        protected string? CurrentUserRole => _currentUserService.Role;
    }
}
