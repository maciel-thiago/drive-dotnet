using Drive.Business.DTOs;
using Drive.Business.Services;
using Drive.Domain.Entities;
using Drive.Domain.Interfaces;

namespace Drive.Business.UseCases;

public class ListDeletedFilesHandler(IFileRepository repository)
{
    public async Task<IEnumerable<FileDetailsDto>> HandleAsync(CancellationToken ct)
    {
        var deleted = await repository.GetDeletedAsync(ct);
        return deleted
            .Select(f => new FileDetailsDto(
                f.Id,
                f.UserId,
                f.FileName,
                f.ContentType,
                f.Size,
                f.StoragePath,
                f.CreatedAt,
                f.IsDeleted
            ))
            .ToList();
    }
}
