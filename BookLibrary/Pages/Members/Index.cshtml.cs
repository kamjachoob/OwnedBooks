using BookLibrary.Data.Members;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookLibrary.Domain;

namespace BookLibrary.Pages.Members;

public class IndexModel : PageModel
{
    private readonly IMemberRepository _memberRepository;

    public IndexModel(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public List<Member> Members { get; set; } = new();

    public void OnGet()
    {
        Members = _memberRepository.GetAll();
    }
}
