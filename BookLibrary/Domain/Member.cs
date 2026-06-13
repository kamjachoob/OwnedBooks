namespace BookLibrary.Domain
{
    public class Member
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>();
    }
}
