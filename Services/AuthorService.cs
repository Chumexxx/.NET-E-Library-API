using Microsoft.AspNetCore.Identity;
using ModernLibrary.Data;
using ModernLibrary.DTOs.Author;
using ModernLibrary.Interfaces.Repository;
using ModernLibrary.Interfaces.Service;
using ModernLibrary.Mappers;
using ModernLibrary.Models;
using ModernLibrary.Repository;

namespace ModernLibrary.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepo;
        public AuthorService(IAuthorRepository authorRepo)
        {
           _authorRepo = authorRepo;
        }

        public async Task<AuthorDto> CreateAuthorAsync(CreateAuthorRequestDto authorDto)
        {
            var existingAuthor = await _authorRepo.GetByAuthorNameAsync(authorDto.AuthorName);
            if (existingAuthor != null)
                throw new Exception("Author already exists");

            var authorModel = authorDto.ToAuthorFromCreateDto();
            authorModel.CreatedOn = DateTime.Now;

            await _authorRepo.CreateAuthorAsync(authorModel);

            return authorModel.ToAuthorDto();
        }

        public async Task<AuthorDto> DeleteAuthorAsync(int id)
        {
            var deletedAuthor = await _authorRepo.DeleteAuthorAsync(id);
            if (deletedAuthor == null)
                throw new Exception("Author not found");
            return deletedAuthor.ToAuthorDto();
        }

        public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync()
        {
            var authors = await _authorRepo.GetAllAuthorsAsync();
            return authors.Select(a => a.ToAuthorDto());
        }

        public async Task<List<Author>> GetAuthorByIdAsync(IEnumerable<int> authorId)
        {
            if (authorId == null || !authorId.Any())
                throw new ArgumentException("Author IDs cannot be null or empty");

            var authors = await _authorRepo.GetByAuthorIdAsync(authorId);

            if (!authors.Any())
                throw new Exception("No authors found for the provided IDs");

            return authors;
        }

        public async Task<AuthorDto> UpdateAuthorAsync(int id, UpdateAuthorRequestDto updateDto)
        {
            var updatedAuthor = await _authorRepo.UpdateAuthorAsync(id, updateDto);
            if (updatedAuthor == null)
                throw new Exception("Author not found");
            return updatedAuthor.ToAuthorDto();
        }
    }
}
