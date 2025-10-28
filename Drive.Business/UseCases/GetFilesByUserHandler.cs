using Drive.Business.DTOs;
using Drive.Domain.Interfaces;

namespace Drive.Business.UseCases;

public class GetFilesByUserHandler(IFileRepository repository)
{
    public async Task<IReadOnlyList<FileDetailsDto>> HandleAsync(Guid userId, CancellationToken ct)
    {
        var filesForUser = await repository.GetByUserAsync(userId, ct);
        return filesForUser
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
