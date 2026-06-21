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
        if (!ValidateBookAndMember(bookId, memberId)) throw new ArgumentException("Book or member is not available");

        var limitCount = bookSettings.Value.MaxBorrowLimit;
        var currentBorrowCount = borrowedBookRepository.GetActiveBorrowCountByMember(memberId);
        if (currentBorrowCount >= limitCount)
        {
            throw new InvalidOperationException($"Member has reached the maximum borrow limit of {limitCount} books.");
        }

        var borrowBook = new BorrowedBook 
        {
            bookId = bookId,
            memberId = memberId,
            BorrowedDate = DateTime.Now
        };

        borrowedBookRepository.Save(borrowBook);
    }

    public void ReturnBook(long bookId, long memberId)
    {
        if (!ValidateBookAndMember(bookId, memberId)) throw new ArgumentException("Book or member is not available");

        var borrowedBook = borrowedBookRepository.(memberId);

        borrowedBook.ReturnedDate = DateTime.Now;

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

    private bool ValidateBookAndMember(long bookId, long memberId) 
    {
        var book = bookRepository.GetById(bookId);
        if (book == null) return false;

        var member = memberRepository.GetById(memberId);
        if (member == null) return false;

        return true;
    }
}
