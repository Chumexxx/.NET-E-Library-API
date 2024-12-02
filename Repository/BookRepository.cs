using Microsoft.EntityFrameworkCore;
using ModernLibrary.Data;
using ModernLibrary.DTOs.Book;
using ModernLibrary.Helpers;
using ModernLibrary.Interfaces.Repository;
using ModernLibrary.Models;

namespace ModernLibrary.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDBContext _context;
        public BookRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public Task<bool> BookExists(int id)
        {
            return _context.Books.AnyAsync(b => b.BookId == id);
        }

        public async Task<Book> CreateBookAsync(Book bookModel)
        {
            await _context.Books.AddAsync(bookModel);
            await _context.SaveChangesAsync();
            return bookModel;
        }


        public async Task<Book?> DeleteBookAsync(int id)
        {
            var bookModel = await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);

            if (bookModel == null)
            {
                return null;
            }

            _context.Books.Remove(bookModel);
            await _context.SaveChangesAsync();
            return bookModel;
        }

        public async Task<List<Book>> GetAllBooksAsync(BookQueryObject query)
        {
            var book = _context.Books.AsQueryable();


            if (!string.IsNullOrWhiteSpace(query.AuthorName))
            {
                book = book.Where(s => s.AuthorName.Contains(query.AuthorName));
            }

            if (!string.IsNullOrWhiteSpace(query.Genre))
            {
                book = book.Where(s => s.Genre.Contains(query.Genre));
            }

            if (!string.IsNullOrWhiteSpace(query.BookName))
            {
                book = book.Where(s => s.BookName.Contains(query.BookName));
            }

            if (!string.IsNullOrWhiteSpace(query.Publisher))
            {
                book = book.Where(s => s.Publisher.Contains(query.Publisher));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Publisher", StringComparison.OrdinalIgnoreCase))
                {
                    book = query.IsDescending ? book.OrderByDescending(s => s.Publisher) : book.OrderBy(s => s.Publisher);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await book.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }


        public async Task<Book?> GetByBookIdAsync(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(i => i.BookId == id);
        }

        public async Task<Book?> GetByBookNameAsync(string name)
        {
            return await _context.Books.FirstOrDefaultAsync(t => t.BookName == name);
        }

        public async Task<Book> UpdateBookAsync(int id, UpdateBookRequestDto bookDto)
        {
            var existingBook = await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);

            if (existingBook == null)
            {
                return null;
            }

            //existingBook.BookName = bookDto.BookName;
            existingBook.Genre = bookDto.Genre;
            existingBook.PublishDate = bookDto.PublishDate;
            existingBook.Publisher = bookDto.Publisher;
            existingBook.Synopsis = bookDto.Synopsis;
            existingBook.NumofBooksAvailable = bookDto.NumofBooksAvailable;

            _context.Books.Update(existingBook);
            await _context.SaveChangesAsync();

            return existingBook;

        }

    }
}
