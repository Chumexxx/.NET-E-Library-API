using Microsoft.AspNetCore.Identity;

namespace ModernLibrary.Interfaces.Repository
{
    public interface IRoleRepository
    {
        Task<IdentityUser> FindUserByIdAsync(string userId);
        Task<bool> IsUserInRoleAsync(IdentityUser user, string roleName);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> AddUserToRoleAsync(IdentityUser user, string role);
        Task<IdentityResult> RemoveUserFromRoleAsync(IdentityUser user, string role);
        Task<IList<string>> GetUserRolesAsync(IdentityUser user);
        IQueryable<IdentityRole> GetAllRoles();
    }
}
