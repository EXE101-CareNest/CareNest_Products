# Hướng dẫn Deploy CareNest Products API

## 1. Tổng quan

CareNest Products API là một microservice quản lý sản phẩm sử dụng:
- **Clean Architecture** với 4 layers
- **CQRS Pattern** cho Commands và Queries  
- **PostgreSQL** database
- **Docker** containerization
- **APIService** để kết nối với các service khác

## 2. Cấu trúc dự án

```
CareNest_Products/
├── CareNest_Products.API/           # API Layer
├── CareNest_Products.Application/   # Application Layer  
├── CareNest_Products.Domain/        # Domain Layer
├── CareNest_Products.Infrastructure/ # Infrastructure Layer
├── Shared/                          # Shared Components
├── docker-compose.yml               # Docker Configuration
├── Dockerfile                       # Docker Build
└── CareNest_Products.API.sln       # Solution File
```

## 3. Cấu hình Database

### PostgreSQL Connection
Dự án sử dụng PostgreSQL với cấu hình trong `appsettings.Development.json`:

```json
{
  "DatabaseSettings": {
    "Ip": "100.93.191.32",
    "Port": 5432,
    "User": "postgres", 
    "Password": "HoiLamChi123@",
    "Database": "carenest-products-dev"
  }
}
```

### Chạy PostgreSQL với Docker

```bash
# Tạo network nếu chưa có
docker network create --subnet=192.168.0.0/16 carenest-network

# Chạy PostgreSQL
docker run -d \
  --name carenest-postgres \
  --network carenest-network \
  --ip 192.168.0.12 \
  -e POSTGRES_DB=carenest-products-dev \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres123 \
  -p 5432:5432 \
  postgres:15-alpine
```

## 4. Deploy với Docker

### 4.1. Build và chạy

```bash
# Build và chạy tất cả services
docker-compose up --build

# Chạy trong background
docker-compose up -d --build

# Xem logs
docker-compose logs -f carenest-products-api
```

### 4.2. Cấu hình Docker Compose

File `docker-compose.yml` đã được cấu hình với:
- **Port**: 8016:8080
- **Network**: carenest-network với IP 192.168.0.13
- **Environment**: Development

## 5. Sử dụng API

### 5.1. Health Check

```bash
# Kiểm tra health
curl http://localhost:8016/api/health

# Kiểm tra readiness
curl http://localhost:8016/api/health/ready
```

### 5.2. Swagger UI

Truy cập: `http://localhost:8016` để xem Swagger documentation

### 5.3. API Endpoints

#### Products
- `GET /api/products` - Lấy danh sách sản phẩm
- `GET /api/products/{id}` - Lấy sản phẩm theo ID
- `POST /api/products` - Tạo sản phẩm mới
- `PUT /api/products/{id}` - Cập nhật sản phẩm
- `DELETE /api/products/{id}` - Xóa sản phẩm

#### Product Categories
- `GET /api/productcategories` - Lấy danh sách danh mục
- `GET /api/productcategories/{id}` - Lấy danh mục theo ID
- `POST /api/productcategories` - Tạo danh mục mới

#### Product Details
- `GET /api/productdetails` - Lấy danh sách chi tiết sản phẩm
- `GET /api/productdetails/{id}` - Lấy chi tiết sản phẩm theo ID
- `POST /api/productdetails` - Tạo chi tiết sản phẩm mới

## 6. Kết nối với các Service khác

### 6.1. Cấu hình APIService

Trong `appsettings.Development.json`:

```json
{
  "APIService": {
    "BaseUrlAppointment": "http://192.168.0.10:8080",
    "BaseUrlAppointmentDetail": "http://192.168.0.11:8080", 
    "BaseUrlProducts": "http://192.168.0.13:8080"
  }
}
```

### 6.2. Sử dụng APIService trong code

```csharp
public class YourService
{
    private readonly IAPIService _apiService;

    public YourService(IAPIService apiService)
    {
        _apiService = apiService;
    }

    public async Task<AppointmentResponse> GetAppointment(string id)
    {
        var result = await _apiService.GetAsync<AppointmentResponse>(
            "appointment", 
            $"/api/appointment/{id}"
        );

        if (!result.IsSuccess)
        {
            throw new Exception(result.Message);
        }

        return result.Data!;
    }
}
```

## 7. Database Migration

### 7.1. Tạo Migration

```bash
# Di chuyển vào thư mục API
cd CareNest_Products.API

# Tạo migration mới
dotnet ef migrations add InitialCreate

# Cập nhật database
dotnet ef database update
```

### 7.2. Chạy Migration trong Docker

```bash
# Chạy migration trong container
docker-compose exec carenest-products-api dotnet ef database update
```

## 8. Troubleshooting

### 8.1. Lỗi kết nối Database

```bash
# Kiểm tra PostgreSQL có chạy không
docker ps | grep postgres

# Kiểm tra logs
docker logs carenest-postgres

# Kiểm tra network
docker network inspect carenest-network
```

### 8.2. Lỗi kết nối Service

```bash
# Kiểm tra các service khác có chạy không
curl http://192.168.0.10:8080/api/health
curl http://192.168.0.11:8080/api/health

# Kiểm tra network connectivity
docker exec carenest-products-api-dev ping 192.168.0.10
```

### 8.3. Lỗi Build

```bash
# Clean và rebuild
docker-compose down
docker system prune -f
docker-compose up --build
```

## 9. Development Commands

```bash
# Chạy local development
dotnet run --project CareNest_Products.API

# Chạy tests
dotnet test

# Build solution
dotnet build

# Restore packages
dotnet restore
```

## 10. Production Deployment

### 10.1. Environment Variables

```bash
# Set production environment
export ASPNETCORE_ENVIRONMENT=Production

# Set production database
export DatabaseSettings__Ip=your-production-db-ip
export DatabaseSettings__Database=carenest-products-prod
```

### 10.2. Docker Production

```bash
# Build production image
docker build -t carenest-products-api:prod .

# Run production container
docker run -d \
  --name carenest-products-api-prod \
  --network carenest-network \
  --ip 192.168.0.13 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -p 8016:8080 \
  carenest-products-api:prod
```

## 11. Monitoring và Logging

### 11.1. Health Checks

API cung cấp health check endpoints:
- `/api/health` - Basic health check
- `/api/health/ready` - Readiness check

### 11.2. Logging

Logs được ghi ra console và có thể được collect bởi Docker logging driver.

### 11.3. Metrics

Có thể thêm Application Insights hoặc Prometheus metrics nếu cần.

## 12. Security

### 12.1. Authentication

Hiện tại API chưa có authentication. Có thể thêm JWT authentication nếu cần.

### 12.2. HTTPS

Trong production, nên sử dụng HTTPS với reverse proxy (nginx/traefik).

## 13. Backup và Recovery

### 13.1. Database Backup

```bash
# Backup PostgreSQL
docker exec carenest-postgres pg_dump -U postgres carenest-products-dev > backup.sql

# Restore PostgreSQL  
docker exec -i carenest-postgres psql -U postgres carenest-products-dev < backup.sql
```

### 13.2. Volume Backup

```bash
# Backup volumes
docker run --rm -v carenest_postgres_data:/data -v $(pwd):/backup alpine tar czf /backup/postgres-backup.tar.gz /data
```

Đây là hướng dẫn đầy đủ để deploy và sử dụng CareNest Products API. API đã được cấu hình để hoạt động trong môi trường Docker với PostgreSQL và có thể kết nối với các service khác trong hệ thống CareNest.
