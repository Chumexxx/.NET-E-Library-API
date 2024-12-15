using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModernLibrary.Data;
using ModernLibrary.DTOs.Author;
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
        private readonly ApplicationDBContext _context;
        private readonly IAuthorRepository _authorRepo;
        public BookService(IBookRepository bookRepo, ApplicationDBContext context, IAuthorRepository authorRepo)
        {
            _authorRepo = authorRepo;
            _context = context;
            _bookRepo = bookRepo;
        }
        public async Task<BookDto> CreateBookAsync(CreateBookRequestDto bookDto)
        {
            //var existingBook = await _bookRepo.GetByBookNameAsync(bookDto.BookName);
            //if (existingBook != null)
            //    throw new Exception("This book already exists");

            //var bookModel = bookDto.ToBookFromCreateDto();
            //bookModel.CreatedOn = DateTime.Now;

            //await _bookRepo.CreateBookAsync(bookModel);
            //return bookModel.ToBookDto();

            var existingBook = await _bookRepo.GetByBookNameAsync(bookDto.BookName);
            if (existingBook != null)
                throw new Exception("This book already exists");

            var bookModel = bookDto.ToBookFromCreateDto();
            bookModel.CreatedOn = DateTime.Now;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var authors = await _authorRepo.GetByAuthorIdAsync(bookDto.AuthorId);
                var invalidAuthorId = bookDto.AuthorId.Except(authors.Select(a => a.AuthorId)).ToList();

                if (invalidAuthorId.Any())
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Invalid Author ID(s): {string.Join(", ", invalidAuthorId)}");
                }

                foreach (var author in authors)
                {
                    bookModel.Authors.Add(author);
                    author.Books.Add(bookModel);
                }

                await _bookRepo.CreateBookAsync(bookModel);

                foreach (var author in authors)
                {
                    await _authorRepo.UpdateAuthorAsync(author.AuthorId, new UpdateAuthorRequestDto
                    {
                        AuthorName = author.AuthorName,
                        //Books = author.Books.Select(b => b.ToBookDto()).ToList()
                    });
                }

                await transaction.CommitAsync();

                return bookModel.ToBookDto();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("An error occurred while creating the book: " + ex.Message, ex);
            }
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
