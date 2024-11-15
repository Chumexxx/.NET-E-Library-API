using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernLibrary.DTOs.Account;
using ModernLibrary.Interfaces;
using ModernLibrary.Models;
using System.Security.Cryptography.Xml;

namespace ModernLibrary.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signinManager;
        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService, SignInManager<AppUser> signinManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _signinManager = signinManager;
        }


        [HttpPost("registerCustomer")]
        public async Task<IActionResult> CustomerRegister([FromBody] CustomerRegisterDto customerRegisterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userManager.FindByEmailAsync(customerRegisterDto.Email);
                if (user != null) return BadRequest("Email is already being used.");

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

                var createdUser = await _userManager.CreateAsync(appUser, customerRegisterDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "Customer");

                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new SignedInDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("registerStaff")]
        public async Task<IActionResult> StaffRegister([FromBody] StaffRegisterDto staffRegisterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userManager.FindByEmailAsync(staffRegisterDto.Email);
                if (user != null) return BadRequest("Email is already being used.");

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

                var createdUser = await _userManager.CreateAsync(appUser, staffRegisterDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "Staff");

                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new SignedInDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("registerLibrarian")]
        public async Task<IActionResult> LibrarianRegister([FromBody] LibrarianRegisterDto librarianRegisterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userManager.FindByEmailAsync(librarianRegisterDto.Email);
                if (user != null) return BadRequest("Email is already being used.");

                var appUser = new AppUser
                {
                    UserName = librarianRegisterDto.UserName,
                    FirstName = librarianRegisterDto.FirstName,
                    LastName = librarianRegisterDto.LastName,
                    Email = librarianRegisterDto.Email,
                    PhoneNumber = librarianRegisterDto.PhoneNumber,
                    DateOfBirth = librarianRegisterDto.DateOfBirth,
                    HomeAddress = librarianRegisterDto.HomeAddress,
                };

                var createdUser = await _userManager.CreateAsync(appUser, librarianRegisterDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "Librarian");

                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new SignedInDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


        [HttpPost("registerSuperAdmin")]
        public async Task<IActionResult> SuperAdminRegister([FromBody] SuperAdminRegisterDto superAdminRegisterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userManager.FindByEmailAsync(superAdminRegisterDto.Email);
                if (user != null) return BadRequest("Email is already being used.");

                var appUser = new AppUser
                {
                    UserName = superAdminRegisterDto.UserName,
                    FirstName = superAdminRegisterDto.FirstName,
                    LastName = superAdminRegisterDto.LastName,
                    Email = superAdminRegisterDto.Email,
                    PhoneNumber = superAdminRegisterDto.PhoneNumber,
                };

                var createdUser = await _userManager.CreateAsync(appUser, superAdminRegisterDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "SuperAdmin");

                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new SignedInDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null)
                return Unauthorized("Invalid username");

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Username not found and/or incorrect Password");

            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = await _tokenService.CreateToken(user)
                }
            );
        }

    }
}