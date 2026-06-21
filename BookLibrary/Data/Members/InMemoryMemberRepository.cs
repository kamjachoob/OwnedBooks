using BookLibrary.Domain;

namespace BookLibrary.Data.Members;

public class InMemoryMemberRepository : IMemberRepository
{
    private readonly Dictionary<long, Member> _store = new();

    Member IMemberRepository.GetById(long id) => _store[id];

    void IMemberRepository.Save(Member member) => _store[member.Id] = member;
}