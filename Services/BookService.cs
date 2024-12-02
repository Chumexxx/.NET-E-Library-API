using Microsoft.AspNetCore.Identity;
using ModernLibrary.Data;
using ModernLibrary.DTOs.Book;
using ModernLibrary.Helpers;
using ModernLibrary.Interfaces.Repository;
using ModernLibrary.Interfaces.Service;
using ModernLibrary.Mappers;
using ModernLibrary.Models;

namespace ModernLibrary.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepo;
        public BookService(IBookRepository bookRepo)
        {
            _bookRepo = bookRepo;
        }
        public async Task<BookDto> CreateBookAsync(CreateBookRequestDto bookDto)
        {
            var existingBook = await _bookRepo.GetByBookNameAsync(bookDto.BookName);
            if (existingBook != null)
                throw new Exception("This book already exists");

            var bookModel = bookDto.ToBookFromCreateDto();
            bookModel.CreatedOn = DateTime.Now;

            await _bookRepo.CreateBookAsync(bookModel);
            return bookModel.ToBookDto();
        }

        public async Task<BookDto> DeleteBookAsync(int id)
        {
            var deletedBook = await _bookRepo.DeleteBookAsync(id);
            if (deletedBook == null)
                throw new Exception("Book does not exist");
            return deletedBook.ToBookDto();
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync(BookQueryObject query)
        {
            var books = await _bookRepo.GetAllBooksAsync(query);
            return books.Select(b => b.ToBookDto());
        }

        public async Task<BookDto> GetBookByIdAsync(int id)
        {
            var book = await _bookRepo.GetByBookIdAsync(id);
            if (book == null)
                throw new Exception("Book not found");
            return book.ToBookDto();
        }

        public async Task<BookDto> UpdateBookAsync(int id, UpdateBookRequestDto updateDto)
        {
            var updatedBook = await _bookRepo.UpdateBookAsync(id, updateDto);
            if (updatedBook == null)
                throw new Exception("Book does not exist");
            return updatedBook.ToBookDto();
        }
    }
}
