using BookLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookLibrary.Domain;

namespace BookLibrary.Pages.MyBooks;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly MyBooksService _service;

    public DeleteModel(MyBooksService service)
    {
        _service = service;
    }

    public Book? Book { get; set; }
    public string? ErrorMessage { get; set; }

    private long GetCurrentMemberId()
    {
        var idClaim = User.FindFirst("MemberId")?.Value;
        if (long.TryParse(idClaim, out var id)) return id;
        throw new InvalidOperationException("User is not authenticated as a member.");
    }

    public IActionResult OnGet(long id)
    {
        Book = _service.GetById(id);
        if (Book == null) return NotFound();
        if (Book.OwnerId != GetCurrentMemberId()) return Forbid();
        return Page();
    }

    public IActionResult OnPost(long id)
    {
        try
        {
            _service.Delete(id, GetCurrentMemberId());
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            Book = _service.GetById(id);
            return Page();
        }
    }
}
