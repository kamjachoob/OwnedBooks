namespace BookLibrary.Domain
{
    public class Book
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public BookStatus Status { get; set; }

        // Owner of the book (the person who owns/lends it)
        public long? OwnerId { get; set; }
        public Member? Owner { get; set; }

        public ICollection<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>();
    }
}

public enum BookStatus { Available, Borrowed, Reserved, Lost }
