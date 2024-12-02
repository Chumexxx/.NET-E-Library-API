using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModernLibrary.Data;
using ModernLibrary.DTOs.Book;
using ModernLibrary.DTOs.BorrowedBook;
using ModernLibrary.DTOs.BorrowingRecord;
using ModernLibrary.Helpers;
using ModernLibrary.Interfaces.Repository;
using ModernLibrary.Interfaces.Service;
using ModernLibrary.Mappers;
using ModernLibrary.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ModernLibrary.Services
{
    public class BorrowingRecordService : IBorrowingRecordService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IBookRepository _bookRepo;
        private readonly IBorrowingRecordRepository _borrowingRecordRepo;
        private readonly ApplicationDBContext _context;

        public BorrowingRecordService(UserManager<AppUser> userManager, IBookRepository bookRepo, IBorrowingRecordRepository borrowingRecordRepo,
            ApplicationDBContext context)
        {
            _userManager = userManager;
            _bookRepo = bookRepo;
            _borrowingRecordRepo = borrowingRecordRepo;
            _context = context;

        }
        public async Task<List<BorrowingRecordDto>> GetAllBorrowingRecordsAsync(string? userId, BorrowingRecordQueryObjects query)
        {
            var borrowingRecords = await _borrowingRecordRepo.GetAllBorrowingRecordAsync(userId, query);
            return borrowingRecords.Select(s => s.ToBorrowingRecordDto()).ToList();
        }

        public async Task<List<BorrowingRecordDto>> GetAllUserBorrowingRecordsAsync(string username)
        {
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
                throw new Exception("User not found");

            var borrowingRecord = await _borrowingRecordRepo.GetAllUserBorrowingRecordAsync(appUser);
            return borrowingRecord.Select(s => s.ToBorrowingRecordDto()).ToList();
        }

        public async Task<List<BorrowingRecordDto>> GetUserPendingBorrowingRecordsAsync(string username)
        {
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
                throw new Exception("User not found");

            var borrowingRecord = await _borrowingRecordRepo.GetUserPendingBorrowingRecordAsync(appUser);
            return borrowingRecord.Select(s => s.ToBorrowingRecordDto()).ToList();
        }

        public async Task<BorrowingRecordDto?> GetBorrowingRecordByIdAsync(string username, int id)
        {
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
                throw new Exception("User not found");

            var userBorrowingRecord = await _borrowingRecordRepo.GetBorrowingRecordById(appUser, id);

            return userBorrowingRecord?.ToBorrowingRecordDto()
                ?? throw new Exception($"No BorrowRequest found for the user with ID {id}");
        }

        public async Task<Object> MakeBorrowRequestAsync(string username, CreateBorrowingRecordRequestDto recordDto)
        {
            var appUser = await _userManager.FindByNameAsync(username);

            if (appUser == null)
                throw new Exception("User not found");

            if (recordDto.Books.Count > 2)
            {
                return (new
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
                return (new
                {
                    Message = "You have reached your maximum borrow limit of 2 books. Please return some books before borrowing more."
                });
            }

            if (recordDto.Books.Count > remainingBorrowLimit)
            {
                return (new
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
                    var book = await _bookRepo.GetByBookIdAsync(bookRequest.BookId);

                    if (book == null)
                    {
                        await borrowingBook.RollbackAsync();
                        return (new
                        {
                            Message = $"Book with ID {bookRequest.BookId} does not exist.",
                        });
                    }

                    if (book.NumofBooksAvailable <= 0)
                    {
                        await borrowingBook.RollbackAsync();
                        return (new
                        {
                            Message = $"Book '{book.BookName}' is currently out of stock.",
                        });
                    }

                    if (bookRequest.QtyNeeded != 1)
                    {
                        await borrowingBook.RollbackAsync();
                        return (new
                        {
                            Message = $"You can only borrow 1 copy of '{book.BookName}'. Requested quantity: {bookRequest.QtyNeeded}",
                        });
                    }

                    var existingBorrow = await _context.BorrowingRecords
                        .Include(br => br.BorrowedBooks)
                        .Where(br => br.AppUserId == appUser.Id && !br.IsReturned && !br.IsCancelled)
                        .FirstOrDefaultAsync(br => br.BorrowedBooks.Any(bb => bb.BookId == bookRequest.BookId));

                    if (existingBorrow != null)
                    {
                        await borrowingBook.RollbackAsync();
                        return (new
                        {
                            Message = $"You already have an active borrow record for '{book.BookName}'. " +
                                     "Please return or cancel the existing borrow record before borrowing it again."
                        });
                    }

                    var bookDto = new UpdateBookRequestDto
                    {
                        NumofBooksAvailable = book.NumofBooksAvailable,
                        Synopsis = book.Synopsis,
                        Genre = book.Genre,
                        Publisher = book.Publisher,
                        PublishDate = book.PublishDate,
                    };
                    await _bookRepo.UpdateBookAsync(book.BookId, bookDto);

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

                return (new
                {
                    BorrowedBooks = borrowedBooks.Select(b => new
                    {
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
                return (500, "An error occurred while trying to borrow the book(s).");
            }
        }

        public async Task<Object> ReturnBookBorrowedAsync(string username, int id)
        {
            var appUser = await _userManager.FindByNameAsync(username);

            var borrowingRecord = await _borrowingRecordRepo.GetBorrowingRecordById(appUser, id);

            if (borrowingRecord == null)
                return ("Borrow Request not found");

            if (borrowingRecord.IsReturned)
                return ("Book has already been returned");

            if (borrowingRecord.IsCancelled)
                return("Cannot return a book that already has a cancelled request. Kindly verify your Id");

            borrowingRecord.ReturnDate = DateTime.Now;

            await _borrowingRecordRepo.ReturnBorrowingRecordAsync(appUser, id);

            foreach (var borrowedBooks in borrowingRecord.BorrowedBooks)
            {
                var book = await _bookRepo.GetByBookIdAsync(borrowedBooks.BookId);

                if (book == null)
                    return($"Book with ID {borrowedBooks.BookId} not found");

                book.NumofBooksAvailable += borrowedBooks.QtyNeeded;

                var bookDto = new UpdateBookRequestDto
                {
                    NumofBooksAvailable = book.NumofBooksAvailable,
                    Publisher = book.Publisher,
                    Genre = book.Genre,
                    Synopsis = book.Synopsis,
                };

                await _bookRepo.UpdateBookAsync(book.BookId, bookDto);
            }

            var returnMessage = borrowingRecord.IsOverdue
                ? $"Thanks for returning the book. Note: You are {borrowingRecord.OverdueDays} days overdue."
                : "Thanks for returning the book you borrowed";

            return (new
            {
                message = returnMessage,
                isOverdue = borrowingRecord.IsOverdue,
                overdueDays = borrowingRecord.OverdueDays
            });
        }

        public async Task<Object> CancelBorrowRequestAsync(string username, int id)
        {
            var appUser = await _userManager.FindByNameAsync(username);

            var borrowingRecord = await _borrowingRecordRepo.GetBorrowingRecordById(appUser, id);

            if (borrowingRecord == null)
                return("BorrowRequest not found");

            if (borrowingRecord.IsCancelled)
                return("BorrowRequest is already cancelled");

            if (borrowingRecord.IsReturned)
                return("Cannot cancel a BorrowRequest for a book that has already been returned. Kindly verify your BorrowingRecordID");

            await _borrowingRecordRepo.CancelBorrowingRecordAsync(appUser, id);

            foreach (var borrowedBook in borrowingRecord.BorrowedBooks)
            {
                var book = await _bookRepo.GetByBookIdAsync(borrowedBook.BookId);

                if (book == null)
                    return($"Book with ID {borrowedBook.BookId} not found");

                book.NumofBooksAvailable += borrowedBook.QtyNeeded;

                var bookDto = new UpdateBookRequestDto
                {
                    NumofBooksAvailable = book.NumofBooksAvailable,
                    Publisher = book.Publisher,
                    Genre = book.Genre,
                    Synopsis = book.Synopsis,
                };

                await _bookRepo.UpdateBookAsync(book.BookId, bookDto);
            }

            return (new { message = "BorrowRequest was successfully cancelled" });
        }

        public async Task<IEnumerable<Object>> GetAllOverdueRecordsAsync()
        {
            var overdueRecords = await _borrowingRecordRepo.GetAllOverdueRecordsAsync();
            return overdueRecords.Select(r => new 
            {
                BorrowingRecordId = r.BorrowingRecordId,
                BorrowedBy = r.BorrowedBy,
                DueDate = r.DueDate,
                OverdueDays = r.OverdueDays
            });
        }

        public async Task<IEnumerable<object>> GetAllUserOverdueRecordsAsync(string username)
        {
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
                throw new Exception("User not found");

            var overdueRecords = await _borrowingRecordRepo.GetAllUserOverdueRecordsAsync(appUser);
            return overdueRecords.Select(r => new
            {
                BorrowingRecordId = r.BorrowingRecordId,
                BorrowedBy = r.BorrowedBy,
                DueDate = r.DueDate,
                OverdueDays = r.OverdueDays
            });
        }

    }


}
