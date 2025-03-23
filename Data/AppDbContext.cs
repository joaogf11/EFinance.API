using EFinnance.API.ViewModels.Category;
using EFinnance.API.ViewModels.Expense;
using EFinnance.API.ViewModels.Revenue;
using EFinnance.API.ViewModels.User;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserViewModel> Users { get; set; }
    public DbSet<CategoryViewModel> Categories { get; set; }
    public DbSet<RevenueViewModel> Revenues { get; set; }
    public DbSet<ExpenseViewModel> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração de chaves estrangeiras
        modelBuilder.Entity<CategoryViewModel>()
            .HasOne(c => c.User)
            .WithMany(u => u.Categories)
            .HasForeignKey(c => c.UserId);

        modelBuilder.Entity<RevenueViewModel>()
            .HasOne(r => r.User)
            .WithMany(u => u.Revenues)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<RevenueViewModel>()
            .HasOne(r => r.Category)
            .WithMany(c => c.Revenues)
            .HasForeignKey(r => r.CategoryId);

        modelBuilder.Entity<ExpenseViewModel>()
            .HasOne(e => e.User)
            .WithMany(u => u.Expenses)
            .HasForeignKey(e => e.UserId);

        modelBuilder.Entity<ExpenseViewModel>()
            .HasOne(e => e.Category)
            .WithMany(c => c.Expenses)
            .HasForeignKey(e => e.CategoryId);
    }
}
