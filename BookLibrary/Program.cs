using BookLibrary.Common.Settings;
using BookLibrary.Data;
using BookLibrary.Data.Books;
using BookLibrary.Data.BorrowedBooks;
using BookLibrary.Data.Members;
using BookLibrary.Domain;
using BookLibrary.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MyDbContext") ?? throw new InvalidOperationException("Connection string 'MyDbContext' not found.");

//builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddRazorPages();

// Add cookie authentication (simple demo login)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
});

builder.Services.AddScoped<BorrowingService>();

IBookRepository bookRepo = new InMemoryBookRepository();
IMemberRepository memberRepo = new InMemoryMemberRepository();
IBorrowedBookRepository borrowRecordRepo = new InMemoryBorrowedBookRepository();

builder.Services.AddSingleton<IBookRepository>(bookRepo);
builder.Services.AddSingleton<IMemberRepository>(memberRepo);
builder.Services.AddSingleton<IBorrowedBookRepository>(borrowRecordRepo);

builder.Services.Configure<BookSettings>(builder.Configuration.GetSection("BookSettings"));

// Seed demo data: members and books owned by them
var alice = new Member { Name = "Alice Johnson" };
var bob = new Member { Name = "Bob Smith" };

memberRepo.Save(alice);
memberRepo.Save(bob);

var book1 = new Book
{
    Title = "Clean Code",
    Author = "Robert C. Martin",
    ISBN = "978-0132350884",
    Status = BookStatus.Available,
    OwnerId = alice.Id
};

var book2 = new Book
{
    Title = "The Pragmatic Programmer",
    Author = "Andrew Hunt",
    ISBN = "978-0201616224",
    Status = BookStatus.Available,
    OwnerId = bob.Id
};

bookRepo.Save(book1);
bookRepo.Save(book2);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
