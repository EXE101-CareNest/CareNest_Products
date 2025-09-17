# Product Management System

## Tổng quan

Hệ thống quản lý sản phẩm được xây dựng bằng .NET C# với Visual Studio 2022, hỗ trợ quản lý sản phẩm theo mô hình 3 tầng: **Product** → **Product Category** → **Product Detail**.

## Cấu trúc dữ liệu

### 🏷️ Products (Sản phẩm chính)
- **Id**: GUID (Primary Key)
- **ShopId**: GUID (Foreign Key - tham chiếu đến Shop)
- **ProductName**: string (*required) - Tên sản phẩm (1-100 ký tự)
- **Description**: string (optional) - Mô tả sản phẩm (0-500 ký tự)
- **Status**: bool - Trạng thái (true: active, false: inactive)
- **ImgUrls**: string (*required) - Danh sách URL hình ảnh (phân tách bằng dấu phẩy)

**Ví dụ**: "Áo thun nam", "Quần jean nữ", "Giày thể thao"

### 🎨 Product_Category (Phân loại sản phẩm)
- **Id**: GUID (Primary Key)
- **ProductId**: GUID (Foreign Key - tham chiếu đến Products)
- **Name**: string (*required) - Tên danh mục/phân loại

**Ví dụ**: "Áo thun nam màu xanh", "Áo thun nam màu đỏ", "Quần jean nữ skinny"

### 💰 Product_Detail (Chi tiết sản phẩm)
- **Id**: GUID (Primary Key)
- **CategoryId**: GUID (Foreign Key - tham chiếu đến Product_Category)
- **Name**: string (*required) - Tên chi tiết cụ thể
- **Price**: int (*required) - Giá sản phẩm
- **Status**: bool (*required) - Trạng thái kho (true: còn hàng, false: hết hàng)
- **Discount**: double (optional) - Phần trăm giảm giá
- **IsDefault**: bool (*required) - Đánh dấu mặc định
- **ImgUrls**: string (optional) - Hình ảnh riêng cho chi tiết này
- **QuantityInStock**: int (*required) - Số lượng tồn kho

**Ví dụ**: "Áo thun nam màu xanh Size M - 120,000đ", "Áo thun nam màu xanh Size L - 130,000đ"

## Ví dụ về mối quan hệ

```
📦 Product: "Áo thun nam basic"
├── 🎨 Category: "Áo thun nam màu xanh"
│   ├── 💰 Detail: "Size M" - 120,000đ - Còn 50 cái
│   ├── 💰 Detail: "Size L" - 130,000đ - Còn 30 cái
│   └── 💰 Detail: "Size XL" - 140,000đ - Còn 20 cái
├── 🎨 Category: "Áo thun nam màu đỏ"
│   ├── 💰 Detail: "Size M" - 120,000đ - Còn 40 cái
│   └── 💰 Detail: "Size L" - 130,000đ - Hết hàng
└── 🎨 Category: "Áo thun nam màu đen"
    ├── 💰 Detail: "Size M" - 125,000đ - Còn 35 cái
    └── 💰 Detail: "Size L" - 135,000đ - Còn 25 cái
```

## Công nghệ sử dụng

- **.NET Framework/Core**: C#
- **IDE**: Visual Studio 2022
- **Database**: SQL Server (khuyến nghị)
- **ORM**: Entity Framework Core (khuyến nghị)

## Cài đặt và chạy dự án

### Yêu cầu hệ thống
- Visual Studio 2022
- .NET 6.0 hoặc mới hơn
- SQL Server 2019 hoặc mới hơn

### Bước 1: Clone repository
```bash
git clone <repository-url>
cd product-management-system
```

