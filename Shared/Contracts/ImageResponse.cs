namespace Shared.Contracts
{
    public class ImageResponse
    {
        public string Id { get; set; } = string.Empty;
        public string OwnerId { get; set; } = string.Empty;
        public string PublicId { get; set; } = string.Empty;
        public string SecureUrl { get; set; } = string.Empty;
        public string OptimizedUrl { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public long Bytes { get; set; }
        public string Format { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public DateTimeOffset CreatedAtUtc { get; set; }
    }
}


