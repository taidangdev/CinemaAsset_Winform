# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Project Overview

Cinema Asset Management System (CinemaAsset) - A Windows Forms application built with C# and .NET Framework 4.8 for managing cinema infrastructure assets. The system handles equipment tracking across auditoriums, vendor management, purchase statistics, and warehouse inventory.

## Core Technologies

- **Framework**: .NET Framework 4.8
- **UI Library**: Guna.UI2.WinForms 2.0.4.7 (modern UI components)
- **Database**: SQL Server with LocalDB/SQL Server Express
- **JSON Processing**: Newtonsoft.Json 13.0.3
- **Architecture**: Windows Forms MDI with stored procedure-based data layer

## Build and Development Commands

### Build Commands
```powershell
# Build the solution
msbuild CinameAsset.sln -p:Configuration=Debug
# Or in Visual Studio: Ctrl+Shift+B

# Build Release version
msbuild CinameAsset.sln -p:Configuration=Release
```

### Run Commands
```powershell
# Run from Visual Studio: F5 (Debug) or Ctrl+F5 (Without Debug)

# Run executable directly
.\bin\Debug\CinameAsset.exe
```

### Package Management
```powershell
# Restore NuGet packages
nuget restore CinameAsset.sln

# Install required packages if missing
Install-Package Guna.UI2.WinForms -Version 2.0.4.7
Install-Package Newtonsoft.Json -Version 13.0.3
```

### Database Setup
```sql
-- 1. Create database in SQL Server
CREATE DATABASE CinemaAssetDB;

-- 2. Run SQL scripts in order:
sqlcmd -S localhost -d CinemaAssetDB -i "SQL\CREATE_TABLE.sql"
sqlcmd -S localhost -d CinemaAssetDB -i "SQL\Thongtin_Hatang.sql"
sqlcmd -S localhost -d CinemaAssetDB -i "SQL\doitac.sql"
sqlcmd -S localhost -d CinemaAssetDB -i "SQL\thongke.sql"
sqlcmd -S localhost -d CinemaAssetDB -i "SQL\tonkho.sql"
```

## Application Architecture

### Main Form Structure
- **MainForm**: MDI container with navigation sidebar
  - Infrastructure Management (default view)
  - Vendor Management
  - Purchase Statistics  
  - Warehouse Management
- **Child Forms**: Each module loads as child form in `panelContent`

### Form Hierarchy
```
MainForm (MDI Container)
├── InfrastructureManagement (equipment per auditorium)
├── VendorManagement (vendor CRUD + catalog)
├── PurchaseStatistics (bill history + analytics)
├── WarehouseManagement (inventory tracking)
└── Supporting Forms:
    ├── AddVendorForm
    ├── PurchaseForm  
    ├── BillDetailForm
    └── UserControls: AddAssetControl, AddSeatControl
```

### Database Layer Pattern
- **Connection String**: Centralized in `App.config` and hardcoded in MainForm
- **Data Access**: Direct SqlConnection/SqlCommand with stored procedures
- **JSON Integration**: Uses Newtonsoft.Json for complex parameter passing to stored procedures
- **Transaction Management**: Stored procedures handle transactions with proper locking

### Key Database Components

#### Core Tables
- `Auditorium`: Cinema rooms/theaters
- `Asset`: Equipment (screens, speakers, AC) with unit numbers
- `Seat`: Special asset type with row/position (A1, B2, etc.)  
- `Vendor`: Equipment suppliers
- `VendorCatalog`: Many-to-many vendor ↔ asset types
- `Warehouse`: Inventory tracking with min/max levels
- `Bill/BillItem`: Purchase orders and line items

#### Stored Procedures
- **Vendor Management**: `sp_Vendor_CreateWithCatalog`, `sp_VendorCatalog_Set`
- **Purchase Processing**: `usp_ReceiveBill`, `sp_Vendor_ReceivePurchase`  
- **Statistics**: `sp_PurchaseStats_ListAndTotal`, `sp_Bill_GetDetail`
- **Warehouse**: `sp_Warehouse_ReorderSuggestion`
- **Infrastructure**: See `Thongtin_Hatang.sql` for asset installation procedures

#### Views & Functions
- `vw_VendorActiveWithCatalog`: Vendor listing with asset type aggregation
- `vw_WarehouseStatus`: Inventory with low-stock flagging
- `fn_WarehouseFilter`: Parameterized inventory filtering
- `fn_BillSummary`: Purchase history with date/vendor filtering

### Asset Type System
Predefined types with Vietnamese display names:
- `SEAT` → "Ghế" 
- `SCREEN` → "Màn hình"
- `SPEAKER` → "Loa"  
- `AIR_CON` → "Máy lạnh"

### UI Patterns
- **Guna2 Controls**: Consistent UI using Guna2Button, Guna2Panel, Guna2DataGridView
- **Color Scheme**: Blue (#5E90FF) for active states, dark gray (#2D3436) for sidebar
- **Form Loading**: Child forms set to `TopLevel = false`, `Dock = Fill` in content panel
- **Data Binding**: Manual DataGridView population via SqlDataReader loops

## Development Notes

### Connection String Management
Default connection in `App.config`:
```xml
<connectionStrings>
  <add name="CinemaAssetDB" connectionString="Server=localhost;Database=CinemaAssetDB;User Id=sa;Password=1234;" />
</connectionStrings>
```

### JSON Payload Patterns
When working with stored procedures expecting JSON parameters:
```csharp
// Asset types for vendor catalog
var assetTypes = new List<object>();
foreach (var item in checkedItems) {
    assetTypes.Add(new { asset_type_id = item.Id });
}
string json = JsonConvert.SerializeObject(assetTypes);
```

### Error Handling Standards  
- Use try-catch blocks around all database operations
- Display user-friendly Vietnamese messages via MessageBox
- Log technical details in catch blocks for debugging

### Form Navigation Pattern
```csharp
private void OpenChildForm(Form childForm)
{
    if (currentChildForm != null) {
        currentChildForm.Close();
        currentChildForm.Dispose();
    }
    
    currentChildForm = childForm;
    childForm.TopLevel = false;
    childForm.FormBorderStyle = FormBorderStyle.None;
    childForm.Dock = DockStyle.Fill;
    
    panelContent.Controls.Clear();
    panelContent.Controls.Add(childForm);
    childForm.Show();
}
```

### Concurrency Control
The system uses SQL Server application locks for warehouse operations:
```sql
EXEC sp_getapplock 
     @Resource = N'WAREHOUSE_LOCK',
     @LockMode = 'Exclusive',
     @LockOwner = 'Session',
     @LockTimeout = 10000;
```

## Testing Database Changes

When modifying stored procedures or database schema:
1. Test with sample data in SQL Server Management Studio
2. Verify trigger behavior for data normalization (especially `Seat` table)
3. Check constraint violations and foreign key relationships
4. Test transaction rollback scenarios in error conditions

## Localization Notes

The application uses Vietnamese language for:
- UI labels and buttons
- Database error messages  
- Asset type display names
- Status indicators

When adding new features, maintain Vietnamese localization consistency with existing patterns.