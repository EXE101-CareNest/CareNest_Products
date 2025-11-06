using CareNest_Products.Application.Common.Options;
using CareNest_Products.Application.Interfaces.Services;
using Microsoft.Extensions.Options;
using Shared.Contracts;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CareNest_Products.Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly HttpClient _httpClient;
        private readonly APIServiceOption _option;

        public ImageService(HttpClient httpClient, IOptions<APIServiceOption> option)
        {
            _httpClient = httpClient;
            _option = option.Value;
        }

        public async Task<ImageResponse> UploadAsync(
            Stream fileStream,
            string fileName,
            string contentType,
            string ownerId,
            string folder,
            string publicId)
        {
            var baseUrl = _option.BaseUrlImage?.Trim();
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new InvalidOperationException("APIService:BaseUrlImage chưa được cấu hình");
            }

            // Đảm bảo baseUrl là absolute URI (có http:// hoặc https://)
            if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var baseUri))
            {
                throw new InvalidOperationException($"APIService:BaseUrlImage không phải là absolute URI hợp lệ. Giá trị hiện tại: {baseUrl}");
            }

            // Loại bỏ trailing slash từ baseUrl
            baseUrl = baseUrl.TrimEnd('/');

            var endpoint = $"/api/Images/{Uri.EscapeDataString(ownerId)}";
            var fullUrl = $"{baseUrl}{endpoint}";
            
            // Validate fullUrl là absolute URI
            if (!Uri.TryCreate(fullUrl, UriKind.Absolute, out var fullUri))
            {
                throw new InvalidOperationException($"URL không hợp lệ: {fullUrl}");
            }

            using var form = new MultipartFormDataContent();

            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            form.Add(streamContent, "file", fileName);
            form.Add(new StringContent(folder), "folder");
            form.Add(new StringContent(publicId), "publicId");

            using var response = await _httpClient.PostAsync(fullUrl, form);
            response.EnsureSuccessStatusCode();

            var payload = await response.Content.ReadFromJsonAsync<ImageResponse>();
            if (payload == null)
            {
                throw new InvalidOperationException("Phản hồi upload ảnh không hợp lệ");
            }

            return payload;
        }
    }
}


