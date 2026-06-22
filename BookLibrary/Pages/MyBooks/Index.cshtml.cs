using BookLibrary.Domain;
using BookLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookLibrary.Pages.MyBooks;

[Authorize]
public class IndexModel : PageModel
{
    private readonly MyBooksService _service;

    public IndexModel(MyBooksService service)
    {
        _service = service;
    }

    public List<Book> MyBooks { get; set; } = new();

    private long GetCurrentMemberId()
    {
        var idClaim = User.FindFirst("MemberId")?.Value;
        if (long.TryParse(idClaim, out var id)) return id;
        throw new InvalidOperationException("User is not authenticated as a member.");
    }

    public void OnGet()
    {
        var memberId = GetCurrentMemberId();
        MyBooks = _service.GetByOwner(memberId);
    }
}
