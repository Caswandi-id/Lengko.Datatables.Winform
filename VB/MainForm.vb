Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Lengko.Datatables.WinForms

Namespace Lengko.Datatables.Demo.VB

    Public Class MainForm
        Inherits Form

        Private _grid As LengkoDataTableGrid
        Private _filterChips As LengkoDataTableFilterChips
        Private _isDark As Boolean = False
        Private _savedState As String = Nothing

        Public Sub New()
            InitializeUi()
        End Sub

        Private Sub InitializeUi()
            Text = "Lengko.Datatables.WinForms — VB.NET Demo"
            Width = 1060
            Height = 720
            StartPosition = FormStartPosition.CenterScreen
            Font = New Font("Segoe UI", 9.5F)
            BackColor = Color.FromArgb(248, 250, 252)

            Dim root As New TableLayoutPanel With {
                .Dock = DockStyle.Fill,
                .ColumnCount = 1,
                .RowCount = 4,
                .Padding = New Padding(16)
            }
            root.RowStyles.Add(New RowStyle(SizeType.Absolute, 60))
            root.RowStyles.Add(New RowStyle(SizeType.Absolute, 42))
            root.RowStyles.Add(New RowStyle(SizeType.Absolute, 36))
            root.RowStyles.Add(New RowStyle(SizeType.Percent, 100))

            ' 1. Header
            Dim header As New Panel With {.Dock = DockStyle.Fill}
            Dim title As New Label With {
                .Text = "Lengko.Datatables Demo (VB.NET WinForms)",
                .Font = New Font("Segoe UI", 15.0F, FontStyle.Bold),
                .ForeColor = Color.FromArgb(15, 23, 42),
                .AutoSize = True,
                .Location = New Point(0, 2)
            }
            Dim subTitle As New Label With {
                .Text = "Inspired by DataTables.net. Zero external dependencies. Instant search, pagination, export, filters & summaries.",
                .Font = New Font("Segoe UI", 9.5F),
                .ForeColor = Color.FromArgb(100, 116, 139),
                .AutoSize = True,
                .Location = New Point(0, 30)
            }
            header.Controls.Add(title)
            header.Controls.Add(subTitle)
            root.Controls.Add(header, 0, 0)

            ' 2. Feature Bar
            Dim featureBar As New FlowLayoutPanel With {
                .Dock = DockStyle.Fill,
                .FlowDirection = FlowDirection.LeftToRight,
                .WrapContents = True,
                .Margin = New Padding(0)
            }

            Dim btnTheme = CreateSmallButton("Theme: Light", Color.FromArgb(71, 85, 105))
            Dim btnFilterRow = CreateSmallButton("Filter Row: On", Color.FromArgb(59, 130, 246))
            Dim btnMultiSelect = CreateSmallButton("Multi-Select: Off", Color.FromArgb(16, 185, 129))
            Dim btnSummary = CreateSmallButton("Summary: On", Color.FromArgb(139, 92, 246))
            Dim btnRowGroup = CreateSmallButton("Group by Office: Off", Color.FromArgb(245, 158, 11))
            Dim btnChildRows = CreateSmallButton("Child Rows: Off", Color.FromArgb(5, 150, 105))
            Dim btnConditional = CreateSmallButton("Format (Age ≥ 60): Off", Color.FromArgb(220, 38, 38))
            Dim btnSaveState = CreateSmallButton("Save State", Color.FromArgb(14, 165, 233))
            Dim btnRestoreState = CreateSmallButton("Restore State", Color.FromArgb(14, 165, 233))
            Dim btnReset = CreateSmallButton("Reset Filters", Color.FromArgb(100, 116, 139))

            featureBar.Controls.AddRange({btnTheme, btnFilterRow, btnMultiSelect, btnSummary, btnRowGroup, btnChildRows, btnConditional, btnSaveState, btnRestoreState, btnReset})
            root.Controls.Add(featureBar, 0, 1)

            ' 3. Filter Chips Bar
            Dim chipsBar As New FlowLayoutPanel With {
                .Dock = DockStyle.Fill,
                .FlowDirection = FlowDirection.LeftToRight,
                .Margin = New Padding(0)
            }
            Dim chipsLabel As New Label With {
                .Text = "Office Quick Filter:",
                .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
                .ForeColor = Color.FromArgb(51, 65, 85),
                .Margin = New Padding(0, 5, 8, 0),
                .AutoSize = True
            }
            _filterChips = New LengkoDataTableFilterChips With {
                .ColumnName = "Office",
                .ShowCounts = True,
                .Margin = New Padding(0, 2, 0, 0)
            }
            chipsBar.Controls.Add(chipsLabel)
            chipsBar.Controls.Add(_filterChips)
            root.Controls.Add(chipsBar, 0, 2)

            ' 4. Main Table Card
            Dim tableCard As New Panel With {
                .Dock = DockStyle.Fill,
                .BackColor = Color.White,
                .Padding = New Padding(12)
            }
            AddHandler tableCard.Paint, Sub(s, e)
                Using p As New Pen(Color.FromArgb(226, 232, 240), 1.0F)
                    e.Graphics.DrawRectangle(p, 0, 0, tableCard.Width - 1, tableCard.Height - 1)
                End Using
            End Sub

            ' Grid
            _grid = New LengkoDataTableGrid With {
                .Dock = DockStyle.Fill,
                .PageLength = 10,
                .ZebraStriping = True,
                .FilterRowEnabled = True,
                .ShowFooterSummary = True
            }
            _grid.ChildRowHeight = 55
            _grid.ChildRowTextFactory = Function(row)
                                            If TypeOf row Is DataRow Then
                                                Dim dr = DirectCast(row, DataRow)
                                                Return String.Format("👤 {0} — {1} ({2}) | Compensation: {3} | Joined: {4}", dr("Name"), dr("Position"), dr("Office"), dr("Salary"), dr("Start Date"))
                                            End If
                                            If row IsNot Nothing Then
                                                Return row.ToString()
                                            End If
                                            Return String.Empty
                                        End Function
            _filterChips.Grid = _grid

            ' Top Toolbar
            Dim topBar As New Panel With {
                .Dock = DockStyle.Top,
                .Height = 44,
                .Padding = New Padding(0, 0, 0, 8)
            }
            Dim length As New LengkoDataTableLength With {
                .Grid = _grid,
                .Dock = DockStyle.Left,
                .Width = 140
            }
            Dim buttons As New LengkoDataTableButtons With {
                .Grid = _grid,
                .Dock = DockStyle.Left,
                .ShowColVis = True,
                .ShowResetFilter = True,
                .Margin = New Padding(8, 0, 0, 0)
            }
            Dim search As New LengkoDataTableSearch With {
                .Grid = _grid,
                .Dock = DockStyle.Right,
                .Width = 240,
                .PlaceholderText = "Search employees..."
            }

            topBar.Controls.Add(search)
            topBar.Controls.Add(buttons)
            topBar.Controls.Add(length)

            ' Bottom Footer
            Dim bottomBar As New Panel With {
                .Dock = DockStyle.Bottom,
                .Height = 42,
                .Padding = New Padding(0, 6, 0, 0)
            }
            Dim info As New LengkoDataTableInfo With {
                .Grid = _grid,
                .Dock = DockStyle.Left,
                .Width = 300
            }
            Dim pager As New LengkoDataTablePagination With {
                .Grid = _grid,
                .Dock = DockStyle.Right,
                .Width = 360
            }

            bottomBar.Controls.Add(pager)
            bottomBar.Controls.Add(info)

            tableCard.Controls.Add(_grid)
            tableCard.Controls.Add(topBar)
            tableCard.Controls.Add(bottomBar)
            root.Controls.Add(tableCard, 0, 3)

            Controls.Add(root)

            ' Load Sample Data
            LoadData()

            ' Summary config
            _grid.SetColumnSummary("Salary", DataTableSummaryType.Count)
            _grid.SetColumnSummary("Age", DataTableSummaryType.Average)

            ' Event Handlers
            AddHandler btnTheme.Click, Sub(s, e)
                _isDark = Not _isDark
                btnTheme.Text = If(_isDark, "Theme: Dark", "Theme: Light")
                Dim theme = If(_isDark, LengkoDataTableTheme.Dark, LengkoDataTableTheme.Light)
                LengkoDataTableThemeScope.ApplyToTree(Me, theme)
                BackColor = If(_isDark, Color.FromArgb(15, 23, 42), Color.FromArgb(248, 250, 252))
                tableCard.BackColor = If(_isDark, Color.FromArgb(30, 41, 59), Color.White)
                title.ForeColor = If(_isDark, Color.White, Color.FromArgb(15, 23, 42))
                subTitle.ForeColor = If(_isDark, Color.FromArgb(148, 163, 184), Color.FromArgb(100, 116, 139))
                chipsLabel.ForeColor = If(_isDark, Color.FromArgb(226, 232, 240), Color.FromArgb(51, 65, 85))
            End Sub

            AddHandler btnFilterRow.Click, Sub(s, e)
                _grid.FilterRowEnabled = Not _grid.FilterRowEnabled
                btnFilterRow.Text = If(_grid.FilterRowEnabled, "Filter Row: On", "Filter Row: Off")
            End Sub

            AddHandler btnMultiSelect.Click, Sub(s, e)
                _grid.MultiSelectCheckbox = Not _grid.MultiSelectCheckbox
                btnMultiSelect.Text = If(_grid.MultiSelectCheckbox, "Multi-Select: On", "Multi-Select: Off")
            End Sub

            AddHandler btnSummary.Click, Sub(s, e)
                _grid.ShowFooterSummary = Not _grid.ShowFooterSummary
                btnSummary.Text = If(_grid.ShowFooterSummary, "Summary: On", "Summary: Off")
            End Sub

            AddHandler btnRowGroup.Click, Sub(s, e)
                _grid.RowGroupEnabled = Not _grid.RowGroupEnabled
                _grid.RowGroupColumnName = If(_grid.RowGroupEnabled, "Office", "")
                btnRowGroup.Text = If(_grid.RowGroupEnabled, "Group by Office: On", "Group by Office: Off")
            End Sub

            AddHandler btnChildRows.Click, Sub(s, e)
                _grid.ChildRowsEnabled = Not _grid.ChildRowsEnabled
                btnChildRows.Text = If(_grid.ChildRowsEnabled, "Child Rows: On", "Child Rows: Off")
            End Sub

            Dim isConditional As Boolean = False
            AddHandler btnConditional.Click, Sub(s, e)
                isConditional = Not isConditional
                btnConditional.Text = If(isConditional, "Format (Age ≥ 60): On", "Format (Age ≥ 60): Off")
                _grid.ClearConditionalFormats()
                If isConditional Then
                    _grid.AddConditionalFormat(
                        "Age",
                        Function(val) TypeOf val Is Integer AndAlso CInt(val) >= 60,
                        Color.FromArgb(254, 226, 226),
                        Color.FromArgb(185, 28, 28),
                        FontStyle.Bold)
                End If
                _grid.RefreshData()
            End Sub

            AddHandler btnSaveState.Click, Sub(s, e)
                _savedState = _grid.SaveStateJson()
                MessageBox.Show("Grid state saved (page, sort, search, filters)!", "State Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Sub

            AddHandler btnRestoreState.Click, Sub(s, e)
                If String.IsNullOrEmpty(_savedState) Then
                    MessageBox.Show("Belum ada state yang disimpan. Klik 'Save State' terlebih dahulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
                _grid.RestoreStateJson(_savedState)
                MessageBox.Show("Grid state berhasil dipulihkan!", "State Restored", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Sub

            AddHandler btnReset.Click, Sub(s, e)
                _grid.ResetAllFilters()
                search.SearchText = String.Empty
                _filterChips.SelectChip(Nothing)
            End Sub
        End Sub

        Private Sub LoadData()
            Dim dt As New DataTable()
            dt.Columns.Add("Name", GetType(String))
            dt.Columns.Add("Position", GetType(String))
            dt.Columns.Add("Office", GetType(String))
            dt.Columns.Add("Age", GetType(Integer))
            dt.Columns.Add("Start Date", GetType(String))
            dt.Columns.Add("Salary", GetType(String))

            dt.Rows.Add("Tiger Nixon", "System Architect", "Edinburgh", 61, "2011-04-25", "$320,800")
            dt.Rows.Add("Garrett Winters", "Accountant", "Tokyo", 63, "2011-07-25", "$170,750")
            dt.Rows.Add("Ashton Cox", "Junior Technical Author", "San Francisco", 66, "2009-01-12", "$86,000")
            dt.Rows.Add("Cedric Kelly", "Senior Javascript Developer", "Edinburgh", 22, "2012-03-29", "$433,060")
            dt.Rows.Add("Airi Satou", "Accountant", "Tokyo", 33, "2008-11-28", "$162,700")
            dt.Rows.Add("Brielle Williamson", "Integration Specialist", "New York", 61, "2012-12-02", "$372,000")
            dt.Rows.Add("Herrod Chandler", "Sales Assistant", "San Francisco", 59, "2012-08-06", "$137,500")
            dt.Rows.Add("Rhona Davidson", "Integration Specialist", "Tokyo", 55, "2010-10-14", "$327,900")
            dt.Rows.Add("Colleen Hurst", "Javascript Developer", "San Francisco", 39, "2009-09-15", "$205,500")
            dt.Rows.Add("Sonya Frost", "Software Engineer", "Edinburgh", 23, "2008-12-13", "$103,600")
            dt.Rows.Add("Jena Gaines", "Office Manager", "London", 30, "2008-12-19", "$90,560")
            dt.Rows.Add("Quinn Flynn", "Support Lead", "Edinburgh", 22, "2013-03-03", "$342,000")
            dt.Rows.Add("Charde Marshall", "Regional Director", "San Francisco", 36, "2008-10-16", "$470,600")
            dt.Rows.Add("Haley Kennedy", "Senior Marketing Designer", "London", 43, "2012-12-18", "$313,500")
            dt.Rows.Add("Tatyana Fitzpatrick", "Regional Director", "London", 19, "2010-03-17", "$385,750")
            dt.Rows.Add("Michael Silva", "Marketing Designer", "London", 66, "2012-11-27", "$198,500")
            dt.Rows.Add("Paul Byrd", "Chief Financial Officer (CFO)", "New York", 64, "2010-06-09", "$725,000")
            dt.Rows.Add("Gloria Little", "Systems Administrator", "New York", 59, "2009-04-10", "$237,500")
            dt.Rows.Add("Bradley Greer", "Software Engineer", "London", 41, "2012-10-13", "$132,000")
            dt.Rows.Add("Dai Rios", "Personnel Lead", "Edinburgh", 35, "2012-09-26", "$217,500")
            dt.Rows.Add("Jenette Caldwell", "Development Lead", "New York", 30, "2011-09-03", "$345,000")
            dt.Rows.Add("Yuri Berry", "Chief Marketing Officer (CMO)", "New York", 40, "2009-06-25", "$675,000")
            dt.Rows.Add("Caesar Vance", "Pre-Sales Support", "New York", 21, "2011-12-12", "$106,450")
            dt.Rows.Add("Doris Wilder", "Sales Assistant", "Sydney", 23, "2010-09-20", "$85,600")
            dt.Rows.Add("Angelica Ramos", "Chief Executive Officer (CEO)", "London", 47, "2009-10-09", "$1,200,000")
            dt.Rows.Add("Gavin Joyce", "Developer", "Edinburgh", 42, "2010-12-22", "$92,575")
            dt.Rows.Add("Jennifer Chang", "Regional Director", "Singapore", 28, "2010-11-14", "$357,650")
            dt.Rows.Add("Brenden Wagner", "Software Engineer", "San Francisco", 28, "2011-06-07", "$206,850")
            dt.Rows.Add("Fiona Green", "Chief Operating Officer (COO)", "San Francisco", 48, "2010-03-11", "$850,000")
            dt.Rows.Add("Shou Itou", "Regional Marketing", "Tokyo", 20, "2011-08-14", "$163,000")
            dt.Rows.Add("Michelle House", "Integration Specialist", "Sydney", 37, "2011-06-02", "$95,400")
            dt.Rows.Add("Suki Burks", "Developer", "London", 53, "2009-10-22", "$114,500")
            dt.Rows.Add("Prescott Bartlett", "Technical Author", "London", 27, "2011-05-07", "$145,000")
            dt.Rows.Add("Gavin Cortez", "Team Leader", "San Francisco", 22, "2008-10-26", "$235,500")
            dt.Rows.Add("Martena Mccray", "Post-Sales support", "Edinburgh", 46, "2011-03-09", "$324,050")
            dt.Rows.Add("Unity Butler", "Marketing Designer", "San Francisco", 47, "2009-12-09", "$85,675")
            dt.Rows.Add("Howard Hatfield", "Office Manager", "San Francisco", 51, "2008-12-16", "$164,500")
            dt.Rows.Add("Hope Fuentes", "Secretary", "San Francisco", 41, "2010-02-12", "$109,850")
            dt.Rows.Add("Vivian Harrell", "Financial Controller", "San Francisco", 62, "2009-02-14", "$452,500")
            dt.Rows.Add("Timothy Mooney", "Office Manager", "London", 37, "2013-12-11", "$136,200")
            dt.Rows.Add("Jackson Bradshaw", "Director", "New York", 65, "2008-09-26", "$645,750")
            dt.Rows.Add("Olivia Liang", "Support Engineer", "Singapore", 64, "2011-02-03", "$234,500")
            dt.Rows.Add("Bruno Nash", "Software Engineer", "London", 38, "2011-05-03", "$163,500")
            dt.Rows.Add("Sakura Yamamoto", "Support Engineer", "Tokyo", 37, "2009-08-19", "$139,575")
            dt.Rows.Add("Thor Walton", "Developer", "New York", 61, "2013-08-11", "$98,540")
            dt.Rows.Add("Finn Camacho", "Support Engineer", "San Francisco", 47, "2009-07-07", "$87,500")
            dt.Rows.Add("Serge Baldwin", "Data Coordinator", "Singapore", 64, "2012-04-09", "$138,575")
            dt.Rows.Add("Zenaida Frank", "Software Engineer", "New York", 63, "2010-01-04", "$125,250")
            dt.Rows.Add("Zorita Serrano", "Software Engineer", "San Francisco", 56, "2012-06-01", "$115,000")
            dt.Rows.Add("Jennifer Acosta", "Junior Javascript Developer", "Edinburgh", 43, "2013-02-01", "$75,650")
            dt.Rows.Add("Cara Stevens", "Sales Assistant", "New York", 46, "2011-12-06", "$145,600")
            dt.Rows.Add("Hermione Butler", "Regional Director", "London", 47, "2011-03-21", "$356,250")
            dt.Rows.Add("Lael Greer", "Systems Administrator", "London", 21, "2009-02-27", "$103,500")
            dt.Rows.Add("Jonas Alexander", "Developer", "San Francisco", 30, "2010-07-14", "$86,500")
            dt.Rows.Add("Shad Decker", "Regional Director", "Edinburgh", 51, "2008-11-13", "$183,000")
            dt.Rows.Add("Michael Bruce", "Javascript Developer", "Singapore", 29, "2011-06-27", "$183,000")
            dt.Rows.Add("Donna Snider", "Customer Support", "New York", 27, "2011-01-25", "$112,000")

            _grid.DataSource = dt
            _filterChips.AutoPopulate()
        End Sub

        Private Function CreateSmallButton(text As String, backColor As Color) As Button
            Dim btn As New Button With {
                .Text = text,
                .Height = 28,
                .AutoSize = True,
                .Padding = New Padding(8, 0, 8, 0),
                .Margin = New Padding(0, 0, 6, 6),
                .BackColor = backColor,
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat,
                .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold),
                .Cursor = Cursors.Hand
            }
            btn.FlatAppearance.BorderSize = 0
            Return btn
        End Function

    End Class

End Namespace
