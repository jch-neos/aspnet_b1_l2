using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
namespace MyApi.Controllers;
using Microsoft.EntityFrameworkCore;
using MyApi.Models;

/// <summary> Controller for books </summary>
[Route("api/v0/[controller]")]
public class SimpleBooksController : ControllerBase {
  [HttpPost] public async Task<ActionResult<Book>> CreateBook(Book book) {
    if(!ModelState.IsValid) {
      return BadRequest(ModelState);
    }
    _context.Books.Add(book);
    await _context.SaveChangesAsync();
    return CreatedAtAction("GetBook", new { id = book.Id }, book);
  }
  /// <summary> Create a new book </summary>

  #region plumbing
  private readonly AppDbContext _context;
  private static readonly Expression<Func<Book, Book>> map = x => new Book { Title = x.Title, Id = x.Id, AuthorId = x.AuthorId, ISBN = x.ISBN, Author = new Author { Name = x.Author.Name, Id = x.AuthorId } };

  /// <summary> Constructor </summary>
  /// <param name="context"> The database context </param>
  public SimpleBooksController(AppDbContext context) {
    _context = context;
  }
  #endregion
}