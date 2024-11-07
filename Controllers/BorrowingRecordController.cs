using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernLibrary.Data;
using ModernLibrary.DTOs.Book;
using ModernLibrary.DTOs.BorrowingRecord;
using ModernLibrary.Extensions;
using ModernLibrary.Interfaces;
using ModernLibrary.Mappers;
using ModernLibrary.Models;
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
        private readonly ApplicationDBContext _context;

        public BorrowingRecordController(UserManager<AppUser> userManager, IBookRepository bookRepo, 
            IBorrowingRecordRepository borrowingRecordRepo, IBorrowedBookRepository borrowedBookRepo, 
            ApplicationDBContext context)
        {
            _userManager = userManager;
            _bookRepo = bookRepo;
            _borrowingRecordRepo = borrowingRecordRepo;
            _borrowedBookRepo = borrowedBookRepo;
            _context = context;
        }

        [HttpGet("getAllBorrowingRecords")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> GetAllBorrowingRecords()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var borrowingRecords = await _borrowingRecordRepo.GetAllBorrowingRecordAsync();

            var borrowingRecordDto = borrowingRecords.Select(s => s.ToBorrowingRecordDto());

            return Ok(borrowingRecordDto);
        }


        [HttpGet("getBorrowingRecord")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin")]
        public async Task<IActionResult> GetUserBorrowingRecord()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var username = User.GetUsername();

            var appUser = await _userManager.FindByNameAsync(username);

            var borrowingRecord = await _borrowingRecordRepo.GetUserBorrowingRecordAsync(appUser);

            var borrowingRecordDto = borrowingRecord.Select(s => s.ToBorrowingRecordDto());

            return Ok(borrowingRecordDto);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin")]
        public async Task<IActionResult> GetBorrowingRecordById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();

            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
                return NotFound("User not found");

            var userBorrowingRecord = await _borrowingRecordRepo.GetBorrowingRecordById(appUser, id);

            if (userBorrowingRecord == null)
                return NotFound($"No BorrowRequest found for the user with ID {id}");


            return Ok(userBorrowingRecord.ToBorrowingRecordDto());

        }


        [HttpPost("makeBorrowRequest")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin")]
        public async Task<IActionResult> MakeBorrowRequest([FromBody] CreateBorrowingRecordRequestDto recordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
                return NotFound("User not found");

            var borrowedBooks = new List<BorrowedBook>();
            var errorMessages = new List<string>();

            var borrowingBook = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var bookRequest in recordDto.Books)
                {
                    var book = await _bookRepo.GetByIdAsync(bookRequest.BookId);

                    if (book == null)
                    {
                        errorMessages.Add($"Book with ID {bookRequest.BookId} does not exist.");
                        continue;
                    }

                    if (bookRequest.QtyNeeded != 1)
                    {
                        errorMessages.Add($"You can only borrow 1 copy of '{book.BookName}'. Requested quantity: {bookRequest.QtyNeeded}");
                        continue;
                    }

                    if (book.NumofBooksAvailable <= 0)
                    {
                        errorMessages.Add($"Book '{book.BookName}' is currently out of stock.");
                        continue;
                    }

                    var existingBorrow = await _context.BorrowingRecords
                        .Include(br => br.BorrowedBooks)
                        .Where(br => br.AppUserId == appUser.Id && !br.IsReturned && !br.IsCancelled)
                        .FirstOrDefaultAsync(br => br.BorrowedBooks.Any(bb => bb.BookId == bookRequest.BookId));

                    if (existingBorrow != null)
                    {
                        errorMessages.Add($"You already have an active borrow record for '{book.BookName}'. " +
                                        "Please return or cancel the existing borrow record before borrowing it again.");
                    }
                }

                if (errorMessages.Any())
                {
                    await borrowingBook.RollbackAsync();
                    return BadRequest(new
                    {
                        Message = "Some books could not be borrowed. Please review the errors below.",
                        Errors = errorMessages
                    });
                }

                foreach (var bookRequest in recordDto.Books)
                {
                    var book = await _bookRepo.GetByIdAsync(bookRequest.BookId);

                    book.NumofBooksAvailable -= 1;
                    var bookDto = new UpdateBookRequestDto
                    {
                        NumofBooksAvailable = book.NumofBooksAvailable,
                        Publisher = book.Publisher,
                        Genre = book.Genre,
                        Synopsis = book.Synopsis,
                    };
                    await _bookRepo.UpdateAsync(book.BookId, bookDto);

                    var borrowedBook = new BorrowedBook
                    {
                        BookName = book.BookName,
                        BookId = book.BookId,
                        QtyNeeded = 1,
                    };
                    borrowedBooks.Add(borrowedBook);
                }

                var recordModel = new BorrowingRecord
                {
                    AppUserId = appUser.Id,
                    BorrowedBy = appUser.UserName,
                    BorrowedBooks = borrowedBooks,
                };

                await _borrowingRecordRepo.CreateBorrowingRecordAsync(recordModel);
                await borrowingBook.CommitAsync();

                return Ok(new
                {
                    Message = $"You have successfully borrowed {borrowedBooks.Count} book(s)",
                    BorrowedBooks = borrowedBooks.Select(b => new {
                        BookName = b.BookName,
                        BookId = b.BookId
                    }).ToList(),
                    DueDate = recordModel.DueDate
                });
            }
            catch (Exception ex)
            {
                await borrowingBook.RollbackAsync();
                return StatusCode(500, "An error occurred while trying to borrow the book(s).");
            }
        }

        [HttpPut("returnBookBorrowed")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin")]
        public async Task<IActionResult> ReturnBookBorrowed(int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var borrowingRecord = await _borrowingRecordRepo.GetBorrowingRecordById(appUser, Id);

            if (borrowingRecord == null)
                return NotFound("Borrow Request not found");

            if (borrowingRecord.IsReturned)
                return BadRequest("Book has already been returned");

            if (borrowingRecord.IsCancelled)
                return BadRequest("Cannot return a book that already has a cancelled request. Kindly verify your Id");

            borrowingRecord.ReturnDate = DateTime.Now;

            await _borrowingRecordRepo.ReturnBorrowingRecordAsync(appUser, Id);

            foreach (var borrowedBooks in borrowingRecord.BorrowedBooks)
            {
                var book = await _bookRepo.GetByIdAsync(borrowedBooks.BookId);

                if (book == null)
                    return BadRequest($"Book with ID {borrowedBooks.BookId} not found");

                book.NumofBooksAvailable += borrowedBooks.QtyNeeded;

                var bookDto = new UpdateBookRequestDto
                {
                    NumofBooksAvailable = book.NumofBooksAvailable,
                    Publisher = book.Publisher,
                    Genre = book.Genre,
                    Synopsis = book.Synopsis,
                };

                await _bookRepo.UpdateAsync(book.BookId, bookDto);
            }

            var returnMessage = borrowingRecord.IsOverdue
                ? $"Thanks for returning the book. Note: You are {borrowingRecord.OverdueDays} days overdue."
                : "Thanks for returning the book you borrowed";

            return Ok(new
            {
                message = returnMessage,
                isOverdue = borrowingRecord.IsOverdue,
                overdueDays = borrowingRecord.OverdueDays
            });
        }

        [HttpPut("cancelBorrowRequest")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin")]
        public async Task<IActionResult> CancelBorrowRequest(int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var borrowingRecord = await _borrowingRecordRepo.GetBorrowingRecordById(appUser, Id);

            if (borrowingRecord == null)
                return NotFound("BorrowRequest not found");

            if (borrowingRecord.IsCancelled)
                return BadRequest("BorrowRequest is already cancelled");

            if (borrowingRecord.IsReturned)
                return BadRequest("Cannot cancel a BorrowRequest for a book that has already been returned. Kindly verify your BorrowingRecordID");

            await _borrowingRecordRepo.CancelBorrowingRecordAsync(appUser, Id);

            foreach (var borrowedBook in borrowingRecord.BorrowedBooks)
            {
                var book = await _bookRepo.GetByIdAsync(borrowedBook.BookId);

                if (book == null)
                    return BadRequest($"Book with ID {borrowedBook.BookId} not found");

                book.NumofBooksAvailable += borrowedBook.QtyNeeded;

                var bookDto = new UpdateBookRequestDto
                {
                    NumofBooksAvailable = book.NumofBooksAvailable,
                    Publisher = book.Publisher,
                    Genre = book.Genre,
                    Synopsis = book.Synopsis,
                };

                await _bookRepo.UpdateAsync(book.BookId, bookDto);
            }

            return Ok(new { message = "BorrowRequest was successfully cancelled" });
        }


        [HttpGet("overdueRecords")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> GetOverdueRecords()
        {
            var overdueRecords = await _borrowingRecordRepo.CheckAndUpdateOverdueRecordsAsync();
            return Ok(overdueRecords.Select(r => new
            {
                r.BorrowingRecordId,
                r.BorrowedBy,
                r.DueDate,
                r.OverdueDays
            }));
        }

    }

}
