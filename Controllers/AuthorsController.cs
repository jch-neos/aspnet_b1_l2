using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi;
using MyApi.Models;

/// <summary> Controller for authors </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class AuthorController : ControllerBase {
    private readonly AppDbContext _context;

    /// <summary> Constructor </summary>
    /// <param name="context"> The database context </param>
    public AuthorController(AppDbContext context) {
        _context = context;
    }

    /// <summary> Get all authors </summary>
    /// <returns> A list of all authors </returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors() {
        return await _context.Authors.AsNoTracking().ToListAsync();
    }

    /// <summary> Get an author by his id </summary>
    /// <param name="id"> The id of the author to get </param>
    [HttpGet("{id}")]
    public async Task<ActionResult<Author>> GetAuthor(int id) {
        var author = await _context.Authors.AsNoTracking().Include(x=>x.Books).FirstOrDefaultAsync(x=>x.Id == id);
        return author ?? (ActionResult<Author>)NotFound();
    }

    /// <summary> Create a new author </summary>
    /// <param name="author"> The author to create </param>
    [HttpPost]
    public async Task<ActionResult<Author>> CreateAuthor(Author author) {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
    }

    /// <summary> Update an existing author </summary>
    /// <param name="id"> The id of the author to update </param>
    /// <param name="author"> The new author data </param>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor(int id, Author author) {
        if (id != author.Id) {
            return BadRequest();
        }

        _context.Entry(author).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!AuthorExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary> Delete an existing author </summary>
    /// <param name="id"> The id of the author to delete </param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id) {
        var author = await _context.Authors.FindAsync(id);
        if (author is null) {
            return NotFound();
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AuthorExists(int id) {
        return _context.Authors.Any(e => e.Id == id);
    }
}