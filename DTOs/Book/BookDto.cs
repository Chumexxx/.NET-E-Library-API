namespace ModernLibrary.DTOs.Book
{
    public class BookDto
    {
        public string BookName { get; set; } = string.Empty;
        public int BookId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string Synopsis { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public DateTime? PublishDate { get; set; } = DateTime.Now;
        public int NumofBooksAvailable { get; set; }
    }
}
