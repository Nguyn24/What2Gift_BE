# API Examples - Query Parameters

## Base URL
```
http://localhost:5010 hoặc https://localhost:7196
```

---

## Admin APIs

### 1. Get Dashboard Stats
```
GET /api/admin/dashboard/stats?fromDate=2024-01-01T00:00:00Z&toDate=2024-12-31T23:59:59Z
GET /api/admin/dashboard/stats
```

### 2. Get All Users
```
GET /api/admin/users?pageNumber=1&pageSize=10
GET /api/admin/users?pageNumber=1&pageSize=10&searchTerm=john
GET /api/admin/users?pageNumber=1&pageSize=10&status=1
GET /api/admin/users?pageNumber=1&pageSize=10&createdFrom=2024-01-01&createdTo=2024-12-31
GET /api/admin/users?pageNumber=1&pageSize=10&searchTerm=test&status=1&createdFrom=2024-01-01
```

**Query Parameters:**
- `pageNumber` (int, default: 1)
- `pageSize` (int, default: 10)
- `searchTerm` (string, optional) - Search by username or email
- `status` (int, optional) - 0=Inactive, 1=Active, 2=Suspended
- `createdFrom` (DateOnly, optional) - Format: YYYY-MM-DD
- `createdTo` (DateOnly, optional) - Format: YYYY-MM-DD

### 3. Get All Memberships
```
GET /api/admin/memberships?pageNumber=1&pageSize=10
GET /api/admin/memberships?pageNumber=1&pageSize=10&searchTerm=user@email.com
GET /api/admin/memberships?pageNumber=1&pageSize=10&isActive=true
GET /api/admin/memberships?pageNumber=1&pageSize=10&membershipPlanId=123e4567-e89b-12d3-a456-426614174000
GET /api/admin/memberships?pageNumber=1&pageSize=10&startDateFrom=2024-01-01&startDateTo=2024-12-31
```

**Query Parameters:**
- `pageNumber` (int, default: 1)
- `pageSize` (int, default: 10)
- `searchTerm` (string, optional)
- `isActive` (bool, optional) - true/false
- `membershipPlanId` (Guid, optional)
- `startDateFrom` (DateOnly, optional) - Format: YYYY-MM-DD
- `startDateTo` (DateOnly, optional) - Format: YYYY-MM-DD

### 4. Get All Payments
```
GET /api/admin/payments?pageNumber=1&pageSize=10
GET /api/admin/payments?pageNumber=1&pageSize=10&status=2
GET /api/admin/payments?pageNumber=1&pageSize=10&createdFrom=2024-01-01&createdTo=2024-12-31
GET /api/admin/payments?pageNumber=1&pageSize=10&minAmount=1000&maxAmount=50000
GET /api/admin/payments?pageNumber=1&pageSize=10&searchTerm=user@email.com
GET /api/admin/payments?pageNumber=1&pageSize=10&sortBy=paymentType&sortOrder=0
GET /api/admin/payments?pageNumber=1&pageSize=10&sortBy=status&sortOrder=1
GET /api/admin/payments?pageNumber=1&pageSize=10&sortBy=amount&sortOrder=0&status=2
```

**Query Parameters:**
- `pageNumber` (int, default: 1)
- `pageSize` (int, default: 10)
- `searchTerm` (string, optional)
- `status` (int, optional) - 1=Pending, 2=Success, 3=Failed
- `createdFrom` (DateOnly, optional) - Format: YYYY-MM-DD
- `createdTo` (DateOnly, optional) - Format: YYYY-MM-DD
- `minAmount` (decimal, optional)
- `maxAmount` (decimal, optional)
- `sortBy` (string, optional) - Options: "amount", "createdAt", "status", "paymentMethod", "paymentType"
- `sortOrder` (int, optional) - 0=Ascending, 1=Descending (default: 1)

### 5. Get All Top-Up Transactions (Pending Only)
```
GET /api/admin/topup/transactions?pageNumber=1&pageSize=10
GET /api/admin/topup/transactions?pageNumber=1&pageSize=10&searchTerm=nap1234
GET /api/admin/topup/transactions?pageNumber=1&pageSize=10&createdFrom=2024-01-01&createdTo=2024-12-31
GET /api/admin/topup/transactions?pageNumber=1&pageSize=10&minAmount=10000&maxAmount=100000
GET /api/admin/topup/transactions?pageNumber=1&pageSize=10&sortBy=amount&sortOrder=0
GET /api/admin/topup/transactions?pageNumber=1&pageSize=10&sortBy=createdAt&sortOrder=1
```

