namespace MyApi.Models;
using System.Collections.Generic;

/// <summary> Book in the library </summary>
public class Book
{
    /// <summary> technical id </summary>
    public int Id { get; set; }
    /// <summary> title of the book </summary>
    public string Title { get; set; } = String.Empty;
    /// <summary> ISBN of the book </summary>
    public string ISBN { get; set; } = String.Empty;
    /// <summary> id of the author </summary>
    public int AuthorId { get; set; }
    /// <summary> author of the book </summary>
    public Author? Author { get; set; }
}



/// <summary> Author of books </summary>
public class Author
{
    /// <summary> technical id </summary>
    public int Id { get; set; }
    /// <summary> Full name of the author </summary>
    public string Name { get; set; } = String.Empty;
    /// <summary> Books written by the author </summary>
    public ICollection<Book>? Books { get; set; }
}

/// <summary> Collection of books </summary>
public class Collection
{
    /// <summary> technical id </summary>
    public int Id { get; set; }
    /// <summary> name of the collection </summary>
    public string Name { get; set; } = String.Empty;
    /// <summary> books in the collection </summary>
    public ICollection<Book> Books { get; set; } =  new List<Book>();
}

/// <summary> Book in a collection </summary>
public class CollectionBook
{
    /// <summary> technical id for the book </summary>
    public int BookId { get; set; }
    /// <summary> book in the collection </summary>
    public Book Book { get; set; } = null!;
    /// <summary> technical id for the collection </summary>
    public int CollectionId { get; set; }
    /// <summary> collection the book is in </summary>
    public Collection Collection { get; set; } = null!;

    // public int? NumberInSeries { get; set; }
}

/// <summary> A User favorite book </summary>
public class Favorite
{
    /// <summary> technical id </summary>
    public int Id { get; set; }

    /// <summary> technical id for the user </summary>
    public int UserId { get; set; }
    /// <summary> technical id for the book </summary>
    public int BookId { get; set; }
    /// <summary> book in the favorites </summary>
    public Book Book { get; set; } = null!;
}
