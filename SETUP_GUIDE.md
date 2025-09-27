# Hướng dẫn Setup Project trong Visual Studio

## Bước 1: Mở Project trong Visual Studio

1. **Mở Visual Studio 2019/2022**
2. **File → Open → Project/Solution**
3. **Chọn file `CinameAsset.sln`** trong thư mục `d:\STUDY\NAM3\DBMS\PROJECT_FINAL\CinameAsset\`

## Bước 2: Cài đặt NuGet Packages

Nếu Visual Studio không tự động restore packages, thực hiện các bước sau:

### Cách 1: Sử dụng Package Manager Console
1. **Tools → NuGet Package Manager → Package Manager Console**
2. Chạy các lệnh sau:
```powershell
Install-Package Guna.UI2.WinForms -Version 2.0.4.6
Install-Package Newtonsoft.Json -Version 13.0.3
```

### Cách 2: Sử dụng Package Manager UI
1. **Right-click vào project → Manage NuGet Packages**
2. **Browse tab → Tìm "Guna.UI2.WinForms" → Install**
3. **Browse tab → Tìm "Newtonsoft.Json" → Install**

## Bước 3: Kiểm tra References

Đảm bảo các references sau đã được thêm:
- ✅ System
- ✅ System.Core
- ✅ System.Data
- ✅ System.Drawing
- ✅ System.Windows.Forms
- ✅ Guna.UI2
- ✅ Newtonsoft.Json

## Bước 4: Cấu hình Database

1. **Đảm bảo SQL Server đang chạy**
2. **Tạo database `CinemaAssetDB`**
3. **Chạy script `CREATE_TABLE.sql`**
4. **Chạy script `Thongtin_Hatang.sql`**
5. **Kiểm tra connection string trong `App.config`**

## Bước 5: Build và Run

1. **Build Solution**: `Ctrl + Shift + B`
2. **Start Debugging**: `F5`

## Troubleshooting

### Lỗi "Could not load file or assembly 'Guna.UI2'"
- Cài đặt lại NuGet package Guna.UI2.WinForms
- Kiểm tra Target Framework là .NET Framework 4.8

### Lỗi "The name 'Guna' does not exist in the current context"
- Clean Solution: `Build → Clean Solution`
- Rebuild Solution: `Build → Rebuild Solution`

### Lỗi Designer không mở được
- Close Visual Studio
- Xóa thư mục `bin` và `obj`
- Mở lại Visual Studio và Rebuild

### Lỗi kết nối Database
- Kiểm tra SQL Server đang chạy
- Kiểm tra connection string trong App.config
- Đảm bảo database CinemaAssetDB đã được tạo

## Cấu trúc File Project

```
CinameAsset/
├── InfrastructureManagement.cs          # Form chính
├── InfrastructureManagement.Designer.cs # Designer form chính
├── InfrastructureManagement.resx        # Resources form chính
├── AddAssetControl.cs                   # UserControl thêm thiết bị
├── AddAssetControl.Designer.cs          # Designer UserControl
├── AddAssetControl.resx                 # Resources UserControl
├── Program.cs                           # Entry point
├── App.config                           # Cấu hình ứng dụng
├── packages.config                      # NuGet packages
├── CinameAsset.csproj                   # Project file
├── CinameAsset.sln                      # Solution file
├── Properties/
│   ├── AssemblyInfo.cs
│   ├── Resources.resx
│   ├── Resources.Designer.cs
│   ├── Settings.settings
│   └── Settings.Designer.cs
├── CREATE_TABLE.sql                     # Script tạo database
├── Thongtin_Hatang.sql                 # Script views & procedures
└── README.md                           # Hướng dẫn sử dụng
```

## Lưu ý quan trọng

1. **Target Framework**: Đảm bảo project sử dụng .NET Framework 4.8
2. **Guna UI License**: Guna.UI2.WinForms có thể yêu cầu license cho commercial use
3. **SQL Server**: Cần SQL Server 2016 trở lên để hỗ trợ JSON functions
4. **Windows Forms**: Project này sử dụng Windows Forms, chỉ chạy trên Windows

## Liên hệ hỗ trợ

Nếu gặp vấn đề, hãy kiểm tra:
1. Visual Studio Output window
2. Error List window
3. Package Manager Console output
