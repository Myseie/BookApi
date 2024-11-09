using Microsoft.AspNetCore.Mvc;
using BookApi.Models;
using BookApi.Services;
namespace BookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : Controller
    {
       private readonly BookService _bookService;
        
        public BookController(BookService bookService)
        {
            _bookService = bookService;

        }

        [HttpGet]
        public ActionResult <List<Book>> GetAll() => _bookService.GetAllBooks();

        [HttpGet("{id}")]
        public ActionResult<Book> GetById(string id)
        {
            var book = _bookService.GetBookById(id);
            if (book == null) return NotFound();
            return Ok(book);
            
        }

        [HttpPost]
        public ActionResult<Book> Create(Book book)
        {
           if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            try
            {
                _bookService.CreateBook(book);
                return CreatedAtAction(nameof(GetById), new { id = book.Id.ToString() }, book);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating book: {ex.Message}");
                return StatusCode(500, "An error occurred while creating the book.");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, Book updatedBook)
        {
            var existingBook = _bookService.GetBookById(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            // Sätt rätt Id på boken för uppdatering
            updatedBook.Id = id;

            _bookService.UpdateBook(id, updatedBook);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var book = _bookService.GetBookById(id);
            if (book == null) return NotFound();

            _bookService.DeleteBook(id);

            return NoContent();     
        }

        [HttpGet("filter-by-genre")]

        public ActionResult<List<Book>> GetBookByGenre(string genre)
        {
            var books = _bookService.GetBooksByGenre(genre);
            return books.Any() ? Ok(books) : NotFound();
        }

        [HttpGet("sort")]
        public ActionResult<List<Book>> GetBooksSortedByDate(string sortOrder = "asc")
        {
            var books = _bookService.GetBooksSortedByDate(sortOrder);
            return books.Any() ? Ok(books) : NotFound();
        }

        [HttpGet("filter")]
        public ActionResult<List<Book>> Filter([FromQuery] string? genre, [FromQuery] string? author)
        {
            var filteredBooks = _bookService.FilterBooks(genre, author);
            return Ok(filteredBooks);
        }

        [HttpGet("search")]

        public ActionResult<List<Book>> Search(string? title, string? author, string? genre)
        {
            var results = _bookService.SearchBooks(title, author, genre);

            if (results.Count == 0 )
            {
                return NotFound("No books found with the specified criteria");
            }

            return Ok(results);
        }
    }
}
