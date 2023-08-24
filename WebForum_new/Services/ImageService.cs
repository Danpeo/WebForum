using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using WebForum_new.Data.Settings;

namespace WebForum_new.Services;

public class ImageService : IImageService
{
    private readonly Cloudinary _cloundinary;

    public ImageService(IOptions<CloudinarySettings> config)
    {
        var account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );
        _cloundinary = new Cloudinary(account);
    }

    public async Task<ImageUploadResult?> UploadImageAsync(IFormFile? file, int height = 500, int width = 500)
    {
        if (file != null && file.Length > 0)
        {
            var uploadResult = new ImageUploadResult();

            await using Stream stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(height).Width(width).Crop("fill").Gravity("face")
            };
            uploadResult = await _cloundinary.UploadAsync(uploadParams);

            return uploadResult;
        }

        return null;
    }

    public async Task<DeletionResult> DeleteImageAsync(string url)
    {
        string publicId = url.Split('/').Last().Split('.')[0];
        var deleteParams = new DeletionParams(publicId);
        return await _cloundinary.DestroyAsync(deleteParams);
    }
}