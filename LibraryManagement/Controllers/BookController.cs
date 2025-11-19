using LibraryManagement.Data;
using LibraryManagement.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            return Ok(await _context.Books.ToListAsync());
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound("Book not found");

            return Ok(book);
        }

        // 🔐 ONLY LOGGED-IN USERS CAN ADD BOOKS
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookModel model)
        {
            await _context.Books.AddAsync(model);
            await _context.SaveChangesAsync();
            return Ok("Book added successfully");
        }

        // 🔐 ONLY LOGGED-IN USERS CAN UPDATE BOOKS
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookModel model)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound("Book not found");

            book.Title = model.Title;
            book.Author = model.Author;
            
            book.Year = model.Year;
            

            await _context.SaveChangesAsync();
            return Ok("Book updated successfully");
        }

        // 🔐 ONLY LOGGED-IN USERS CAN DELETE BOOKS
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound("Book not found");

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return Ok("Book deleted successfully");
        }
    }
}
