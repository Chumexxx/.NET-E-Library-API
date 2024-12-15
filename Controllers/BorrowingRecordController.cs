using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernLibrary.Data;
using ModernLibrary.DTOs.Book;
using ModernLibrary.DTOs.BorrowingRecord;
using ModernLibrary.Extensions;
using ModernLibrary.Helpers;
using ModernLibrary.Interfaces.Repository;
using ModernLibrary.Interfaces.Service;
using ModernLibrary.Mappers;
using ModernLibrary.Models;
using ModernLibrary.Services;
using System.ComponentModel.DataAnnotations;

namespace ModernLibrary.Controllers
{
    [Route("api/borrowingRecord")]
    [ApiController]
    [Authorize]
    public class BorrowingRecordController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IBorrowedBookRepository _borrowedBookRepo;
        private readonly IBorrowingRecordService _borrowingRecordService;
        private readonly ApplicationDBContext _context;

        public BorrowingRecordController(UserManager<AppUser> userManager, IBookRepository bookRepo, IBorrowedBookRepository borrowedBookRepo,
            IBorrowingRecordService borrowingRecordService, ApplicationDBContext context)
        {
            _userManager = userManager;
            _borrowedBookRepo = borrowedBookRepo;
            _borrowingRecordService = borrowingRecordService;
            _context = context;
        }

        [HttpGet("getAllBorrowingRecords")]
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetAllBorrowingRecords([FromQuery] BorrowingRecordQueryObjects query, [FromQuery] string? userId = null)
        {
            try
            {
                var borrowingRecords = await _borrowingRecordService.GetAllBorrowingRecordsAsync(userId, query);
                Console.WriteLine("user called get all borrowing records endpoint");
                return Ok(borrowingRecords);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in get all borrowing records endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getAllUserBorrowingRecord")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetAllUserBorrowingRecord()
        {
            try
            {
                var username = User.GetUsername();

                var borrowingRecords = await _borrowingRecordService.GetAllUserBorrowingRecordsAsync(username);
                Console.WriteLine("user called get all user borrowing record endpoint");
                return Ok(borrowingRecords);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in get all user borrowing record endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getUserPendingBorrowingRecord")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetUserPendingBorrowingRecord()
        {
            try
            {
                var username = User.GetUsername();

                var borrowingRecords = await _borrowingRecordService.GetUserPendingBorrowingRecordsAsync(username);
                Console.WriteLine("user called get user pending borrowing record endpoint");
                return Ok(borrowingRecords);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in user pending borrowing record endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetBorrowingRecordById([FromRoute] int id)
        {
            try
            {
                var username = User.GetUsername();

                var userBorrowingRecord = await _borrowingRecordService.GetBorrowingRecordByIdAsync(username, id);
                Console.WriteLine("user called get borrowing record by id endpoint");
                return Ok(userBorrowingRecord);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in get borrowing record by id endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("makeBorrowRequest")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> MakeBorrowRequest([FromBody] CreateBorrowingRecordRequestDto recordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var username = User.GetUsername();

                var borrowBook = await _borrowingRecordService.MakeBorrowRequestAsync(username, recordDto);
                Console.WriteLine("user called make borrow request endpoint");
                return Ok(borrowBook);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in makee borrow request endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("returnBookBorrowed")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> ReturnBookBorrowed(int id)
        {
            try
            {
                var username = User.GetUsername();

                var returnBook = await _borrowingRecordService.ReturnBookBorrowedAsync(username, id);
                Console.WriteLine("user called return book endpoint");
                return Ok(returnBook);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in return borrowed book endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("cancelBorrowRequest")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> CancelBorrowRequest(int Id)
        {
            try
            {
                var username = User.GetUsername();

                var cancelBook = await _borrowingRecordService.CancelBorrowRequestAsync(username, Id);
                Console.WriteLine("user called cancel borrow request endpoint");
                return Ok(cancelBook);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in cancel borrow request endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("getAllOverdueRecords")]
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetAllOverdueRecords()
        {
            try
            {
                var overdueRecords = await _borrowingRecordService.GetAllOverdueRecordsAsync();
                Console.WriteLine("user called get all overdue records endpoint");
                return Ok(overdueRecords);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in get all overdue records endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getAllUserOverdueRecords")]
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetAllUserOverdueRecords()
        {
            try
            {
                var username = User.GetUsername();

                var userOverdueRecords = await _borrowingRecordService.GetAllUserOverdueRecordsAsync(username);
                Console.WriteLine("user called get all user overdue records endpoint");
                return Ok(userOverdueRecords);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in get user overdue records endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
            
        }

    }

}
