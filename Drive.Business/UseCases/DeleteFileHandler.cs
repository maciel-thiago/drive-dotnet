using Drive.Business.Services;
using Drive.Domain.Interfaces;

namespace Drive.Business.UseCases;

public class DeleteFileHandler(IFileRepository repository, IFileStorage storage)
{
    public async Task<bool> HandleAsync(Guid id, CancellationToken ct)
    {
        var entity = await repository.GetByIdAsync(id, ct);
        if (entity is null)
            return false;

        await storage.DeleteAsync(entity.StoragePath, ct);
        await repository.DeleteAsync(entity, ct);
        await repository.SaveChangesAsync(ct);
        return true;
    }
}
