-- =============================================
-- Import Products Data from CSV to PostgreSQL
-- =============================================

-- 1. INSERT BRANDS
-- =============================================
INSERT INTO "Brands" ("Id", "Name", "Description", "CreatedAt", "UpdatedAt") VALUES
('550e8400-e29b-41d4-a716-446655440001', 'Canifa', 'Thương hiệu thời trang Việt Nam', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440002', 'IVY Moda', 'Thương hiệu thời trang nữ cao cấp', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440003', 'Levi''s', 'Thương hiệu quần jeans nổi tiếng thế giới', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440004', 'H&M', 'Thương hiệu thời trang nhanh toàn cầu', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440005', 'Routine', 'Thương hiệu thời trang nam cao cấp', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440006', 'NEM', 'Thương hiệu thời trang Việt Nam', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440007', 'Lacoste', 'Thương hiệu thời trang Pháp cao cấp', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440008', 'Uniqlo', 'Thương hiệu thời trang Nhật Bản', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440009', 'Nike', 'Thương hiệu thể thao hàng đầu thế giới', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440010', 'Zara', 'Thương hiệu thời trang nhanh Tây Ban Nha', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440011', 'Local', 'Thương hiệu thời trang địa phương', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440012', 'Casio', 'Thương hiệu đồng hồ Nhật Bản', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440013', 'Daniel Wellington', 'Thương hiệu đồng hồ Thụy Điển', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440014', 'Charles & Keith', 'Thương hiệu túi xách Singapore', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440015', 'Adidas', 'Thương hiệu thể thao Đức', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440016', 'PNJ', 'Thương hiệu trang sức Việt Nam', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440017', 'Ray-Ban', 'Thương hiệu kính mát Mỹ', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440018', 'Gucci', 'Thương hiệu thời trang cao cấp Ý', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440019', 'Apple', 'Thương hiệu công nghệ Mỹ', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440020', 'Samsonite', 'Thương hiệu vali hành lý', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440021', 'Gentle Monster', 'Thương hiệu kính mát Hàn Quốc', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440022', 'Citizen', 'Thương hiệu đồng hồ Nhật Bản', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440023', 'The North Face', 'Thương hiệu outdoor Mỹ', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440024', 'Orient', 'Thương hiệu đồng hồ Nhật Bản', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440025', 'Prada', 'Thương hiệu thời trang cao cấp Ý', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440026', 'Xiaomi', 'Thương hiệu công nghệ Trung Quốc', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440027', 'Seiko', 'Thương hiệu đồng hồ Nhật Bản', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440028', 'Dior', 'Thương hiệu thời trang cao cấp Pháp', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440029', 'CK', 'Calvin Klein - Thương hiệu thời trang Mỹ', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440030', 'G-Shock', 'Thương hiệu đồng hồ thể thao Casio', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440031', 'DW', 'Daniel Wellington', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440032', 'Chanel', 'Thương hiệu thời trang cao cấp Pháp', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440033', 'YSL', 'Yves Saint Laurent - Thương hiệu thời trang Pháp', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440034', 'Samsung', 'Thương hiệu công nghệ Hàn Quốc', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440035', 'Vans', 'Thương hiệu giày thể thao Mỹ', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440036', 'Burberry', 'Thương hiệu thời trang cao cấp Anh', NOW(), NOW()),
('550e8400-e29b-41d4-a716-446655440037', 'Oakley', 'Thương hiệu kính mát thể thao Mỹ', NOW(), NOW());

-- 2. INSERT CATEGORIES
-- =============================================
INSERT INTO "Categories" ("Id", "Name", "Description", "CreatedAt", "UpdatedAt") VALUES
('660e8400-e29b-41d4-a716-446655440001', 'shirt', 'Áo sơ mi, áo thun, áo polo', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440002', 'pants', 'Quần dài, quần jeans, quần tây', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440003', 'dress', 'Váy, đầm các loại', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440004', 'jacket', 'Áo khoác, blazer, vest', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440005', 'hoodie', 'Áo hoodie, áo nỉ có mũ', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440006', 'shorts', 'Quần short, quần đùi', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440007', 'sweater', 'Áo len, áo nỉ', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440008', 'leggings', 'Quần legging, quần bó', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440009', 'vest', 'Áo vest, áo gile', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440010', 'watch', 'Đồng hồ đeo tay', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440011', 'wallet', 'Ví, ví cầm tay', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440012', 'bag', 'Túi xách, balo, cặp', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440013', 'shoes', 'Giày dép các loại', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440014', 'hat', 'Mũ, nón các loại', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440015', 'accessory', 'Phụ kiện trang sức', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440016', 'belt', 'Thắt lưng, dây nịt', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440017', 'scarf', 'Khăn choàng, khăn quàng cổ', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440018', 'socks', 'Vớ, tất', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440019', 'glasses', 'Kính mát, kính cận', NOW(), NOW()),
('660e8400-e29b-41d4-a716-446655440020', 'perfume', 'Nước hoa, nước thơm', NOW(), NOW());

-- 3. INSERT OCCASIONS (Tạm thời để trống)
-- =============================================
-- Không có dữ liệu occasion trong CSV, để trống

-- 4. INSERT PRODUCTS (Sample - First 10 products)
-- =============================================
INSERT INTO "Products" ("Id", "BrandId", "CategoryId", "OccasionId", "Name", "Description", "ImageUrl", "CreatedAt", "UpdatedAt") VALUES
('4895dcf3-38dd-4165-9328-1de30a639a71', '550e8400-e29b-41d4-a716-446655440001', '660e8400-e29b-41d4-a716-446655440001', NULL, 'Áo Thun Nam Cổ Tròn', 'Chất liệu cotton, thoáng mát', 'https://bizweb.dktcdn.net/100/534/573/products/frame-4415.jpg?v=1732700717757', NOW(), NOW()),
('73ebac8c-036a-4779-9e5b-0406d6910b9c', '550e8400-e29b-41d4-a716-446655440002', '660e8400-e29b-41d4-a716-446655440001', NULL, 'Áo Sơ Mi Nữ Công Sở', 'Kiểu dáng thanh lịch, dễ phối', 'https://bizweb.dktcdn.net/100/409/545/files/305661599-2233859066799333-2816418916497377809-n.jpg?v=1663732679803', NOW(), NOW()),
('e7742d83-2e38-48c7-ad7d-6f70e6f9776e', '550e8400-e29b-41d4-a716-446655440003', '660e8400-e29b-41d4-a716-446655440002', NULL, 'Quần Jeans Nam Rách', 'Phong cách trẻ trung, bền màu', 'https://product.hstatic.net/200000325151/product/r2efk-quan-jean-rach-nam_9684de705f0b42e4bdab711e79809221.png', NOW(), NOW()),
('b5a65130-63d6-4784-9c81-a8b8ddc841f7', '550e8400-e29b-41d4-a716-446655440004', '660e8400-e29b-41d4-a716-446655440003', NULL, 'Váy Maxi Hoa Nhí', 'Dáng dài, họa tiết hoa nhí', 'https://mdufashion.com/wp-content/uploads/2025/04/Gold-Home-Real-Estate-Social-Media-Post-1024x1024.png', NOW(), NOW()),
('6204b146-cfbf-4a86-affb-12d69811eebf', '550e8400-e29b-41d4-a716-446655440005', '660e8400-e29b-41d4-a716-446655440004', NULL, 'Áo Khoác Bomber Nam', 'Chống gió, cá tính', 'https://product.hstatic.net/1000209952/product/z4933862247115_f37575e7f14e9f115b631cdfd3734e56_428962b169ab46fb95dd15fa6fcad503_large.jpg', NOW(), NOW()),
('880d7708-3e22-4bc5-99fe-6fa55c8f2557', '550e8400-e29b-41d4-a716-446655440006', '660e8400-e29b-41d4-a716-446655440002', NULL, 'Quần Tây Nữ Lưng Cao', 'Lưng cao, tôn dáng', 'https://lamia.com.vn/storage/anh-seo/quan-tay-nu-lung-cao-dep.jpg', NOW(), NOW()),
('5a67b4bd-2ce4-4192-900a-3b39ea57911b', '550e8400-e29b-41d4-a716-446655440007', '660e8400-e29b-41d4-a716-446655440001', NULL, 'Áo Polo Nam Thêu Logo', 'Thêu logo, chất liệu cao cấp', 'https://product.hstatic.net/200000690725/product/7_4d50f92cb2ff4039b3f7cfbc1f0509f6.png', NOW(), NOW()),
('14720581-d829-4bff-81b9-29bd53b9c17a', '550e8400-e29b-41d4-a716-446655440008', '660e8400-e29b-41d4-a716-446655440004', NULL, 'Áo Khoác Dù Nữ', 'Chống nước, nhẹ', 'https://bizweb.dktcdn.net/thumb/1024x1024/100/310/270/products/z5659612544162-f0bf3566fc6c41e026f546497bedab8b.jpg?v=1721713066580', NOW(), NOW()),
('ee318f7f-800e-4d57-862b-92e4c9d33b9b', '550e8400-e29b-41d4-a716-446655440009', '660e8400-e29b-41d4-a716-446655440002', NULL, 'Quần Jogger Nam Thể Thao', 'Co giãn, thoải mái vận động', 'https://thegioidotap.vn/wp-content/uploads/2024/06/quan-dai-the-thao-nam-jogger-sg10-10.jpg', NOW(), NOW()),
('44a442b8-409b-4c5b-8089-ddd178561f8c', '550e8400-e29b-41d4-a716-446655440010', '660e8400-e29b-41d4-a716-446655440003', NULL, 'Váy Xòe Chấm Bi', 'Chấm bi, nữ tính', 'https://thoitrangnuhoang.com/data/Product/dam-xoe-cham-bi-tay-lo-3_1606657340.jpg', NOW(), NOW());

-- 5. INSERT PRODUCT SOURCES (Sample - First 10 products)
-- =============================================
INSERT INTO "ProductSources" ("Id", "ProductId", "VendorName", "Price", "AffiliateLink", "CreatedAt", "UpdatedAt") VALUES
('a1b2c3d4-e5f6-7890-abcd-ef1234567890', '4895dcf3-38dd-4165-9328-1de30a639a71', 'Default Vendor', 159000, '', NOW(), NOW()),
('b2c3d4e5-f6g7-8901-bcde-f23456789012', '73ebac8c-036a-4779-9e5b-0406d6910b9c', 'Default Vendor', 249000, '', NOW(), NOW()),
('c3d4e5f6-g7h8-9012-cdef-345678901234', 'e7742d83-2e38-48c7-ad7d-6f70e6f9776e', 'Default Vendor', 499000, '', NOW(), NOW()),
('d4e5f6g7-h8i9-0123-defg-456789012345', 'b5a65130-63d6-4784-9c81-a8b8ddc841f7', 'Default Vendor', 299000, '', NOW(), NOW()),
('e5f6g7h8-i9j0-1234-efgh-567890123456', '6204b146-cfbf-4a86-affb-12d69811eebf', 'Default Vendor', 399000, '', NOW(), NOW()),
('f6g7h8i9-j0k1-2345-fghi-678901234567', '880d7708-3e22-4bc5-99fe-6fa55c8f2557', 'Default Vendor', 259000, '', NOW(), NOW()),
('g7h8i9j0-k1l2-3456-ghij-789012345678', '5a67b4bd-2ce4-4192-900a-3b39ea57911b', 'Default Vendor', 599000, '', NOW(), NOW()),
('h8i9j0k1-l2m3-4567-hijk-890123456789', '14720581-d829-4bff-81b9-29bd53b9c17a', 'Default Vendor', 349000, '', NOW(), NOW()),
('i9j0k1l2-m3n4-5678-ijkl-901234567890', 'ee318f7f-800e-4d57-862b-92e4c9d33b9b', 'Default Vendor', 299000, '', NOW(), NOW()),
('j0k1l2m3-n4o5-6789-jklm-012345678901', '44a442b8-409b-4c5b-8089-ddd178561f8c', 'Default Vendor', 329000, '', NOW(), NOW());

-- =============================================
-- NOTES:
-- =============================================
-- 1. This script contains sample data for the first 10 products
-- 2. To import all 250 products, run the generate_sql.py script to generate complete INSERT statements
-- 3. AffiliateLink field is left empty as requested
-- 4. OccasionId is set to NULL as there's no occasion data in the CSV
-- 5. All timestamps use NOW() function
-- 6. Make sure to run this script in the correct order to maintain foreign key relationships
