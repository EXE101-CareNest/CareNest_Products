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

# Clean Architecture với CQRS Pattern - Template Dự Án

## Tổng Quan

Dự án này sử dụng **Clean Architecture** kết hợp với **CQRS (Command Query Responsibility Segregation)** pattern để xây dựng một API .NET 8 có cấu trúc rõ ràng, dễ bảo trì và mở rộng.

## Kiến Trúc Dự Án

### 1. Cấu Trúc Thư Mục

```
ProjectName/
├── ProjectName_Service/              # Presentation Layer (API)
│   ├── Controllers/                  # API Controllers
│   ├── Middleware/                   # Custom Middleware
│   ├── Extensions/                   # Extension Methods
│   ├── Program.cs                    # Application Entry Point
│   └── appsettings.json             # Configuration
├── ProjectName.Application/          # Application Layer
│   ├── Common/                       # Shared Application Objects
│   ├── Features/                     # CQRS Features
│   │   ├── Commands/                 # Command Handlers
│   │   │   ├── Create/
│   │   │   ├── Update/
│   │   │   └── Delete/
│   │   └── Queries/                  # Query Handlers
│   │       ├── GetAllPaging/
│   │       └── GetById/
│   ├── Interfaces/                   # Application Interfaces
│   │   ├── CQRS/                     # CQRS Interfaces
│   │   ├── Services/                 # Service Interfaces
│   │   └── UOW/                      # Unit of Work Interface
│   ├── UseCases/                     # Use Case Dispatcher
│   └── Exceptions/                   # Custom Exceptions
├── ProjectName.Domain/               # Domain Layer
│   ├── Entitites/                    # Domain Entities
│   ├── Repositories/                 # Repository Interfaces
│   └── Commons/                      # Domain Constants & Base Classes
├── ProjectName.Infrastructure/       # Infrastructure Layer
│   ├── Persistences/                 # Data Access
│   │   ├── Database/                 # DbContext & Configuration
│   │   └── Repository/               # Repository Implementations
│   ├── Services/                     # Infrastructure Services
│   └── UOW/                          # Unit of Work Implementation
└── Shared/                           # Shared Utilities
    └── Helper/                       # Helper Classes
```

### 2. Các Layer và Trách Nhiệm

#### **Presentation Layer (ProjectName_Service)**
- **Mục đích**: Xử lý HTTP requests/responses
- **Thành phần chính**:
  - Controllers: Xử lý API endpoints
  - Middleware: Xử lý cross-cutting concerns (exception handling, logging)
  - Extensions: Extension methods cho controllers

#### **Application Layer (ProjectName.Application)**
- **Mục đích**: Chứa business logic và use cases
- **Thành phần chính**:
  - Features: CQRS commands và queries
  - Interfaces: Định nghĩa contracts
  - UseCases: Dispatcher cho CQRS
  - Common: Shared objects (ApiResponse, PageResult)

#### **Domain Layer (ProjectName.Domain)**
- **Mục đích**: Chứa business entities và domain logic
- **Thành phần chính**:
  - Entities: Domain models
  - Repositories: Repository interfaces
  - Commons: Base classes và constants

#### **Infrastructure Layer (ProjectName.Infrastructure)**
- **Mục đích**: Triển khai các interface từ Application layer
- **Thành phần chính**:
  - Persistences: Database context và repositories
  - Services: Infrastructure services
  - UOW: Unit of Work implementation

## CQRS Pattern

### Commands (Ghi dữ liệu)
- **CreateCommand**: Tạo mới entity
- **UpdateCommand**: Cập nhật entity
- **DeleteCommand**: Xóa entity

### Queries (Đọc dữ liệu)
- **GetAllPagingQuery**: Lấy danh sách có phân trang
- **GetByIdQuery**: Lấy entity theo ID

## Quy Tắc Đặt Tên File

### 1. Entity Files
```
EntityName.cs                    # Domain entity
```

### 2. Command Files
```
CreateCommand.cs                 # Command definition
CreateCommandHandler.cs          # Command handler
UpdateCommand.cs                 # Update command
UpdateCommandHandler.cs          # Update handler
UpdateRequest.cs                 # Update request DTO
DeleteCommand.cs                 # Delete command
DeleteCommandHandler.cs          # Delete handler
```

