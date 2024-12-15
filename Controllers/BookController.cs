using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModernLibrary.DTOs.Book;
using ModernLibrary.Helpers;
using ModernLibrary.Interfaces.Service;
using ModernLibrary.Mappers;
using ModernLibrary.Models;
using ModernLibrary.Services;

namespace ModernLibrary.Controllers
{
    [Route("api/Books")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        
        [HttpGet("getAllBooks")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetAllBooks([FromQuery] BookQueryObject query)
        {
            try
            {
                var books = await _bookService.GetAllBooksAsync(query);
                Console.WriteLine("user called get all books endpoint");
                return Ok(books);
                
            } catch (Exception ex)
            {
                Console.WriteLine("Error in get all books endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Customer, SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> GetBookById([FromRoute] int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                Console.WriteLine("user called get book by id endpoint");
                return Ok(book);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in get books by id endpoint ", ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPost("addBook")]
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookRequestDto bookDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdBook = await _bookService.CreateBookAsync(bookDto);
                Console.WriteLine("user called create book endpoint");
                return CreatedAtAction(nameof(GetBookById), new { id = createdBook.BookId }, createdBook);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in create book endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> UpdateBook([FromRoute] int id, [FromBody] UpdateBookRequestDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedBook = await _bookService.UpdateBookAsync(id, updateDto);
                Console.WriteLine("user called the update book endpoint");
                return Ok(updatedBook);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in update book endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "SuperAdmin, Admin, Librarian, Staff")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            try
            {
                await _bookService.DeleteBookAsync(id);
                Console.WriteLine("user called the delete book endpoint");
                return Ok(new { message = "Book has been deleted successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in delete book endpoint ", ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
