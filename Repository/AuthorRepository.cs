using Microsoft.EntityFrameworkCore;
using ModernLibrary.Data;
using ModernLibrary.DTOs.Author;
using ModernLibrary.Interfaces;
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

        public async Task<Author> CreateAsync(Author authorModel)
        {
            await _context.Author.AddAsync(authorModel);
            await _context.SaveChangesAsync();
            return authorModel;
        }


        public async Task<Author?> DeleteAsync(int id)
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

        public async Task<List<Author>> GetAllAsync()
        {
            return await _context.Author.Include(c => c.Books).ToListAsync();
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            return await _context.Author.Include(c => c.Books).FirstOrDefaultAsync(a => a.AuthorId == id);
        }

        public async Task<Author> UpdateAsync(int id, UpdateAuthorRequestDto authorDto)
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
