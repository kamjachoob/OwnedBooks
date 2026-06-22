using BookLibrary.Common.Settings;
using BookLibrary.Data.Books;
using BookLibrary.Data.BorrowedBooks;
using BookLibrary.Data.Members;
using BookLibrary.Domain;
using Microsoft.Extensions.Options;

namespace BookLibrary.Services;

public class BorrowingService(IBookRepository bookRepository, IBorrowedBookRepository borrowedBookRepository, IMemberRepository memberRepository, IOptions<BookSettings> bookSettings)
{
    public void BorrowBook(long bookId, long memberId)
    {
        var book = bookRepository.GetById(bookId);
        if (book == null) throw new ArgumentException("Book is not available");

        var member = memberRepository.GetById(memberId);
        if (member == null) throw new ArgumentException("member is not available");

        var limitCount = bookSettings.Value.MaxBorrowLimit;
        var currentBorrowCount = borrowedBookRepository.GetActiveBorrowCountByMember(memberId);
        if (currentBorrowCount >= limitCount)
        {
            throw new InvalidOperationException($"Member has reached the maximum borrow limit of {limitCount} books.");
        }

        book.Status = BookStatus.Borrowed;

        var borrowBook = new BorrowedBook 
        {
            bookId = bookId,
            memberId = memberId,
            BorrowedDate = DateTime.Now
        };

        bookRepository.Save(book); 
        borrowedBookRepository.Save(borrowBook);
    }

    public void ReturnBook(long bookId, long memberId)
    {
        var book = bookRepository.GetById(bookId);
        if (book == null) throw new ArgumentException("Book is not available");

        var member = memberRepository.GetById(memberId);
        if (member == null) throw new ArgumentException("member is not available");

        var borrowedBook = borrowedBookRepository.GetBorrowedBook(bookId, memberId);

        borrowedBook.ReturnedDate = DateTime.Now;
        book.Status = BookStatus.Available;

        bookRepository.Save(book);
        borrowedBookRepository.Save(borrowedBook);
    }

    public List<BorrowedBook> GetBorrowedBooks() => borrowedBookRepository.GetAllBorrowedBooks();

    public List<BorrowedBook> GetMemberBorrowedBooks(long memberId)
    {
        var member = memberRepository.GetById(memberId);
        if (member == null) throw new ArgumentException("member is not available");

        return borrowedBookRepository.GetBorrowedBooks(memberId);
    }

    public bool IsOverdue(long borrowedBookId)
    {
        var borrowedBook = borrowedBookRepository.GetById(borrowedBookId);
        if (borrowedBook == null) throw new ArgumentException("book is not available");

        var isOverDue = borrowedBook.BorrowedDate.AddDays(14) < DateTime.Today ? true : false;
        
        return isOverDue;
    }

    public List<Book> GetAvailableBooks() => bookRepository.GetAllAvailable();
}
