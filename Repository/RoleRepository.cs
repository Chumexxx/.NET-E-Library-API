using Microsoft.AspNetCore.Identity;
using ModernLibrary.Interfaces.Repository;

namespace ModernLibrary.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleRepository(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityUser> FindUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<bool> IsUserInRoleAsync(IdentityUser user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<IdentityResult> AddUserToRoleAsync(IdentityUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> RemoveUserFromRoleAsync(IdentityUser user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IList<string>> GetUserRolesAsync(IdentityUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public IQueryable<IdentityRole> GetAllRoles()
        {
            return _roleManager.Roles;
        }
    }
}
