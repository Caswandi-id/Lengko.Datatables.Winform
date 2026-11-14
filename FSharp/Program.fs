namespace Lengko.Datatables.Demo.FSharp

open System
open System.Data
open System.Drawing
open System.Windows.Forms
open Lengko.Datatables.WinForms

module Program =

    let createSampleTable () =
        let dt = new DataTable("Employees")
        dt.Columns.Add("ID", typeof<int>) |> ignore
        dt.Columns.Add("Name", typeof<string>) |> ignore
        dt.Columns.Add("Position", typeof<string>) |> ignore
        dt.Columns.Add("Department", typeof<string>) |> ignore
        dt.Columns.Add("Office", typeof<string>) |> ignore
        dt.Columns.Add("Age", typeof<int>) |> ignore
        dt.Columns.Add("Salary", typeof<string>) |> ignore
        dt.Columns.Add("Status", typeof<string>) |> ignore

        let rows = [|
            (1, "Airi Satou", "Accountant", "Finance", "Tokyo", 33, "$162,700", "Active")
            (2, "Angelica Ramos", "Chief Executive Officer (CEO)", "Executive", "London", 47, "$1,200,000", "Active")
            (3, "Ashton Cox", "Junior Technical Author", "Editorial", "San Francisco", 66, "$86,000", "Active")
            (4, "Bradley Greer", "Software Engineer", "Engineering", "London", 41, "$132,000", "Active")
            (5, "Brenden Wagner", "Software Engineer", "Engineering", "San Francisco", 28, "$206,850", "Active")
            (6, "Brielle Williamson", "Integration Specialist", "IT", "New York", 61, "$372,000", "Active")
            (7, "Bruno Nash", "Software Engineer", "Engineering", "London", 38, "$163,500", "Active")
            (8, "Caesar Vance", "Pre-Sales Support", "Sales", "New York", 21, "$106,450", "Inactive")
            (9, "Cara Stevens", "Sales Assistant", "Sales", "New York", 46, "$145,600", "Active")
            (10, "Cedric Kelly", "Senior Javascript Developer", "Engineering", "Edinburgh", 22, "$433,060", "Active")
            (11, "Charde Marshall", "Regional Director", "Management", "San Francisco", 36, "$470,600", "Active")
            (12, "Colleen Hurst", "Javascript Developer", "Engineering", "San Francisco", 39, "$205,500", "Active")
            (13, "Dai Rios", "Personnel Lead", "HR", "Edinburgh", 35, "$217,500", "Active")
            (14, "Donna Snider", "Customer Support", "Support", "New York", 27, "$112,000", "Active")
            (15, "Doris Wilder", "Sales Assistant", "Sales", "Sydney", 23, "$85,600", "Active")
            (16, "Finn Camacho", "Support Engineer", "Support", "San Francisco", 47, "$87,500", "Active")
            (17, "Fiona Green", "Chief Operating Officer (COO)", "Executive", "San Francisco", 48, "$850,000", "Active")
            (18, "Garrett Winters", "Accountant", "Finance", "Tokyo", 63, "$170,750", "Active")
            (19, "Gavin Cortez", "Team Leader", "Management", "San Francisco", 22, "$235,500", "Active")
            (20, "Gavin Joyce", "Developer", "Engineering", "Edinburgh", 42, "$92,575", "Active")
            (21, "Gloria Little", "Systems Administrator", "IT", "New York", 59, "$237,500", "Active")
            (22, "Haley Kennedy", "Senior Marketing Designer", "Marketing", "London", 43, "$313,500", "Active")
            (23, "Hermione Butler", "Regional Director", "Management", "London", 47, "$356,250", "Active")
            (24, "Herrod Chandler", "Sales Assistant", "Sales", "San Francisco", 59, "$137,500", "Active")
            (25, "Hope Fuentes", "Secretary", "Administration", "San Francisco", 41, "$109,850", "Inactive")
        |]

        for (id, name, pos, dept, off, age, sal, st) in rows do
            dt.Rows.Add(box id, box name, box pos, box dept, box off, box age, box sal, box st) |> ignore

        dt

    [<STAThread>]
    [<EntryPoint>]
    let main _ =
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(false)

        let form = new Form(
            Text = "Lengko.Datatables.WinForms — F# Demo",
            Width = 1060,
            Height = 720,
            StartPosition = FormStartPosition.CenterScreen,
            Font = new Font("Segoe UI", 9.5f),
            BackColor = Color.FromArgb(248, 250, 252)
        )

        let root = new TableLayoutPanel(
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = Padding(16)
        )
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 60.0f)) |> ignore
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 42.0f)) |> ignore
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 36.0f)) |> ignore
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f)) |> ignore

        // Header
        let header = new Panel(Dock = DockStyle.Fill)
        let title = new Label(
            Text = "Lengko.Datatables Demo (F# WinForms)",
            Font = new Font("Segoe UI", 15.0f, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42),
            AutoSize = true,
            Location = Point(0, 2)
        )
        let subTitle = new Label(
            Text = "Modern data table with instant search, pagination, column sorting, filter chips, and export.",
            Font = new Font("Segoe UI", 9.5f),
            ForeColor = Color.FromArgb(100, 116, 139),
            AutoSize = true,
            Location = Point(0, 30)
        )
        header.Controls.Add(title)
        header.Controls.Add(subTitle)
        root.Controls.Add(header, 0, 0)

        // Main Table Card
        let card = new Panel(
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = Padding(14)
        )

        let grid = new LengkoDataTableGrid(
            Dock = DockStyle.Fill,
            PageLength = 10,
            ZebraStriping = true
        )

        let filterChips = new LengkoDataTableFilterChips(
            Dock = DockStyle.Fill,
            Grid = grid
        )
        root.Controls.Add(filterChips, 0, 2)

        // Feature Bar
        let featureBar = new FlowLayoutPanel(
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true,
            Margin = Padding(0)
        )

        let mutable isDark = false
        let btnTheme = new Button(
            Text = "Toggle Dark Mode",
            Size = Size(140, 32),
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
            BackColor = Color.FromArgb(241, 245, 249),
            Cursor = Cursors.Hand
        )
        btnTheme.FlatAppearance.BorderSize <- 0
        btnTheme.Click.Add(fun _ ->
            isDark <- not isDark
            let theme = if isDark then LengkoDataTableTheme.Dark else LengkoDataTableTheme.Light
            LengkoDataTableThemeScope.ApplyToTree(form, theme)
            if isDark then
                form.BackColor <- Color.FromArgb(15, 23, 42)
                card.BackColor <- Color.FromArgb(30, 41, 59)
                title.ForeColor <- Color.White
            else
                form.BackColor <- Color.FromArgb(248, 250, 252)
                card.BackColor <- Color.White
                title.ForeColor <- Color.FromArgb(15, 23, 42))
        featureBar.Controls.Add(btnTheme)

        let btnExportCsv = new Button(
            Text = "Export to CSV",
            Size = Size(120, 32),
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
            BackColor = Color.FromArgb(241, 245, 249),
            Cursor = Cursors.Hand
        )
        btnExportCsv.FlatAppearance.BorderSize <- 0
        btnExportCsv.Click.Add(fun _ ->
            use sfd = new SaveFileDialog(Filter = "CSV files (*.csv)|*.csv", FileName = "Employees.csv")
            if sfd.ShowDialog() = DialogResult.OK then
                grid.ExportToCsv(sfd.FileName)
                MessageBox.Show("Export CSV berhasil!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore)
        featureBar.Controls.Add(btnExportCsv)

        root.Controls.Add(featureBar, 0, 1)

        // Top Controls
        let topBar = new Panel(Dock = DockStyle.Top, Height = 44, Padding = Padding(0, 0, 0, 8))
        let lengthMenu = new LengkoDataTableLength(Grid = grid, Dock = DockStyle.Left, Width = 140)
        let buttons = new LengkoDataTableButtons(Grid = grid, Dock = DockStyle.Left, ShowColVis = true, ShowResetFilter = true)
        let search = new LengkoDataTableSearch(Grid = grid, Dock = DockStyle.Right, Width = 240, PlaceholderText = "Search employees...")
        topBar.Controls.Add(search)
        topBar.Controls.Add(buttons)
        topBar.Controls.Add(lengthMenu)

        // Bottom Controls
        let bottomBar = new Panel(Dock = DockStyle.Bottom, Height = 42, Padding = Padding(0, 6, 0, 0))
        let info = new LengkoDataTableInfo(Grid = grid, Dock = DockStyle.Left, AutoSize = true)
        let pagination = new LengkoDataTablePagination(Grid = grid, Dock = DockStyle.Right, AutoSize = true)
        bottomBar.Controls.Add(pagination)
        bottomBar.Controls.Add(info)

        // Bind data
        let dt = createSampleTable ()
        grid.DataSource <- dt

        card.Controls.Add(grid)
        card.Controls.Add(topBar)
        card.Controls.Add(bottomBar)
        root.Controls.Add(card, 0, 3)

        form.Controls.Add(root)
        Application.Run(form)
        0
