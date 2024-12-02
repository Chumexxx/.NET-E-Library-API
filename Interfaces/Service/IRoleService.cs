using Microsoft.AspNetCore.Identity;
using ModernLibrary.DTOs.Role;

namespace ModernLibrary.Interfaces.Service
{
    public interface IRoleService
    {
        Task<RoleOperationResultDto> AssignRoleToStaffAsync(string userId, string role);
        Task<RoleOperationResultDto> RemoveRoleFromUserAsync(string userId, string role);
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task<IEnumerable<string>> GetAllRolesAsync();
    }
}
