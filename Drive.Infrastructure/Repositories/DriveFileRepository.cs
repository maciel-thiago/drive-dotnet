using Drive.Domain.Entities;
using Drive.Domain.Interfaces;
using Drive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Drive.Infrastructure.Repositories;

public sealed class DriveFileRepository(DriveDbContext db) : IFileRepository
{
    public Task<DriveFile?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.Files.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id, ct);

    public Task<DriveFile?> GetByIdIncludingDeletedAsync(Guid id, CancellationToken ct = default) =>
        db.Files.IgnoreQueryFilters().FirstOrDefaultAsync(f => f.Id == id, ct);

    public async Task AddAsync(DriveFile file, CancellationToken ct = default) =>
        await db.Files.AddAsync(file, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) => db.SaveChangesAsync(ct);

    public Task DeleteAsync(DriveFile file, CancellationToken ct = default)
    {
        file.MarkAsDeleted();
        db.Files.Update(file);
        return Task.CompletedTask;
    }

    public async Task<IReadOnlyList<DriveFile>> GetDeletedAsync(CancellationToken ct = default)
    {
        return await db
            .Files.IgnoreQueryFilters()
            .Where(f => f.IsDeleted)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync(ct);
    }
}
