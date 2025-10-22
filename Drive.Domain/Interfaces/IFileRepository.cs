using Drive.Domain.Entities;

namespace Drive.Domain.Interfaces;

public interface IFileRepository
{
    Task<DriveFile> CreateAsync(DriveFile entity, CancellationToken cancellationToken = default);
    Task<DriveFile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DriveFile>> SearchAsync(string? query = null, int? ownerId = null, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, bool hardDelete = false, CancellationToken cancellationToken = default);
}