using System.ComponentModel.DataAnnotations;

namespace ModernLibrary.DTOs.BorrowingRecord
{
    public class ReturnBorrowingRecordRequestDto
    {
        [Required]
        public string BookName { get; set; } = string.Empty;
    }
}
