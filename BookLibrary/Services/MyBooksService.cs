using BookLibrary.Data.Books;
using BookLibrary.Domain;

namespace BookLibrary.Services;

public class MyBooksService
{
    private readonly IBookRepository _bookRepository;

    public MyBooksService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public List<Book> GetByOwner(long ownerId) => _bookRepository.GetByOwner(ownerId);

    public Book? GetById(long id) => _bookRepository.GetById(id);

    public void Create(Book book, long ownerId)
    {
        book.OwnerId = ownerId;
        book.Status = BookStatus.Available;
        _bookRepository.Save(book);
    }

    public void Update(Book book, long ownerId)
    {
        var existing = _bookRepository.GetById(book.Id);
        if (existing == null) throw new ArgumentException("Book not found");
        if (existing.OwnerId != ownerId) throw new UnauthorizedAccessException("Not the owner");

        // Preserve owner and status; apply edits
        book.OwnerId = ownerId;
        _bookRepository.Save(book);
    }

    public void Delete(long id, long ownerId)
    {
        var existing = _bookRepository.GetById(id);
        if (existing == null) throw new ArgumentException("Book not found");
        if (existing.OwnerId != ownerId) throw new UnauthorizedAccessException("Not the owner");

        _bookRepository.Delete(id);
    }
}
