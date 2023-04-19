using Bogus;
using Microsoft.EntityFrameworkCore;
using MyApi.Models;

#pragma warning disable CS1591
namespace MyApi
{
    public class AppDbContext : DbContext
    {
        private const int MagicNumber = 434592;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; private set; }
        public DbSet<Author> Authors { get; private set; }
        public DbSet<Collection> Collections { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId);

            _ = modelBuilder.Entity<Collection>()
                .HasMany(b => b.Books)
                .WithMany()
                .UsingEntity<CollectionBook>(
                    j => j
                        .HasOne(cb => cb.Book)
                        .WithMany()
                        .HasForeignKey(cb => cb.BookId),
                    j => j
                        .HasOne(cb => cb.Collection)
                        .WithMany()
                        .HasForeignKey(cb => cb.CollectionId),
                    j => j.HasKey(cb => new { cb.BookId, cb.CollectionId }));

            _ = modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId);
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            Randomizer.Seed = new Random(MagicNumber);

            // Generate authors
            var authorFaker = new Faker<Author>()
                .RuleFor(a => a.Id, f => f.IndexFaker + 1)
                .RuleFor(a => a.Name, f => f.Name.FullName());

            var authors = authorFaker.Generate(10);

            modelBuilder.Entity<Author>().HasData(authors);

            // Generate books
            var bookFaker = new Faker<Book>()
                .RuleFor(b => b.Id, f => f.IndexFaker + 1)
                .RuleFor(b => b.Title, f => f.Lorem.Sentence())
                .RuleFor(b => b.ISBN, f => f.Random.Replace("###-#-##-######-#"))
                .RuleFor(b => b.AuthorId, f => f.PickRandom(authors).Id);

            var books = bookFaker.Generate(30);

            modelBuilder.Entity<Book>().HasData(books);
        }
      }
}
#pragma warning restore CS1591