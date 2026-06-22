using BookLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using BookLibrary.Domain;

namespace BookLibrary.Pages.Members;

[Authorize]
public class ReservationsModel : PageModel
{
    private readonly BorrowingService _borrowingService;

    public ReservationsModel(BorrowingService borrowingService)
    {
        _borrowingService = borrowingService;
    }

    public List<BorrowedBook> PendingRequests { get; set; } = new();
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    private long GetCurrentMemberId()
    {
        var idClaim = User.FindFirst("MemberId")?.Value;
        if (long.TryParse(idClaim, out var id)) return id;
        throw new InvalidOperationException("User is not authenticated as a member.");
    }

    public void OnGet()
    {
        var ownerId = GetCurrentMemberId();
        PendingRequests = _borrowingService.GetPendingRequestsForOwner(ownerId);
    }

    public IActionResult OnPostAccept(long requestId)
    {
        try
        {
            var ownerId = GetCurrentMemberId();
            _borrowingService.AcceptReservation(requestId, ownerId);
            SuccessMessage = "Request accepted.";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }

        OnGet();
        return Page();
    }
}
