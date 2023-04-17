namespace MyApi.Models;
using System.Collections.Generic;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ISBN { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
}



public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Book> Books { get; set; }
}

public class Collection
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Book> Books { get; set; }
}

public class CollectionBook
{
    public int BookId { get; set; }
    public Book Book { get; set; }
    public int CollectionId { get; set; }
    public Collection Collection { get; set; }

    // public int? NumberInSeries { get; set; }
}


public class Favorite
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
}
