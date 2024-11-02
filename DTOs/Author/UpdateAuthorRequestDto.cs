using System.ComponentModel.DataAnnotations;

namespace ModernLibrary.DTOs.Author
{
    public class UpdateAuthorRequestDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "Author's Last Name must not be less than 2 characters")]
        [MaxLength(20, ErrorMessage = "Author's Last Name must not be more than 15 characters")]
        public string AuthorName { get; set; } = string.Empty;
    }
}
