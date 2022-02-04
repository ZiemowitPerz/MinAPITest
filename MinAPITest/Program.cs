using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UserDb>(opt => opt.UseInMemoryDatabase("UserDatabase"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();


app.MapGet("/", () => "PrzejdŸ na /users lub /users/id");

app.MapGet("/users", async (UserDb db) =>
    await db.Users.ToListAsync());

app.MapGet("/users/{id}", async (int id, UserDb db) =>
    await db.Users.FindAsync(id)
        is User user
            ? Results.Ok(user)
            : Results.NotFound());

app.MapPost("/users", async (User user, UserDb db) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/users/{user.Id}", user);
});


app.Run();


class User
{
    public int Id { get; set; }
    public string? Imie { get; set; }
    public string? Nazwisko { get; set; }
    public DateTime? Data_Urodzenia { get; set; }
    public string? Adres { get; set; }
}

class UserDb : DbContext
{
    public UserDb(DbContextOptions<UserDb> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
}