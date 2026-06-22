using BookLibrary.Domain;
using BookLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookLibrary.Pages.MyBooks;

[Authorize]
public class EditModel : PageModel
{
    private readonly MyBooksService _service;

    public EditModel(MyBooksService service)
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

    public IActionResult OnGet(long id)
    {
        var book = _service.GetById(id);
        if (book == null) return NotFound();
        if (book.OwnerId != GetCurrentMemberId()) return Forbid();

        Book = book;
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();

        _service.Update(Book, GetCurrentMemberId());
        return RedirectToPage("./Index");
    }
}
