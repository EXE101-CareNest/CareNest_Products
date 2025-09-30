using System.ComponentModel.DataAnnotations;

namespace CareNest_Products.Domain.Commons
{
    /// <summary>
    /// Base entity class với các thuộc tính chung
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Primary key - GUID
        /// </summary>
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Thời gian cập nhật
        /// </summary>
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Người cập nhật
        /// </summary>
        public string? UpdatedBy { get; set; }
    }
}
