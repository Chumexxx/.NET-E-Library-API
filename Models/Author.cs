using System.ComponentModel.DataAnnotations.Schema;

namespace ModernLibrary.Models
{
    [Table("Authors")]
    public class Author
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
