using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernLibrary.DTOs.Account;
using ModernLibrary.Interfaces.Service;
using ModernLibrary.Models;
using ModernLibrary.Services;
using System.Security.Cryptography.Xml;

namespace ModernLibrary.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost("registerCustomer")]
        public async Task<IActionResult> CustomerRegister([FromBody] CustomerRegisterDto customerRegisterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _accountService.RegisterCustomerAsync(customerRegisterDto);
                Console.WriteLine("user hit the customer register endpoint");
                return Ok(result);
            }
            catch (Exception ex) {
                Console.WriteLine("Error in customerRegister endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("registerStaff")]
        public async Task<IActionResult> StaffRegister([FromBody] StaffRegisterDto staffRegisterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _accountService.RegisterStaffAsync(staffRegisterDto);
                Console.WriteLine("user hit the staff register endpoint");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in staffRegister endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("registerSuperAdmin")]
        public async Task<IActionResult> SuperAdminRegister([FromBody] SuperAdminRegisterDto superAdminRegisterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _accountService.RegisterSuperAdminAsync(superAdminRegisterDto);
                Console.WriteLine("user hit the super admin register endpoint");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in superAdminRegister endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _accountService.LoginAsync(loginDto);
                Console.WriteLine("user hit the login endpoint");
                return Ok(result);
            }
            catch (Exception ex) 
            { 
                Console.WriteLine("Error in login endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }

        }
    }
}