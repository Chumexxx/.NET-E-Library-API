using ModernLibrary.DTOs.Book;
using System.ComponentModel.DataAnnotations;

namespace ModernLibrary.DTOs.BorrowingRecord
{
    public class CreateBorrowingRecordRequestDto
    {
        public List<BorrowingRecordList> Books { get; set; }
    }
}
