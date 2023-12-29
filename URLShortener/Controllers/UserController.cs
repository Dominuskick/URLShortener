using AutoMapper;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using URLShortener.BLL.DTOs;
using URLShortener.BLL.Services.Interfaces;
using URLShortener.Data;

namespace URLShortener.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        #region Constructor
        private IUserService _userService;

        public UserController(
            UserManager<ApplicationUser> userManager,
            ILogger<UserController> logger,
            IConfiguration configuration,
            IMapper mapper,
            IUserService us) : base(userManager, mapper, logger, configuration)
        {
            _userService = us;
        }
        #endregion

        #region Crud
        /// <summary>
        /// Searches for users
        /// </summary>
        /// <param name="req"></param>
        /// <returns>Returns a paged list of matching users</returns>
        /*[HttpGet]
        public ActionResult Get([FromQuery] UserSearchInput req) =>
            Respond(_userService.SearchUsers(req));*/

        // GET api/user/5        
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id) =>
            Respond(await _userService.GetUser(id));

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserInput dto) =>
            Respond(await _userService.AddUser(dto));

        // PUT api/user/5        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UserSimpleDTO dto) =>
            Respond(await _userService.UpdateUser(id, dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id) =>
            Respond(await _userService.DeleteUser(id));


        #endregion


        #region Roles

        // GET api/user/5/roles
        [HttpGet("{userid}/roles")]
        public ActionResult GetUserRoles(Guid userid) =>
            Respond(_userService.GetUserRoles(userid));

        [HttpPost("{userid}/role")]
        public async Task<ActionResult> AddRoleToUser(Guid userid, [FromBody] UserDTO req) =>
            Respond(await _userService.AddRoleToUser(userid, req));


        [HttpDelete("{userid}/role/{roleid}")]
        public async Task<ActionResult> DeleteRoleFromUser(Guid userid, Guid roleid) =>
            Respond(await _userService.DeleteRoleFromUser(userid, roleid));

        #endregion
    }
}
