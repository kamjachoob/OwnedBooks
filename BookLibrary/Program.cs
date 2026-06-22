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
builder.Services.AddScoped<BorrowingService>();

IBookRepository bookRepo = new InMemoryBookRepository();
IMemberRepository memberRepo = new InMemoryMemberRepository();
IBorrowedBookRepository borrowRecordRepo = new InMemoryBorrowedBookRepository();

builder.Services.AddSingleton<IBookRepository>(bookRepo);
builder.Services.AddSingleton<IMemberRepository>(memberRepo);
builder.Services.AddSingleton<IBorrowedBookRepository>(borrowRecordRepo);

builder.Services.Configure<BookSettings>(builder.Configuration.GetSection("BookSettings"));

// 3. Seed the data into those instances
var book = new Book
{
    Id = 1,
    Title = "Clean Code",
    Author = "Robert C. Martin",
    ISBN = "978-0132350884",
    Status = BookStatus.Available,
};

var member = new Member
{
    Id = 1,
    Name = "Alice Johnson"
};

bookRepo.Save(book);
memberRepo.Save(member);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();