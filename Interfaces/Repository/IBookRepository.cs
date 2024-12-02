using ModernLibrary.DTOs.Book;
using ModernLibrary.Helpers;
using ModernLibrary.Models;

namespace ModernLibrary.Interfaces.Repository
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllBooksAsync(BookQueryObject query);
        Task<Book?> GetByBookIdAsync(int id);
        Task<Book?> GetByBookNameAsync(string name);
        Task<Book> CreateBookAsync(Book bookModel);
        Task<Book> UpdateBookAsync(int id, UpdateBookRequestDto bookDto);
        Task<Book?> DeleteBookAsync(int id);
        Task<bool> BookExists(int id);
    }
}
