using ModernLibrary.DTOs.Book;
using System.ComponentModel.DataAnnotations;

namespace ModernLibrary.DTOs.BorrowingRecord
{
    public class UpdateBorrowingRecordRequestDto
    {
        public int BorrowedBookId { get; set; }
        public List<BorrowingRecordList> Books { get; set; }
    }
}
