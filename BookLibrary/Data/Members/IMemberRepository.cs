using BookLibrary.Domain;

namespace BookLibrary.Data.Members;

public interface IMemberRepository
{
    public Member GetById(long id);
    public void Save(Member member);
    public List<Member> GetAll();
}
