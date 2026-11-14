<p align="center">
  <img src="logo.png" alt="Lengko Datatables WinForms Logo" width="140" />
</p>

# Lengko.Datatables.WinForms — Demos & Samples

High-performance, feature-packed data table for **Windows Forms** inspired by DataTables.net. Zero external dependencies, pure WinForms + GDI+ double-buffered rendering.

[![Platform](https://img.shields.io/badge/Platform-Windows%20Forms-brightgreen.svg)]()
[![Languages](https://img.shields.io/badge/Languages-C%23%20%7C%20VB.NET%20%7C%20F%23-blue.svg)]()
[![Target](https://img.shields.io/badge/.NET-8.0--windows-512BD4.svg)]()
[![License](https://img.shields.io/badge/License-Freeware-orange.svg)](LICENSE.txt)

---

## Projects in this Folder

| Language | Project File | Description |
|---|---|---|
| **C#** | `CSharp/Lengko.Datatables.Demo.CSharp.csproj` | Comprehensive C# showcase with search, pagination, multi-select, filter row, summary rows, row grouping, child rows, conditional formatting, and state persistence. |
| **VB.NET** | `VB/Lengko.Datatables.Demo.VB.vbproj` | Full VB.NET showcase implementing grid features, toolbar controls, theme toggling, and data export. |
| **F#** | `FSharp/Lengko.Datatables.Demo.FSharp.fsproj` | Functional F# WinForms demo demonstrating data binding, search, pagination, filter chips, theme switching, and CSV export. |

Dedicated solution: **`Lengko.Datatables.WinForms.sln`** (builds all 3 demos alongside the library).

---

## Controls & Components Demonstrated

- `LengkoDataTableGrid`: Core data table with sorting, row grouping, zebra striping, multi-select, child rows, summary footer.
- `LengkoDataTableSearch`: Real-time search filter with debounce and clear button.
- `LengkoDataTableLength`: Page length selector dropdown (10, 25, 50, 100).
- `LengkoDataTablePagination`: Page navigation buttons with smart ellipsis.
- `LengkoDataTableInfo`: Real-time record summary counter ("Showing 1 to 10 of 25 entries").
- `LengkoDataTableFilterChips`: Active filter tag chips with quick-remove buttons.
- `LengkoDataTableButtons`: Dropdown toolbar for column visibility toggles and export commands.
- `LengkoDataTableThemeScope`: Global theme provider for Light and Dark modes.
- `LengkoDataExporter`: Built-in CSV data export engine.

---

## Quick Start / How to Run

### Via Visual Studio
Open `Lengko.Datatables.WinForms.sln`, select any demo project as Startup Project, and press `F5`.

### Via .NET CLI

```powershell
# Run C# Demo
dotnet run --project CSharp/Lengko.Datatables.Demo.CSharp.csproj

# Run VB.NET Demo
dotnet run --project VB/Lengko.Datatables.Demo.VB.vbproj

# Run F# Demo
dotnet run --project FSharp/Lengko.Datatables.Demo.FSharp.fsproj
```

---

## Code Examples

### C#
```csharp
using Lengko.Datatables.WinForms;

var grid = new LengkoDataTableGrid
{
    Dock = DockStyle.Fill,
    PageLength = 10,
    ZebraStriping = true
};
grid.DataSource = myDataTable;

// Top controls
var search = new LengkoDataTableSearch { Grid = grid, Dock = DockStyle.Right };
var length = new LengkoDataTableLength { Grid = grid, Dock = DockStyle.Left };

// CSV Export
grid.ExportToCsv("employees.csv");
```

### VB.NET
```vb
Imports Lengko.Datatables.WinForms

Dim grid As New LengkoDataTableGrid With {
    .Dock = DockStyle.Fill,
    .PageLength = 10,
    .ZebraStriping = True
}
grid.DataSource = myDataTable

' Top controls
Dim search As New LengkoDataTableSearch With { .Grid = grid, .Dock = DockStyle.Right }
Dim length As New LengkoDataTableLength With { .Grid = grid, .Dock = DockStyle.Left }

' CSV Export
grid.ExportToCsv("employees.csv")
```

### F#
```fsharp
open Lengko.Datatables.WinForms

let grid = new LengkoDataTableGrid(
    Dock = DockStyle.Fill,
    PageLength = 10,
    ZebraStriping = true
)
grid.DataSource <- myDataTable

// Top controls
let search = new LengkoDataTableSearch(Grid = grid, Dock = DockStyle.Right)
let length = new LengkoDataTableLength(Grid = grid, Dock = DockStyle.Left)

// CSV Export
grid.ExportToCsv("employees.csv")
```

<img width="1020" height="518" alt="image" src="https://github.com/user-attachments/assets/66123578-f743-4eec-92d6-df79e401b7f3" />


---

## License
Proprietary Freeware License (PFL). Free for personal, educational, and commercial use. See [LICENSE.txt](LICENSE.txt).
