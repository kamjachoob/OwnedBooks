namespace BookLibrary.Domain
{
    public class BorrowedBook
    {
        public long BookId { get; set; }
        public long MemberId { get; set; }

        public DateTime BorrowedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }

        public Book Book { get; set; }
        public Member Member { get; set; }
    }
}
