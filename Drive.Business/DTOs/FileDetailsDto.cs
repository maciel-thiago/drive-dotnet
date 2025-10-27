namespace Drive.Business.DTOs;

public sealed record FileDetailsDto(
    Guid Id,
    Guid UserId,
    string FileName,
    string ContentType,
    long Size,
    string StoragePath,
    DateTime CreatedAt,
    bool IsDeleted
);
