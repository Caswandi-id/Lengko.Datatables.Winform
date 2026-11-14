using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Lengko.Datatables.WinForms;

namespace Lengko.Datatables.Demo.CSharp;

public class MainForm : Form
{
    private LengkoDataTableGrid _grid = null!;
    private LengkoDataTableFilterChips _filterChips = null!;
    private bool _isDark = false;
    private string? _savedState = null;

    public MainForm()
    {
        InitializeUi();
    }

    private void InitializeUi()
    {
        Text = "Lengko.Datatables.WinForms — C# Demo";
        Width = 1060;
        Height = 720;
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 9.5f);
        BackColor = Color.FromArgb(248, 250, 252);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(16)
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 60)); // Header
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 42)); // Feature Bar
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 36)); // Filter Chips Bar
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Main Table Card

        // 1. Header
        var header = new Panel { Dock = DockStyle.Fill };
        var title = new Label
        {
            Text = "Lengko.Datatables Demo (C# WinForms)",
            Font = new Font("Segoe UI", 15f, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42),
            AutoSize = true,
            Location = new Point(0, 2)
        };
        var subTitle = new Label
        {
            Text = "Inspired by DataTables.net. Zero external dependencies. Instant search, pagination, export, filters & summaries.",
            Font = new Font("Segoe UI", 9.5f),
            ForeColor = Color.FromArgb(100, 116, 139),
            AutoSize = true,
            Location = new Point(0, 30)
        };
        header.Controls.Add(title);
        header.Controls.Add(subTitle);
        root.Controls.Add(header, 0, 0);

        // 2. Feature Bar (Toggles)
        var featureBar = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true,
            Margin = new Padding(0)
        };

        var btnTheme = CreateSmallButton("Theme: Light", Color.FromArgb(71, 85, 105));
        var btnFilterRow = CreateSmallButton("Filter Row: On", Color.FromArgb(59, 130, 246));
        var btnMultiSelect = CreateSmallButton("Multi-Select: Off", Color.FromArgb(16, 185, 129));
        var btnSummary = CreateSmallButton("Summary: On", Color.FromArgb(139, 92, 246));
        var btnRowGroup = CreateSmallButton("Group by Office: Off", Color.FromArgb(245, 158, 11));
        var btnChildRows = CreateSmallButton("Child Rows: Off", Color.FromArgb(5, 150, 105));
        var btnConditional = CreateSmallButton("Format (Age ≥ 60): Off", Color.FromArgb(220, 38, 38));
        var btnSaveState = CreateSmallButton("Save State", Color.FromArgb(14, 165, 233));
        var btnRestoreState = CreateSmallButton("Restore State", Color.FromArgb(14, 165, 233));
        var btnReset = CreateSmallButton("Reset Filters", Color.FromArgb(100, 116, 139));

        featureBar.Controls.AddRange(new Control[]
        {
            btnTheme, btnFilterRow, btnMultiSelect, btnSummary, btnRowGroup, btnChildRows, btnConditional, btnSaveState, btnRestoreState, btnReset
        });
        root.Controls.Add(featureBar, 0, 1);

        // 3. Filter Chips Bar
        var chipsBar = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Margin = new Padding(0)
        };
        var chipsLabel = new Label
        {
            Text = "Office Quick Filter:",
            Font = new Font("Segoe UI", 9f, FontStyle.Bold),
            ForeColor = Color.FromArgb(51, 65, 85),
            Margin = new Padding(0, 5, 8, 0),
            AutoSize = true
        };
        _filterChips = new LengkoDataTableFilterChips
        {
            ColumnName = "Office",
            ShowCounts = true,
            Margin = new Padding(0, 2, 0, 0)
        };
        chipsBar.Controls.Add(chipsLabel);
        chipsBar.Controls.Add(_filterChips);
        root.Controls.Add(chipsBar, 0, 2);

        // 4. Main Table Card
        var tableCard = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(12)
        };
        tableCard.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(226, 232, 240), 1f);
            e.Graphics.DrawRectangle(pen, 0, 0, tableCard.Width - 1, tableCard.Height - 1);
        };

        // Grid
        _grid = new LengkoDataTableGrid
        {
            Dock = DockStyle.Fill,
            PageLength = 10,
            ZebraStriping = true,
            FilterRowEnabled = true,
            ShowFooterSummary = true
        };
        _grid.ChildRowHeight = 55;
        _grid.ChildRowTextFactory = row =>
        {
            if (row is DataRow dr)
            {
                return $"👤 {dr["Name"]} — {dr["Position"]} ({dr["Office"]}) | Compensation: {dr["Salary"]} | Joined: {dr["Start Date"]}";
            }
            return row?.ToString() ?? string.Empty;
        };
        _filterChips.Grid = _grid;

        // Top Toolbar
        var topBar = new Panel
        {
            Dock = DockStyle.Top,
            Height = 44,
            Padding = new Padding(0, 0, 0, 8)
        };
        var length = new LengkoDataTableLength
        {
            Grid = _grid,
            Dock = DockStyle.Left,
            Width = 140
        };
        var buttons = new LengkoDataTableButtons
        {
            Grid = _grid,
            Dock = DockStyle.Left,
            ShowColVis = true,
            ShowResetFilter = true,
            Margin = new Padding(8, 0, 0, 0)
        };
        var search = new LengkoDataTableSearch
        {
            Grid = _grid,
            Dock = DockStyle.Right,
            Width = 240,
            PlaceholderText = "Search employees..."
        };

        topBar.Controls.Add(search);
        topBar.Controls.Add(buttons);
        topBar.Controls.Add(length);

        // Bottom Footer
        var bottomBar = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 42,
            Padding = new Padding(0, 6, 0, 0)
        };
        var info = new LengkoDataTableInfo
        {
            Grid = _grid,
            Dock = DockStyle.Left,
            Width = 300
        };
        var pager = new LengkoDataTablePagination
        {
            Grid = _grid,
            Dock = DockStyle.Right,
            Width = 360
        };

        bottomBar.Controls.Add(pager);
        bottomBar.Controls.Add(info);

        tableCard.Controls.Add(_grid);
        tableCard.Controls.Add(topBar);
        tableCard.Controls.Add(bottomBar);
        root.Controls.Add(tableCard, 0, 3);

        Controls.Add(root);

        // Load Sample Data
        LoadData();

        // Summary config
        _grid.SetColumnSummary("Salary", DataTableSummaryType.Count);
        _grid.SetColumnSummary("Age", DataTableSummaryType.Average);

        // Feature Wireup
        btnTheme.Click += (s, e) =>
        {
            _isDark = !_isDark;
            btnTheme.Text = _isDark ? "Theme: Dark" : "Theme: Light";
            var theme = _isDark ? LengkoDataTableTheme.Dark : LengkoDataTableTheme.Light;
            LengkoDataTableThemeScope.ApplyToTree(this, theme);
            BackColor = _isDark ? Color.FromArgb(15, 23, 42) : Color.FromArgb(248, 250, 252);
            tableCard.BackColor = _isDark ? Color.FromArgb(30, 41, 59) : Color.White;
            title.ForeColor = _isDark ? Color.White : Color.FromArgb(15, 23, 42);
            subTitle.ForeColor = _isDark ? Color.FromArgb(148, 163, 184) : Color.FromArgb(100, 116, 139);
            chipsLabel.ForeColor = _isDark ? Color.FromArgb(226, 232, 240) : Color.FromArgb(51, 65, 85);
        };

        btnFilterRow.Click += (s, e) =>
        {
            _grid.FilterRowEnabled = !_grid.FilterRowEnabled;
            btnFilterRow.Text = _grid.FilterRowEnabled ? "Filter Row: On" : "Filter Row: Off";
        };

        btnMultiSelect.Click += (s, e) =>
        {
            _grid.MultiSelectCheckbox = !_grid.MultiSelectCheckbox;
            btnMultiSelect.Text = _grid.MultiSelectCheckbox ? "Multi-Select: On" : "Multi-Select: Off";
        };

        btnSummary.Click += (s, e) =>
        {
            _grid.ShowFooterSummary = !_grid.ShowFooterSummary;
            btnSummary.Text = _grid.ShowFooterSummary ? "Summary: On" : "Summary: Off";
        };

        btnRowGroup.Click += (s, e) =>
        {
            _grid.RowGroupEnabled = !_grid.RowGroupEnabled;
            _grid.RowGroupColumnName = _grid.RowGroupEnabled ? "Office" : "";
            btnRowGroup.Text = _grid.RowGroupEnabled ? "Group by Office: On" : "Group by Office: Off";
        };

        btnChildRows.Click += (s, e) =>
        {
            _grid.ChildRowsEnabled = !_grid.ChildRowsEnabled;
            btnChildRows.Text = _grid.ChildRowsEnabled ? "Child Rows: On" : "Child Rows: Off";
        };

        bool isConditional = false;
        btnConditional.Click += (s, e) =>
        {
            isConditional = !isConditional;
            btnConditional.Text = isConditional ? "Format (Age ≥ 60): On" : "Format (Age ≥ 60): Off";
            _grid.ClearConditionalFormats();
            if (isConditional)
            {
                _grid.AddConditionalFormat(
                    "Age",
                    val => val is int age && age >= 60,
                    backColor: Color.FromArgb(254, 226, 226),
                    foreColor: Color.FromArgb(185, 28, 28),
                    fontStyle: FontStyle.Bold);
            }
            _grid.RefreshData();
        };

        btnSaveState.Click += (s, e) =>
        {
            _savedState = _grid.SaveStateJson();
            MessageBox.Show("Grid state saved (page, sort, search, filters)!", "State Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        };

        btnRestoreState.Click += (s, e) =>
        {
            if (string.IsNullOrEmpty(_savedState))
            {
                MessageBox.Show("Belum ada state yang disimpan. Klik 'Save State' terlebih dahulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _grid.RestoreStateJson(_savedState);
            MessageBox.Show("Grid state berhasil dipulihkan!", "State Restored", MessageBoxButtons.OK, MessageBoxIcon.Information);
        };

        btnReset.Click += (s, e) =>
        {
            _grid.ResetAllFilters();
            search.SearchText = string.Empty;
            _filterChips.SelectChip(null);
        };
    }

    private void LoadData()
    {
        var dt = new DataTable();
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("Position", typeof(string));
        dt.Columns.Add("Office", typeof(string));
        dt.Columns.Add("Age", typeof(int));
        dt.Columns.Add("Start Date", typeof(string));
        dt.Columns.Add("Salary", typeof(string));

        dt.Rows.Add("Tiger Nixon", "System Architect", "Edinburgh", 61, "2011-04-25", "$320,800");
        dt.Rows.Add("Garrett Winters", "Accountant", "Tokyo", 63, "2011-07-25", "$170,750");
        dt.Rows.Add("Ashton Cox", "Junior Technical Author", "San Francisco", 66, "2009-01-12", "$86,000");
        dt.Rows.Add("Cedric Kelly", "Senior Javascript Developer", "Edinburgh", 22, "2012-03-29", "$433,060");
        dt.Rows.Add("Airi Satou", "Accountant", "Tokyo", 33, "2008-11-28", "$162,700");
        dt.Rows.Add("Brielle Williamson", "Integration Specialist", "New York", 61, "2012-12-02", "$372,000");
        dt.Rows.Add("Herrod Chandler", "Sales Assistant", "San Francisco", 59, "2012-08-06", "$137,500");
        dt.Rows.Add("Rhona Davidson", "Integration Specialist", "Tokyo", 55, "2010-10-14", "$327,900");
        dt.Rows.Add("Colleen Hurst", "Javascript Developer", "San Francisco", 39, "2009-09-15", "$205,500");
        dt.Rows.Add("Sonya Frost", "Software Engineer", "Edinburgh", 23, "2008-12-13", "$103,600");
        dt.Rows.Add("Jena Gaines", "Office Manager", "London", 30, "2008-12-19", "$90,560");
        dt.Rows.Add("Quinn Flynn", "Support Lead", "Edinburgh", 22, "2013-03-03", "$342,000");
        dt.Rows.Add("Charde Marshall", "Regional Director", "San Francisco", 36, "2008-10-16", "$470,600");
        dt.Rows.Add("Haley Kennedy", "Senior Marketing Designer", "London", 43, "2012-12-18", "$313,500");
        dt.Rows.Add("Tatyana Fitzpatrick", "Regional Director", "London", 19, "2010-03-17", "$385,750");
        dt.Rows.Add("Michael Silva", "Marketing Designer", "London", 66, "2012-11-27", "$198,500");
        dt.Rows.Add("Paul Byrd", "Chief Financial Officer (CFO)", "New York", 64, "2010-06-09", "$725,000");
        dt.Rows.Add("Gloria Little", "Systems Administrator", "New York", 59, "2009-04-10", "$237,500");
        dt.Rows.Add("Bradley Greer", "Software Engineer", "London", 41, "2012-10-13", "$132,000");
        dt.Rows.Add("Dai Rios", "Personnel Lead", "Edinburgh", 35, "2012-09-26", "$217,500");
        dt.Rows.Add("Jenette Caldwell", "Development Lead", "New York", 30, "2011-09-03", "$345,000");
        dt.Rows.Add("Yuri Berry", "Chief Marketing Officer (CMO)", "New York", 40, "2009-06-25", "$675,000");
        dt.Rows.Add("Caesar Vance", "Pre-Sales Support", "New York", 21, "2011-12-12", "$106,450");
        dt.Rows.Add("Doris Wilder", "Sales Assistant", "Sydney", 23, "2010-09-20", "$85,600");
        dt.Rows.Add("Angelica Ramos", "Chief Executive Officer (CEO)", "London", 47, "2009-10-09", "$1,200,000");
        dt.Rows.Add("Gavin Joyce", "Developer", "Edinburgh", 42, "2010-12-22", "$92,575");
        dt.Rows.Add("Jennifer Chang", "Regional Director", "Singapore", 28, "2010-11-14", "$357,650");
        dt.Rows.Add("Brenden Wagner", "Software Engineer", "San Francisco", 28, "2011-06-07", "$206,850");
        dt.Rows.Add("Fiona Green", "Chief Operating Officer (COO)", "San Francisco", 48, "2010-03-11", "$850,000");
        dt.Rows.Add("Shou Itou", "Regional Marketing", "Tokyo", 20, "2011-08-14", "$163,000");
        dt.Rows.Add("Michelle House", "Integration Specialist", "Sydney", 37, "2011-06-02", "$95,400");
        dt.Rows.Add("Suki Burks", "Developer", "London", 53, "2009-10-22", "$114,500");
        dt.Rows.Add("Prescott Bartlett", "Technical Author", "London", 27, "2011-05-07", "$145,000");
        dt.Rows.Add("Gavin Cortez", "Team Leader", "San Francisco", 22, "2008-10-26", "$235,500");
        dt.Rows.Add("Martena Mccray", "Post-Sales support", "Edinburgh", 46, "2011-03-09", "$324,050");
        dt.Rows.Add("Unity Butler", "Marketing Designer", "San Francisco", 47, "2009-12-09", "$85,675");
        dt.Rows.Add("Howard Hatfield", "Office Manager", "San Francisco", 51, "2008-12-16", "$164,500");
        dt.Rows.Add("Hope Fuentes", "Secretary", "San Francisco", 41, "2010-02-12", "$109,850");
        dt.Rows.Add("Vivian Harrell", "Financial Controller", "San Francisco", 62, "2009-02-14", "$452,500");
        dt.Rows.Add("Timothy Mooney", "Office Manager", "London", 37, "2013-12-11", "$136,200");
        dt.Rows.Add("Jackson Bradshaw", "Director", "New York", 65, "2008-09-26", "$645,750");
        dt.Rows.Add("Olivia Liang", "Support Engineer", "Singapore", 64, "2011-02-03", "$234,500");
        dt.Rows.Add("Bruno Nash", "Software Engineer", "London", 38, "2011-05-03", "$163,500");
        dt.Rows.Add("Sakura Yamamoto", "Support Engineer", "Tokyo", 37, "2009-08-19", "$139,575");
        dt.Rows.Add("Thor Walton", "Developer", "New York", 61, "2013-08-11", "$98,540");
        dt.Rows.Add("Finn Camacho", "Support Engineer", "San Francisco", 47, "2009-07-07", "$87,500");
        dt.Rows.Add("Serge Baldwin", "Data Coordinator", "Singapore", 64, "2012-04-09", "$138,575");
        dt.Rows.Add("Zenaida Frank", "Software Engineer", "New York", 63, "2010-01-04", "$125,250");
        dt.Rows.Add("Zorita Serrano", "Software Engineer", "San Francisco", 56, "2012-06-01", "$115,000");
        dt.Rows.Add("Jennifer Acosta", "Junior Javascript Developer", "Edinburgh", 43, "2013-02-01", "$75,650");
        dt.Rows.Add("Cara Stevens", "Sales Assistant", "New York", 46, "2011-12-06", "$145,600");
        dt.Rows.Add("Hermione Butler", "Regional Director", "London", 47, "2011-03-21", "$356,250");
        dt.Rows.Add("Lael Greer", "Systems Administrator", "London", 21, "2009-02-27", "$103,500");
        dt.Rows.Add("Jonas Alexander", "Developer", "San Francisco", 30, "2010-07-14", "$86,500");
        dt.Rows.Add("Shad Decker", "Regional Director", "Edinburgh", 51, "2008-11-13", "$183,000");
        dt.Rows.Add("Michael Bruce", "Javascript Developer", "Singapore", 29, "2011-06-27", "$183,000");
        dt.Rows.Add("Donna Snider", "Customer Support", "New York", 27, "2011-01-25", "$112,000");

        _grid.DataSource = dt;
        _filterChips.AutoPopulate();
    }

    private Button CreateSmallButton(string text, Color backColor)
    {
        var btn = new Button
        {
            Text = text,
            Height = 28,
            AutoSize = true,
            Padding = new Padding(8, 0, 8, 0),
            Margin = new Padding(0, 0, 6, 6),
            BackColor = backColor,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btn.FlatAppearance.BorderSize = 0;
        return btn;
    }
}
