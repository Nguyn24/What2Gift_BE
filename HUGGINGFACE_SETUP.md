# Hugging Face API Setup Guide

## Tổng quan
Thay vì sử dụng Ollama local, ứng dụng hiện tại sử dụng Hugging Face Inference API để:
- **Embeddings**: Tạo vector embeddings cho products
- **Chat**: Tạo personalized gift suggestions

## Lợi ích của Hugging Face API
✅ **Miễn phí**: Có gói miễn phí với rate limits hợp lý  
✅ **Không cần host local**: Gọi trực tiếp từ web  
✅ **Đơn giản**: REST API dễ sử dụng  
✅ **Ổn định**: Không gặp lỗi protocol như gRPC  
✅ **Đa dạng models**: Nhiều models để lựa chọn  

## Cách setup

### 1. Tạo tài khoản Hugging Face
1. Truy cập [huggingface.co](https://huggingface.co)
2. Đăng ký tài khoản miễn phí
3. Xác thực email

### 2. Tạo API Token
1. Vào [Settings > Access Tokens](https://huggingface.co/settings/tokens)
2. Click "New token"
3. Chọn "Read" permission
4. Copy token (dạng: `hf_xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx`)

### 3. Cập nhật Configuration
Thay thế `your-huggingface-api-key-here` trong các file config:

**appsettings.json:**
```json
{
  "HuggingFace": {
    "ApiKey": "hf_your_actual_token_here",
    "EmbeddingModel": "sentence-transformers/all-MiniLM-L6-v2",
    "ChatModel": "microsoft/DialoGPT-medium"
  }
}
```

**appsettings.Development.json:**
```json
{
  "HuggingFace": {
    "ApiKey": "hf_your_actual_token_here",
    "EmbeddingModel": "sentence-transformers/all-MiniLM-L6-v2", 
    "ChatModel": "microsoft/DialoGPT-medium"
  }
}
```

### 4. Models được sử dụng

#### Embedding Model: `sentence-transformers/all-MiniLM-L6-v2`
- **Mục đích**: Tạo vector embeddings cho products
- **Dimension**: 384 (thay vì 1536 như trước)
- **Chất lượng**: Tốt cho semantic search
- **Miễn phí**: Có sẵn trên Inference API

#### Chat Model: `microsoft/DialoGPT-medium`
- **Mục đích**: Tạo personalized gift suggestions
- **Chất lượng**: Tốt cho conversation
- **Miễn phí**: Có sẵn trên Inference API

### 5. Rate Limits (Miễn phí)
- **Requests**: 1000 requests/month
- **Compute time**: 30,000 seconds/month
- **Concurrent requests**: 2

### 6. Chạy ứng dụng
```bash
# Không cần chạy Ollama nữa
docker-compose up -d

# Chỉ cần:
# - PostgreSQL
# - Qdrant  
# - Seq (logging)
```

## Troubleshooting

### Lỗi "API key not configured"
- Kiểm tra API key trong appsettings.json
- Đảm bảo format: `hf_xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx`

### Lỗi "Rate limit exceeded"
- Đã vượt quá 1000 requests/tháng
- Có thể upgrade lên Pro plan hoặc chờ reset tháng sau

### Lỗi "Model not found"
- Model có thể đang loading lần đầu
- Hugging Face sẽ tự động load model khi có request đầu tiên
- Có thể mất 1-2 phút

## So sánh với Ollama

| Aspect | Ollama (Local) | Hugging Face API |
|--------|----------------|------------------|
| **Setup** | Phức tạp, cần download models | Đơn giản, chỉ cần API key |
| **Resources** | Tốn RAM/CPU local | Không tốn resources local |
| **Stability** | Có thể gặp lỗi protocol | Ổn định hơn |
| **Cost** | Miễn phí nhưng tốn điện | Miễn phí với limits |
| **Maintenance** | Cần update models | Tự động update |

## Kết luận
Việc chuyển sang Hugging Face API giúp:
- Giảm complexity trong setup
- Tránh lỗi gRPC protocol
- Tiết kiệm resources local
- Dễ dàng scale và maintain
