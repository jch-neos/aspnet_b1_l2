namespace MyApi.Models;

using FluentValidation;
public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(b => b.Title).NotEmpty().MaximumLength(100);
        RuleFor(b => b.ISBN).NotEmpty().MaximumLength(20);
        RuleFor(b => b.AuthorId).NotEmpty();
    }
}

public class AuthorValidator : AbstractValidator<Author>
{
    public AuthorValidator()
    {
        RuleFor(a => a.Name).NotEmpty().MaximumLength(100);
    }
}
