using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi;
using MyApi.Models;

[Route("api/[controller]")]
[ApiController]
public class AuthorController : ControllerBase {
    private readonly AppDbContext _context;

    public AuthorController(AppDbContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors() {
        return await _context.Authors.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Author>> GetAuthor(int id) {
        var author = await _context.Authors.FindAsync(id);
        return author ?? (ActionResult<Author>)NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Author>> CreateAuthor(Author author) {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
    }

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