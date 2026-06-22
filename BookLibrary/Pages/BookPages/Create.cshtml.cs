using BookLibrary.Data.Books;
using BookLibrary.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookLibrary.Pages.BookPages;

public class CreateModel : PageModel
{
    private readonly IBookRepository _bookRepository;

    public CreateModel(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _bookRepository.Save(Book);

        return RedirectToPage("./Index");
    }
}
