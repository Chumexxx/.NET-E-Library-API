using ModernLibrary.DTOs.BorrowedBook;
using ModernLibrary.DTOs.BorrowingRecord;
using ModernLibrary.Models;

namespace ModernLibrary.Mappers
{
    public static class BorrowedBookMapper
    {
        public static BorrowedBookDto ToBorrowedBookDto(this BorrowedBook borrowedBookModel)
        {
            return new BorrowedBookDto
            {
                BookName = borrowedBookModel.BookName,
                QtyNeeded = borrowedBookModel.QtyNeeded,
            };
        }
    }
}
