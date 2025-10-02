using Microsoft.EntityFrameworkCore;
using ShAbedi.PayaSystem.Domain.Entities;
using ShAbedi.PayaSystem.Domain.Enums;

namespace ShAbedi.PayaSystem.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        optionsBuilder.UseSqlServer(
    //            "Data Source=.;Initial Catalog=PayaSystem;User ID=sa;Password=123;Persist Security Info=True;TrustServerCertificate=True");
    //    }
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Account Config
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Accounts");

            entity.HasKey(a => a.Id);

            entity.Property(a => a.OwnerName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(a => a.ShebaNumber)
                .IsRequired()
                .HasMaxLength(26);

            entity.HasIndex(a => a.ShebaNumber)
                .IsUnique();

            entity.Property(a => a.Balance)
                .IsRequired();

            entity.HasMany(a => a.Transactions)
                .WithOne(t => t.Account!)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(a => a.AmountLocks)
                .WithOne(t => t.Account!)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Transaction Config
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("Transactions");

            entity.HasKey(t => t.Id);

            entity.Property(t => t.Amount)
                .IsRequired();

            entity.Property(t => t.Type)
                .HasConversion<string>()
                .IsRequired();

            entity.Property(t => t.CreatedAt)
                .IsRequired();

            entity.Property(t => t.Note)
                .HasMaxLength(500);
        });

        modelBuilder.Entity<AmountLock>(entity =>
        {
            entity.ToTable("AmountLocks");

            entity.HasKey(l => l.Id);

            entity.Property(l => l.Amount).IsRequired();
            entity.Property(l => l.ShebaRequestId).IsRequired();

            entity.Property(r => r.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue(AmountLockStatus.Locked);
            // Global Query Filter 
            entity.HasQueryFilter(l => l.Status == AmountLockStatus.Locked);

            entity.HasOne( p=> p.Account) // هر Lock به یک Account تعلق دارد
                .WithMany(a => a.AmountLocks)
                .HasForeignKey(a => a.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p=> p.ShebaRequest) // هر Lock به یک ShebaRequest تعلق دارد
                .WithMany(r => r.AmountLocks)
                .HasForeignKey(a => a.ShebaRequestId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ShebaRequest>(entity =>
        {
            entity.ToTable("ShebaRequests");

            entity.HasKey(r => r.Id);

            entity.Property(r => r.Price)
                .IsRequired();

            entity.Property(r => r.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(r=> r.FromAccount)
                .WithMany()
                .HasForeignKey(r => r.FromAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(r => r.FromShebaNumber)
                .IsRequired()
                .HasMaxLength(26);

            entity.Property(r => r.ToShebaNumber)
                .IsRequired()
                .HasMaxLength(26);

            entity.Property(r => r.Note)
                .HasMaxLength(500);

            entity.HasMany(r => r.AmountLocks)
                .WithOne(p=> p.ShebaRequest)
                .HasForeignKey(a => a.ShebaRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            // ارتباط غیرمستقیم با Account از طریق شماره شبا (نه FK)
            // پس نیازی به HasOne<Account> نیست چون ما ShebaNumber نگه‌می‌داریم
        });
    }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<ShebaRequest> ShebaRequest => Set<ShebaRequest>();
}
