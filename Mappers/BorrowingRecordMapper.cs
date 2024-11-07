using ModernLibrary.DTOs.BorrowingRecord;
using ModernLibrary.Models;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace ModernLibrary.Mappers
{
    public static class BorrowingRecordMapper
    {
        public static BorrowingRecordDto ToBorrowingRecordDto(this BorrowingRecord borrowingRecordModel)
        {
            return new BorrowingRecordDto
            {
                BorrowingRecordId = borrowingRecordModel.BorrowingRecordId,
                BorrowedBy = borrowingRecordModel.AppUser.UserName,
                BorrowedDate = borrowingRecordModel.BorrowedDate,
                DueDate = borrowingRecordModel.DueDate,
                IsReturned = borrowingRecordModel.IsReturned,
                IsCancelled = borrowingRecordModel.IsCancelled,
                IsOverdue = borrowingRecordModel.IsOverdue,
                BorrowedBook = borrowingRecordModel.BorrowedBooks.Select(b => b.ToBorrowedBookDto()).ToList(),
            };
        }

        public static BorrowingRecord ToBorrowingRecordFromCreateDto(this CreateBorrowingRecordRequestDto recordDto)
        {
            return new BorrowingRecord
            {
                BorrowedBooks = new List<BorrowedBook>(),
            };
        }

    }
}
