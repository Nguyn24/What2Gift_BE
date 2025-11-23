# Hướng dẫn Import dữ liệu từ CSV vào Database

## Yêu cầu

1. Python 3.7 trở lên
2. Thư viện `psycopg2-binary`

## Cài đặt

```bash
pip install -r requirements.txt
```

## Chạy script

```bash
python import_products.py
```

## Cách hoạt động

Script sẽ:
1. Đọc file `database.csv` trong thư mục hiện tại
2. Kết nối đến database PostgreSQL (thông tin từ `appsettings.Production.json`)
3. Với mỗi dòng trong CSV:
   - Tạo hoặc lấy Brand từ tên brand
   - Tạo hoặc lấy Category từ tên category
   - Tạo Product mới với thông tin từ CSV
   - Tạo ProductSource với giá và link affiliate (nếu có)

## Mapping dữ liệu

- `product_name` → `Products.Name`
- `description` → `Products.Description`
- `image_url` → `Products.ImageUrl`
- `brand` → `Brands.Name` (tạo mới nếu chưa có)
- `category` → `Categories.Name` (tạo mới nếu chưa có)
- `price` → `ProductSources.Price`
- `product_link` → `ProductSources.AffiliateLink`
- `product_link` → `ProductSources.VendorName` (tự động extract từ domain)

## Lưu ý

- Script sẽ commit sau mỗi 50 sản phẩm để tránh mất dữ liệu nếu có lỗi
- Các trường không có trong schema (stock, rating, num_reviews, sex) sẽ bị bỏ qua
- Nếu product_link rỗng, ProductSource sẽ không được tạo
- Tên category sẽ được normalize (chữ hoa đầu tiên)

