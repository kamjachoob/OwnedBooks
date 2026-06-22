using BookLibrary.Domain;

namespace BookLibrary.Data.Books;

public interface IBookRepository
{
    public Book GetById(long id);
    public void Save(Book book);
    public List<Book> GetAllAvailable();
    public void Delete(long id);
    public List<Book> GetByOwner(long ownerId);
}
