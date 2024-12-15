using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModernLibrary.DTOs.Author;
using ModernLibrary.Interfaces.Repository;
using ModernLibrary.Interfaces.Service;
using ModernLibrary.Mappers;
using ModernLibrary.Services;

namespace ModernLibrary.Controllers
{
    [Route("api/Authors")]
    [ApiController]
    [Authorize]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepo;
        private readonly IAuthorService _authorService;
        private readonly IBookRepository _bookRepo;
        public AuthorController(IAuthorRepository authorRepo, IBookRepository bookRepo, IAuthorService authorService)
        {
            _authorRepo = authorRepo;
            _authorService = authorService;
            _bookRepo = bookRepo;
        }

        [HttpGet("getAllAuthors")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<ActionResult> GetAllAuthors()
        {
            try
            {
                var authors = await _authorService.GetAllAuthorsAsync();
                Console.WriteLine("user called the get all authors endpoint");
                return Ok(authors);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in getAllAuthors endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<ActionResult> GetAuthorById([FromRoute] int id)
        {
            try
            {
                var authors = await _authorService.GetAuthorByIdAsync(new[] { id });

                if (!authors.Any())
                    return NotFound("Author not found.");

                Console.WriteLine("User called the get author by ID endpoint.");
                return Ok(authors.FirstOrDefault());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in get author by ID endpoint: ", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("createAuthor")]
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorRequestDto authorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdAuthor = await _authorService.CreateAuthorAsync(authorDto);
                Console.WriteLine("user called the create author endpoint");
                return CreatedAtAction(nameof(GetAuthorById), new { id = createdAuthor.AuthorId }, createdAuthor);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in create author endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> UpdateAuthor([FromRoute] int id, [FromBody] UpdateAuthorRequestDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedAuthor = await _authorService.UpdateAuthorAsync(id, updateDto);
                Console.WriteLine("user called the update author endpoint");
                return Ok(updatedAuthor);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in update author endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> DeleteAuthor([FromRoute] int id)
        {
            try
            {
                var deletedAuthor = await _authorService.DeleteAuthorAsync(id);
                Console.WriteLine("user called the delete author endpoint");
                return Ok(deletedAuthor);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in delete author endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
