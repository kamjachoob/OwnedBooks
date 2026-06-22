using BookLibrary.Domain;

namespace BookLibrary.Data.Books;

public class InMemoryBookRepository : IBookRepository
{
    private readonly Dictionary<long, Book> _store = new();
    private long _nextId = 1;

    List<Book> IBookRepository.GetAllAvailable() => _store.Values.Where(book => book.Status == BookStatus.Available).ToList();

    Book IBookRepository.GetById(long id)
    {
        _store.TryGetValue(id, out var book);
        return book;
    }

    void IBookRepository.Save(Book book)
    {
        if (book.Id == 0)
        {
            book.Id = _nextId++;
        }

        _store[book.Id] = book;
    }

    public void Delete(long id)
        { _store.Remove(id); }

    public List<Book> GetByOwner(long ownerId) => _store.Values.Where(b => b.OwnerId == ownerId).ToList();
}
