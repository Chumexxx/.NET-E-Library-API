using ModernLibrary.DTOs.Book;
using System.ComponentModel.DataAnnotations;

namespace ModernLibrary.DTOs.Author
{
    public class AuthorDto
    {
        public int? AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public List<BookDto> Books { get; set; }
    }
}
