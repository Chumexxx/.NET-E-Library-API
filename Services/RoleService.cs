using Microsoft.AspNetCore.Identity;
using ModernLibrary.DTOs.Role;
using ModernLibrary.Interfaces.Repository;
using ModernLibrary.Interfaces.Service;

namespace ModernLibrary.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RoleOperationResultDto> AssignRoleToStaffAsync(string userId, string role)
        {
            var user = await _roleRepository.FindUserByIdAsync(userId);
            if (user == null)
            {
                return new RoleOperationResultDto
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var isStaff = await _roleRepository.IsUserInRoleAsync(user, "Staff");
            if (!isStaff)
            {
                return new RoleOperationResultDto
                {
                    Success = false,
                    Message = "Role can only be assigned to Staff members. This user is not a Staff member."
                };
            }

            if (!await _roleRepository.RoleExistsAsync(role))
            {
                return new RoleOperationResultDto
                {
                    Success = false,
                    Message = "Role does not exist"
                };
            }

            var result = await _roleRepository.AddUserToRoleAsync(user, role);
            return new RoleOperationResultDto
            {
                Success = result.Succeeded,
                Message = result.Succeeded
                    ? $"Role {role} assigned to staff member {user.UserName}"
                    : "Failed to assign role"
            };
        }

        public async Task<RoleOperationResultDto> RemoveRoleFromUserAsync(string userId, string role)
        {
            if (!await _roleRepository.RoleExistsAsync(role))
            {
                return new RoleOperationResultDto
                {
                    Success = false,
                    Message = $"Role {role} does not exist"
                };
            }

            if (role.Equals("Customer", StringComparison.OrdinalIgnoreCase))
            {
                return new RoleOperationResultDto
                {
                    Success = false,
                    Message = "Cannot remove Customer role as it is a default user role."
                };
            }

            var user = await _roleRepository.FindUserByIdAsync(userId);
            if (user == null)
            {
                return new RoleOperationResultDto
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var result = await _roleRepository.RemoveUserFromRoleAsync(user, role);
            return new RoleOperationResultDto
            {
                Success = result.Succeeded,
                Message = result.Succeeded
                    ? $"Role {role} successfully removed from user {user.UserName}"
                    : "Failed to remove role"
            };
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _roleRepository.FindUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return await _roleRepository.GetUserRolesAsync(user);
        }

        public async Task<IEnumerable<string?>> GetAllRolesAsync()
        {
            return _roleRepository.GetAllRoles().Select(r => r.Name);
        }
    }
}
