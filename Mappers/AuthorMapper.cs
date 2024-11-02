using ModernLibrary.DTOs.Author;
using ModernLibrary.Models;

namespace ModernLibrary.Mappers
{
    public static class AuthorMapper
    {
        public static AuthorDto ToAuthorDto(this Author authorModel)
        {
            return new AuthorDto
            {
                AuthorName = authorModel.AuthorName,
                AuthorId = authorModel.AuthorId,
                CreatedOn = authorModel.CreatedOn,
                Books = authorModel.Books.Select(p => p.ToBookDto()).ToList(),
            };
        }

        public static Author ToAuthorFromCreateDto(this CreateAuthorRequestDto authorDto)
        {
            return new Author
            {
                AuthorName = authorDto.AuthorName,
            };
        }

        public static Author ToAuthorFromUpdate(this UpdateAuthorRequestDto authorDto)
        {
            return new Author
            {
                AuthorName = authorDto.AuthorName,

            };
        }
    }
}
