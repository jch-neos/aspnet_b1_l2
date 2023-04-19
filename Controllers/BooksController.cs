using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Models;

namespace MyApi.Controllers;
/// <summary> Controller for books </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize("Bearer")]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _context;

    /// <summary> Constructor </summary>
    /// <param name="context"> The database context </param>
    public BooksController(AppDbContext context)
    {
        _context = context;
    }

    readonly Expression<Func<Book,Book>> map = x=>new Book{ Title=x.Title, Id=x.Id, AuthorId=x.AuthorId, ISBN=x.ISBN, Author=new Author{ Name = x.Author.Name, Id=x.AuthorId } };


    /// <summary> Get all books </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        return await _context.Books.Select(map).ToListAsync();
    }

    /// <summary> Get a book by its id </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        var book = await _context.Books.Select(map).FirstOrDefaultAsync(b => b.Id == id);

        return book ?? (ActionResult<Book>)NotFound();
    }

    /// <summary> Create a new book </summary>
    [Authorize("BearerWrite")]
    [HttpPost]
    public async Task<ActionResult<Book>> CreateBook(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetBook", new { id = book.Id }, book);
    }

    /// <summary> Update an existing book </summary>
    [Authorize("BearerWrite")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, Book book)
    {
        if (id != book.Id)
        {
            return BadRequest();
        }

        _context.Entry(book).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    /// <summary> Delete a book </summary>
    [Authorize("BearerWrite")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool BookExists(int id)
    {
        return _context.Books.Any(e => e.Id == id);
    }
}