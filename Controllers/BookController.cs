using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModernLibrary.DTOs.Book;
using ModernLibrary.Helpers;
using ModernLibrary.Interfaces;
using ModernLibrary.Mappers;

namespace ModernLibrary.Controllers
{
    [Route("api/Books")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepo;
        public BookController(IBookRepository bookRepo)
        {
            _bookRepo = bookRepo;
        }
        
        [HttpGet("getAllBooks")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetAllBooks([FromQuery] BookQueryObject query)
        {
            var books = await _bookRepo.GetAllAsync(query);

            var bookDto = books.Select(b => b.ToBookDto()).ToList();

            return Ok(bookDto);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var book = await _bookRepo.GetByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book.ToBookDto());
        }

        [HttpPost("addBook")]
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> Create([FromBody] CreateBookRequestDto bookDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = await _bookRepo.GetByNameAsync(bookDto.BookName);

            if (book != null)
            {
                return BadRequest("This book already exists");
            }

            var bookModel = bookDto.ToBookFromCreateDto();

            bookModel.CreatedOn = DateTime.Now;

            await _bookRepo.CreateAsync(bookModel);

            return CreatedAtAction(nameof(Create), new { id = bookModel.BookId }, bookModel.ToBookDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateBookRequestDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookModel = await _bookRepo.UpdateAsync(id, updateDto);

            if (bookModel == null)
            {
                return NotFound("Book does not exist");
            }

            return Ok(bookModel.ToBookDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorModel = await _bookRepo.DeleteAsync(id);

            if (authorModel == null)
            {
                return NotFound("Book does not exist");
            }

            return Ok(new {message = "Book has been deleted successfully"});
        }
    }
}
