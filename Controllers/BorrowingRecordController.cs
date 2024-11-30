using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernLibrary.Data;
using ModernLibrary.DTOs.Book;
using ModernLibrary.DTOs.BorrowingRecord;
using ModernLibrary.Extensions;
using ModernLibrary.Helpers;
using ModernLibrary.Interfaces;
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
        private readonly IBookRepository _bookRepo;
        private readonly IBorrowingRecordRepository _borrowingRecordRepo;
        private readonly IBorrowedBookRepository _borrowedBookRepo;
        private readonly IBorrowingRecordService _borrowingRecordService;
        private readonly ApplicationDBContext _context;

        public BorrowingRecordController(UserManager<AppUser> userManager, IBookRepository bookRepo, IBorrowedBookRepository borrowedBookRepo,
            IBorrowingRecordService borrowingRecordService, ApplicationDBContext context, IBorrowingRecordRepository borrowingRecordRepo)
        {
            _userManager = userManager;
            _bookRepo = bookRepo;
            _borrowedBookRepo = borrowedBookRepo;
            _borrowingRecordService = borrowingRecordService;
            _context = context;
            _borrowingRecordRepo = borrowingRecordRepo;
        }

        [HttpGet("getAllBorrowingRecords")]
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetAllBorrowingRecords([FromQuery] BorrowingRecordQueryObjects query, [FromQuery] string? userId = null)
        {
            //var borrowingRecords = await _borrowingRecordRepo.GetAllBorrowingRecordAsync(userId, query);

            //var borrowingRecordDto = borrowingRecords.Select(s => s.ToBorrowingRecordDto());

            //return Ok(borrowingRecordDto);
            var borrowingRecords = await _borrowingRecordService.GetAllBorrowingRecordsAsync(userId, query);

            return Ok(borrowingRecords);
        }



        [HttpGet("getAllUserBorrowingRecord")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetAllUserBorrowingRecord()
        {
            var username = User.GetUsername();

            var borrowingRecords = await _borrowingRecordService.GetAllUserBorrowingRecordsAsync(username);
            return Ok(borrowingRecords);
        }


        [HttpGet("getUserPendingBorrowingRecord")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetUserPendingBorrowingRecord()
        {
            var username = User.GetUsername();

            var borrowingRecords = await _borrowingRecordService.GetUserPendingBorrowingRecordsAsync(username);
            return Ok(borrowingRecords);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetBorrowingRecordById([FromRoute] int id)
        {
            var username = User.GetUsername();

            //var appUser = await _userManager.FindByNameAsync(username);

            //if (appUser == null)
            //    return NotFound("User not found");

            //var userBorrowingRecord = await _borrowingRecordRepo.GetBorrowingRecordById(appUser, id);

            //if (userBorrowingRecord == null)
            //    return NotFound($"No BorrowRequest found for the user with ID {id}");

            //return Ok(userBorrowingRecord.ToBorrowingRecordDto());

            var userBorrowingRecord = await _borrowingRecordService.GetBorrowingRecordByIdAsync(username, id);
            return Ok(userBorrowingRecord);

        }


        [HttpPost("makeBorrowRequest")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> MakeBorrowRequest([FromBody] CreateBorrowingRecordRequestDto recordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();

            var borrowBook = await _borrowingRecordService.MakeBorrowRequestAsync(username, recordDto);
            return Ok(borrowBook);
        }

        [HttpPut("returnBookBorrowed")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> ReturnBookBorrowed(int id)
        {
            var username = User.GetUsername();
            var returnBook = await _borrowingRecordService.ReturnBookBorrowedAsync(username, id);
            return Ok(returnBook);
        }

        [HttpPut("cancelBorrowRequest")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> CancelBorrowRequest(int Id)
        {
            var username = User.GetUsername();
            var cancelBook = await _borrowingRecordService.CancelBorrowRequestAsync(username, Id);
            return Ok(cancelBook);
        }


        [HttpGet("getAllOverdueRecords")]
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetAllOverdueRecords()
        {
            var overdueRecords = await _borrowingRecordService.GetAllOverdueRecordsAsync();
            return Ok(overdueRecords);
        }

        [HttpGet("getAllUserOverdueRecords")]
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetAllUserOverdueRecords()
        {
            var username = User.GetUsername();

            var userOverdueRecords = await _borrowingRecordService.GetAllUserOverdueRecordsAsync(username);
            return Ok(userOverdueRecords);
        }

    }

}
