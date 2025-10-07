# AI RAG Setup Guide for What2Gift (Free Version)

## Tổng quan
Hệ thống AI RAG (Retrieval-Augmented Generation) đã được tích hợp vào What2Gift để cung cấp gợi ý quà tặng thông minh dựa trên thông tin người dùng cung cấp. **Phiên bản này hoàn toàn miễn phí** và không cần API key từ bất kỳ dịch vụ trả phí nào.

## Kiến trúc hệ thống

### 1. **Vector Database (Qdrant)**
- Lưu trữ embeddings của products
- Hỗ trợ tìm kiếm similarity search
- Port: 6333 (HTTP), 6334 (gRPC)

### 2. **AI Services (Miễn phí)**
- **Ollama Embedding Service**: Tạo embeddings sử dụng model local (nomic-embed-text)
- **Ollama Chat Service**: Generate suggestions sử dụng Llama 3.2
- **Vector Database Service**: Quản lý vector storage và search
- **Gift Suggestion AI Service**: Kết hợp RAG với local LLM

### 3. **Web Scraping Services**
- **Shopee Scraping**: Lấy sản phẩm từ Shopee.vn
- **eBay Scraping**: Lấy sản phẩm từ eBay.com
- **Affiliate Links**: Tự động tạo affiliate links cho sản phẩm

### 4. **API Endpoints**
- `POST /api/aisuggestion/suggest-products`: Lấy gợi ý sản phẩm từ AI
- `POST /api/aisuggestion/index-products`: Index tất cả products vào vector database

## Setup Instructions

### 1. **Cài đặt Dependencies**

```bash
# Restore packages
dotnet restore

# Build project
dotnet build
```

### 2. **Cấu hình Ollama (Miễn phí)**

Cập nhật `appsettings.Development.json`:

```json
{
  "Ollama": {
    "Url": "http://localhost:11434",
    "EmbeddingModel": "nomic-embed-text",
    "ChatModel": "llama3.2"
  }
}
```

### 3. **Chạy Infrastructure Services**

```bash
# Start Ollama, Qdrant và PostgreSQL
docker-compose up -d ollama qdrant postgres

# Download AI models (chạy sau khi Ollama container đã start)
docker exec ollama ollama pull nomic-embed-text
docker exec ollama ollama pull llama3.2
```

### 4. **Index Products vào Vector Database**

```bash
# Chạy API và index products
curl -X POST "http://localhost:5152/api/aisuggestion/index-products" \
  -H "Authorization: Bearer your-jwt-token"
```

### 5. **Test AI Suggestion**

```bash
curl -X GET "http://localhost:5152/api/aisuggestion/suggest-products?userId=user-guid&recipientGender=1&recipientAge=25&recipientHobby=photography&budgetMin=50&budgetMax=200" \
  -H "Authorization: Bearer your-jwt-token"
```

## Workflow AI Suggestion

1. **User Input**: Người dùng cung cấp thông tin về người nhận quà
2. **Local Database Search**: Tìm kiếm products trong database local
3. **Web Scraping**: Nếu không đủ sản phẩm, scrape từ Shopee/eBay
4. **Query Embedding**: Tạo embedding từ thông tin người dùng (Ollama)
5. **Vector Search**: Tìm kiếm products tương tự trong Qdrant
6. **AI Ranking**: Sử dụng Llama 3.2 để rank và filter products
7. **Personalized Suggestion**: Generate gợi ý cá nhân hóa bằng tiếng Việt

## Cấu trúc dữ liệu

### GiftSuggestion Entity
```csharp
public class GiftSuggestion : Entity
{
    public Guid UserId { get; set; }
    public Guid? OccasionId { get; set; }
    public RecipientGender RecipientGender { get; set; }
    public int RecipientAge { get; set; }
    public string RecipientHobby { get; set; }
    public decimal BudgetMin { get; set; }
    public decimal BudgetMax { get; set; }
    public Guid? SuggestedProductId { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### Product Embedding
Mỗi product được convert thành text và tạo embedding:
```
Product: [Name]
Description: [Description]
Brand: [Brand Name]
Category: [Category Name]
Occasion: [Occasion Name]
```

## Monitoring và Logging

- **Qdrant Dashboard**: http://localhost:6333/dashboard
- **Application Logs**: Sử dụng Serilog với Seq
- **Health Checks**: `/health` endpoint

## Troubleshooting

### 1. **Qdrant Connection Issues**
```bash
# Check Qdrant status
curl http://localhost:6333/collections

# Restart Qdrant
docker-compose restart qdrant
```

### 2. **Ollama Issues**
- Kiểm tra Ollama container đã chạy chưa: `docker ps | grep ollama`
- Verify models đã được download: `docker exec ollama ollama list`
- Check Ollama API: `curl http://localhost:11434/api/tags`

### 3. **Embedding Generation Fails**
- Kiểm tra product data có đầy đủ không
- Verify Ollama API response: `curl http://localhost:11434/api/embeddings`
- Check error logs trong application

### 4. **Web Scraping Issues**
- Kiểm tra network connectivity
- Verify website selectors (có thể thay đổi theo thời gian)
- Check rate limiting và user agent

## Performance Optimization

1. **Batch Processing**: Index products theo batch để tránh rate limiting
2. **Caching**: Cache embeddings để tránh regenerate
3. **Vector Indexing**: Sử dụng HNSW index cho Qdrant
4. **Query Optimization**: Tối ưu similarity threshold

## Security Considerations

1. **No API Keys Required**: Hệ thống hoàn toàn miễn phí, không cần API keys
2. **Rate Limiting**: Implement rate limiting cho AI endpoints
3. **Input Validation**: Validate user input trước khi gửi đến AI services
4. **Data Privacy**: Đảm bảo user data không được log hoặc store không cần thiết
5. **Web Scraping Ethics**: Respect robots.txt và implement delays giữa các requests

## Lợi ích của phiên bản miễn phí

1. **Không có chi phí**: Hoàn toàn miễn phí, không cần trả tiền cho API calls
2. **Privacy**: Dữ liệu được xử lý local, không gửi đến bên thứ 3
3. **Offline capable**: Có thể hoạt động offline sau khi download models
4. **Customizable**: Có thể fine-tune models theo nhu cầu
5. **Real-time data**: Web scraping cung cấp dữ liệu sản phẩm real-time từ e-commerce
