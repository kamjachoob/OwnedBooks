using BookLibrary.Data.Members;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BookLibrary.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IMemberRepository _memberRepository;

    public LoginModel(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public List<BookLibrary.Domain.Member> Members { get; set; } = new();

    public void OnGet()
    {
        Members = _memberRepository.GetAll();
    }

    public async Task<IActionResult> OnPostAsync(long memberId)
    {
        var member = _memberRepository.GetById(memberId);
        if (member == null) return BadRequest();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, member.Name),
            new Claim("MemberId", member.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToPage("/Index");
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage();
    }
}
