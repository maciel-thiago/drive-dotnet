namespace Drive.Business.DTOs;

public sealed record FileDetailsDTO
(
    Guid Id,
    int UserId,
    string FileName,
    string ContentType,
    long Size,
    string StoragePath,
    DateTime CreatedAt,
    bool IsDeleted
);