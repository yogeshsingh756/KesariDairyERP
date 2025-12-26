using KesariDairyERP.Api.Authorization;
using KesariDairyERP.Application.DTOs.Users;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KesariDairyERP.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [HasPermission(Permissions.UserView)]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetUsersAsync());
        }

        [HttpPost]
        [HasPermission(Permissions.UserCreate)]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            await _userService.CreateUserAsync(dto);
            return Ok("User created successfully");
        }

        [HttpPut]
        [HasPermission(Permissions.UserEdit)]
        public async Task<IActionResult> UpdateUser(UpdateUserDto dto)
        {
            await _userService.UpdateUserAsync(dto);
            return Ok("User updated successfully");
        }

        [HttpDelete("{id:long}")]
        [HasPermission(Permissions.UserDelete)]
        public async Task<IActionResult> DeleteUser(long id)
        {
            await _userService.SoftDeleteUserAsync(id);
            return Ok("User deleted successfully");
        }
        [HttpGet("{id:long}")]
        [HasPermission(Permissions.UserView)]
        public async Task<IActionResult> GetById(long id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }
    }
}
