using ModernLibrary.DTOs.Author;
using ModernLibrary.Models;

namespace ModernLibrary.Interfaces.Service
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync();
        Task<List<Author>> GetAuthorByIdAsync(IEnumerable<int> authorId);
        Task<AuthorDto> CreateAuthorAsync(CreateAuthorRequestDto authorDto);
        Task<AuthorDto> UpdateAuthorAsync(int id, UpdateAuthorRequestDto updateDto);
        Task<AuthorDto> DeleteAuthorAsync(int id);
    }
}
