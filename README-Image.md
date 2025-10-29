# Tài liệu tích hợp Lưu trữ Hình ảnh (Image Storage) – CareNest Service

## Tổng quan
Tài liệu này mô tả đầy đủ cách tích hợp tính năng lưu trữ hình ảnh đã triển khai trong dự án CareNest để bạn có thể tái sử dụng cho dự án .NET khác. Nội dung bao gồm kiến trúc, endpoint, hợp đồng request/response, cấu hình, tích hợp dịch vụ, cấu hình Swagger cho upload file, kiểm thử và xử lý sự cố.

## Kiến trúc và luồng xử lý
- **Client** gửi form-data chứa file hình ảnh và dữ liệu khác lên API Service (`CareNest_Service`).
- **API Service** gọi **Image Service** bên ngoài (qua HTTP) để upload file.
- **Image Service** trả về thông tin ảnh (URL tối ưu/URL bảo mật, kích thước, định dạng, v.v.).
- **API Service** lưu `ImgUrl` (thường dùng `optimizedUrl`) vào entity nghiệp vụ (ví dụ: `Service`).

Sơ đồ khái quát:
Client → CareNest Service API → Image Service (external) → Cloud Storage → URL ảnh trả về cho Client

## Endpoint bên ngoài (Image Service)
- URL: `http://localhost:8025/api/Images/{ownerId}`
- Method: `POST`
- Content-Type: `multipart/form-data`

Form fields yêu cầu:
- `file`: File hình ảnh (bắt buộc nếu upload)
- `folder`: Tên thư mục lưu trữ (khuyến nghị dùng `ServiceCategoryId` hay danh mục tương ứng)
- `publicId`: Định danh công khai của ảnh (khuyến nghị dùng ID thực thể, ví dụ `ServiceId`)

Ví dụ response từ Image Service:
```json
{
  "id": "0b34e349-dfba-4194-95b2-cae2994bdbba",
  "ownerId": "img",
  "publicId": "category-123/service-456",
  "secureUrl": "https://res.cloudinary.com/.../string.png",
  "optimizedUrl": "https://res.cloudinary.com/.../c_fill,f_auto,q_auto/v1/category-123/service-456",
  "width": 1778,
  "height": 1387,
  "bytes": 46903,
  "format": "png",
  "version": "1760108610",
  "createdAtUtc": "2025-10-13T00:45:24.7535502Z"
}
```

## Hợp đồng dữ liệu dùng chung
Các contract đã có sẵn trong thư mục `Shared/Contracts/`:
- `ImageRequest` (yêu cầu upload)
- `ImageResponse` (phản hồi upload)

Tái sử dụng các model này ở dự án khác để đồng bộ hoá dữ liệu qua các lớp API.

## Cấu hình ứng dụng
Thiết lập base URL cho Image Service trong `appsettings.Development.json` (và môi trường khác nếu cần):
```json
{
  "APIService": {
    "BaseUrlServiceCategory": "http://192.168.0.22:8080",
    "BaseUrlImage": "http://localhost:8025"
  }
}
```

Nếu deploy (ví dụ Railway), bạn có thể dùng biến môi trường:
```
APISERVICE__BASEURLIMAGE=https://your-image-service.com
```

## Interface và Service tích hợp
- Interface: `CareNest.Application/Interfaces/Services/IImageService.cs`
- Triển khai: `CareNest.Infrastructure/Services/ImageService.cs`

Chức năng chính:
- Nhận `IFormFile` và metadata (`ownerId`, `folder`, `publicId`).
- Gửi form-data tới Image Service (sử dụng `IAPIService` với phương thức POST form-data).
- Trả về `ImageResponse` để lưu URL ảnh vào domain entity (ví dụ `ImgUrl`).

Gợi ý tích hợp cho dự án khác:
1) Định nghĩa `IImageService` tương tự (trong layer Application).
2) Cài đặt `ImageService` (trong Infrastructure) sử dụng một HTTP client chung (`IAPIService` hay `HttpClientFactory`).
3) Đăng ký DI trong `Program.cs`.

## Tích hợp ở luồng nghiệp vụ (ví dụ: Tạo Service)
Luồng mẫu khi tạo mới thực thể có ảnh (ví dụ `Service`):
1. Validate ràng buộc liên quan (ví dụ `ServiceCategoryId`).
2. Tạo entity trước để có ID (dùng làm `ownerId` và/hoặc `publicId`).
3. Nếu có `imageFile`:
   - Gọi `IImageService.UploadAsync(imageFile, ownerId, folder, publicId)`.
   - Nhận `optimizedUrl` hoặc `secureUrl` từ `ImageResponse`.
   - Gán vào `entity.ImgUrl` và cập nhật entity.
