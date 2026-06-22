using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookLibrary.Domain;
using BookLibrary.Data;
using BookLibrary.Data.Books;

namespace BookLibrary.Pages.BookPages;

public class EditModel : PageModel
{
    private readonly IBookRepository _bookRepository;

    public EditModel(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(long id)
    {
        var book = _bookRepository.GetById(id);
        if (book is null)
        {
            return NotFound();
        }
        Book = book;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

            _bookRepository.Save(Book);
        //catch (DbUpdateConcurrencyException)
        //{
        //    //if (!BookExists(Book.Id))
        //    //{
        //    //    return NotFound();
        //    //}
        //    else
        //    {
        //        throw;
        //    }
        //}

        return RedirectToPage("./Index");
    }

    //private bool BookExists(long id)
    //{
    //    return _context.Books.Any(e => e.Id == id);
    //}
}
