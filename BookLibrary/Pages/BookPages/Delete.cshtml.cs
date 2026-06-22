using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookLibrary.Domain;
using BookLibrary.Data;
using BookLibrary.Data.Books;

namespace BookLibrary.Pages.BookPages;

public class DeleteModel : PageModel
{
    private readonly IBookRepository _bookRepository;

    public DeleteModel(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(long id)
    {
        var book =  _bookRepository.GetById(id);
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

    public async Task<IActionResult> OnPostAsync(long id)
    {

        var book = _bookRepository.GetById(id);
        
        if (book != null)
        {
            Book = book;
            _bookRepository.Delete(id);
        }

        else
        {
            return NotFound();
        }

        return RedirectToPage("./Index");
    }
}
