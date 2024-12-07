
Imports System.Windows.Forms.DataVisualization.Charting
Imports MySql.Data.MySqlClient


Public Class MainDashboardControl
    Dim medical As DataTable = GetTableData("medical")
    Dim pdl As DataTable = GetTableData("pdl")
    Dim crimes As DataTable = GetTableData("crimes")
    Dim visitor As DataTable = GetTableData("visitors")

    Private Sub MainDashboardControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        LoadInmateData()
        LoadDataToPieChart(pdl)


        ' Initialize the ComboBox with filter options
        ComboBox2.SelectedIndex = 0 ' Set default selection

        ' Additional initialization logic if needed
    End Sub
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub TableLayoutPanel1_Paint(sender As Object, e As PaintEventArgs) Handles TableLayoutPanel1.Paint

        Dim sql = "SELECT * FROM pdl"
        LoadToDGVForDisplay(sql, dgvCellblockInfo)
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs)
        Dim mainForm As MainDashboard = TryCast(Me.ParentForm, MainDashboard)

        If mainForm IsNot Nothing Then
            mainForm.SwitchToInmateHomeControl()
        End If
        Dim adminDashboard As AdminMainDashboard = TryCast(Me.ParentForm, AdminMainDashboard)

        If adminDashboard IsNot Nothing Then
            adminDashboard.SwitchToInmateHomeControl()
        End If
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs)


        Dim mainForm As MainDashboard = TryCast(Me.ParentForm, MainDashboard)

        If mainForm IsNot Nothing Then
            mainForm.SwitchToAddStaffControlHome()
        End If
        Dim adminDashboard As AdminMainDashboard = TryCast(Me.ParentForm, AdminMainDashboard)

        If adminDashboard IsNot Nothing Then
            adminDashboard.SwitchToAddStaffControlHome()
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        ' Get the selected filter from the combo box
        Dim selectedFilter As String = ComboBox2.SelectedItem.ToString()
        Dim orderBy As String = ""

        ' Determine the ORDER BY clause based on the selected filter
        Select Case selectedFilter
            Case "A-Z"
                orderBy = "ORDER BY first_name ASC"
            Case "Date (oldest)"
                orderBy = "ORDER BY date_of_birth ASC"
            Case "Date (newest)"
                orderBy = "ORDER BY date_of_birth DESC"
            Case "ID"
                orderBy = "ORDER BY pdl_id ASC"
        End Select

        ' Fetch the sorted data using the new function
        Dim dt As DataTable = GetTableOrder("pdl", orderBy)

        ' Bind the data to the DataGridView
        dgvCellblockInfo.DataSource = dt
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        ' Get the search input
        Dim searchText As String = TextBox1.Text.Trim()

        ' If the search box is empty, load the unordered data
        If String.IsNullOrWhiteSpace(searchText) Then
            LoadInmateData() ' Reload the full dataset
            Return
        End If

        ' Build the filter condition for the SQL query
        Dim filter As String = $"LOWER(first_name) LIKE '%{searchText.ToLower()}%' OR " &
                               $"LOWER(middle_name) LIKE '%{searchText.ToLower()}%' OR " &
                               $"LOWER(last_name) LIKE '%{searchText.ToLower()}%' OR " &
                               $"LOWER(CONCAT(first_name, ' ', last_name)) LIKE '%{searchText.ToLower()}%' OR " &
                               $"LOWER(CONCAT(last_name, ' ', first_name)) LIKE '%{searchText.ToLower()}%' OR " &
                               $"LOWER(CONCAT(first_name, ' ', middle_name, ' ', last_name)) LIKE '%{searchText.ToLower()}%'"

        ' Fetch the filtered data using the GetTableData method
        Dim dt As DataTable = GetTableData("pdl", filter)

        ' Bind the data to the DataGridView
        dgvCellblockInfo.DataSource = dt
    End Sub

    Public Sub LoadInmateData()
        Dim dt As DataTable = GetTableData("pdl")
        dgvCellblockInfo.DataSource = dt
    End Sub



    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        ' Get the selected text
        Dim selectedText As String = ComboBox1.SelectedItem.ToString()

        ' Handle different cases based on the selected text
        If selectedText = "sex population" Then
            ' Load sex population pie chart
            LoadSexDistributionToPieChart(pdl)
            pieChart.Titles.Clear()
            pieChart.Titles.Add("sex population") ' Set title for this chart
        ElseIf selectedText = "cell population" Then
            ' Load cell population pie chart
            LoadDataToPieChart(pdl)
            pieChart.Titles.Clear()
            pieChart.Titles.Add("cell population") ' Set title for this chart
        ElseIf selectedText = "visitors" Then
            ' Load visitor status pie chart
            LoadVisitorStatusToPieChart(visitor)
            pieChart.Titles.Clear()
            pieChart.Titles.Add("visitors") ' Set title for this chart
        Else
            ' Handle any other index (optional)
            MessageBox.Show("Other selection made")
        End If
    End Sub
    Private Sub LoadSexDistributionToPieChart(dt As DataTable)
        pieChart.Series.Clear()
        pieChart.ChartAreas.Clear()

        Dim chartArea As New ChartArea()
        pieChart.ChartAreas.Add(chartArea)

        Dim series As New Series("PieChartSeries")
        series.ChartType = SeriesChartType.Pie

        Dim maleCount As Integer = 0
        Dim femaleCount As Integer = 0

        For Each row As DataRow In dt.Rows
            Dim sex As String = row("sex").ToString().ToLower()
            If sex = "male" Then
                maleCount += 1
            ElseIf sex = "female" Then
                femaleCount += 1
            End If
        Next

        series.Points.AddXY("Male", maleCount)
        series.Points.AddXY("Female", femaleCount)

        pieChart.Series.Add(series)
        pieChart.Titles.Clear()
        pieChart.Titles.Add("Gender Distribution")

        ' Set the legend to show the category and value
        pieChart.Legends.Clear()
        Dim legend As New Legend()
        pieChart.Legends.Add(legend)
    End Sub

    Private Sub LoadDataToPieChart(dt As DataTable)
        pieChart.Series.Clear()
        pieChart.ChartAreas.Clear()

        Dim chartArea As New ChartArea()
        pieChart.ChartAreas.Add(chartArea)

        Dim series As New Series("PieChartSeries")
        series.ChartType = SeriesChartType.Pie

        Dim cells As String() = {"minimum security", "maximum security", "medium security", "juvenile correction", "women's facility", "rehabilitational facility"}
        Dim categoryCounts As New Dictionary(Of String, Integer)

        For Each category As String In cells
            categoryCounts.Add(category, 0)
        Next

        For Each row As DataRow In dt.Rows
            Dim category As String = row("cell").ToString().ToLower()

            If categoryCounts.ContainsKey(category) Then
                categoryCounts(category) += 1
            End If
        Next

        For Each category As String In cells
            Dim value As Integer = categoryCounts(category)
            series.Points.AddXY(category, value)
        Next

        pieChart.Series.Add(series)
        pieChart.Titles.Clear()
        pieChart.Titles.Add("Population Distribution")

        ' Set the legend to show the category and value
        pieChart.Legends.Clear()
        Dim legend As New Legend()
        pieChart.Legends.Add(legend)
    End Sub

    Private Sub LoadVisitorStatusToPieChart(dt As DataTable)
        pieChart.Series.Clear()
        pieChart.ChartAreas.Clear()

        Dim chartArea As New ChartArea()
        pieChart.ChartAreas.Add(chartArea)

        Dim series As New Series("PieChartSeries")
        series.ChartType = SeriesChartType.Pie

        Dim pendingCount As Integer = 0
        Dim rejectedCount As Integer = 0
        Dim approvedCount As Integer = 0

        For Each row As DataRow In dt.Rows
            Dim status As String = row("status").ToString().ToLower()

            Select Case status
                Case "pending"
                    pendingCount += 1
                Case "rejected"
                    rejectedCount += 1
                Case "approved"
                    approvedCount += 1
            End Select
        Next

        series.Points.AddXY("Pending", pendingCount)
        series.Points.AddXY("Rejected", rejectedCount)
        series.Points.AddXY("Approved", approvedCount)

        pieChart.Series.Add(series)
        pieChart.Titles.Clear()
        pieChart.Titles.Add("Visitor Status Distribution")

        ' Set the legend to show the category and value
        pieChart.Legends.Clear()
        Dim legend As New Legend()
        pieChart.Legends.Add(legend)
    End Sub


    Private Sub pieChart_Click(sender As Object, e As EventArgs) Handles pieChart.Click
        Dim mainform As MainDashboard = TryCast(Me.ParentForm, MainDashboard)
        mainform.SwitchToReportHomeControl()
    End Sub

    Private Sub CrimeChart_Click(sender As Object, e As EventArgs)

    End Sub



    Private Sub dgvCellblockInfo_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCellblockInfo.CellContentClick
        Dim mainform As MainDashboard = TryCast(Me.ParentForm, MainDashboard)
        mainform.SwitchToInmateHomeControl()
    End Sub
End Class
