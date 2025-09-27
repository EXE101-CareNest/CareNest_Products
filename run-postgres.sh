#!/bin/bash

# Script để chạy PostgreSQL cho CareNest Products API

echo "🚀 Starting PostgreSQL for CareNest Products API..."

# Tạo network nếu chưa có
echo "📡 Creating network if not exists..."
docker network create --subnet=192.168.0.0/16 carenest-network 2>/dev/null || echo "Network already exists"

# Dừng container cũ nếu có
echo "🛑 Stopping existing PostgreSQL container..."
docker stop carenest-postgres 2>/dev/null || echo "No existing container to stop"
docker rm carenest-postgres 2>/dev/null || echo "No existing container to remove"

# Chạy PostgreSQL container
echo "🐘 Starting PostgreSQL container..."
docker run -d \
  --name carenest-postgres \
  --network carenest-network \
  --ip 192.168.0.12 \
  -e POSTGRES_DB=carenest-products-dev \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres123 \
  -p 5432:5432 \
  -v postgres_data:/var/lib/postgresql/data \
  postgres:15-alpine

# Đợi PostgreSQL khởi động
echo "⏳ Waiting for PostgreSQL to start..."
sleep 10

# Kiểm tra kết nối
echo "🔍 Testing PostgreSQL connection..."
docker exec carenest-postgres pg_isready -U postgres

if [ $? -eq 0 ]; then
    echo "✅ PostgreSQL is ready!"
    echo "📊 Database: carenest-products-dev"
    echo "👤 User: postgres"
    echo "🔑 Password: postgres123"
    echo "🌐 Host: localhost:5432"
    echo "🐳 Container IP: 192.168.0.12"
    echo ""
    echo "🚀 You can now run the API with:"
    echo "   docker-compose up --build"
else
    echo "❌ PostgreSQL failed to start!"
    echo "📋 Check logs with: docker logs carenest-postgres"
fi
