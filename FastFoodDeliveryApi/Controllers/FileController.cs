using FastFoodDeliveryApi.Data;
using FastFoodDeliveryApi.Models.Models.File;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastFoodDeliveryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    
    private readonly AppDbContext _context;

    public FileController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("GetFiles")]
    public async Task<IActionResult> GetFiles()
    {
        var files = await _context.Files.ToListAsync();
        if (files.Count == 0)
        {
            return NotFound();
        }
        return Ok(files);
    }
    
    [HttpPost]
    [Route("UploadFile")]
    public async Task<IActionResult> UploadFile(IFormFile uploadedFile)
    {
        if (uploadedFile == null || uploadedFile.Length == 0)
        {
            return BadRequest("File is empty or null.");
        }

        try
        {
            using var memoryStream = new MemoryStream();
            await uploadedFile.CopyToAsync(memoryStream);

            var fileEntity = new Models.Entities.File
            {
                FileId = Guid.NewGuid(),
                FileName = uploadedFile.FileName,
                Extension = Path.GetExtension(uploadedFile.FileName),
                Content = memoryStream.ToArray(),
                FileSize = uploadedFile.Length
            };

            _context.Files.Add(fileEntity);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "File uploaded successfully", fileId = fileEntity.FileId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    [HttpGet]
    [Route("GetFile")]
    public async Task<IActionResult> GetFile(Guid fileId)
    {
        var file = await _context.Files.FindAsync(fileId);
        if (file == null)
        {
            return NotFound($"File with ID {fileId} not found.");
        }

        var fileDto = new FileModel
        {
            FileId = file.FileId,
            FileExtension = file.Extension,
            FileName = file.FileName,
            ContentBase64 = Convert.ToBase64String(file.Content)
        };
        return Ok(fileDto);
    }
    
    [HttpDelete]
    [Route("DeleteFile")]
    public async Task<IActionResult> DeleteFile(Guid fileId)
    {
        var file = await _context.Files.FindAsync(fileId);
        if (file == null)
        {
            return NotFound($"File with ID {fileId} not found.");
        }
        _context.Files.Remove(file);
        await _context.SaveChangesAsync();
        return Ok(new { Message = "File deleted successfully." });
    }
    
}