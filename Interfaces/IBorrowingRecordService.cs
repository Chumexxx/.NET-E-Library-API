using ModernLibrary.DTOs.BorrowingRecord;
using ModernLibrary.Helpers;
using ModernLibrary.Models;

namespace ModernLibrary.Interfaces
{
    public interface IBorrowingRecordService
    {
        Task<IEnumerable<BorrowingRecordDto>> GetAllBorrowingRecordsAsync(string? userId, BorrowingRecordQueryObjects query);
        Task<IEnumerable<BorrowingRecordDto>> GetAllUserBorrowingRecordsAsync(string username);
        Task<IEnumerable<BorrowingRecordDto>> GetUserPendingBorrowingRecordsAsync(string username);
        Task<BorrowingRecordDto?> GetBorrowingRecordByIdAsync(string username, int id);
        Task<Object> MakeBorrowRequestAsync(string username, CreateBorrowingRecordRequestDto recordDto);
        Task<Object> ReturnBookBorrowedAsync(string username,  int id);
        Task<Object> CancelBorrowRequestAsync(string username, int id);
        Task<IEnumerable<Object>> GetAllOverdueRecordsAsync();
        Task<IEnumerable<Object>> GetAllUserOverdueRecordsAsync(string username);
    }
}
