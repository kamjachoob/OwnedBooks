using BookLibrary.Domain;

namespace BookLibrary.Data.Books;

public class InMemoryBookRepository : IBookRepository
{
    private readonly Dictionary<long, Book> _store = new();

    List<Book> IBookRepository.GetAllAvailable() => _store.Values.Where(book => book.IsAvailable).ToList();

    Book IBookRepository.GetById(long id) => _store[id];

    void IBookRepository.Save(Book book) => _store[book.Id] = book;
        
}
