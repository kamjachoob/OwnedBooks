using BookLibrary.Domain;

namespace BookLibrary.Data.BorrowedBooks;

public class InMemoryBorrowedBookRepository : IBorrowedBookRepository
{
    private readonly Dictionary<long, BorrowedBook> _store = new();

    public List<BorrowedBook> GetBorrowedBooks(long memberId) => _store.Values.Where(book => book.memberId == memberId).ToList();

    public BorrowedBook GetBorrowedBook(long bookId, long memberId) => _store.Values.FirstOrDefault(book => book.memberId == memberId
        && book.bookId == bookId) ?? new BorrowedBook();

    public void SaveAll(List<BorrowedBook> borrowedBooks) => borrowedBooks.ForEach(book => _store[book.Id] = book);

    int IBorrowedBookRepository.GetActiveBorrowCountByMember(long memberId) => _store.Values.Count(b => b.memberId == memberId && b.ReturnedDate == null);

    List<BorrowedBook> IBorrowedBookRepository.GetAllBorrowedBooks() => _store.Values.ToList();

    BorrowedBook IBorrowedBookRepository.GetById(long id) => _store[id];

    void IBorrowedBookRepository.Save(BorrowedBook borrowedBook) => _store[borrowedBook.Id] = borrowedBook;
}
