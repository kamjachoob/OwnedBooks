using BookLibrary.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Data
{
    public class MyDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<BorrowedBook> BorrowedBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BorrowedBook>(entity =>
            {
                // Define the Composite Key (A member can't borrow the same book instance twice simultaneously without returning it)
                entity.HasKey(e => new { e.memberId, e.bookId });

                entity.HasOne(bb => bb.Member)
                      .WithMany(m => m.BorrowedBooks)
                      .HasForeignKey(bb => bb.memberId);

                entity.HasOne(bb => bb.Book)
                      .WithMany(b => b.BorrowedBooks)
                      .HasForeignKey(bb => bb.bookId);
            });
        }
    }
}
