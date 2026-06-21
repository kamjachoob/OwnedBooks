using BookLibrary.Domain;

namespace BookLibrary.Data.BorrowedBooks;

public interface IBorrowedBookRepository
{
    public BorrowedBook GetById(long id);
    public List<BorrowedBook> GetBorrowedBooks(long memberId);
    public BorrowedBook GetBorrowedBook(long bookId ,long memberId);
    public void Save(BorrowedBook borrowedBook);
    public void SaveAll(List<BorrowedBook> borrowedBooks);
    public int GetActiveBorrowCountByMember(long memberId);
    public List<BorrowedBook> GetAllBorrowedBooks();
}
