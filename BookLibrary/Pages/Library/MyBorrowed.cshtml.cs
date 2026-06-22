using BookLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookLibrary.Pages.Library;

[Authorize]
public class MyBorrowedModel : PageModel
{
    private readonly BorrowingService _borrowingService;

    public MyBorrowedModel(BorrowingService borrowingService)
    {
        _borrowingService = borrowingService;
    }

    public List<BookLibrary.Domain.BorrowedBook> Borrowed { get; set; } = new();

    private long GetCurrentMemberId()
    {
        var idClaim = User.FindFirst("MemberId")?.Value;
        if (long.TryParse(idClaim, out var id)) return id;
        throw new InvalidOperationException("User is not authenticated as a member.");
    }

    public void OnGet()
    {
        var memberId = GetCurrentMemberId();
        Borrowed = _borrowingService.GetMemberBorrowedBooks(memberId).Where(b => b.Status == BookLibrary.Domain.BorrowStatus.Borrowed).ToList();
    }
}