### 3. Query Files
```
GetAllPagingQuery.cs             # Query definition
GetAllPagingQueryHandler.cs      # Query handler
GetByIdQuery.cs                  # Get by ID query
GetByIdQueryHandler.cs           # Get by ID handler
EntityResponse.cs                # Response DTO
```

### 4. Controller Files
```
EntityController.cs              # API controller
```

### 5. Repository Files
```
IGenericRepository.cs            # Generic repository interface
GenericRepository.cs             # Generic repository implementation
```

## Template Code cho Entity Mới

### 1. Tạo Domain Entity

```csharp
// ProjectName.Domain/Entitites/EntityName.cs
using ProjectName.Domain.Commons;

namespace ProjectName.Domain.Entitites
{
    public class EntityName : BaseEntity
    {
        /// <summary>
        /// Mô tả thuộc tính
        /// </summary>
        public string? PropertyName { get; set; }
        
        // Thêm các thuộc tính khác...
    }
}
```

### 2. Tạo Commands

#### Create Command
```csharp
// ProjectName.Application/Features/Commands/Create/CreateEntityNameCommand.cs
using ProjectName.Application.Interfaces.CQRS.Commands;
using ProjectName.Domain.Entitites;

namespace ProjectName.Application.Features.Commands.Create
{
    public class CreateEntityNameCommand : ICommand<EntityName>
    {
        public string? PropertyName { get; set; }
        // Thêm các thuộc tính khác...
    }
}
```

#### Create Command Handler
```csharp
// ProjectName.Application/Features/Commands/Create/CreateEntityNameCommandHandler.cs
using ProjectName.Application.Interfaces.CQRS.Commands;
using ProjectName.Application.Interfaces.UOW;
using ProjectName.Domain.Entitites;
using Shared.Helper;

namespace ProjectName.Application.Features.Commands.Create
{
    public class CreateEntityNameCommandHandler : ICommandHandler<CreateEntityNameCommand, EntityName>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateEntityNameCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EntityName> HandleAsync(CreateEntityNameCommand command)
        {
            // Validation logic
            Validate.ValidateCreate(command);

            EntityName entity = new()
            {
                PropertyName = command.PropertyName,
                CreatedAt = TimeHelper.GetUtcNow()
                // Map các thuộc tính khác...
            };
            
            await _unitOfWork.GetRepository<EntityName>().AddAsync(entity);
            await _unitOfWork.SaveAsync();

            return entity;
        }
    }
}
```

### 3. Tạo Queries

#### Get All Paging Query
```csharp
// ProjectName.Application/Features/Queries/GetAllPaging/GetAllEntityNamePagingQuery.cs
using ProjectName.Application.Common;
using ProjectName.Application.Interfaces.CQRS.Queries;

namespace ProjectName.Application.Features.Queries.GetAllPaging
{
    public class GetAllEntityNamePagingQuery : IQuery<PageResult<EntityNameResponse>>
    {
        public int Index { get; set; }
        public int PageSize { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; }
    }
}
```

#### Get All Paging Query Handler
```csharp
// ProjectName.Application/Features/Queries/GetAllPaging/GetAllEntityNamePagingQueryHandler.cs
using ProjectName.Application.Common;
using ProjectName.Application.Interfaces.CQRS.Queries;
using ProjectName.Application.Interfaces.UOW;
using ProjectName.Domain.Entitites;

namespace ProjectName.Application.Features.Queries.GetAllPaging
{
    public class GetAllEntityNamePagingQueryHandler : IQueryHandler<GetAllEntityNamePagingQuery, PageResult<EntityNameResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllEntityNamePagingQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PageResult<EntityNameResponse>> HandleAsync(GetAllEntityNamePagingQuery query)
        {
            var selector = ObjectMapperExtensions.CreateMapExpression<EntityName, EntityNameResponse>();

            var orderByFunc = GetOrderByFunc(query.SortColumn, query.SortDirection);

            IEnumerable<EntityNameResponse> result = await _unitOfWork.GetRepository<EntityName>().FindAsync(
                predicate: null,
                orderBy: orderByFunc,
                selector: selector,
                pageSize: query.PageSize,
                pageIndex: query.Index);

            return new PageResult<EntityNameResponse>(result, 1, query.PageSize, query.Index);
        }

        private Func<IQueryable<EntityName>, IOrderedQueryable<EntityName>> GetOrderByFunc(string? sortColumn, string? sortDirection)
        {
            var ascending = string.IsNullOrWhiteSpace(sortDirection) || sortDirection.ToLower() != "desc";

            return sortColumn?.ToLower() switch
            {
                "propertyname" => q => ascending ? q.OrderBy(a => a.PropertyName) : q.OrderByDescending(a => a.PropertyName),
                "updateat" => q => ascending ? q.OrderBy(a => a.UpdatedAt) : q.OrderByDescending(a => a.UpdatedAt),
                _ => q => q.OrderBy(a => a.CreatedAt)
            };
        }
    }
}
```

