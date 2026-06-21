namespace BookLibrary.Domain
{
    public class BorrowedBook
    {
        public long Id { get; set; }
        public long bookId { get; set; }
        public long memberId { get; set; }

        public DateTime BorrowedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }

        public Book Book { get; set; }
        public Member Member { get; set; }
    }
}
