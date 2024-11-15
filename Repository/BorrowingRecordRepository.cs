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

        public async Task<List<BorrowingRecord>> CheckOverdueRecordsAsync()
        {
            var currentDate = DateTime.Now;
            var overdueRecords = await _context.BorrowingRecords
                .Where(br => !br.IsReturned && !br.IsCancelled && br.DueDate < currentDate)
                .ToListAsync();

            //foreach (var record in overdueRecords)
            //{
            //    record.IsOverdue = true;
            //    record.OverdueDays = (int)Math.Ceiling((currentDate - record.DueDate).Value.TotalDays);
            //}

            await _context.SaveChangesAsync();
            return overdueRecords;

        }

        public async Task<BorrowingRecord> CreateBorrowingRecordAsync(BorrowingRecord recordModel)
        {
            await _context.BorrowingRecords.AddAsync(recordModel);
            await _context.SaveChangesAsync();
            return recordModel;
        }

        public async Task<List<BorrowingRecord>> GetAllBorrowingRecordAsync(BorrowingRecordQueryObjects query)
        {
            //return await _context.BorrowingRecords.Include(o => o.AppUser).Include(o => o.BorrowedBooks).ToListAsync();
            //var book = _context.Books.AsQueryable();
            var borrowingRecords = _context.BorrowingRecords.Include(br => br.BorrowedBooks).Include(br => br.AppUser).AsQueryable();

            // Filter by Borrowed Date range
            if (query.StartBorrowDate.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.BorrowedDate >= query.StartBorrowDate.Value.Date);
            }
            if (query.EndBorrowDate.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.BorrowedDate <= query.EndBorrowDate.Value.Date.AddDays(1).AddTicks(-1));
            }

            // Filter by Due Date range
            if (query.StartDueDate.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.DueDate >= query.StartDueDate.Value.Date);
            }
            if (query.EndDueDate.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.DueDate <= query.EndDueDate.Value.Date.AddDays(1).AddTicks(-1));
            }

            // Filter by Return Date range
            if (query.StartReturnDate.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.ReturnDate >= query.StartReturnDate.Value.Date);
            }
            if (query.EndReturnDate.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.ReturnDate <= query.EndReturnDate.Value.Date.AddDays(1).AddTicks(-1));
            }

            // Filter by status
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

            // Filter by Overdue Days range
            if (query.MinOverdueDays.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.OverdueDays >= query.MinOverdueDays.Value);
            }
            if (query.MaxOverdueDays.HasValue)
            {
                borrowingRecords = borrowingRecords.Where(br => br.OverdueDays <= query.MaxOverdueDays.Value);
            }

            // Get total count before pagination
            var totalCount = await borrowingRecords.CountAsync();

            // Apply pagination
            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            // Calculate current overdue status
            var currentDate = DateTime.Now;
            var records = await borrowingRecords.ToListAsync();

            return await borrowingRecords.Skip(skipNumber).Take(query.PageSize).ToListAsync();

            //foreach (var record in records)
            //{
            //    if (!record.IsReturned && !record.IsCancelled && record.DueDate.HasValue)
            //    {
            //        record.IsOverdue = currentDate > record.DueDate;
            //        if (record.IsOverdue)
            //        {
            //            record.OverdueDays = (int)(currentDate - record.DueDate.Value).TotalDays;
            //        }
            //    }
            //}

            //return (records, totalCount);
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
