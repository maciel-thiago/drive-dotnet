using Drive.Business.DTOs;
using Drive.Business.Services;
using Drive.Domain.Entities;
using Drive.Domain.Interfaces;

namespace Drive.Business.UseCases;

public sealed class UploadFileHandler(IFileRepository repository, IFileStorage fileStorage)
{
    public async Task<FileDetailsDto> HandleAsync(
        Guid userId,
        string filename,
        string contentType,
        Stream stream,
        CancellationToken ct
    )
    {
        var stored = await fileStorage.SaveFileAsync(stream, filename, ct);
        var driveFile = new DriveFile(userId, filename, contentType, stream.Length, stored);
        var entity = driveFile;
        await repository.AddAsync(entity, ct);
        await repository.SaveChangesAsync(ct);

        return new FileDetailsDto(
            entity.Id,
            entity.UserId,
            entity.FileName,
            entity.ContentType,
            entity.Size,
            entity.StoragePath,
            entity.CreatedAt,
            entity.IsDeleted
        );
    }
}
