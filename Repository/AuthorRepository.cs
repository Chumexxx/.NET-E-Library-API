using Microsoft.EntityFrameworkCore;
using ModernLibrary.Data;
using ModernLibrary.DTOs.Author;
using ModernLibrary.Interfaces.Repository;
using ModernLibrary.Models;

namespace ModernLibrary.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDBContext _context;
        public AuthorRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Author> CreateAuthorAsync(Author authorModel)
        {
            var existingAuthor = await _context.Author.FirstOrDefaultAsync(x => x.AuthorName == authorModel.AuthorName);

            if (existingAuthor != null)
            {
                return null;
            }
            await _context.Author.AddAsync(authorModel);
            await _context.SaveChangesAsync();
            return authorModel;
        }


        public async Task<Author?> DeleteAuthorAsync(int id)
        {
            var authorModel = await _context.Author.FirstOrDefaultAsync(b => b.AuthorId == id);

            if (authorModel == null)
            {
                return null;
            }

            _context.Author.Remove(authorModel);
            await _context.SaveChangesAsync();
            return authorModel;
        }

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            return await _context.Author.Include(c => c.Books).ToListAsync();
        }

        public async Task<Author?> GetByAuthorIdAsync(int id)
        {
            return await _context.Author.Include(c => c.Books).FirstOrDefaultAsync(a => a.AuthorId == id);
        }

        public async Task<Author?> GetByAuthorNameAsync(string name)
        {
            return await _context.Author.FirstOrDefaultAsync(t => t.AuthorName == name);
        }

        public async Task<Author> UpdateAuthorAsync(int id, UpdateAuthorRequestDto authorDto)
        {
            var existingAuthor = await _context.Author.FirstOrDefaultAsync(b => b.AuthorId == id);

            if (existingAuthor == null)
            {
                return null;
            }

            existingAuthor.AuthorName = authorDto.AuthorName;

            await _context.SaveChangesAsync();

            return existingAuthor;

        }
    }
}
