using BookLibrary.Domain;
using BookLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookLibrary.Pages.MyBooks;

[Authorize]
public class CreateModel : PageModel
{
    private readonly MyBooksService _service;

    public CreateModel(MyBooksService service)
    {
        _service = service;
    }

    [BindProperty]
    public Book Book { get; set; } = new();

    private long GetCurrentMemberId()
    {
        var idClaim = User.FindFirst("MemberId")?.Value;
        if (long.TryParse(idClaim, out var id)) return id;
        throw new InvalidOperationException("User is not authenticated as a member.");
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();

        var memberId = GetCurrentMemberId();
        _service.Create(Book, memberId);

        return RedirectToPage("./Index");
    }
}
