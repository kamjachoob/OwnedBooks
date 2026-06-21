using BookLibrary.Domain;

namespace BookLibrary.Data.Members;

public class InMemoryMemberRepository : IMemberRepository
{
    private readonly Dictionary<long, Member> _store = new();
    private long _nextId = 1;

    Member IMemberRepository.GetById(long id)
    {
        _store.TryGetValue(id, out var member);
        return member;
    }

    void IMemberRepository.Save(Member member)
    {
        if (member.Id == 0)
        {
            member.Id = _nextId++;
        }

        _store[member.Id] = member;
    }
}