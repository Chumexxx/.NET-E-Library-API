using Microsoft.EntityFrameworkCore;
using ModernLibrary.Data;
using ModernLibrary.DTOs.Book;
using ModernLibrary.DTOs.BorrowingRecord;
using ModernLibrary.Helpers;
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

        public async Task<List<BorrowingRecord>> GetAllOverdueRecordsAsync()
        {
            var currentDate = DateTime.Now;
            var overdueRecords = await _context.BorrowingRecords.Where(br => !br.IsReturned && !br.IsCancelled && br.DueDate < currentDate)
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

        public async Task<List<BorrowingRecord>> GetAllBorrowingRecordAsync(string? userId, BorrowingRecordQueryObjects query)
        {
            var borrowingRecords =  _context.BorrowingRecords.Include(br => br.BorrowedBooks).Include(br => br.AppUser).AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                borrowingRecords = borrowingRecords.Where(br => br.AppUserId == userId);
            }

            if (query.StartBorrowDate.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.BorrowedDate >= query.StartBorrowDate.Value.Date);
            }
            if (query.EndBorrowDate.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.BorrowedDate <= query.EndBorrowDate.Value.Date.AddDays(1).AddTicks(-1));
            }

            if (query.StartDueDate.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.DueDate >= query.StartDueDate.Value.Date);
            }
            if (query.EndDueDate.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.DueDate <= query.EndDueDate.Value.Date.AddDays(1).AddTicks(-1));
            }

            if (query.StartReturnDate.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.ReturnDate >= query.StartReturnDate.Value.Date);
            }
            if (query.EndReturnDate.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.ReturnDate <= query.EndReturnDate.Value.Date.AddDays(1).AddTicks(-1));
            }

            if (query.IsReturned.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.IsReturned == query.IsReturned.Value);
            }
            if (query.IsCancelled.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.IsCancelled == query.IsCancelled.Value);
            }
            if (query.IsOverdue.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.IsOverdue == query.IsOverdue.Value);
            }

            if (query.MinOverdueDays.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.OverdueDays >= query.MinOverdueDays.Value);
            }
            if (query.MaxOverdueDays.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.OverdueDays <= query.MaxOverdueDays.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("OverdueDays", StringComparison.OrdinalIgnoreCase))
                {
                    borrowingRecords = query.IsDescending ? borrowingRecords.OrderByDescending(p => p.OverdueDays) : borrowingRecords.OrderBy(p => p.OverdueDays);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await borrowingRecords.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<List<BorrowingRecord>> GetAllUserBorrowingRecordAsync(AppUser user)
        {
            return await _context.BorrowingRecords.Include(i => i.BorrowedBooks).Where(o => o.AppUserId == user.Id).ToListAsync();
        }

        public async Task<BorrowingRecord?> GetBorrowingRecordById(AppUser appUser, int id)
        {
            return await _context.BorrowingRecords.Include(i => i.BorrowedBooks).FirstOrDefaultAsync(c => c.AppUserId == appUser.Id && c.BorrowingRecordId == id && !c.IsReturned && !c.IsCancelled);
        }

        public async Task<List<BorrowingRecord>> GetUserPendingBorrowingRecordAsync(AppUser user)
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

        public async Task<List<BorrowingRecord>> GetAllUserOverdueRecordsAsync(AppUser user)
        {
            var currentDate = DateTime.Now;
            var userOverdueRecords = await _context.BorrowingRecords.Where(br => br.AppUserId == user.Id && !br.IsReturned
                 && !br.IsCancelled && br.DueDate < currentDate).ToListAsync();


            foreach (var record in userOverdueRecords)
            {
                record.IsOverdue = true;
                record.OverdueDays = (int)Math.Ceiling((currentDate - record.DueDate).Value.TotalDays);
            }

            await _context.SaveChangesAsync();
            return userOverdueRecords;
        }
    }
}
