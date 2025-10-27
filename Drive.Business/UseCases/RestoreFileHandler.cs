using Drive.Domain.Interfaces;

namespace Drive.Business.UseCases;

public class RestoreFileHandler(IFileRepository repository)
{
    public async Task<bool> HandleAsync(Guid id, CancellationToken ct)
    {
        var entity = await repository.GetByIdIncludingDeletedAsync(id, ct);
        if (entity is null)
            return false;

        if (!entity.IsDeleted)
            return true;

        entity.Restore();

        await repository.SaveChangesAsync();
        return true;
    }
}
