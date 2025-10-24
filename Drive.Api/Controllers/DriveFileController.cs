using Drive.Business.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Drive.Api.Controllers;

[ApiController]
[Route("/files")]
public class DriveFileController(
    UploadFileHandler uploadFileHandler,
    GetFileHandler getFileHandler,
    DeleteFileHandler deleteFileHandler
) : ControllerBase
{
    [HttpPost(Name = "PostDriveFile")]
    public async Task<IActionResult> UploadFile(
        [FromHeader(Name = "X-User-Id")] string? userId,
        CancellationToken ct
    )
    {
        if (string.IsNullOrWhiteSpace(userId) || !int.TryParse(userId, out var uid) || uid <= 0)
            return BadRequest("Cabeçalho X-User-Id inválido");

        if (!Request.HasFormContentType)
            return BadRequest("Envie multipart/form-data");

        var form = await Request.ReadFormAsync(ct);
        var file = form.Files.GetFile("file");

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

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteFile(Guid id, CancellationToken ct)
    {
        var ok = await deleteFileHandler.HandleAsync(id, ct);
        if (!ok)
            return NotFound();
        return NoContent();
    }
}
