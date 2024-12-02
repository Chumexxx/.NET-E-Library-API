using ModernLibrary.DTOs.Author;
using ModernLibrary.Models;

namespace ModernLibrary.Interfaces.Repository
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetAllAuthorsAsync();
        Task<Author?> GetByAuthorIdAsync(int id);
        Task<Author?> GetByAuthorNameAsync(string name);
        Task<Author> CreateAuthorAsync(Author authorModel);
        Task<Author> UpdateAuthorAsync(int id, UpdateAuthorRequestDto authorDto);
        Task<Author?> DeleteAuthorAsync(int id);
    }
}
