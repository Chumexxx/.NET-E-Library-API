using ModernLibrary.DTOs.BorrowingRecord;
using ModernLibrary.Helpers;
using ModernLibrary.Models;

namespace ModernLibrary.Interfaces.Repository
{
    public interface IBorrowingRecordRepository
    {

        Task<List<BorrowingRecord>> GetUserPendingBorrowingRecordAsync(AppUser user);
        Task<List<BorrowingRecord>> GetAllUserBorrowingRecordAsync(AppUser user);
        Task<List<BorrowingRecord>> GetAllBorrowingRecordAsync(string? userId, BorrowingRecordQueryObjects query);
        Task<BorrowingRecord?> GetBorrowingRecordById(AppUser appUser, int id);
        Task<BorrowingRecord> CreateBorrowingRecordAsync(BorrowingRecord recordModel);
        Task<BorrowingRecord> ReturnBorrowingRecordAsync(AppUser user, int id);
        Task<BorrowingRecord> CancelBorrowingRecordAsync(AppUser user, int id);
        Task<List<BorrowingRecord>> GetAllOverdueRecordsAsync();
        Task<List<BorrowingRecord>> GetAllUserOverdueRecordsAsync(AppUser user);
    }
}
