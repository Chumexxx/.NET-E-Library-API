namespace ModernLibrary.Helpers
{
    public class BorrowingRecordQueryObjects
    {
        //public string? BookName { get; set; } = null;
        //public bool IsOverdue { get; set; } = true;
        //public int OverdueDays { get; set; }
        public string? BorrowedBy { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
