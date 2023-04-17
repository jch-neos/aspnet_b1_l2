using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Models;

namespace MyApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CollectionController : ControllerBase {
    private readonly AppDbContext _context;

    public CollectionController(AppDbContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Collection>>> GetCollections() {
        return await _context.Collections.Include(c => c.Books).ToListAsync();
    }

    [HttpPost("{collectionId}/books/add/{bookId}")]
    public async Task<IActionResult> AddBookToCollection(int collectionId, int bookId) {
        var collection = await _context.Collections.Include(c => c.Books).FirstOrDefaultAsync(c => c.Id == collectionId);
        var book = await _context.Books.FindAsync(bookId);

        if (collection == null || book == null) {
            return NotFound();
        }

        collection.Books.Add(book);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Collection>> GetCollection(int id) {
        var collection = await _context.Collections.Include(c => c.Books).FirstOrDefaultAsync(c => c.Id == id);

        if (collection == null) {
            return NotFound();
        }

        return collection;
    }

    [HttpPost]
    public async Task<ActionResult<Collection>> CreateCollection(Collection collection) {
        _context.Collections.Add(collection);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCollection", new { id = collection.Id }, collection);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCollection(int id, Collection collection) {
        if (id != collection.Id) {
            return BadRequest();
        }

        _context.Entry(collection).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException) {
            if (!CollectionExists(id)) {
                return NotFound();
            } else {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Collection>> DeleteCollection(int id) {
        var collection = await _context.Collections.FindAsync(id);
        if (collection == null) {
            return NotFound();
        }

        _context.Collections.Remove(collection);
        await _context.SaveChangesAsync();

        return collection;
    }

    private bool CollectionExists(int id) {
        return _context.Collections.Any(e => e.Id == id);
    }
}