### 4. Tạo Response DTO

```csharp
// ProjectName.Application/Features/Queries/GetAllPaging/EntityNameResponse.cs
namespace ProjectName.Application.Features.Queries.GetAllPaging
{
    public class EntityNameResponse
    {
        public string Id { get; set; } = string.Empty;
        public string? PropertyName { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        // Thêm các thuộc tính khác cần thiết cho response...
    }
}
```

### 5. Tạo Controller

```csharp
// ProjectName_Service/Controllers/EntityNameController.cs
using ProjectName.Application.Common;
using ProjectName.Application.Features.Commands.Create;
using ProjectName.Application.Features.Commands.Delete;
using ProjectName.Application.Features.Commands.Update;
using ProjectName.Application.Features.Queries.GetAllPaging;
using ProjectName.Application.Features.Queries.GetById;
using ProjectName.Application.Interfaces.CQRS;
using ProjectName.Domain.Commons.Constant;
using ProjectName.Domain.Entitites;
using ProjectName_Service.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ProjectName_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntityNameController : ControllerBase
    {
        private readonly IUseCaseDispatcher _dispatcher;

        public EntityNameController(IUseCaseDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Lấy danh sách entity với phân trang
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPaging(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortColumn = null,
            [FromQuery] string? sortDirection = "asc")
        {
            var query = new GetAllEntityNamePagingQuery()
            {
                Index = pageIndex,
                PageSize = pageSize,
                SortColumn = sortColumn,
                SortDirection = sortDirection
            };
            var result = await _dispatcher.DispatchQueryAsync<GetAllEntityNamePagingQuery, PageResult<EntityNameResponse>>(query);
            return this.OkResponse(result, MessageConstant.SuccessGet);
        }

        /// <summary>
        /// Lấy entity theo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetByIdEntityNameQuery() { Id = id };
            EntityName result = await _dispatcher.DispatchQueryAsync<GetByIdEntityNameQuery, EntityName>(query);
            return this.OkResponse(result, MessageConstant.SuccessGet);
        }

        /// <summary>
        /// Tạo mới entity
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEntityNameCommand command)
        {
            EntityName result = await _dispatcher.DispatchAsync<CreateEntityNameCommand, EntityName>(command);
            return this.OkResponse(result, MessageConstant.SuccessCreate);
        }

        /// <summary>
        /// Cập nhật entity
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateEntityNameRequest request)
        {
            var command = new UpdateEntityNameCommand()
            {
                Id = id,
                PropertyName = request.PropertyName
                // Map các thuộc tính khác...
            };
            EntityName result = await _dispatcher.DispatchAsync<UpdateEntityNameCommand, EntityName>(command);
            return this.OkResponse(result, MessageConstant.SuccessUpdate);
        }

        /// <summary>
        /// Xóa entity
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _dispatcher.DispatchAsync(new DeleteEntityNameCommand { Id = id });
            return this.OkResponse(MessageConstant.SuccessDelete);
        }
    }
}
```

### 6. Cập nhật Database Context

```csharp
// ProjectName.Infrastructure/Persistences/Database/DatabaseContext.cs
// Thêm DbSet cho entity mới
public DbSet<EntityName> EntityNames { get; set; }
```

