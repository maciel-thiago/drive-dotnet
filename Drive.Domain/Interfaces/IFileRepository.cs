using Drive.Domain.Entities;

namespace Drive.Domain.Interfaces;

public interface IFileRepository
{
    Task<DriveFile?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(DriveFile file, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
    Task DeleteAsync(DriveFile file, CancellationToken ct = default);
}