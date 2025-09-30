using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Domain.Entities;
using System.Text.Json.Serialization;

namespace CareNest_Products.Application.Features.Commands.Update
{
    /// <summary>
    /// Command cập nhật chi tiết sản phẩm
    /// </summary>
    public class UpdateProductDetailCommand : ICommand<ProductDetail>
    {
        [JsonIgnore]
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public bool Status { get; set; } = true;
        public double? Discount { get; set; }
        public bool IsDefault { get; set; }
        public string? ImgUrls { get; set; }
        public int QuantityInStock { get; set; }
    }
}


