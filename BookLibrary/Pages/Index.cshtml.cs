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
    // These will automatically catch the data sent from the HTML forms
    [BindProperty] public long BorrowBookId { get; set; }
    [BindProperty] public long ReturnBookId { get; set; }

    // -- Feedback --
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    // Helper method to get the logged-in user's ID
    private long GetCurrentMemberId()
    {
        var idClaim = User.FindFirst("MemberId")?.Value;
        if (long.TryParse(idClaim, out var id)) return id;

        // Fallback for testing if not logged in yet
        return 1;
    }

    public void OnGet()
    {
        LoadMemberData();
    }

    public IActionResult OnPostBorrow()
    {
        try
        {
            var currentMemberId = GetCurrentMemberId();

            borrowingService.RequestBorrow(BorrowBookId, currentMemberId);

            SuccessMessage = "Borrow request submitted.";
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
            var currentMemberId = GetCurrentMemberId();

            borrowingService.ReturnBook(ReturnBookId, currentMemberId);

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
        MemberBorrowedBooks = borrowingService.GetMemberBorrowedBooks(GetCurrentMemberId());
        AvailableBooks = borrowingService.GetAvailableBooks();
    }
}