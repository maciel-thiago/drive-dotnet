using Drive.Business.DTOs;
using Drive.Business.Services;
using Drive.Domain.Interfaces;

namespace Drive.Business.UseCases;

public sealed class GetFileHandler(IFileRepository repository, IFileStorage fileStorage)
{
    public async Task<(FileDetailsDTO meta, Stream? content)> HandleAsync(
        Guid id,
        bool withContent,
        CancellationToken ct
    )
    {
        var entity = await repository.GetByIdAsync(id, ct);
        if (entity is null)
            return (null, null);

        var meta = new FileDetailsDTO(
            entity.Id,
            entity.UserId,
            entity.FileName,
            entity.ContentType,
            entity.Size,
            entity.StoragePath,
            entity.CreatedAt,
            entity.IsDeleted
        );
        if (!withContent)
            return (meta, null);

        var stream = await fileStorage.OpenFileAync(entity.FileName, ct);
        return (meta, stream);
    }
}
