namespace ModernLibrary.Helpers
{
    public class BookQueryObject
    {
        public string? BookName { get; set; } = null;
        public string? AuthorName { get; set; } = null;
        public string? Genre { get; set; } = null;
        public string? Publisher { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
