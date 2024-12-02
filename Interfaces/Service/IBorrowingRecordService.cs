using ModernLibrary.DTOs.BorrowingRecord;
using ModernLibrary.Helpers;
using ModernLibrary.Models;

namespace ModernLibrary.Interfaces.Service
{
    public interface IBorrowingRecordService
    {
        Task<List<BorrowingRecordDto>> GetAllBorrowingRecordsAsync(string? userId, BorrowingRecordQueryObjects query);
        Task<List<BorrowingRecordDto>> GetAllUserBorrowingRecordsAsync(string username);
        Task<List<BorrowingRecordDto>> GetUserPendingBorrowingRecordsAsync(string username);
        Task<BorrowingRecordDto?> GetBorrowingRecordByIdAsync(string username, int id);
        Task<object> MakeBorrowRequestAsync(string username, CreateBorrowingRecordRequestDto recordDto);
        Task<object> ReturnBookBorrowedAsync(string username, int id);
        Task<object> CancelBorrowRequestAsync(string username, int id);
        Task<IEnumerable<object>> GetAllOverdueRecordsAsync();
        Task<IEnumerable<object>> GetAllUserOverdueRecordsAsync(string username);
    }
}
