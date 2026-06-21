using BookLibrary.Domain;

namespace BookLibrary.Data.BorrowedBooks;

public class InMemoryBorrowedBookRepository : IBorrowedBookRepository
{
    private readonly Dictionary<long, BorrowedBook> _store = new();
    private long _nextId = 1;

    public List<BorrowedBook> GetBorrowedBooks(long memberId) => _store.Values.Where(book => book.memberId == memberId).ToList();

    public BorrowedBook GetBorrowedBook(long bookId, long memberId) => _store.Values.FirstOrDefault(book => book.bookId == bookId
        && book.memberId == memberId && book.ReturnedDate == null) ?? new BorrowedBook();

    public void SaveAll(List<BorrowedBook> borrowedBooks) => borrowedBooks.ForEach(book => _store[book.Id] = book);

    int IBorrowedBookRepository.GetActiveBorrowCountByMember(long memberId) => _store.Values.Count(b => b.memberId == memberId && b.ReturnedDate == null);

    List<BorrowedBook> IBorrowedBookRepository.GetAllBorrowedBooks() => _store.Values.ToList();

    BorrowedBook IBorrowedBookRepository.GetById(long id)
    {
        _store.TryGetValue(id, out var book);
        return book;
    }

    void IBorrowedBookRepository.Save(BorrowedBook borrowedBook)
    {
        if (borrowedBook.Id == 0)
        {
            borrowedBook.Id = _nextId++;
        }

        _store[borrowedBook.Id] = borrowedBook;
    }
}