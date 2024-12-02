using ModernLibrary.DTOs.BorrowingRecord;
using ModernLibrary.Models;

namespace ModernLibrary.Interfaces.Repository
{
    public interface IBorrowedBookRepository
    {
        Task<BorrowedBook> CreateAsync(BorrowedBook borrowedBookModel);
        Task<BorrowedBook?> GetBorrowedBookAsync(int bookId);
        Task<BorrowedBook> UpdateBorrowedBookAsync(int bookId, UpdateBorrowingRecordRequestDto updateDto);
    }
}
