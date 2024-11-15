using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModernLibrary.DTOs.Author;
using ModernLibrary.Interfaces;
using ModernLibrary.Mappers;

namespace ModernLibrary.Controllers
{
    [Route("api/Authors")]
    [ApiController]
    [Authorize]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepo;
        private readonly IBookRepository _bookRepo;
        public AuthorController(IAuthorRepository authorRepo, IBookRepository bookRepo)
        {
            _authorRepo = authorRepo;
            _bookRepo = bookRepo;
        }

        [HttpGet("getAllAuthors")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<ActionResult> GetAllAuthors()
        {
            var authors = await _authorRepo.GetAllAsync();
            var authorDto = authors.Select(a => a.ToAuthorDto());

            return Ok(authorDto);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var author = await _authorRepo.GetByIdAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(author.ToAuthorDto());
        }

        [HttpPost("addAuthor")]
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> Create([FromBody] CreateAuthorRequestDto authorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var author = await _authorRepo.GetByNameAsync(authorDto.AuthorName);

            if (author != null)
            {
                return BadRequest("Author already exists");
            }

            var authorModel = authorDto.ToAuthorFromCreateDto();

            authorModel.CreatedOn = DateTime.Now;

            await _authorRepo.CreateAsync(authorModel);

            return CreatedAtAction(nameof(Create), new { id = authorModel }, authorModel.ToAuthorDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateAuthorRequestDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var author = await _authorRepo.UpdateAsync(id, updateDto);

            if (author == null)
            {
                return NotFound("This id does not exist");
            }

            return Ok(author.ToAuthorDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorModel = await _authorRepo.DeleteAsync(id);

            if (authorModel == null)
            {
                return NotFound("Author does not exist");
            }

            return Ok(authorModel);
        }
    }
}
