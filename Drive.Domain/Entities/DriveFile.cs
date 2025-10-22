namespace Drive.Domain.Entities;

public class DriveFile
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int OwnerId { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public string StoragePath { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
}