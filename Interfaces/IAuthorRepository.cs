using ModernLibrary.DTOs.Author;
using ModernLibrary.Models;

namespace ModernLibrary.Interfaces
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetAllAsync();
        Task<Author?> GetByIdAsync(int id);
        Task<Author> CreateAsync(Author authorModel);
        Task<Author> UpdateAsync(int id, UpdateAuthorRequestDto authorDto);
        Task<Author?> DeleteAsync(int id);
    }
}
