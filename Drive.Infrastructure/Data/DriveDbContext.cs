using Drive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Drive.Infrastructure.Data;

public sealed class DriveDbContext(DbContextOptions<DriveDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<DriveFile> Files => Set<DriveFile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(cfg =>
        {
            cfg.ToTable("users");
            cfg.HasKey(x => x.Id);
            cfg.Property(x => x.Email).IsRequired().HasMaxLength(256);
            cfg.Property(x => x.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<DriveFile>(cfg =>
        {
            cfg.ToTable("files");
            cfg.HasKey(x => x.Id);
            cfg.Property(x => x.FileName).IsRequired().HasMaxLength(256);
            cfg.Property(x => x.ContentType).IsRequired().HasMaxLength(256);
            cfg.Property(x => x.Size).IsRequired();
            cfg.Property(x => x.StoragePath).IsRequired().HasMaxLength(512);
            cfg.Property(x => x.CreatedAt).IsRequired();
            cfg.Property(x => x.IsDeleted).HasDefaultValue(false).IsRequired();

            cfg.HasQueryFilter(x => !x.IsDeleted);

            cfg.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