### 7. Đăng ký Services trong Program.cs

```csharp
// ProjectName_Service/Program.cs
// Thêm vào phần đăng ký services

// Command handlers
builder.Services.AddScoped<ICommandHandler<CreateEntityNameCommand, EntityName>, CreateEntityNameCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateEntityNameCommand, EntityName>, UpdateEntityNameCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteEntityNameCommand>, DeleteEntityNameCommandHandler>();

// Query handlers
builder.Services.AddScoped<IQueryHandler<GetAllEntityNamePagingQuery, PageResult<EntityNameResponse>>, GetAllEntityNamePagingQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetByIdEntityNameQuery, EntityName>, GetByIdEntityNameQueryHandler>();
```

## Các Pattern Được Sử Dụng

### 1. Repository Pattern
- **IGenericRepository<T>**: Interface cho generic repository
- **GenericRepository<T>**: Implementation với Entity Framework

### 2. Unit of Work Pattern
- **IUnitOfWork**: Interface quản lý repositories và transactions
- **UnitOfWork**: Implementation với transaction support

### 3. CQRS Pattern
- **Commands**: Xử lý các thao tác ghi dữ liệu
- **Queries**: Xử lý các thao tác đọc dữ liệu
- **Handlers**: Triển khai logic cho commands/queries

### 4. Dependency Injection
- Sử dụng built-in DI container của .NET
- Đăng ký services trong Program.cs

## Cấu Hình Database

### PostgreSQL Configuration
```json
{
  "DatabaseSettings": {
    "Host": "localhost",
    "Port": 5432,
    "Database": "projectname-dev",
    "Username": "username",
    "Password": "password"
  }
}
```

### Entity Framework Migrations
```bash
# Tạo migration
dotnet ef migrations add MigrationName --project ProjectName.Infrastructure --startup-project ProjectName_Service

# Cập nhật database
dotnet ef database update --project ProjectName.Infrastructure --startup-project ProjectName_Service
```

## Middleware và Exception Handling

### Global Exception Handling
- **GlobalExceptionHandlingMiddleware**: Xử lý exceptions toàn cục
- **BadRequestException**: Validation errors
- **InternalException**: Internal server errors

### Response Extensions
- **ControllerResponseExtensions**: Extension methods cho controllers
- Chuẩn hóa response format với ApiResponse<T>

## Best Practices

### 1. Naming Conventions
- **Entities**: PascalCase (EntityName)
- **Commands**: PascalCase + Command suffix
- **Queries**: PascalCase + Query suffix
- **Handlers**: PascalCase + Handler suffix
- **Controllers**: PascalCase + Controller suffix

### 2. File Organization
- Mỗi feature có thư mục riêng
- Commands và Queries tách biệt
- Response DTOs trong thư mục Queries

### 3. Error Handling
- Sử dụng custom exceptions
- Global exception middleware
- Validation trong command handlers

### 4. Async/Await
- Tất cả database operations đều async
- Controllers sử dụng async methods

## Mở Rộng Dự Án

### Thêm Entity Mới
1. Tạo entity trong Domain layer
2. Tạo commands và queries trong Application layer
3. Tạo controller trong Presentation layer
4. Cập nhật DatabaseContext
5. Đăng ký services trong Program.cs

### Thêm Business Logic
1. Tạo service interfaces trong Application layer
2. Implement services trong Infrastructure layer
3. Inject services vào command/query handlers

### Thêm Validation
1. Tạo validation rules trong Validators folder
2. Sử dụng trong command handlers
3. Throw BadRequestException khi validation fail

## Kết Luận

Kiến trúc này cung cấp:
- **Separation of Concerns**: Mỗi layer có trách nhiệm riêng biệt
- **Testability**: Dễ dàng unit test và integration test
- **Maintainability**: Code dễ đọc, dễ bảo trì
- **Scalability**: Dễ dàng mở rộng và thêm features mới
- **Clean Code**: Tuân thủ SOLID principles

Template này có thể được sử dụng cho bất kỳ dự án .NET nào cần kiến trúc Clean Architecture với CQRS pattern.
