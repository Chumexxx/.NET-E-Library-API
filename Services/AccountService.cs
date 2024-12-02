using ModernLibrary.DTOs.Account;
using ModernLibrary.Interfaces.Repository;
using ModernLibrary.Interfaces.Service;
using ModernLibrary.Models;

namespace ModernLibrary.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;

        public AccountService(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public async Task<SignedInDto> RegisterCustomerAsync(CustomerRegisterDto customerRegisterDto)
        {
            var result = await _accountRepo.RegisterCustomerAsync(customerRegisterDto);

            if (!result.Succeeded)
                throw new Exception(result.Errors.FirstOrDefault()?.Description ?? "Registration failed");

            var user = await _accountRepo.FindUserByUsernameAsync(customerRegisterDto.UserName);

            return new SignedInDto
            {
                userId = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }

        public async Task<SignedInDto> RegisterStaffAsync(StaffRegisterDto staffRegisterDto)
        {
            var result = await _accountRepo.RegisterStaffAsync(staffRegisterDto);

            if (!result.Succeeded)
                throw new Exception(result.Errors.FirstOrDefault()?.Description ?? "Registration failed");

            var user = await _accountRepo.FindUserByUsernameAsync(staffRegisterDto.UserName);

            return new SignedInDto
            {
                userId = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }

        public async Task<SignedInDto> RegisterSuperAdminAsync(SuperAdminRegisterDto superAdminRegisterDto)
        {
            var result = await _accountRepo.RegisterSuperAdminAsync(superAdminRegisterDto);

            if (!result.Succeeded)
                throw new Exception(result.Errors.FirstOrDefault()?.Description ?? "Registration failed");

            var user = await _accountRepo.FindUserByUsernameAsync(superAdminRegisterDto.UserName);

            return new SignedInDto
            {
                userId = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }

        public async Task<NewUserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _accountRepo.FindUserByUsernameAsync(loginDto.Username);

            if (user == null)
                throw new Exception("Invalid username");

            var passwordCheck = await _accountRepo.CheckPasswordAsync(user, loginDto.Password);

            if (!passwordCheck)
                throw new Exception("Invalid credentials");

            return new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = await _accountRepo.CreateTokenAsync(user)
            };
        }
    }
}
