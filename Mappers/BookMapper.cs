using ModernLibrary.DTOs.Author;
using ModernLibrary.DTOs.Book;
using ModernLibrary.Models;

namespace ModernLibrary.Mappers
{
    public static class BookMapper
    {
        public static BookDto ToBookDto(this Book bookModel)
        {
            return new BookDto
            {
                BookName = bookModel.BookName,
                BookId = bookModel.BookId,
                Genre = bookModel.Genre,
                Publisher = bookModel.Publisher,
                PublishDate = bookModel.PublishDate,
                Synopsis = bookModel.Synopsis,
                CreatedOn = bookModel.CreatedOn,
                NumofBooksAvailable = bookModel.NumofBooksAvailable,
                Authors = bookModel.Authors?.Select(author => new AuthorDto
                {
                    AuthorId = author.AuthorId,
                    AuthorName = author.AuthorName
                }).ToList() ?? new List<AuthorDto>(),

            };
        }

        public static Book ToBookFromCreateDto(this CreateBookRequestDto bookDto)
        {
            return new Book
            {
                BookName = bookDto.BookName,
                Genre = bookDto.Genre,
                Synopsis = bookDto.Synopsis,
                Publisher = bookDto.Publisher,
                PublishDate = bookDto.PublishDate,
                NumofBooksAvailable = bookDto.NumofBooksAvailable,
                Authors = new List<Author>(),
                CreatedOn = DateTime.Now,
            };
        }

        public static Book ToBookFromUpdate(this UpdateBookRequestDto bookDto)
        {
            return new Book
            {
                BookName = bookDto.BookName,
                Genre = bookDto.Genre,
                Synopsis = bookDto.Synopsis,
                Publisher = bookDto.Publisher,
                PublishDate = bookDto.PublishDate,
                NumofBooksAvailable = bookDto.NumofBooksAvailable,

            };
        }
    }
}
