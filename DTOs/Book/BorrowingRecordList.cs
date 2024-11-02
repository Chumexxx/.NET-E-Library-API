using System.ComponentModel.DataAnnotations;

namespace ModernLibrary.DTOs.Book
{
    public class BorrowingRecordList
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        public int QtyNeeded { get; set; }
    }
}
