using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModernLibrary.Data;
using ModernLibrary.Models;
using System.Data;

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

        [HttpPost("assignRole")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var isStaff = await _userManager.IsInRoleAsync(user, "Staff");
            if (!isStaff)
            {
                return BadRequest(new
                {
                    Message = "Role can only be assigned to Staff members. This user is not a Staff member."
                });
            }

            if (!await _roleManager.RoleExistsAsync(role))
            {
                return BadRequest("Role does not exist");
            }

            var result = await _userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                return Ok($"Role {role} assigned to staff member {user.UserName}");
            }

            return BadRequest("Failed to assign role");
        }

        [HttpPost("removeRole")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                return BadRequest($"Role {role} does not exist");
            }

            if (role.Equals("Customer", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new
                {
                    Message = "Cannot remove Customer role as it is a default user role."
                });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (result.Succeeded)
            {
                return Ok($"Role {role} successfully removed from user {user.UserName}");
            }

            return BadRequest("Failed to remove role");
        }

        [HttpGet("getUserRole")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpGet("showAllRoles")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles;
            return Ok(roles);
        }
    }
}
