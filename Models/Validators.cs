namespace MyApi.Models;

using FluentValidation;

/// <summary> validator for books </summary>
public class BookValidator : AbstractValidator<Book>
{
    /// <summary> constructor : init rules</summary>
    public BookValidator()
    {
        RuleFor(b => b.Title).NotEmpty().MaximumLength(100);
        RuleFor(b => b.ISBN).NotEmpty().MaximumLength(20);
        RuleFor(b => b.AuthorId).NotEmpty();
    }
}

/// <summary> validator for authors </summary>
public class AuthorValidator : AbstractValidator<Author>
{
    /// <summary> constructor : init rules</summary>
    public AuthorValidator()
    {
        RuleFor(a => a.Name).NotEmpty().MaximumLength(100);
    }
}
