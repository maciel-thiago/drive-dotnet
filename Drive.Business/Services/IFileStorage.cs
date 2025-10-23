namespace Drive.Business.Services;

public interface IFileStorage
{
    Task<string> SaveFileAsync(Stream stream, string filename, CancellationToken ct = default);
    Task<Stream?> OpenFileAync(string storagePath, CancellationToken ct = default);
    Task DeleteAsync(string storagePath, CancellationToken ct = default);
}