using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModernLibrary.DTOs.Account;
using ModernLibrary.Interfaces.Repository;
using ModernLibrary.Interfaces.Service;
using ModernLibrary.Models;

namespace ModernLibrary.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountRepository(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<IdentityResult> RegisterCustomerAsync(CustomerRegisterDto customerRegisterDto)
        {
            var user = await _userManager.FindByEmailAsync(customerRegisterDto.Email);
            if (user != null)
                throw new Exception("Email is already being used.");

            var appUser = new AppUser
            {
                UserName = customerRegisterDto.UserName,
                FirstName = customerRegisterDto.FirstName,
                LastName = customerRegisterDto.LastName,
                Email = customerRegisterDto.Email,
                DateOfBirth = customerRegisterDto.DateOfBirth,
                HomeAddress = customerRegisterDto.HomeAddress,
                PhoneNumber = customerRegisterDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(appUser, customerRegisterDto.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "Customer");
                return roleResult;
            }

            return result;
        }

        public async Task<IdentityResult> RegisterStaffAsync(StaffRegisterDto staffRegisterDto)
        {
            var user = await _userManager.FindByEmailAsync(staffRegisterDto.Email);
            if (user != null)
                throw new Exception("Email is already being used.");

            var appUser = new AppUser
            {
                UserName = staffRegisterDto.UserName,
                FirstName = staffRegisterDto.FirstName,
                LastName = staffRegisterDto.LastName,
                Email = staffRegisterDto.Email,
                DateOfBirth = staffRegisterDto.DateOfBirth,
                HomeAddress = staffRegisterDto.HomeAddress,
                PhoneNumber = staffRegisterDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(appUser, staffRegisterDto.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "Staff");
                return roleResult;
            }

            return result;
        }

        public async Task<IdentityResult> RegisterSuperAdminAsync(SuperAdminRegisterDto superAdminRegisterDto)
        {
            var user = await _userManager.FindByEmailAsync(superAdminRegisterDto.Email);
            if (user != null)
                throw new Exception("Email is already being used.");

            var appUser = new AppUser
            {
                UserName = superAdminRegisterDto.UserName,
                FirstName = superAdminRegisterDto.FirstName,
                LastName = superAdminRegisterDto.LastName,
                Email = superAdminRegisterDto.Email,
                DateOfBirth= superAdminRegisterDto.DateOfBirth,
                HomeAddress = superAdminRegisterDto.HomeAddress,
                PhoneNumber = superAdminRegisterDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(appUser, superAdminRegisterDto.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "SuperAdmin");
                return roleResult;
            }

            return result;
        }

        public async Task<AppUser> FindUserByUsernameAsync(string username)
        {
            return await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == username.ToLower());
        }

        public async Task<bool> CheckPasswordAsync(AppUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<string> CreateTokenAsync(AppUser user)
        {
            return await _tokenService.CreateToken(user);
        }
    }
}
