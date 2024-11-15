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
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetAllBorrowingRecords([FromQuery] BorrowingRecordQueryObjects query)
        {
            var borrowingRecords = await _borrowingRecordRepo.GetAllBorrowingRecordAsync(query);

            var borrowingRecordDto = borrowingRecords.Select(s => s.ToBorrowingRecordDto());

            return Ok(borrowingRecordDto);
        }


        [HttpGet("getBorrowingRecord")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetUserBorrowingRecord()
        {
            var username = User.GetUsername();

            var appUser = await _userManager.FindByNameAsync(username);

            var borrowingRecord = await _borrowingRecordRepo.GetUserBorrowingRecordAsync(appUser);

            var borrowingRecordDto = borrowingRecord.Select(s => s.ToBorrowingRecordDto());

            return Ok(borrowingRecordDto);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetBorrowingRecordById([FromRoute] int id)
        {
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
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> MakeBorrowRequest([FromBody] CreateBorrowingRecordRequestDto recordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            if (appUser == null)
                return NotFound("User not found");

            if (recordDto.Books.Count > 2)
            {
                return BadRequest(new
                {
                    Message = "You cannot borrow more than 2 books at once."
                });
            }

            var numOfBooksBorrowed = await _context.BorrowingRecords
                .Include(br => br.BorrowedBooks)
                .Where(br => br.AppUserId == appUser.Id && !br.IsReturned && !br.IsCancelled)
                .SelectMany(br => br.BorrowedBooks)
                .CountAsync();

            var remainingBorrowLimit = 2 - numOfBooksBorrowed;

            if (remainingBorrowLimit <= 0)
            {
                return BadRequest(new
                {
                    Message = "You have reached your maximum borrow limit of 2 books. Please return some books before borrowing more."
                });
            }

            if (recordDto.Books.Count > remainingBorrowLimit)
            {
                return BadRequest(new
                {
                    Message = $"You can only borrow {remainingBorrowLimit} more book(s) at this time. You currently have {numOfBooksBorrowed} book(s) borrowed."
                });
            }

            var borrowedBooks = new List<BorrowedBook>();
            var borrowingBook = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var bookRequest in recordDto.Books)
                {
                    var book = await _bookRepo.GetByIdAsync(bookRequest.BookId);

                    if (book == null)
                    {
                        await borrowingBook.RollbackAsync();
                        return BadRequest(new
                        {
                            Message = $"Book with ID {bookRequest.BookId} does not exist.",
                        });
                    }

                    if (book.NumofBooksAvailable <= 0)
                    {
                        await borrowingBook.RollbackAsync();
                        return BadRequest(new
                        {
                            Message = $"Book '{book.BookName}' is currently out of stock.",
                        });
                    }

                    if (bookRequest.QtyNeeded != 1)
                    {
                        await borrowingBook.RollbackAsync();
                        return BadRequest(new
                        {
                            Message = $"You can only borrow 1 copy of '{book.BookName}'. Requested quantity: {bookRequest.QtyNeeded}",
                        });
                    }

                    // Check if user already has this specific book borrowed
                    var existingBorrow = await _context.BorrowingRecords
                        .Include(br => br.BorrowedBooks)
                        .Where(br => br.AppUserId == appUser.Id && !br.IsReturned && !br.IsCancelled)
                        .FirstOrDefaultAsync(br => br.BorrowedBooks.Any(bb => bb.BookId == bookRequest.BookId));

                    if (existingBorrow != null)
                    {
                        await borrowingBook.RollbackAsync();
                        return BadRequest(new
                        {
                            Message = $"You already have an active borrow record for '{book.BookName}'. " +
                                     "Please return or cancel the existing borrow record before borrowing it again."
                        });
                    }

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

                    book.NumofBooksAvailable -= 1;
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
                    BorrowedBooks = borrowedBooks.Select(b => new {
                        BookName = b.BookName,
                        BookId = b.BookId
                    }).ToList(),
                    DueDate = recordModel.DueDate,
                    RemainingBorrowLimit = $"You have {remainingBorrowLimit - recordDto.Books.Count} book(s) left to borrow. Return to borrow again"

                });

            }
            catch (Exception ex)
            {
                await borrowingBook.RollbackAsync();
                return StatusCode(500, "An error occurred while trying to borrow the book(s).");
            }
        }

        [HttpPut("returnBookBorrowed")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
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
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
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
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetOverdueRecords()
        {
            var overdueRecords = await _borrowingRecordRepo.CheckOverdueRecordsAsync();
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