**Query Parameters:**
- `pageNumber` (int, default: 1)
- `pageSize` (int, default: 10)
- `searchTerm` (string, optional) - Search by transfer content, username, or email
- `createdFrom` (DateOnly, optional) - Format: YYYY-MM-DD
- `createdTo` (DateOnly, optional) - Format: YYYY-MM-DD
- `minAmount` (decimal, optional)
- `maxAmount` (decimal, optional)
- `sortBy` (string, optional) - Options: "amount", "createdAt", "status"
- `sortOrder` (int, optional) - 0=Ascending, 1=Descending (default: 1)

**Note:** Only pending transactions are returned

---

## User APIs

### 6. Get Top-Up Info
```
GET /api/topup/info
```
**Headers:** Authorization: Bearer {token}

---

## Other APIs

### 7. Get All Membership Plans
```
GET /api/membership-plan/get-all?pageNumber=1&pageSize=10
```

### 8. Get All Feedbacks
```
GET /api/feedback/get-all-feedbacks?pageNumber=1&pageSize=10
```

### 9. Get All Occasions
```
GET /api/occasion/get-all-occasions?pageNumber=1&pageSize=10
```

### 10. Get All Categories
```
GET /api/category/get-all-categories?pageNumber=1&pageSize=10
```

### 11. Get All Brands
```
GET /api/brand/get-all-brands?pageNumber=1&pageSize=10
```

### 12. Get All Products
```
GET /api/product/get-all-product?pageNumber=1&pageSize=10
```

### 13. Search Products
```
GET /api/product/search-product?pageNumber=1&pageSize=10
GET /api/product/search-product?pageNumber=1&pageSize=10&brandId=123e4567-e89b-12d3-a456-426614174000
GET /api/product/search-product?pageNumber=1&pageSize=10&categoryId=123e4567-e89b-12d3-a456-426614174000&occasionId=123e4567-e89b-12d3-a456-426614174000
GET /api/product/search-product?pageNumber=1&pageSize=10&minPrice=10000&maxPrice=500000
GET /api/product/search-product?pageNumber=1&pageSize=10&brandId=xxx&categoryId=xxx&minPrice=10000&maxPrice=500000
```

**Query Parameters:**
- `pageNumber` (int, required)
- `pageSize` (int, required)
- `brandId` (Guid, optional)
- `categoryId` (Guid, optional)
- `occasionId` (Guid, optional)
- `minPrice` (decimal, optional)
- `maxPrice` (decimal, optional)

### 14. Get All Users (Public)
```
GET /api/user/get-all-users?pageNumber=1&pageSize=10
```

---

## Examples với curl

### Get All Payments với sorting
```bash
curl -X GET "https://localhost:7196/api/admin/payments?pageNumber=1&pageSize=10&sortBy=paymentType&sortOrder=0" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### Get All Top-Up Transactions
```bash
curl -X GET "https://localhost:7196/api/admin/topup/transactions?pageNumber=1&pageSize=10&sortBy=createdAt&sortOrder=1" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### Get All Payments với filter và sort
```bash
curl -X GET "https://localhost:7196/api/admin/payments?pageNumber=1&pageSize=10&status=2&sortBy=amount&sortOrder=1&createdFrom=2024-01-01" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

---

## Status Values

### PaymentTransactionStatus
- `1` = Pending
- `2` = Success
- `3` = Failed

### TopUpTransactionStatus
- `1` = Pending
- `2` = Approved
- `3` = Rejected

### UserStatus
- `0` = Inactive
- `1` = Active
- `2` = Suspended

### SortOrder
- `0` = Ascending
- `1` = Descending

---

## Sort Options

### For Payments (GetAllPayments)
- `sortBy=amount` - Sort by amount
- `sortBy=createdAt` - Sort by creation date
- `sortBy=status` - Sort by status
- `sortBy=paymentMethod` - Sort by payment method
- `sortBy=paymentType` - Sort by payment type (TOP_UP / MEMBERSHIP)

### For Top-Up Transactions
- `sortBy=amount` - Sort by amount
- `sortBy=createdAt` - Sort by creation date
- `sortBy=status` - Sort by status

