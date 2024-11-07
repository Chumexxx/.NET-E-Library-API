using ModernLibrary.DTOs.Book;
using ModernLibrary.DTOs.BorrowedBook;
using System.ComponentModel.DataAnnotations;

namespace ModernLibrary.DTOs.BorrowingRecord
{
    public class BorrowingRecordDto
    {
        public int BorrowingRecordId { get; set; }
        public string BorrowedBy { get; set; } = string.Empty;
        public DateTime? BorrowedDate { get; set; } = DateTime.Now;    
        public DateTime? DueDate { get; set; }
        public bool IsReturned { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsOverdue { get; set; }
        //public int OverdueDays { get; set; }
        public ICollection<BorrowedBookDto> BorrowedBook { get; set; }
    }
}
