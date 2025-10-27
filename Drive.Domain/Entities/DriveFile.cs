namespace Drive.Domain.Entities;

public sealed class DriveFile
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; private set; }
    public string FileName { get; private set; } = null!;
    public string ContentType { get; private set; } = "application/octet-stream";
    public long Size { get; private set; }
    public string StoragePath { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public bool IsDeleted { get; private set; }

    public DriveFile() { }

    public DriveFile(
        Guid userId,
        string fileName,
        string contentType,
        long size,
        string storagePath
    )
    {
        UserId = userId;
        FileName = fileName;
        ContentType = contentType;
        Size = size;
        StoragePath = storagePath;
    }

    public void MarkAsDeleted() => IsDeleted = true;

    public void Restore() => IsDeleted = false;
}
