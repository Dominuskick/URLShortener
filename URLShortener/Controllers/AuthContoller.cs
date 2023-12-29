using AutoMapper;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using URLShortener.BLL.DTOs;
using URLShortener.BLL.Services.Interfaces;
using URLShortener.Data;

namespace URLShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthContoller : BaseController
    {
        private IUserService _userService;
        protected RoleManager<ApplicationRole> _roleManager;
        private IAuthService _authService;


        public AuthContoller(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration,
            IMapper mapper,
            IUserService us,
            ILogger<UserDbContext> logger,
            IAuthService asvc) : base(userManager, mapper, logger, configuration)
        {
            _roleManager = roleManager;
            _userService = us;
            _authService = asvc;
        }

        #region Login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model) =>
            Respond(await _authService.Login(model));

        #endregion

        #region Validations
        [AllowAnonymous]
        [HttpPost("confirmemail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmInput model) =>
            Respond(await _authService.ConfirmEmail(model));

        [AllowAnonymous]
        [HttpPost("confirmpassword")]
        public async Task<IActionResult> ConfirmPassword([FromBody] ConfirmInput model) =>
            Respond(await _authService.ConfirmPassword(model));

        [AllowAnonymous]
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] string email) =>
            Respond(await _authService.ResetPassword(email));

        #endregion
    }
}
