using Microsoft.EntityFrameworkCore;
using ModernLibrary.Data;
using ModernLibrary.DTOs.BorrowingRecord;
using ModernLibrary.Interfaces;
using ModernLibrary.Models;

namespace ModernLibrary.Repository
{
    public class BorrowedBookRepository : IBorrowedBookRepository
    {
        private readonly ApplicationDBContext _context;
        public BorrowedBookRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<BorrowedBook> CreateAsync(BorrowedBook borrowedBookModel)
        {
            await _context.BorrowedBooks.AddAsync(borrowedBookModel);
            await _context.SaveChangesAsync();
            return borrowedBookModel;
        }

        public async Task<BorrowedBook?> GetBorrowedBookAsync(int bookId)
        {
            return await _context.BorrowedBooks.FirstOrDefaultAsync(oi => oi.BookId == bookId);
        }

        public async Task<BorrowedBook> UpdateBorrowedBookAsync(int bookId, UpdateBorrowingRecordRequestDto updateDto)
        {
            var existingBook = await _context.BorrowedBooks.FirstOrDefaultAsync(oi => oi.BookId == bookId);

            if (existingBook == null)
            {
                return null;
            }

            await _context.SaveChangesAsync();

            return existingBook;
        }
    }
}
