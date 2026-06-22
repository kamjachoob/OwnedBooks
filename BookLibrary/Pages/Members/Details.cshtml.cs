using BookLibrary.Data.Members;
using BookLibrary.Data.Books;
using BookLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using BookLibrary.Domain;

namespace BookLibrary.Pages.Members;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly IMemberRepository _memberRepository;
    private readonly IBookRepository _bookRepository;
    private readonly BorrowingService _borrowingService;

    public DetailsModel(IMemberRepository memberRepository, IBookRepository bookRepository, BorrowingService borrowingService)
    {
        _memberRepository = memberRepository;
        _bookRepository = bookRepository;
        _borrowingService = borrowingService;
    }

    public Member Member { get; set; } = new();
    public List<Book> OwnedBooks { get; set; } = new();
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    private long GetCurrentMemberId()
    {
        var idClaim = User.FindFirst("MemberId")?.Value;
        if (long.TryParse(idClaim, out var id)) return id;
        throw new InvalidOperationException("User is not authenticated as a member.");
    }

    public void OnGet(long id)
    {
        Member = _memberRepository.GetById(id) ?? new Member();
        OwnedBooks = _bookRepository.GetByOwner(id);
    }

    public IActionResult OnPostBorrow(long bookId, long ownerId)
    {
        try
        {
            var currentMemberId = GetCurrentMemberId();
            _borrowingService.RequestBorrow(bookId, currentMemberId);
            SuccessMessage = "Borrow request submitted.";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }

        OnGet(ownerId);
        return Page();
    }
}
