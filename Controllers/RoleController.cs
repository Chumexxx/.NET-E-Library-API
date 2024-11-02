using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModernLibrary.Data;
using ModernLibrary.Models;

namespace ModernLibrary.Controllers
{
    [Route("api/Roles")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(ApplicationDBContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost("addRoleToExistingUser")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddRoleToUser(string UserName, string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                return BadRequest($"Role {roleName} does not exist");
            }

            var userName = await _userManager.FindByNameAsync(UserName);
            if (userName == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.AddToRoleAsync(userName, roleName);
            if (result.Succeeded)
            {
                return Ok($"Role {roleName} successfully added to user {userName.UserName}");
            }

            return BadRequest("Failed to add role");
        }

        [HttpPost("removeRoleFromExistingUser")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RemoveRoleFromUser(string UserName, string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                return BadRequest($"Role {roleName} does not exist");
            }

            var userName = await _userManager.FindByNameAsync(UserName);
            if (userName == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.RemoveFromRoleAsync(userName, roleName);
            if (result.Succeeded)
            {
                return Ok($"Role {roleName} successfully removed from user {userName.UserName}");
            }

            return BadRequest("Failed to remove role");
        }

        [HttpGet("getRoleOfAUser")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetUserRoles(string UserName)
        {
            var userName = await _userManager.FindByNameAsync(UserName);
            if (userName == null)
            {
                return NotFound("User not found");
            }

            var roles = await _userManager.GetRolesAsync(userName);
            return Ok(roles);
        }

        [HttpGet("showAllUserRoles")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles;
            return Ok(roles);
        }
    }
}
