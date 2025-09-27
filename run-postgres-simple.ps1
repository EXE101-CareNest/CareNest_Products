# Script PowerShell để chạy PostgreSQL cho CareNest Products API

Write-Host "Starting PostgreSQL for CareNest Products API..." -ForegroundColor Green

# Tạo network nếu chưa có
Write-Host "Creating network if not exists..." -ForegroundColor Yellow
docker network create --subnet=192.168.0.0/16 carenest-network 2>$null

# Dừng container cũ nếu có
Write-Host "Stopping existing PostgreSQL container..." -ForegroundColor Yellow
docker stop carenest-postgres 2>$null
docker rm carenest-postgres 2>$null

# Chạy PostgreSQL container
Write-Host "Starting PostgreSQL container..." -ForegroundColor Yellow
docker run -d --name carenest-postgres --network carenest-network --ip 192.168.0.12 -e POSTGRES_DB=carenest-products-dev -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres123 -p 5432:5432 -v postgres_data:/var/lib/postgresql/data postgres:15-alpine

# Đợi PostgreSQL khởi động
Write-Host "Waiting for PostgreSQL to start..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# Kiểm tra kết nối
Write-Host "Testing PostgreSQL connection..." -ForegroundColor Yellow
docker exec carenest-postgres pg_isready -U postgres

if ($LASTEXITCODE -eq 0) {
    Write-Host "PostgreSQL is ready!" -ForegroundColor Green
    Write-Host "Database: carenest-products-dev" -ForegroundColor Cyan
    Write-Host "User: postgres" -ForegroundColor Cyan
    Write-Host "Password: postgres123" -ForegroundColor Cyan
    Write-Host "Host: localhost:5432" -ForegroundColor Cyan
    Write-Host "Container IP: 192.168.0.12" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "You can now run the API with:" -ForegroundColor Green
    Write-Host "   docker-compose up --build" -ForegroundColor White
} else {
    Write-Host "PostgreSQL failed to start!" -ForegroundColor Red
    Write-Host "Check logs with: docker logs carenest-postgres" -ForegroundColor Yellow
}
