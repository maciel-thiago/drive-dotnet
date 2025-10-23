using Drive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Drive.Infrastructure.Data;

public sealed class DriveDbContext(DbContextOptions<DriveDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<DriveFile> Files => Set<DriveFile>();
}
