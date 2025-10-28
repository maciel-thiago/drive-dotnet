using Drive.Business.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Drive.Api.Controllers;

[ApiController]
[Route("files")]
public class DriveFileController(
    UploadFileHandler uploadFileHandler,
    GetFileHandler getFileHandler,
    DeleteFileHandler deleteFileHandler,
    RestoreFileHandler restoreFileHandler,
    ListDeletedFilesHandler listDeletedFilesHandler,
    GetFilesByUserHandler getFileByUserHandler
) : ControllerBase
{
    [HttpPost(Name = "PostDriveFile")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadFile(
        [FromHeader(Name = "X-User-Id")] string? userId,
        IFormFile? file,
        CancellationToken ct
    )
    {
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out Guid uid))
            return BadRequest("Cabeçalho X-User-Id inválido");

        if (!Request.HasFormContentType)
            return BadRequest("Envie multipart/forma-data");

        if (file == null || file.Length == 0)
            return BadRequest("Arquivo ausente");

        await using var fileStream = file.OpenReadStream();

        var dto = await uploadFileHandler.HandleAsync(
            uid,
            file.FileName,
            file.ContentType ?? "application/octet-stream",
            fileStream,
            ct
        );

        return CreatedAtAction(nameof(GetFile), new { id = dto.Id }, dto);
    }

    [HttpGet("{id:guid}", Name = "GetDriveFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFile(
        Guid id,
        [FromQuery] bool download,
        CancellationToken ct
    )
    {
        var (meta, content) = await getFileHandler.HandleAsync(id, download, ct);

        if (!download)
            return Ok(meta);

        if (content is null)
            return NotFound();

        return File(content, meta.ContentType, meta.FileName);
    }

    [HttpPost("{id:guid}/restore", Name = "RestoreFile")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RestoreFile(Guid id, CancellationToken ct)
    {
        var ok = await restoreFileHandler.HandleAsync(id, ct);
        if (!ok)
            return NotFound();
        return NoContent();
    }

    [HttpGet("deleted", Name = "GetDeletedFiles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDeletedFiles(CancellationToken ct)
    {
        var deletedFiles = await listDeletedFilesHandler.HandleAsync(ct);
        return Ok(deletedFiles);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFile(Guid id, CancellationToken ct)
    {
        var ok = await deleteFileHandler.HandleAsync(id, ct);
        if (!ok)
            return NotFound();
        return NoContent();
    }

    [HttpGet("user/{userId:guid}", Name = "GetFilesByUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFilesByUser(Guid userId, CancellationToken ct)
    {
        var files = await getFileByUserHandler.HandleAsync(userId, ct);
        return Ok(files);
    }
}
