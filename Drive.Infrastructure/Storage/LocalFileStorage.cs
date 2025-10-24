using Drive.Business.Services;

namespace Drive.Infrastructure.Storage;

public class LocalFileStorage : IFileStorage
{
    private readonly string _root;

    public LocalFileStorage(string? root = null)
    {
        _root = root ?? Path.Combine(Directory.GetCurrentDirectory(), "storage");
        Directory.CreateDirectory(_root);
    }

    public async Task<string> SaveFileAsync(
        Stream stream,
        string filename,
        CancellationToken ct = default
    )
    {
        var storedName = $"{Guid.NewGuid():N}_{Path.GetFileName(filename)}";
        var storagePath = Path.Combine(_root, storedName);
        stream.Position = 0;
        await using var fileStream = new FileStream(
            storagePath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None
        );
        await stream.CopyToAsync(fileStream, ct);
        return storedName;
    }

    public Task<Stream?> OpenFileAync(string storagePath, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(_root, storagePath);
        if (!File.Exists(fullPath))
            return Task.FromResult<Stream?>(null);
        Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Task.FromResult<Stream?>(stream);
    }

    public Task DeleteAsync(string storagePath, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(_root, storagePath);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
        return Task.CompletedTask;
    }
}
