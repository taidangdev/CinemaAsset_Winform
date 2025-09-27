# Cinema Asset Management System

## Mô tả
Hệ thống quản lý thông tin hạ tầng rạp chiếu phim được xây dựng bằng C# WinForms với Guna UI 2.0.

## Tính năng chính
- **Quản lý thiết bị theo phòng chiếu**: Xem danh sách thiết bị trong từng phòng chiếu
- **Lọc theo loại thiết bị**: Hiển thị tất cả thiết bị, chỉ ghế, hoặc thiết bị khác (màn hình, loa, máy lạnh)
- **Lắp thêm thiết bị**: Thêm thiết bị mới từ kho vào phòng chiếu
- **Cập nhật trạng thái**: Đánh dấu thiết bị hỏng hoặc thay thế từ kho
- **Xóa thiết bị**: Gỡ bỏ thiết bị khỏi phòng (thiết bị tốt sẽ được trả về kho)

## Cấu trúc Database
- **CinemaAssetDB**: Database chính
- **Auditorium**: Bảng phòng chiếu
- **Asset**: Bảng thiết bị (màn hình, loa, máy lạnh)
- **Seat**: Bảng ghế ngồi
- **AssetType**: Bảng loại thiết bị
- **Warehouse**: Bảng kho tồn

## Cài đặt và chạy

### Yêu cầu hệ thống
- .NET Framework 4.8
- SQL Server (LocalDB hoặc SQL Server Express)
- Visual Studio 2019 hoặc mới hơn

### Cài đặt NuGet Packages
```
Install-Package Guna.UI2.WinForms -Version 2.0.4.6
Install-Package Newtonsoft.Json -Version 13.0.3
```

### Cấu hình Database
1. Tạo database `CinemaAssetDB` trên SQL Server
2. Chạy script `CREATE_TABLE.sql` để tạo bảng và dữ liệu mẫu
3. Chạy script `Thongtin_Hatang.sql` để tạo view và stored procedures
4. Cập nhật connection string trong `App.config` nếu cần:
   ```xml
   <connectionStrings>
       <add name="CinemaAssetDB" connectionString="Server=localhost;Database=CinemaAssetDB;User Id=sa;Password=1234;" />
   </connectionStrings>
   ```

### Chạy ứng dụng
1. Mở project trong Visual Studio
2. Build solution (Ctrl+Shift+B)
3. Chạy ứng dụng (F5)

## Hướng dẫn sử dụng

### Giao diện chính
- **ComboBox Phòng chiếu**: Chọn phòng muốn xem thông tin
- **ComboBox Loại thiết bị**: Lọc hiển thị theo loại (Tất cả, Ghế, Thiết bị khác)
- **DataGridView**: Hiển thị danh sách thiết bị với các cột:
  - Loại thiết bị
  - Số thứ tự/Vị trí
  - Trạng thái (Hoạt động/Hỏng)
  - Ngày lắp đặt
  - Nút Cập nhật (đánh dấu hỏng hoặc thay thế)
  - Nút Xóa

### Lắp thêm thiết bị
1. Chọn phòng chiếu
2. Click nút "Lắp Thêm Thiết Bị"
3. Chọn loại thiết bị và số lượng
4. Click "Thêm"

### Cập nhật trạng thái thiết bị
- **Thiết bị đang hoạt động**: Click "Cập Nhật" để đánh dấu hỏng
- **Thiết bị hỏng**: Click "Cập Nhật" để thay thế từ kho (nếu kho đủ)

### Xóa thiết bị
- Click nút "Xóa" để gỡ bỏ thiết bị khỏi phòng
- Thiết bị còn tốt sẽ được trả về kho

## Lưu ý kỹ thuật
- Ứng dụng sử dụng stored procedures để đảm bảo tính toàn vẹn dữ liệu
- Có khóa warehouse để tránh race condition khi cập nhật kho
- Ghế được quản lý riêng trong bảng Seat với format hàng-cột (A1, B2, ...)
- Thiết bị khác được quản lý trong bảng Asset với unit_no tự động tăng

## Troubleshooting
- **Lỗi kết nối database**: Kiểm tra connection string và đảm bảo SQL Server đang chạy
- **Lỗi thiếu Guna UI**: Cài đặt NuGet package Guna.UI2.WinForms
- **Lỗi JSON**: Cài đặt NuGet package Newtonsoft.Json
