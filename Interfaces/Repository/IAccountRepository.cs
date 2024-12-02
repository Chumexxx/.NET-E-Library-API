using Microsoft.AspNetCore.Identity;
using ModernLibrary.DTOs.Account;
using ModernLibrary.Models;

namespace ModernLibrary.Interfaces.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> RegisterCustomerAsync(CustomerRegisterDto customerRegisterDto);
        Task<IdentityResult> RegisterStaffAsync(StaffRegisterDto staffRegisterDto);
        Task<IdentityResult> RegisterSuperAdminAsync(SuperAdminRegisterDto superAdminRegisterDto);
        Task<AppUser> FindUserByUsernameAsync(string username);
        Task<bool> CheckPasswordAsync(AppUser user, string password);
        Task<string> CreateTokenAsync(AppUser user);
    }
}
