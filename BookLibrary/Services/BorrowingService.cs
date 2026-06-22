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

        if (book.OwnerId.HasValue && book.OwnerId.Value == memberId)
        {
            throw new InvalidOperationException("Cannot borrow your own book.");
        }

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
            BorrowedDate = DateTime.Now,
            Status = BorrowStatus.Borrowed
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
        if (borrowedBook == null || borrowedBook.Id == 0) throw new ArgumentException("Borrow record not found");

        borrowedBook.ReturnedDate = DateTime.Now;
        borrowedBook.Status = BorrowStatus.Returned;
        book.Status = BookStatus.Available;

        bookRepository.Save(book);
        borrowedBookRepository.Save(borrowedBook);
    }

    // Create a reservation request for a book owned by someone else
    public void RequestBorrow(long bookId, long requesterMemberId)
    {
        var book = bookRepository.GetById(bookId);
        if (book == null) throw new ArgumentException("Book is not available");

        if (!book.OwnerId.HasValue) throw new InvalidOperationException("Book has no owner");
        if (book.OwnerId.Value == requesterMemberId) throw new InvalidOperationException("Cannot request your own book");

        var limitCount = bookSettings.Value.MaxBorrowLimit;
        var currentBorrowCount = borrowedBookRepository.GetActiveBorrowCountByMember(requesterMemberId);
        if (currentBorrowCount >= limitCount)
        {
            throw new InvalidOperationException($"Member has reached the maximum borrow limit of {limitCount} books.");
        }

        var request = new BorrowedBook
        {
            bookId = bookId,
            memberId = requesterMemberId,
            RequestDate = DateTime.Now,
            Status = BorrowStatus.Requested
        };

        borrowedBookRepository.Save(request);
    }

    // Owner accepts a reservation -> mark as Borrowed and set dates
    public void AcceptReservation(long borrowedRequestId, long ownerMemberId)
    {
        var request = borrowedBookRepository.GetById(borrowedRequestId);
        if (request == null || request.Id == 0) throw new ArgumentException("Request not found");

        var book = bookRepository.GetById(request.bookId);
        if (book == null) throw new ArgumentException("Book not found");

        if (!book.OwnerId.HasValue || book.OwnerId.Value != ownerMemberId)
            throw new InvalidOperationException("Only the owner can accept reservations for this book.");

        // Transition
        request.AcceptedDate = DateTime.Now;
        request.BorrowedDate = DateTime.Now;
        request.Status = BorrowStatus.Borrowed;

        book.Status = BookStatus.Borrowed;

        borrowedBookRepository.Save(request);
        bookRepository.Save(book);
    }

    public List<BorrowedBook> GetBorrowedBooks() => borrowedBookRepository.GetAllBorrowedBooks();

    public List<BorrowedBook> GetMemberBorrowedBooks(long memberId)
    {
        var member = memberRepository.GetById(memberId);
        if (member == null) throw new ArgumentException("member is not available");

        return borrowedBookRepository.GetBorrowedBooks(memberId);
    }

    public List<BorrowedBook> GetPendingRequestsForOwner(long ownerId)
    {
        // Repo returns all pending requests; filter by book owner here and attach navigation props
        var pending = borrowedBookRepository.GetPendingRequests();
        var result = new List<BorrowedBook>();
        foreach (var r in pending)
        {
            var book = bookRepository.GetById(r.bookId);
            if (book == null) continue;
            if (!book.OwnerId.HasValue || book.OwnerId.Value != ownerId) continue;

            r.Book = book;
            r.Member = memberRepository.GetById(r.memberId);
            result.Add(r);
        }

        return result;
    }

    public List<BorrowedBook> GetOutgoingRequests(long memberId)
    {
        return borrowedBookRepository.GetBorrowedBooks(memberId).Where(r => r.Status == BorrowStatus.Requested || r.Status == BorrowStatus.Accepted).ToList();
    }

    public bool IsOverdue(long borrowedBookId)
    {
        var borrowedBook = borrowedBookRepository.GetById(borrowedBookId);
        if (borrowedBook == null) throw new ArgumentException("book is not available");

        if (!borrowedBook.BorrowedDate.HasValue) return false;

        var isOverDue = borrowedBook.BorrowedDate.Value.AddDays(14) < DateTime.Today ? true : false;

        return isOverDue;
    }

    public List<Book> GetAvailableBooks() => bookRepository.GetAllAvailable();
}
