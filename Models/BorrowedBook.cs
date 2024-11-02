using System.ComponentModel.DataAnnotations.Schema;

namespace ModernLibrary.Models
{
    public class BorrowedBook
    {
        public int BorrowedBookId { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; } = string.Empty;
        public int QtyNeeded { get; set; }
        public Book? Book { get; set; }
        public BorrowingRecord? BorrowingRecord { get; set; }
    }
}
