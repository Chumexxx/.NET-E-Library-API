namespace ModernLibrary.Helpers
{
    public class BorrowingRecordQueryObjects
    {
        public DateTime? StartBorrowDate { get; set; }
        public DateTime? EndBorrowDate { get; set; }
        public DateTime? StartDueDate { get; set; }
        public DateTime? EndDueDate { get; set; }
        public DateTime? StartReturnDate { get; set; }
        public DateTime? EndReturnDate { get; set; }
        public int? MinOverdueDays { get; set; }
        public int? MaxOverdueDays { get; set; }
        public bool? IsReturned { get; set; }
        public bool? IsCancelled { get; set; }
        public bool? IsOverdue { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