### Bước 2: Cấu hình Database
1. Mở file `appsettings.json`
2. Cập nhật connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ProductManagementDB;Trusted_Connection=true;"
  }
}
```

### Bước 3: Tạo Database
```bash
dotnet ef database update
```

### Bước 4: Chạy ứng dụng
```bash
dotnet run
```
hoặc nhấn **F5** trong Visual Studio 2022

## Validation Rules

### Products
- ✅ **Id**: Bắt buộc, định dạng GUID hợp lệ, không trùng lặp
- ✅ **ShopId**: Bắt buộc, phải tồn tại trong bảng Shop
- ✅ **ProductName**: Bắt buộc, 1-100 ký tự, không chứa ký tự đặc biệt
- ✅ **ImgUrls**: Bắt buộc, ít nhất 1 URL hợp lệ (http/https)

### Product_Category
- ✅ **Id**: Bắt buộc, định dạng GUID hợp lệ, không trùng lặp
- ✅ **ProductId**: Bắt buộc, phải tồn tại trong bảng Products
- ✅ **Name**: Bắt buộc, không được rỗng

### Product_Detail
- ✅ **Id**: Bắt buộc, định dạng GUID hợp lệ, không trùng lặp
- ✅ **CategoryId**: Bắt buộc, phải tồn tại trong bảng Product_Category
- ✅ **Price**: Bắt buộc, > 0
- ✅ **QuantityInStock**: Bắt buộc, >= 0
- ✅ **Discount**: Tùy chọn, 0-100%

## API Endpoints (Dự kiến)

### Products
- `GET /api/products` - Lấy danh sách sản phẩm
- `GET /api/products/{id}` - Lấy thông tin sản phẩm
- `POST /api/products` - Tạo sản phẩm mới
- `PUT /api/products/{id}` - Cập nhật sản phẩm
- `DELETE /api/products/{id}` - Xóa sản phẩm

### Product Categories
- `GET /api/products/{productId}/categories` - Lấy danh mục của sản phẩm
- `POST /api/products/{productId}/categories` - Tạo danh mục mới
- `PUT /api/categories/{id}` - Cập nhật danh mục
- `DELETE /api/categories/{id}` - Xóa danh mục

### Product Details
- `GET /api/categories/{categoryId}/details` - Lấy chi tiết theo danh mục
- `POST /api/categories/{categoryId}/details` - Tạo chi tiết mới
- `PUT /api/details/{id}` - Cập nhật chi tiết
- `DELETE /api/details/{id}` - Xóa chi tiết

## Cấu trúc thư mục dự án

```
📁 ProductManagementSystem/
├── 📁 Controllers/
│   ├── ProductsController.cs
│   ├── ProductCategoriesController.cs
│   └── ProductDetailsController.cs
├── 📁 Models/
│   ├── Product.cs
│   ├── ProductCategory.cs
│   └── ProductDetail.cs
├── 📁 Data/
│   └── ApplicationDbContext.cs
├── 📁 Services/
│   ├── IProductService.cs
│   └── ProductService.cs
├── 📁 DTOs/
│   ├── ProductDto.cs
│   ├── ProductCategoryDto.cs
│   └── ProductDetailDto.cs
└── Program.cs
```

## Features chính

- ✨ **CRUD Operations**: Tạo, đọc, cập nhật, xóa cho tất cả entities
- 🔍 **Search & Filter**: Tìm kiếm sản phẩm theo tên, danh mục
- 📊 **Inventory Management**: Quản lý tồn kho, trạng thái sản phẩm
- 🖼️ **Image Management**: Upload và quản lý hình ảnh sản phẩm
- 💸 **Pricing & Discount**: Quản lý giá và khuyến mãi
- 📱 **Responsive Design**: Giao diện thân thiện trên mọi thiết bị

## Đóng góp

1. Fork repository
2. Tạo feature branch: `git checkout -b feature/AmazingFeature`
3. Commit thay đổi: `git commit -m 'Add some AmazingFeature'`
4. Push lên branch: `git push origin feature/AmazingFeature`
5. Tạo Pull Request

## License

Distributed under the MIT License. See `LICENSE` for more information.

## Liên hệ

- **Developer**: [Tên của bạn]
- **Email**: [email@example.com]
- **Project Link**: [https://github.com/username/product-management-system]

---

⭐ **Đừng quên star repository nếu project hữu ích!** ⭐