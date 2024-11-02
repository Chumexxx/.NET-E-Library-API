using Microsoft.EntityFrameworkCore;
using ModernLibrary.Data;
using ModernLibrary.DTOs.Book;
using ModernLibrary.DTOs.BorrowingRecord;
using ModernLibrary.Interfaces;
using ModernLibrary.Models;

namespace ModernLibrary.Repository
{
    public class BorrowingRecordRepository : IBorrowingRecordRepository
    {
        private readonly ApplicationDBContext _context;
        public BorrowingRecordRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<BorrowingRecord> CancelBorrowingRecordAsync(AppUser user, int id)
        {
            var borrowingRecordModel = await _context.BorrowingRecords.FirstOrDefaultAsync(x => x.BorrowingRecordId == id);

            if (borrowingRecordModel == null)
            {
                return null;
            }

            borrowingRecordModel.IsCancelled = true;
            borrowingRecordModel.CancelDate = DateTime.Now;

            _context.BorrowingRecords.Update(borrowingRecordModel);
            await _context.SaveChangesAsync();
            return borrowingRecordModel;
        }

        public async Task<List<BorrowingRecord>> CheckAndUpdateOverdueRecordsAsync()
        {
            var currentDate = DateTime.Now;
            var overdueRecords = await _context.BorrowingRecords
                .Where(br => !br.IsReturned && !br.IsCancelled && br.DueDate < currentDate)
                .ToListAsync();

            foreach (var record in overdueRecords)
            {
                record.IsOverdue = true;
                record.OverdueDays = (int)Math.Ceiling((currentDate - record.DueDate).Value.TotalDays);
            }

            await _context.SaveChangesAsync();
            return overdueRecords;

        }

        public async Task<BorrowingRecord> CreateBorrowingRecordAsync(BorrowingRecord recordModel)
        {
            await _context.BorrowingRecords.AddAsync(recordModel);
            await _context.SaveChangesAsync();
            return recordModel;
        }

        public async Task<IEnumerable<BorrowingRecord>> GetAllBorrowingRecordAsync()
        {
            return await _context.BorrowingRecords.Include(o => o.AppUser).Include(o => o.BorrowedBooks).ToListAsync();
        }

        public async Task<BorrowingRecord?> GetBorrowingRecordById(AppUser appUser, int id)
        {
            return await _context.BorrowingRecords.Include(i => i.BorrowedBooks).FirstOrDefaultAsync(c => c.AppUserId == appUser.Id && c.BorrowingRecordId == id && !c.IsReturned && !c.IsCancelled);
        }

        public async Task<List<BorrowingRecord>> GetUserBorrowingRecordAsync(AppUser user)
        {
            return await _context.BorrowingRecords.Include(i => i.BorrowedBooks).Where(o => o.AppUserId == user.Id && !o.IsReturned && !o.IsCancelled).ToListAsync();
        }

        public async Task<BorrowingRecord> ReturnBorrowingRecordAsync(AppUser user, int id)
        {
            var borrowingRecordModel = await _context.BorrowingRecords.FirstOrDefaultAsync(br => br.BorrowingRecordId == id && br.AppUserId == user.Id);

            if (borrowingRecordModel == null)
            {
                return null;
            }

            if (borrowingRecordModel.DueDate < borrowingRecordModel.ReturnDate)
            {
                borrowingRecordModel.IsOverdue = true;
                borrowingRecordModel.OverdueDays = (int)Math.Ceiling((borrowingRecordModel.ReturnDate - borrowingRecordModel.DueDate).Value.TotalDays);
            }

            borrowingRecordModel.IsReturned = true;
            borrowingRecordModel.ReturnDate = DateTime.Now;

            _context.BorrowingRecords.Update(borrowingRecordModel);
            await _context.SaveChangesAsync();
            return borrowingRecordModel;
        }

    }
}
