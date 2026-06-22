using BookLibrary.Data.Books;
using BookLibrary.Data.Members;
using BookLibrary.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookLibrary.Pages.BookPages;

public class CreateModel : PageModel
{
    private readonly IBookRepository _bookRepository;
    private readonly IMemberRepository _memberRepository;

    public CreateModel(IBookRepository bookRepository, IMemberRepository memberRepository)
    {
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
    }

    public IActionResult OnGet()
    {
        Members = _memberRepository.GetAll();
        return Page();
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public List<Member> Members { get; set; } = new();

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Members = _memberRepository.GetAll();
            return Page();
        }

        // Ensure newly added book starts as Available
        Book.Status = BookStatus.Available;

        _bookRepository.Save(Book);

        return RedirectToPage("./Index");
    }
}
