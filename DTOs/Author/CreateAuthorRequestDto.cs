using System.ComponentModel.DataAnnotations;

namespace ModernLibrary.DTOs.Author
{
    public class CreateAuthorRequestDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "Author's Name must not be less than 2 characters")]
        [MaxLength(30, ErrorMessage = "Author's Name must not be more than 30 characters")]
        public string AuthorName { get; set; } = string.Empty;
    }
}
