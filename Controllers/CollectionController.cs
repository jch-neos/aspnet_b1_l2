using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Models;

namespace MyApi.Controllers;
/// <summary> Controller for collections </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class CollectionController : ControllerBase {
    private readonly AppDbContext _context;

    /// <summary> Constructor </summary>
    /// <param name="context"> The database context </param>
    public CollectionController(AppDbContext context) {
        _context = context;
    }

    /// <summary> Get all collections </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Collection>>> GetCollections() {
        return await _context.Collections.ToListAsync();
    }

    /// <summary> Add a book to a collection </summary>
    [HttpPost("{collectionId}/books/add/{bookId}")]
    [ProducesResponseType(204)]
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

    /// <summary> Get a collection by its id </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Collection>> GetCollection(int id) {
        var collection = await _context.Collections.Include(c => c.Books).FirstOrDefaultAsync(c => c.Id == id);

        if (collection == null) {
            return NotFound();
        }

        return collection;
    }

    /// <summary> Create a new collection </summary>
    [HttpPost]
    public async Task<ActionResult<Collection>> CreateCollection(Collection collection) {
        _context.Collections.Add(collection);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCollection", new { id = collection.Id }, collection);
    }

    /// <summary> Update an existing collection </summary>
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

    /// <summary> Delete a collection </summary>
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