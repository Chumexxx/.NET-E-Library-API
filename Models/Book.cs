using System.ComponentModel.DataAnnotations.Schema;

namespace ModernLibrary.Models
{
    [Table("Books")]
    public class Book
    {
        public string BookName { get; set; } = string.Empty;
        public int BookId { get; set; }
        //public string AuthorName { get; set; } = string.Empty;
        //public int AuthorId { get; set; }
        public string Genre { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public DateTime? PublishDate { get; set; }
        public string Synopsis { get; set; } = string.Empty;
        public int NumofBooksAvailable { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public List<Author> Authors { get; set; } = new List<Author>();
    }
}
