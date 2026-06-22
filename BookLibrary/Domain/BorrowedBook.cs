namespace BookLibrary.Domain
{
    public enum BorrowStatus { Requested, Accepted, Borrowed, Returned }

    public class BorrowedBook
    {
        public long Id { get; set; }
        public long bookId { get; set; }
        public long memberId { get; set; }

        // When the request was made
        public DateTime RequestDate { get; set; }
        // When the owner accepted the request
        public DateTime? AcceptedDate { get; set; }
        // When the book was actually picked up / borrowed
        public DateTime? BorrowedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }

        public BorrowStatus Status { get; set; } = BorrowStatus.Requested;

        public Book? Book { get; set; }
        public Member? Member { get; set; }
    }
}
