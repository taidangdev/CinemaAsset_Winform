# HƯỚNG DẪN TRIỂN KHAI HỆ THỐNG RBAC - CINEMA ASSET MANAGEMENT

## 📋 TỔNG QUAN HỆ THỐNG

Hệ thống đã được triển khai với mô hình **Role-Based Access Control (RBAC)** bao gồm:

### 🎭 VAI TRÒ (ROLES)
- **Admin (QuanLy)**: Toàn quyền quản lý hệ thống
- **Staff (NhanVien)**: Chỉ được xem dữ liệu và báo hỏng thiết bị

---

## 🗃️ CẤU TRÚC DATABASE

### Bảng Accounts
```sql
- user_id: ID tài khoản
- FullName: Họ tên người dùng  
- username: Tên đăng nhập (SQL Login)
- password_hash: Mật khẩu đã hash SHA256
- role_name: 'QuanLy' hoặc 'NhanVien'
- is_active: Trạng thái hoạt động
```

### SQL Roles & Permissions
- **[Admin]**: Toàn quyền SELECT, INSERT, UPDATE, DELETE
- **[Staff]**: Chỉ SELECT + 2 procedures báo hỏng

---

## 💻 CÁC FILE C# ĐÃ TẠO

### 1. SecurityHelper.cs
**Chức năng**: Xử lý bảo mật và hashing mật khẩu
```csharp
- HashPassword(string password): Tạo SHA256 hash
- VerifyPassword(string input, string stored): Xác minh mật khẩu
- GenerateSalt(int size): Tạo salt ngẫu nhiên
```

### 2. SessionManager.cs
**Chức năng**: Quản lý phiên đăng nhập và connection string động
```csharp
Properties:
- Username, FullName, RoleName
- IsAdmin, IsStaff, IsLoggedIn
- CurrentUserConnectionString

Methods:
- Login(username, password): Xác thực đăng nhập
- Logout(): Đăng xuất
- GetConnectionString(): Lấy connection string cá nhân hóa
```

### 3. LoginForm.cs
**Chức năng**: Form đăng nhập chính
- Xác thực qua `fn_Login` SQL function
- Tự động hash mật khẩu
- Chuyển đến MainForm sau khi đăng nhập thành công

### 4. UserManagementForm.cs
**Chức năng**: Quản lý tài khoản (chỉ dành cho Admin)
- Thêm tài khoản mới qua `sp_CreateUserAccount`
- Xóa tài khoản khỏi bảng Accounts
- Hiển thị danh sách tài khoản

### 5. MainForm.cs (Đã sửa)
**Chức năng**: Form chính với RBAC
- Kiểm tra quyền truy cập khi load
- Sử dụng connection string cá nhân hóa
- Chức năng đăng xuất
- Áp dụng phân quyền UI theo role

---

## 🔧 CẤU HÌNH VÀ TRIỂN KHAI

### Bước 1: Chạy Script SQL
```sql
-- Chạy các file SQL theo thứ tự:
1. CREATE_TABLE.sql (đã có)
2. role.sql (phân quyền RBAC)
3. Các file procedures/functions khác
```

### Bước 2: Cấu hình Connection String
Trong `SessionManager.cs`, cập nhật thông tin SQL Server:
```csharp
private const string ServerName = "TEN_SQL_SERVER";
private const string DatabaseName = "CinemaAssetDB";
private const string AuthConnectionString = "...";
```

### Bước 3: Tạo Tài Khoản Test
```sql
-- Tạo tài khoản Admin test
EXEC sp_CreateUserAccount 
    @Username = 'admin1',
    @PasswordPlain = 'admin123',
    @PasswordHash = 'hash_của_admin123',
    @FullName = N'Quản trị viên',
    @RoleName = N'QuanLy';

-- Tạo tài khoản Staff test  
EXEC sp_CreateUserAccount 
    @Username = 'staff1',
    @PasswordPlain = 'staff123', 
    @PasswordHash = 'hash_của_staff123',
    @FullName = N'Nhân viên',
    @RoleName = N'NhanVien';
```

---

## 🎯 PHÂN QUYỀN CHI TIẾT

### 👑 ADMIN (QuanLy)
**Có thể làm:**
- ✅ Xem tất cả dữ liệu
- ✅ Thêm/sửa/xóa tất cả records
- ✅ Quản lý tài khoản user
- ✅ Thực hiện tất cả stored procedures
- ✅ Quản lý vendor, kho hàng, thiết bị
- ✅ Báo hỏng và thay thế thiết bị

### 👤 STAFF (NhanVien) 
**Có thể làm:**
- ✅ Xem tất cả dữ liệu (SELECT)
- ✅ Báo hỏng thiết bị (`sp_Asset_MarkBroken`)
- ✅ Báo hỏng ghế (`sp_Seat_MarkBroken`)
- ✅ Xem báo cáo, thống kê

**Không thể làm:**
- ❌ Thêm/sửa/xóa dữ liệu
- ❌ Quản lý tài khoản
- ❌ Thay thế thiết bị từ kho
- ❌ Quản lý vendor
- ❌ Nhập hàng

---

## 🚀 CÁCH SỬ DỤNG

### 1. Khởi chạy ứng dụng
- Ứng dụng sẽ bắt đầu với `LoginForm`
- Nhập username/password để đăng nhập

### 2. Sau khi đăng nhập
- Hệ thống tự động áp dụng quyền theo role
- Connection string được tạo động theo user
- UI hiển thị phù hợp với quyền

### 3. Quản lý tài khoản (Admin)
- Truy cập menu "Quản lý tài khoản"
- Thêm/xóa tài khoản staff
- Chỉ Admin mới thấy menu này

### 4. Đăng xuất
- Click nút "Đăng xuất" 
- Hệ thống xóa phiên và quay về LoginForm

---

## 🛡️ BẢO MẬT

### Mã hóa mật khẩu
- Sử dụng SHA256 hash
- Mật khẩu không lưu dạng plaintext
- Hash trên client trước khi gửi lên server

### Connection String động
- Mỗi user có connection string riêng
- Sử dụng SQL Login của chính user đó
- Quyền được áp dụng tự động bởi SQL Server

### Validation
- Kiểm tra quyền truy cập ở multiple layers
- UI level: Ẩn/disable controls
- Business logic level: Kiểm tra role
- Database level: SQL Server permissions

---

## 📝 GHI CHÚ QUAN TRỌNG

1. **Cần thêm UI controls**: 
   - Button "Quản lý tài khoản" trong MainForm
   - Button "Đăng xuất" trong MainForm

2. **Cần cập nhật các form con**:
   - Truyền quyền user vào các form con
   - Ẩn/disable buttons Add/Edit cho Staff

3. **Test kỹ lưỡng**:
   - Đăng nhập với các role khác nhau
   - Kiểm tra quyền truy cập database
   - Verify UI permissions

4. **Backup dữ liệu** trước khi triển khai production

---

## ⚠️ LƯU Ý BẢO MẬT

- **KHÔNG BAO GIỜ** hardcode password trong code
- **LUÔN** sử dụng parameterized queries
- **THƯỜNG XUYÊN** thay đổi mật khẩu
- **GIÁM SÁT** logs đăng nhập
- **BACKUP** database định kỳ
