using Book.API.Models.Categories;
using Book.API.Models.Payments;
using Book.API.Models.Tokens;
using Book.API.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Book.API.Contexts;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options): base(options) { }

    public DbSet<Models.Books.Book> Books { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Payment>()
            .HasOne(p => p.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);  // Ensure delete behavior is correctly configured

        builder.Entity<Payment>()
            .HasOne(p => p.PaymentMethod)
            .WithMany()
            .HasForeignKey(p => p.PaymentMethodId)
            .OnDelete(DeleteBehavior.Cascade);
        var payments = new List<PaymentMethod>()
        {
            new PaymentMethod {Id = 1, Name = "Credit Card" },
            new PaymentMethod {Id = 2,  Name = "PayPal" }
        };
        builder.Entity<PaymentMethod>().HasData(payments);
    }
}
