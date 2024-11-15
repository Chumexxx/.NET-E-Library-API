using ModernLibrary.DTOs.BorrowingRecord;
using ModernLibrary.Models;

namespace ModernLibrary.Interfaces
{
    public interface IBorrowingRecordRepository
    {

        Task<List<BorrowingRecord>> GetUserBorrowingRecordAsync(AppUser user);
        Task<List<BorrowingRecord>> GetAllBorrowingRecordAsync();
        Task<BorrowingRecord?> GetBorrowingRecordById(AppUser appUser, int id);
        Task<BorrowingRecord> CreateBorrowingRecordAsync(BorrowingRecord recordModel);
        Task<BorrowingRecord> ReturnBorrowingRecordAsync(AppUser user, int id);
        Task<BorrowingRecord> CancelBorrowingRecordAsync(AppUser user, int id);
        Task<List<BorrowingRecord>> CheckAndUpdateOverdueRecordsAsync();
    }
}
