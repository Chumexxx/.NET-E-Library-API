using System.ComponentModel.DataAnnotations.Schema;

namespace ModernLibrary.Models
{
    [Table("Borrowing Record")]
    public class BorrowingRecord
    {
        public string AppUserId { get; set; } = string.Empty;
        public int BorrowingRecordId { get; set; }
        public string BorrowedBy { get; set; } = string.Empty;
        public DateTime? BorrowedDate { get; set; } = DateTime.Now;
        public DateTime? DueDate { get; set; } = DateTime.Now.AddDays(1);
        public DateTime? ReturnDate { get; set; } = DateTime.Now;
        public DateTime? CancelDate { get; set; } = DateTime.Now;
        public bool IsCancelled { get; set; } = false;
        public bool IsReturned { get; set; } = false;
        public bool IsOverdue {  get; set; } = false;
        public int OverdueDays { get; set; }
        public AppUser? AppUser { get; set; }
        public ICollection<BorrowedBook> BorrowedBooks { get; set; }

    }
}
