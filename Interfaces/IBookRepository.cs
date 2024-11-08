﻿using ModernLibrary.DTOs.Book;
using ModernLibrary.Helpers;
using ModernLibrary.Models;

namespace ModernLibrary.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync(QueryObject query);
        Task<Book?> GetByIdAsync(int id);
        Task<Book> CreateAsync(Book bookModel);
        Task<Book> UpdateAsync(int id, UpdateBookRequestDto bookDto);
        Task<Book?> DeleteAsync(int id);
        Task<bool> BookExists(int id);
    }
}
