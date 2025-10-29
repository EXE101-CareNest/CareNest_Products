using Shared.Contracts;

namespace CareNest_Products.Application.Interfaces.Services
{
    public interface IImageService
    {
        Task<ImageResponse> UploadAsync(
            Stream fileStream,
            string fileName,
            string contentType,
            string ownerId,
            string folder,
            string publicId);
    }
}


