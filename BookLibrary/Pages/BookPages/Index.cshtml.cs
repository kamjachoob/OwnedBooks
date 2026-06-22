using BookLibrary.Data.Books;
using BookLibrary.Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookLibrary.Pages.BookPages;

public class IndexModel : PageModel
{
    private readonly IBookRepository _bookRepository;

    public IndexModel(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public IList<Book> Book { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Book = _bookRepository.GetAllAvailable();
    }
}
