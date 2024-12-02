using ModernLibrary.DTOs.Book;
using ModernLibrary.Helpers;
using ModernLibrary.Models;

namespace ModernLibrary.Interfaces.Service
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllBooksAsync(BookQueryObject query);
        Task<BookDto> GetBookByIdAsync(int id);
        Task<BookDto> CreateBookAsync(CreateBookRequestDto bookDto);
        Task<BookDto> UpdateBookAsync(int id, UpdateBookRequestDto updateDto);
        Task<BookDto> DeleteBookAsync(int id);
    }
}
