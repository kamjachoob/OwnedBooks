using BookLibrary.Data.Books;
using BookLibrary.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookLibrary.Pages.BookPages;

public class DetailsModel : PageModel
{
    private readonly IBookRepository _bookRepository;
    public DetailsModel(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(long id)
    {
        var book = _bookRepository.GetById(id);
        if (book is null)
        {
            return NotFound();
        }
        else
        {
            Book = book;
        }

        return Page();
    }
}
