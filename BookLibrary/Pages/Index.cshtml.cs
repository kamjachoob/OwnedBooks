using BookLibrary.Domain;
using BookLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookLibrary.Pages.Library;

public class IndexModel(BorrowingService borrowingService) : PageModel
{
    // -- Displayed data --
    public List<BorrowedBook> MemberBorrowedBooks { get; set; } = [];
    public List<Book> AvailableBooks { get; set; } = [];

    // -- Form inputs --
    [BindProperty] public long BorrowBookId { get; set; }
    [BindProperty] public long ReturnBookId { get; set; }
    [BindProperty] public long MemberId { get; set; }

    // -- Feedback --
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    // Hardcoded for demo — replace with session/auth user later
    private const long CurrentMemberId = 1;

    public void OnGet()
    {
        LoadMemberData();
    }

    public IActionResult OnPostBorrow()
    {
        try
        {
            borrowingService.BorrowBook(BorrowBookId, CurrentMemberId);
            SuccessMessage = "Book borrowed successfully.";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }

        LoadMemberData();
        return Page();
    }

    public IActionResult OnPostReturn()
    {
        try
        {
            borrowingService.ReturnBook(ReturnBookId, CurrentMemberId);
            SuccessMessage = "Book returned successfully.";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }

        LoadMemberData();
        return Page();
    }

    private void LoadMemberData()
    {
        MemberBorrowedBooks = borrowingService.GetMemberBorrowedBooks(CurrentMemberId);
        AvailableBooks = borrowingService.GetAvailableBooks();
    }
}
