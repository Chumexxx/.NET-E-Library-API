using Microsoft.AspNetCore.Identity;

namespace ModernLibrary.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string HomeAddress { get; set; } = string.Empty;
        public List<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>();
    }
}
