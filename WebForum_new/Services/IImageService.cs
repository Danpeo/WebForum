using CloudinaryDotNet.Actions;

namespace WebForum_new.Services;

public interface IImageService
{
    Task<ImageUploadResult?> UploadImageAsync(IFormFile? file, int height = 500, int width = 500);
    Task<DeletionResult> DeleteImageAsync(string url);
}