4. Trả về dữ liệu hoàn chỉnh cho client.

Lưu ý đặt `folder` theo nhóm chức năng (ví dụ dùng `ServiceCategoryId`) để dễ quản trị.

## Controller/API nhận form-data
Endpoint mẫu nhận form-data (tương tự `POST /api/service`):
```
POST /api/your-entity
Content-Type: multipart/form-data

name: "Tên"
categoryId: "category-123"
description: "Mô tả"
imageFile: [chọn file]
status: true
```

## Cấu hình Swagger để hỗ trợ upload file
Để Swagger hiển thị input dạng file đúng chuẩn khi bạn dùng `IFormFile` và `[FromForm]`, cần map kiểu trong `Program.cs`:
```csharp
// Ví dụ cấu hình Swagger Schema cho IFormFile
c.MapType<IFormFile>(() => new OpenApiSchema
{
    Type = "string",
    Format = "binary"
});
```

## Kiểm thử nhanh bằng Postman/Swagger
- URL nội bộ: `http://localhost:5243/api/service`
- Method: `POST`
- Body: `multipart/form-data` gồm các trường như trên.
- Kết quả mong đợi: trả về entity với `imgUrl` là `optimizedUrl` (hoặc `secureUrl`).

## Bảo mật và thực tiễn tốt
- Xác thực/Phân quyền: Bảo vệ endpoint upload nếu ảnh thuộc tài nguyên riêng của người dùng.
- Kích thước file: Áp giới hạn kích thước tối đa ở server và reverse proxy.
- Loại file: Chỉ cho phép định dạng ảnh hợp lệ (png, jpg, webp, …) và kiểm tra MIME.
- Quét virus (nếu cần): Tích hợp bước scan trước khi chuyển tiếp đến Image Service.
- Ẩn nội bộ: Không phơi bày kho lưu trữ nội bộ, chỉ trả URL công khai tối ưu.

## Triển khai (Deployment)
- Thiết lập biến môi trường `APISERVICE__BASEURLIMAGE` trỏ tới Image Service ở môi trường production.
- Đảm bảo CORS phù hợp nếu client gọi trực tiếp Image Service (khuyến nghị gọi qua API chính).

## Xử lý sự cố thường gặp
1) Swagger báo lỗi khi dùng `IFormFile` với `[FromForm]`:
   - Thêm cấu hình map `IFormFile` như phần Swagger bên trên.
2) Lỗi build “file is being used by another process” trên Windows:
   - Dừng tiến trình đang chạy: `taskkill /f /im CareNest_Service.exe`
   - Build lại: `dotnet build CareNest_Service`
   - Chạy lại: `dotnet run --project CareNest_Service`
3) Upload thành công nhưng URL rỗng:
   - Kiểm tra `folder/publicId` truyền vào Image Service có khớp mong đợi không.
   - Đảm bảo đã gán `optimizedUrl`/`secureUrl` vào trường `ImgUrl` và lưu cập nhật.
4) 415 Unsupported Media Type:
   - Đảm bảo header `Content-Type: multipart/form-data` và gửi đúng key `file`.

## Tái sử dụng cho dự án khác – Checklist nhanh
- [ ] Sao chép contract `ImageRequest`, `ImageResponse` hoặc định nghĩa tương đương.
- [ ] Tạo `IImageService` và triển khai `ImageService` gọi Image Service ngoài.
- [ ] Tạo/tiêm `IAPIService` (hoặc `HttpClientFactory`) để gửi multipart/form-data.
- [ ] Thêm cấu hình `APIService:BaseUrlImage` vào `appsettings.*` và biến môi trường.
- [ ] Cập nhật controller/command để nhận `IFormFile` và luồng upload như mô tả.
- [ ] Map `IFormFile` trong Swagger để hiển thị input file đúng.

## Tham khảo trong dự án này
- `Shared/Contracts/ImageRequest.cs`
- `Shared/Contracts/ImageResponse.cs`
- `CareNest.Application/Interfaces/Services/IImageService.cs`
- `CareNest.Infrastructure/Services/ImageService.cs`
- `CareNest.Infrastructure/Services/APIService.cs`
- `CareNest_Service/Controllers/ServiceController.cs` (nhận form-data khi tạo mới)

## Chạy nhanh dự án mẫu
```bash
dotnet run --project CareNest_Service
```

Swagger UI: `http://localhost:5243/swagger`

