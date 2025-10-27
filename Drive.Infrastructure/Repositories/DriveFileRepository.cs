using Drive.Domain.Entities;
using Drive.Domain.Interfaces;
using Drive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Drive.Infrastructure.Repositories;

public sealed class DriveFileRepository(DriveDbContext db) : IFileRepository
{
    public Task<DriveFile?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.Files.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id, ct);

    public async Task AddAsync(DriveFile file, CancellationToken ct = default) =>
        await db.Files.AddAsync(file, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) => db.SaveChangesAsync(ct);

    public Task DeleteAsync(DriveFile file, CancellationToken ct = default)
    {
        file.MarkAsDeleted();
        db.Files.Update(file);
        return Task.CompletedTask;
    }
}
