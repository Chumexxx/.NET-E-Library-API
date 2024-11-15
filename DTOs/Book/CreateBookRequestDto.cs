using System.ComponentModel.DataAnnotations;

namespace ModernLibrary.DTOs.Book
{
    public class CreateBookRequestDto
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Book Name must not be more than 15 characters")]
        public string BookName { get; set; } = string.Empty;
        [Required]
        [MaxLength(25, ErrorMessage = "Genre Name must not be more than 25 characters")]
        public string Genre { get; set; } = string.Empty;
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public string AuthorName { get; set; } = string.Empty;
        [Required]
        public DateTime? PublishDate { get; set; } = DateTime.Now;
        [Required]
        public string Publisher { get; set; } = string.Empty;
        [Required]
        public string Synopsis { get; set; } = string.Empty;
        [Required]
        [Range(0, 3000)]
        public int NumofBooksAvailable { get; set; }
    }
}
