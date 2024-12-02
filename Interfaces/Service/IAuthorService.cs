using ModernLibrary.DTOs.Author;
using ModernLibrary.Models;

namespace ModernLibrary.Interfaces.Service
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync();
        Task<AuthorDto> GetAuthorByIdAsync(int id);
        Task<AuthorDto> CreateAuthorAsync(CreateAuthorRequestDto authorDto);
        Task<AuthorDto> UpdateAuthorAsync(int id, UpdateAuthorRequestDto updateDto);
        Task<AuthorDto> DeleteAuthorAsync(int id);
    }
}
