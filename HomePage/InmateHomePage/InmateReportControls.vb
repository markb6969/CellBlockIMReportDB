Imports QuestPDF.Fluent
Imports QuestPDF.Helpers
Imports QuestPDF.Infrastructure
Imports System.IO
Imports System.Text
Imports DocumentFormat.OpenXml.Wordprocessing
Imports System.Runtime.InteropServices.ComTypes
Imports DocumentFormat.OpenXml.Presentation

Public Class InmateReportControls
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim mainForm As Home = TryCast(Me.ParentForm, Home)
        mainForm.SwitchToInmatePageControl()
    End Sub

    Private Sub btnGenerateReport_Click(sender As Object, e As EventArgs) Handles btnGenerateReport.Click

        ' Ensure a report type is selected
        Dim selectedReport As String = cmbReports.SelectedItem?.ToString()

        If String.IsNullOrEmpty(selectedReport) Then
            MessageBox.Show("Please select a report to generate.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim savePath As String = String.Empty

        Try
            Select Case selectedReport
                Case "PDL Population Summary Report"
                    savePath = GeneratePdfForPDLPopulationReport() ' Assign the return value to savePath
                Case "Criminal Case Summary Report"
                    savePath = GenerateCriminalCasePdf()
                Case "Medical Summary Report"
                    savePath = GenerateMedicalReportPdf()
                Case "PDL Release Summary Report"
                    savePath = GeneratePDLReleaseSummaryReport()
                Case "Staff Population Summary Report"
                    savePath = GenerateStaffPopulationReport()
                Case "Recent Crimes"
                    Dim startDate As DateTime = dtStartDate.Value
                    Dim endDate As DateTime = dtEndDate.Value
                    savePath = GenerateRecentCrimesReport(startDate, endDate)
                Case "Incident Crime Summary Report"
                    savePath = GenerateIncidentReport()

                Case "Recent Inmate Releases"
                    Dim startDate As DateTime = dtStartDate.Value
                    Dim endDate As DateTime = dtEndDate.Value
                    savePath = GenerateRecentInmateReleaseReport(startDate, endDate)

                Case Else
                    ' Default to text format for other reports
                    Dim reportContent As String = GetReportContent(selectedReport)
                    If String.IsNullOrEmpty(reportContent) Then
                        MessageBox.Show("Failed to generate the report.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If
                    savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{selectedReport}.txt")
                    File.WriteAllText(savePath, reportContent)
            End Select

            MessageBox.Show($"Report generated successfully at {savePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show($"Error generating the report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function GetReportContent(reportType As String) As String
        Select Case reportType
            Case "PDL Population Summary Report"
                Return GeneratePdfForPDLPopulationReport() ' This function should return a file path or report content
            Case "Criminal Case Summary Report"
                Dim dt1 As DataTable = modDB.GetTableData("criminal_case")
                Return GenerateCriminalCaseReport(dt1)
            Case "Medical Summary Report"
                Return GenerateMedicalReport()
            Case "PDL Release Summary Report"
                Return GeneratePDLReleaseSummaryReport()
            Case "Staff Population Summary Report"
                Return GenerateStaffPopulationReport()
            Case "Incident Crime Summary Report"
                Return GenerateIncidentReport()
            Case "Recent Crimes"
                Return GenerateRecentCrimesReport(dtStartDate.Value, dtEndDate.Value)
            Case Else
                Return String.Empty
        End Select
    End Function



    Private Function GeneratePdfForPDLPopulationReport() As String
        Dim dt As New DataTable()
        Return GeneratePdfForPDLPopulationReport(dt)
    End Function

    Public Function GeneratePdfForPDLPopulationReport(dt As DataTable) As String
        dt = modDB.GetTableData("pdl") ' Assuming modDB.GetTableData retrieves the DataTable

        ' Basic report metadata
        Dim reportDate As String = DateTime.Now.ToString("yyyy-MM-dd")
        Dim totalPDLs As Integer = dt.Rows.Count
        Dim timenow As String = DateTime.Now.ToString("yyyyMMddHHmmss")
        Dim reportsDir As String = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "reports")
        Dim filepath As String = Path.Combine(reportsDir, $"PDLPopulationReport_{timenow}.pdf")

        ' Ensure the reports directory exists
        If Not Directory.Exists(reportsDir) Then
            Directory.CreateDirectory(reportsDir)
        End If

        ' Grouping and counting occurrences for each field
        Dim sexCounts = dt.AsEnumerable().GroupBy(Function(row) row.Field(Of String)("sex")).Select(Function(g) New With {.Key = g.Key, .Count = g.Count()}).ToList()
        Dim civilStatusCounts = dt.AsEnumerable().GroupBy(Function(row) row.Field(Of String)("civil_status")).Select(Function(g) New With {.Key = g.Key, .Count = g.Count()}).ToList()
        Dim countryCounts = dt.AsEnumerable().GroupBy(Function(row) row.Field(Of String)("country")).Select(Function(g) New With {.Key = g.Key, .Count = g.Count()}).ToList()
        Dim municipalityCounts = dt.AsEnumerable().GroupBy(Function(row) row.Field(Of String)("municipality")).Select(Function(g) New With {.Key = g.Key, .Count = g.Count()}).ToList()
        Dim hairColorCounts = dt.AsEnumerable().GroupBy(Function(row) row.Field(Of String)("hair_color")).Select(Function(g) New With {.Key = g.Key, .Count = g.Count()}).ToList()

        ' PDF Generation
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community
        QuestPDF.Fluent.Document.Create(Sub(container)
                                            container.Page(Sub(page)
                                                               page.Size(PageSizes.A4)
                                                               page.Margin(2, Unit.Centimetre)
                                                               page.PageColor(Colors.White)

                                                               ' Content
                                                               page.Content().Column(Sub(cols)
                                                                                         cols.Item().AlignCenter().Text("CellBlock Central").Bold().FontSize(28)
                                                                                         cols.Item().AlignCenter().Text("PDL Population Report").Bold().FontSize(16)
                                                                                         cols.Item().Text("") ' Spacer
                                                                                         cols.Item().Text("This report provides an overview of the current PDL (Person Deprived of Liberty) population within the facility. It includes a breakdown of key demographics, including gender, age, and status, to support efficient management and policy decision-making.").FontSize(10).Italic()
                                                                                         cols.Item().Text("") ' Spacer
                                                                                         cols.Item().Text($"Report Date: {reportDate}").FontSize(12)
                                                                                         cols.Item().Text($"Total PDLs: {totalPDLs}").FontSize(12)
                                                                                         cols.Item().Text("") ' Spacer

                                                                                         ' Section: Sex
                                                                                         cols.Item().Text("Sex:").Bold().FontSize(14)
                                                                                         cols.Item().Table(Sub(table)
                                                                                                               table.ColumnsDefinition(Sub(x)
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn() ' Added a third column for the "Percentage" section
                                                                                                                                       End Sub)

                                                                                                               ' Column headers with 3 columns
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Sex").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Count").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Percentage").Bold()

                                                                                                               For Each sex In sexCounts
                                                                                                                   Dim sexPercentage As Double = (sex.Count / totalPDLs) * 100
                                                                                                                   table.Cell().Border(1).Text(sex.Key)
                                                                                                                   table.Cell().Border(1).Text(sex.Count.ToString())
                                                                                                                   table.Cell().Border(1).Text($"{sexPercentage:N2}%")
                                                                                                               Next
                                                                                                           End Sub)

                                                                                         cols.Item().Text("") ' Spacer

                                                                                         ' Section: Civil Status
                                                                                         cols.Item().Text("Civil Status:").Bold().FontSize(14)
                                                                                         cols.Item().Table(Sub(table)
                                                                                                               table.ColumnsDefinition(Sub(x)
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn() ' Added a third column for the "Percentage" section
                                                                                                                                       End Sub)

                                                                                                               ' Column headers with 3 columns
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Civil Status").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Count").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Percentage").Bold()

                                                                                                               For Each status In civilStatusCounts
                                                                                                                   Dim statusPercentage As Double = (status.Count / totalPDLs) * 100
                                                                                                                   table.Cell().Border(1).Text(status.Key)
                                                                                                                   table.Cell().Border(1).Text(status.Count.ToString())
                                                                                                                   table.Cell().Border(1).Text($"{statusPercentage:N2}%")
                                                                                                               Next
                                                                                                           End Sub)

                                                                                         cols.Item().Text("") ' Spacer

                                                                                         ' Section: Country
                                                                                         cols.Item().Text("Country:").Bold().FontSize(14)
                                                                                         cols.Item().Table(Sub(table)
                                                                                                               table.ColumnsDefinition(Sub(x)
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn() ' Added a third column for the "Percentage" section
                                                                                                                                       End Sub)

                                                                                                               ' Column headers with 3 columns
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Country").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Count").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Percentage").Bold()

                                                                                                               For Each country In countryCounts
                                                                                                                   Dim countryPercentage As Double = (country.Count / totalPDLs) * 100
                                                                                                                   table.Cell().Border(1).Text(country.Key)
                                                                                                                   table.Cell().Border(1).Text(country.Count.ToString())
                                                                                                                   table.Cell().Border(1).Text($"{countryPercentage:N2}%")
                                                                                                               Next
                                                                                                           End Sub)

                                                                                         cols.Item().Text("") ' Spacer

                                                                                         ' Section: Municipality
                                                                                         cols.Item().Text("Municipality:").Bold().FontSize(14)
                                                                                         cols.Item().Table(Sub(table)
                                                                                                               table.ColumnsDefinition(Sub(x)
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn() ' Added a third column for the "Percentage" section
                                                                                                                                       End Sub)

                                                                                                               ' Column headers with 3 columns
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Municipality").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Count").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Percentage").Bold()

                                                                                                               For Each municipality In municipalityCounts
                                                                                                                   Dim municipalityPercentage As Double = (municipality.Count / totalPDLs) * 100
                                                                                                                   table.Cell().Border(1).Text(municipality.Key)
                                                                                                                   table.Cell().Border(1).Text(municipality.Count.ToString())
                                                                                                                   table.Cell().Border(1).Text($"{municipalityPercentage:N2}%")
                                                                                                               Next
                                                                                                           End Sub)

                                                                                         cols.Item().Text("") ' Spacer

                                                                                         ' Section: Hair Color
                                                                                         cols.Item().Text("Hair Color:").Bold().FontSize(14)
                                                                                         cols.Item().Table(Sub(table)
                                                                                                               table.ColumnsDefinition(Sub(x)
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn() ' Added a third column for the "Percentage" section
                                                                                                                                       End Sub)

                                                                                                               ' Column headers with 3 columns
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Hair Color").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Count").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Percentage").Bold()

                                                                                                               For Each hair In hairColorCounts
                                                                                                                   Dim hairPercentage As Double = (hair.Count / totalPDLs) * 100
                                                                                                                   table.Cell().Border(1).Text(hair.Key)
                                                                                                                   table.Cell().Border(1).Text(hair.Count.ToString())
                                                                                                                   table.Cell().Border(1).Text($"{hairPercentage:N2}%")
                                                                                                               Next
                                                                                                           End Sub)


                                                                                         ' Footer
                                                                                         cols.Item().Text("") ' Spacer
                                                                                         cols.Item().Text("") ' Spacer
                                                                                         cols.Item().AlignRight().Text($"Generated on {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
                                                                                     End Sub)
                                                           End Sub)
                                        End Sub).GeneratePdf(filepath)

        ' Return the file path of the generated PDF
        Return filepath
    End Function





    Private Function GenerateStaffPopulationReport() As String
        ' Retrieve data from the "staffdetails" table
        Dim dt As DataTable = modDB.GetTableData("staffdetails")

        ' Validate DataTable
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            Throw New Exception("No data available in the 'staffdetails' table to generate the report.")
        End If

        ' Basic report metadata
        Dim reportDate As String = DateTime.Now.ToString("yyyy-MM-dd")
        Dim totalStaff As Integer = dt.Rows.Count
        Dim timenow As String = DateTime.Now.ToString("yyyyMMddHHmmss")
        Dim reportsDir As String = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "reports")
        Dim filepath As String = Path.Combine(reportsDir, $"StaffPopulationReport_{timenow}.pdf")

        ' Ensure the reports directory exists
        If Not Directory.Exists(reportsDir) Then
            Directory.CreateDirectory(reportsDir)
        End If

        ' Grouping and counting occurrences for each field
        Dim municipalityCounts = dt.AsEnumerable().GroupBy(Function(row) row.Field(Of String)("municipality")).Select(Function(g) New With {.Key = g.Key, .Count = g.Count()}).ToList()
        Dim cityCounts = dt.AsEnumerable().GroupBy(Function(row) row.Field(Of String)("city")).Select(Function(g) New With {.Key = g.Key, .Count = g.Count()}).ToList()
        Dim regionCounts = dt.AsEnumerable().GroupBy(Function(row) row.Field(Of String)("Region")).Select(Function(g) New With {.Key = g.Key, .Count = g.Count()}).ToList()
        Dim genderCounts = dt.AsEnumerable().GroupBy(Function(row) row.Field(Of String)("gender")).Select(Function(g) New With {.Key = g.Key, .Count = g.Count()}).ToList()
        Dim statusCounts = dt.AsEnumerable().GroupBy(Function(row) row.Field(Of String)("status")).Select(Function(g) New With {.Key = g.Key, .Count = g.Count()}).ToList()
        Dim positionCounts = dt.AsEnumerable().GroupBy(Function(row) row.Field(Of String)("position")).Select(Function(g) New With {.Key = g.Key, .Count = g.Count()}).ToList()
        Dim authorityCounts = dt.AsEnumerable().GroupBy(Function(row) row.Field(Of String)("authority")).Select(Function(g) New With {.Key = g.Key, .Count = g.Count()}).ToList()

        ' PDF Generation
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community
        QuestPDF.Fluent.Document.Create(Sub(container)
                                            container.Page(Sub(page)
                                                               page.Size(PageSizes.A4)
                                                               page.Margin(2, Unit.Centimetre)
                                                               page.PageColor(Colors.White)

                                                               ' Content
                                                               page.Content().Column(Sub(cols)
                                                                                         cols.Item().AlignCenter().Text("CellBlock Central").Bold().FontSize(28)
                                                                                         cols.Item().AlignCenter().Text("Staff Population Report").Bold().FontSize(16)
                                                                                         cols.Item().Text("This report provides a comprehensive overview of the staff population within the facility. It covers key demographics including municipality, city, region, gender, status, position, and authority distributions, along with their respective percentages. This analysis helps in making informed decisions regarding staff management and resource allocation.").FontSize(10).Italic()


                                                                                         cols.Item().Text("") ' Spacer
                                                                                         cols.Item().Text($"Report Date: {reportDate}").FontSize(12)
                                                                                         cols.Item().Text($"Total Staff: {totalStaff}").FontSize(12)
                                                                                         cols.Item().Text("") ' Spacer

                                                                                         ' Section: Municipality
                                                                                         cols.Item().Text("Municipality:").Bold().FontSize(16)
                                                                                         cols.Item().Table(Sub(table)
                                                                                                               table.ColumnsDefinition(Sub(x)
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn() ' Added a third column for the "Percentage" section
                                                                                                                                       End Sub)

                                                                                                               ' Column headers with 3 columns
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Municipality").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Count").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Percentage").Bold()

                                                                                                               ' Rows for municipality counts
                                                                                                               For Each municipality In municipalityCounts
                                                                                                                   Dim municipalityPercentage As Double = (municipality.Count / totalStaff) * 100
                                                                                                                   table.Cell().Border(1).Text(municipality.Key)
                                                                                                                   table.Cell().Border(1).Text(municipality.Count.ToString())
                                                                                                                   table.Cell().Border(1).Text($"{municipalityPercentage:N2}%")
                                                                                                               Next
                                                                                                           End Sub)

                                                                                         cols.Item().Text("") ' Spacer

                                                                                         ' Section: City
                                                                                         cols.Item().Text("City:").Bold().FontSize(16)
                                                                                         cols.Item().Table(Sub(table)
                                                                                                               table.ColumnsDefinition(Sub(x)
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn() ' Added a third column for the "Percentage" section
                                                                                                                                       End Sub)

                                                                                                               ' Column headers with 3 columns
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("City").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Count").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Percentage").Bold()

                                                                                                               ' Rows for city counts
                                                                                                               For Each city In cityCounts
                                                                                                                   Dim cityPercentage As Double = (city.Count / totalStaff) * 100
                                                                                                                   table.Cell().Border(1).Text(city.Key)
                                                                                                                   table.Cell().Border(1).Text(city.Count.ToString())
                                                                                                                   table.Cell().Border(1).Text($"{cityPercentage:N2}%")
                                                                                                               Next
                                                                                                           End Sub)

                                                                                         cols.Item().Text("") ' Spacer

                                                                                         ' Section: Region Distribution
                                                                                         cols.Item().Text("Region:").Bold().FontSize(16)
                                                                                         cols.Item().Table(Sub(table)
                                                                                                               table.ColumnsDefinition(Sub(x)
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                       End Sub)

                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Region").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Count").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Percentage").Bold()

                                                                                                               For Each Regions In regionCounts
                                                                                                                   Dim regionPercentage As Double = (Regions.Count / totalStaff) * 100
                                                                                                                   table.Cell().Border(1).Text(Regions.Key)
                                                                                                                   table.Cell().Border(1).Text(Regions.Count.ToString())
                                                                                                                   table.Cell().Border(1).Text($"{regionPercentage:N2}%")
                                                                                                               Next
                                                                                                           End Sub)

                                                                                         cols.Item().Text("") ' Spacer

                                                                                         ' Section: Gender Distribution
                                                                                         cols.Item().Text("Gender:").Bold().FontSize(16)
                                                                                         cols.Item().Table(Sub(table)
                                                                                                               table.ColumnsDefinition(Sub(x)
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                       End Sub)

                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Gender").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Count").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Percentage").Bold()

                                                                                                               For Each gender In genderCounts
                                                                                                                   Dim genderPercentage As Double = (gender.Count / totalStaff) * 100
                                                                                                                   table.Cell().Border(1).Text(gender.Key)
                                                                                                                   table.Cell().Border(1).Text(gender.Count.ToString())
                                                                                                                   table.Cell().Border(1).Text($"{genderPercentage:N2}%")
                                                                                                               Next
                                                                                                           End Sub)

                                                                                         cols.Item().Text("") ' Spacer

                                                                                         ' Section: Status Distribution
                                                                                         cols.Item().Text("Status:").Bold().FontSize(16)
                                                                                         cols.Item().Table(Sub(table)
                                                                                                               table.ColumnsDefinition(Sub(x)
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                       End Sub)

                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Status").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Count").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Percentage").Bold()

                                                                                                               For Each status In statusCounts
                                                                                                                   Dim statusPercentage As Double = (status.Count / totalStaff) * 100
                                                                                                                   table.Cell().Border(1).Text(status.Key)
                                                                                                                   table.Cell().Border(1).Text(status.Count.ToString())
                                                                                                                   table.Cell().Border(1).Text($"{statusPercentage:N2}%")
                                                                                                               Next
                                                                                                           End Sub)

                                                                                         cols.Item().Text("") ' Spacer

                                                                                         ' Section: Position Distribution
                                                                                         cols.Item().Text("Position:").Bold().FontSize(16)
                                                                                         cols.Item().Table(Sub(table)
                                                                                                               table.ColumnsDefinition(Sub(x)
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                       End Sub)

                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Position").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Count").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.0).Text("Percentage").Bold()

                                                                                                               For Each position In positionCounts
                                                                                                                   Dim positionPercentage As Double = (position.Count / totalStaff) * 100
                                                                                                                   table.Cell().Border(1).Text(position.Key)
                                                                                                                   table.Cell().Border(1).Text(position.Count.ToString())
                                                                                                                   table.Cell().Border(1).Text($"{positionPercentage:N2}%")
                                                                                                               Next
                                                                                                           End Sub)

                                                                                         cols.Item().Text("") ' Spacer

                                                                                         ' Section: Authority Distribution
                                                                                         cols.Item().Text("Authority:").Bold().FontSize(16)
                                                                                         cols.Item().Table(Sub(table)
                                                                                                               table.ColumnsDefinition(Sub(x)
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                       End Sub)

                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Authority").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Count").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.0).Text("Percentage").Bold()

                                                                                                               For Each authority In authorityCounts
                                                                                                                   Dim authorityPercentage As Double = (authority.Count / totalStaff) * 100
                                                                                                                   table.Cell().Border(1).Text(authority.Key)
                                                                                                                   table.Cell().Border(1).Text(authority.Count.ToString())
                                                                                                                   table.Cell().Border(1).Text($"{authorityPercentage:N2}%")
                                                                                                               Next
                                                                                                           End Sub)

                                                                                         ' Footer
                                                                                         cols.Item().Text("") ' Spacer
                                                                                         cols.Item().Text("") ' Spacer
                                                                                         cols.Item().AlignRight().Text($"Generated on {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
                                                                                     End Sub)
                                                           End Sub)
                                        End Sub).GeneratePdf(filepath)

        ' Return the file path of the generated PDF
        Return filepath
    End Function





    Private Function GenerateIncidentReport() As String
        ' Initialize the DataTable to hold incident data
        Dim dt As New DataTable()
        Return GenerateIncidentReport(dt)
    End Function

    ' Define CrimeData class
    Public Class CrimeData
        Public Property Crime As String
        Public Property Count As Integer
    End Class

    Public Function GenerateIncidentReport(dt As DataTable) As String
        ' Fetching incident data from the "crimes" table
        dt = modDB.GetTableData("crimes") ' Assuming modDB.GetTableData retrieves the DataTable

        ' Basic report metadata
        Dim reportDate As String = DateTime.Now.ToString("yyyy-MM-dd")
        Dim totalIncidents As Integer = dt.Rows.Count
        Dim timenow As String = DateTime.Now.ToString("yyyyMMddHHmmss")
        Dim reportsDir As String = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "reports")
        Dim filepath As String = Path.Combine(reportsDir, $"IncidentReport_{timenow}.pdf")

        ' Ensure the reports directory exists
        If Not Directory.Exists(reportsDir) Then
            Directory.CreateDirectory(reportsDir)
        End If

        ' Grouping and counting occurrences for each crime type
        Dim crimeCounts = dt.AsEnumerable() _
                        .GroupBy(Function(row) row.Field(Of String)("crime_committed")) _
                        .Select(Function(g) New CrimeData With {.Crime = g.Key, .Count = g.Count()}) _
                        .OrderByDescending(Function(x) x.Count) ' Sort by the count of each crime, most common first

        ' PDF Generation
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community
        QuestPDF.Fluent.Document.Create(Sub(container)
                                            container.Page(Sub(page)
                                                               page.Size(PageSizes.A4)
                                                               page.Margin(2, Unit.Centimetre)
                                                               page.PageColor(Colors.White)

                                                               ' Content
                                                               page.Content().Column(Sub(cols)
                                                                                         cols.Item().AlignCenter().Text("CellBlock Central").Bold().FontSize(28)
                                                                                         cols.Item().AlignCenter().Text("Incident Report").Bold().FontSize(16)
                                                                                         cols.Item().Text("This report provides an overview of incidents within the facility, detailing the nature, severity, and resolution of each event. It plays a crucial role in ensuring the safety and security of all individuals by tracking critical occurrences and addressing them promptly.").FontSize(10).Italic()

                                                                                         cols.Item().Text("") ' Spacer
                                                                                         cols.Item().Text($"Report Date: {reportDate}").FontSize(12)
                                                                                         cols.Item().Text($"Total Incidents: {totalIncidents}").FontSize(12)
                                                                                         cols.Item().Text("") ' Spacer

                                                                                         ' Section: Crime Counts (table)
                                                                                         cols.Item().Text("Most Common Crimes:").Bold().FontSize(14)
                                                                                         cols.Item().Text("") ' Spacer
                                                                                         cols.Item().Table(Sub(table)
                                                                                                               table.ColumnsDefinition(Sub(x)
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn() ' Added a third column for the "Percentage" section
                                                                                                                                       End Sub)

                                                                                                               ' Column headers with 3 columns
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Crime").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Count").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Percentage").Bold()

                                                                                                               ' Loop through each crime and calculate the percentage
                                                                                                               For Each crimes In crimeCounts
                                                                                                                   Dim crimePercentage As Double = (crimes.Count / totalIncidents) * 100
                                                                                                                   table.Cell().Border(1).Text(crimes.Crime)
                                                                                                                   table.Cell().Border(1).Text(crimes.Count.ToString())
                                                                                                                   table.Cell().Border(1).Text($"{crimePercentage:N2}%")
                                                                                                               Next
                                                                                                           End Sub)

                                                                                         ' Footer
                                                                                         cols.Item().Text("") ' Spacer
                                                                                         cols.Item().Text("") ' Spacer
                                                                                         cols.Item().AlignRight().Text($"Generated on {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
                                                                                     End Sub)
                                                           End Sub)
                                        End Sub).GeneratePdf(filepath)

        ' Return the file path of the generated PDF
        Return filepath
    End Function








    Private Function GenerateCriminalCasePdf() As String
        ' Retrieve the data table for criminal cases
        Dim dt1 As DataTable = modDB.GetTableData("criminal_case")

        ' Generate the criminal case report and get the path
        Dim reportPath As String = GenerateCriminalCaseReport(dt1)

        ' Display message box for success
        MessageBox.Show("PDF generated successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ' Return the generated report path
        Return reportPath
    End Function


    Private Function GenerateMedicalReportPdf() As String
        Dim dt1 As DataTable = modDB.GetTableData("medical")


        Dim reportPath As String = generatePdfForMedicalReport()

        MessageBox.Show("PDF generated successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Return reportPath
    End Function

    Public Function GenerateCriminalCaseReport(dt1 As DataTable) As String
        ' Basic report metadata
        Dim reportID As String = dt1.Rows(0).Item("case_id").ToString()
        Dim reportDate As String = DateTime.Now.ToString("yyyy-MM-dd")
        Dim totalCases As Integer = dt1.Rows.Count
        Dim timenow As String = DateTime.Now.ToString("yyyyMMddHHmmss")
        Dim filepath As String = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "reports", $"CriminalCaseReport_{reportID}_{timenow}.pdf")

        ' Ensure directory exists
        Dim reportsDir As String = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "reports")
        If Not Directory.Exists(reportsDir) Then
            Directory.CreateDirectory(reportsDir)
        End If

        ' Grouping and counting occurrences for offenses and modus operandi
        Dim offenseCounts = dt1.AsEnumerable() _
                          .GroupBy(Function(row) row.Field(Of String)("offence_charge")) _
                          .Select(Function(g) New With {.Offense = g.Key, .Count = g.Count()}) _
                          .OrderByDescending(Function(o) o.Count)
        Dim totalOffenses = offenseCounts.Sum(Function(o) o.Count)

        Dim modusCounts = dt1.AsEnumerable() _
                         .GroupBy(Function(row) row.Field(Of String)("modus_operandi")) _
                         .Select(Function(g) New With {.Modus = g.Key, .Count = g.Count()}) _
                         .OrderByDescending(Function(m) m.Count)
        Dim totalModus = modusCounts.Sum(Function(m) m.Count)

        ' PDF Generation
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community
        QuestPDF.Fluent.Document.Create(Sub(container)
                                            container.Page(Sub(page)
                                                               page.Size(PageSizes.A4)
                                                               page.Margin(2, Unit.Centimetre)
                                                               page.PageColor(Colors.White)

                                                               ' Content
                                                               page.Content().Column(Sub(cols)
                                                                                         ' Header with title
                                                                                         cols.Item().AlignCenter().Text("CellBlock Central").Bold().FontSize(28)
                                                                                         cols.Item().AlignCenter().Text("Criminal Case Report").Bold().FontSize(16).FontColor(Colors.Black)
                                                                                         cols.Item().Text("This report provides an overview of criminal cases involving individuals currently under custody. It includes case statuses, parties involved, and legal proceedings, offering valuable insights for case management and legal reviews.").FontSize(10).Italic()

                                                                                         cols.Item().Text("") ' Spacer
                                                                                         ' Report metadata
                                                                                         cols.Item().Text($"Report Date: {reportDate}").FontSize(12).FontColor(Colors.Black)
                                                                                         cols.Item().Text($"Total Cases: {totalCases}").FontSize(12).FontColor(Colors.Black)
                                                                                         cols.Item().PaddingBottom(20)

                                                                                         ' Section: Most Common Offenses (Table)
                                                                                         cols.Item().Text("Most Common Offenses:").Bold().FontColor(Colors.Black)
                                                                                         cols.Item().PaddingBottom(10)
                                                                                         cols.Item().Table(Sub(table)
                                                                                                               table.ColumnsDefinition(Sub(x)
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                       End Sub)

                                                                                                               ' Column headers with 3 columns
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Offense").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Count").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Percentage").Bold()

                                                                                                               ' Loop through each offense and calculate the percentage
                                                                                                               For Each offense In offenseCounts
                                                                                                                   Dim offensePercentage As Double = (offense.Count / totalOffenses) * 100
                                                                                                                   table.Cell().Border(1).Text(offense.Offense)
                                                                                                                   table.Cell().Border(1).Text(offense.Count.ToString())
                                                                                                                   table.Cell().Border(1).Text($"{offensePercentage:F2}%")
                                                                                                               Next
                                                                                                           End Sub)
                                                                                         cols.Item().PaddingBottom(20)

                                                                                         ' Section: Most Common Modus Operandi (Table)
                                                                                         cols.Item().Text("Most Common Modus Operandi:").Bold().FontColor(Colors.Black)
                                                                                         cols.Item().PaddingBottom(10)
                                                                                         cols.Item().Table(Sub(table)
                                                                                                               table.ColumnsDefinition(Sub(x)
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                           x.RelativeColumn()
                                                                                                                                       End Sub)

                                                                                                               ' Column headers with 3 columns
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Modus Operandi").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Count").Bold()
                                                                                                               table.Cell().ColumnSpan(1).Border(1).Padding(0.5, 0.5).Text("Percentage").Bold()

                                                                                                               ' Loop through each modus and calculate the percentage
                                                                                                               For Each modus In modusCounts
                                                                                                                   Dim modusPercentage As Double = (modus.Count / totalModus) * 100
                                                                                                                   table.Cell().Border(1).Text(modus.Modus)
                                                                                                                   table.Cell().Border(1).Text(modus.Count.ToString())
                                                                                                                   table.Cell().Border(1).Text($"{modusPercentage:F2}%")
                                                                                                               Next
                                                                                                           End Sub)

                                                                                         ' Footer with generation time
                                                                                         cols.Item().Text("") ' Spacer
                                                                                         cols.Item().Text("") ' Spacer
                                                                                         cols.Item().AlignRight().Text($"Generated on {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}")
                                                                                     End Sub)
                                                           End Sub)
                                        End Sub).GeneratePdf(filepath)

        ' Return the file path of the generated report
        Return filepath
    End Function





    Public Function GenerateRecentCrimesReport(startDate As DateTime, endDate As DateTime) As String
        ' Fetch data from the database (assuming modDB.GetTableData retrieves the DataTable)
        Dim dt1 As DataTable = modDB.GetTableData("criminal_case")

        ' Filter data by date range (date_filed)
        Dim filteredData = dt1.AsEnumerable() _
                          .Where(Function(row) row.Field(Of DateTime)("date_filed") >= startDate AndAlso
                                               row.Field(Of DateTime)("date_filed") <= endDate) _
                          .CopyToDataTable()

        ' Check if filteredData contains any rows
        If filteredData.Rows.Count = 0 Then
            MessageBox.Show("No data found for the selected date range.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return String.Empty ' Return early if no data
        End If

        ' Basic report metadata
        Dim reportDate As String = DateTime.Now.ToString("yyyy-MM-dd")
        Dim totalCases As Integer = filteredData.Rows.Count
        Dim timenow As String = DateTime.Now.ToString("yyyyMMddHHmmss")
        Dim filepath As String = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "reports", $"RecentCrimesReport_{timenow}.pdf")

        ' Ensure directory exists
        Dim reportsDir As String = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "reports")
        If Not Directory.Exists(reportsDir) Then
            Directory.CreateDirectory(reportsDir)
        End If

        ' PDF Generation
        QuestPDF.Settings.License = LicenseType.Community
        QuestPDF.Fluent.Document.Create(Sub(container)
                                            container.Page(Sub(page)
                                                               page.Size(PageSizes.A4)
                                                               page.Margin(2, Unit.Centimetre)
                                                               page.PageColor(Colors.White)

                                                               ' Consolidating all content under a single page.Content() method
                                                               page.Content().Column(Sub(cols)

                                                                                         ' Header with title
                                                                                         cols.Item().AlignCenter().Text("CellBlock Central").Bold().FontSize(28)
                                                                                         cols.Item().Text("This report focuses on recent criminal activities in the surrounding community or facility. It includes details about the crimes, individuals involved, and law enforcement actions taken, assisting in identifying trends and implementing safety measures.").FontSize(10).Italic()

                                                                                         cols.Item().AlignCenter().Text("Recent Crimes Report").Bold().FontSize(24).FontColor(Colors.Black).Underline()
                                                                                         ' Report metadata
                                                                                         cols.Item().Text($"Report Date: {reportDate}").FontSize(12).FontColor(Colors.Black)
                                                                                         cols.Item().Text($"Total Cases: {totalCases}").FontSize(12).FontColor(Colors.Black)
                                                                                         cols.Item().PaddingBottom(20)

                                                                                         ' Crimes section - Iterate over filtered data
                                                                                         cols.Item().Text("Crimes within the selected date range:").Bold().FontColor(Colors.Black)
                                                                                         cols.Item().PaddingBottom(10)

                                                                                         For Each row As DataRow In filteredData.Rows
                                                                                             cols.Item().Text($"Case ID: {row("case_id")} - Offense: {row("offence_charge")}")
                                                                                             cols.Item().Text($"   Date: {row("date_filed")}")
                                                                                         Next

                                                                                         ' Most Common Offenses Section (Filtered)
                                                                                         cols.Item().Text("Most Common Offenses:").Bold().FontColor(Colors.Black)
                                                                                         cols.Item().PaddingBottom(10)
                                                                                         Dim offenseCounts = filteredData.AsEnumerable().
                                                                                             GroupBy(Function(row) row.Field(Of String)("offence_charge")).
                                                                                             Select(Function(g) New With {.Offense = g.Key, .Count = g.Count()}).
                                                                                             OrderByDescending(Function(o) o.Count)
                                                                                         Dim totalOffenses = offenseCounts.Sum(Function(o) o.Count)
                                                                                         For Each offense In offenseCounts
                                                                                             Dim percentage = (offense.Count / totalOffenses) * 100
                                                                                             cols.Item().Text($"Offense: {offense.Offense}, Occurrences: {offense.Count} ({percentage:F2}%)").FontSize(12)
                                                                                         Next
                                                                                         cols.Item().PaddingBottom(20)

                                                                                         ' Most Common Modus Operandi Section (Filtered)
                                                                                         cols.Item().Text("Most Common Modus Operandi:").Bold().FontColor(Colors.Black)
                                                                                         cols.Item().PaddingBottom(10)
                                                                                         Dim modusCounts = filteredData.AsEnumerable().
                                                                                                       GroupBy(Function(row) row.Field(Of String)("modus_operandi")).
                                                                                                       Select(Function(g) New With {.Modus = g.Key, .Count = g.Count()}).
                                                                                                       OrderByDescending(Function(m) m.Count)
                                                                                         Dim totalModus = modusCounts.Sum(Function(m) m.Count)
                                                                                         For Each modus In modusCounts
                                                                                             Dim percentage = (modus.Count / totalModus) * 100
                                                                                             cols.Item().Text($"Modus Operandi: {modus.Modus}, Occurrences: {modus.Count} ({percentage:F2}%)").FontSize(12)
                                                                                         Next
                                                                                         cols.Item().PaddingBottom(20)

                                                                                         ' Footer with generation time
                                                                                         cols.Item().AlignRight().Text($"Generated on {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}")
                                                                                     End Sub)
                                                           End Sub)
                                        End Sub).GeneratePdf(filepath)

        ' Return the file path of the generated report
        Return filepath
    End Function







    Private Function GenerateMedicalReport() As String
        ' Fetch the data table from database
        Dim dt As DataTable = modDB.GetTableData("medical")
        Dim report As New StringBuilder()

        ' Title
        report.AppendLine("Medical Report (Summary)".ToUpper() & Environment.NewLine)
        report.AppendLine("Generated on: " & DateTime.Now.ToString("MM/dd/yyyy") & Environment.NewLine)
        report.AppendLine(New String("-"c, 50) & Environment.NewLine)

        ' Blood Type Counts
        Dim bloodTypeCounts = dt.AsEnumerable() _
        .GroupBy(Function(row) row.Field(Of String)("blood_type")) _
        .Select(Function(g) New With {.BloodType = g.Key, .Count = g.Count()}) _
        .OrderByDescending(Function(b) b.Count).ToList()

        report.AppendLine("Most Prevalent Blood Types:" & Environment.NewLine)
        For Each blood In bloodTypeCounts
            report.AppendLine($"- Blood Type: {blood.BloodType}, Occurrences: {blood.Count}")
        Next
        report.AppendLine(Environment.NewLine)

        ' Chronic Illness Counts
        Dim chronicIllnessCounts = dt.AsEnumerable() _
        .GroupBy(Function(row) row.Field(Of String)("chronic_illnesses")) _
        .Select(Function(g) New With {.Illness = g.Key, .Count = g.Count()}) _
        .OrderByDescending(Function(c) c.Count).ToList()

        report.AppendLine("Most Common Chronic Illnesses:" & Environment.NewLine)
        For Each illness In chronicIllnessCounts
            report.AppendLine($"- Illness: {illness.Illness}, Occurrences: {illness.Count}")
        Next
        report.AppendLine(Environment.NewLine)

        ' Allergy Counts
        Dim allergyCounts = dt.AsEnumerable() _
        .GroupBy(Function(row) row.Field(Of String)("allergies")) _
        .Select(Function(g) New With {.Allergy = g.Key, .Count = g.Count()}) _
        .OrderByDescending(Function(a) a.Count).ToList()

        report.AppendLine("Most Common Allergies:" & Environment.NewLine)
        For Each allergy In allergyCounts
            report.AppendLine($"- Allergy: {allergy.Allergy}, Occurrences: {allergy.Count}")
        Next
        report.AppendLine(Environment.NewLine)

        ' Mental Health Status Counts
        Dim mentalHealthCounts = dt.AsEnumerable() _
        .GroupBy(Function(row) row.Field(Of String)("mental_health_status")) _
        .Select(Function(g) New With {.Status = g.Key, .Count = g.Count()}) _
        .OrderByDescending(Function(m) m.Count).ToList()

        report.AppendLine("Most Common Mental Health Status:" & Environment.NewLine)
        For Each status In mentalHealthCounts
            report.AppendLine($"- Mental Health Status: {status.Status}, Occurrences: {status.Count}")
        Next
        report.AppendLine(Environment.NewLine)

        ' Psychiatric Treatment Counts
        Dim psychiatricTreatmentCounts = dt.AsEnumerable() _
        .GroupBy(Function(row) row.Field(Of String)("psychiatric_treatment_required")) _
        .Select(Function(g) New With {.Treatment = g.Key, .Count = g.Count()}) _
        .OrderByDescending(Function(p) p.Count).ToList()

        report.AppendLine("Most Common Psychiatric Treatment Required:" & Environment.NewLine)
        For Each treatment In psychiatricTreatmentCounts
            report.AppendLine($"- Treatment: {treatment.Treatment}, Occurrences: {treatment.Count}")
        Next
        report.AppendLine(Environment.NewLine)

        ' Insurance Provider Counts
        Dim insuranceProviderCounts = dt.AsEnumerable() _
        .GroupBy(Function(row) row.Field(Of String)("insurance_provider")) _
        .Select(Function(g) New With {.Provider = g.Key, .Count = g.Count()}) _
        .OrderByDescending(Function(i) i.Count).ToList()

        report.AppendLine("Most Popular Insurance Providers:" & Environment.NewLine)
        For Each provider In insuranceProviderCounts
            report.AppendLine($"- Insurance Provider: {provider.Provider}, Occurrences: {provider.Count}")
        Next
        report.AppendLine(Environment.NewLine)

        ' Inborn Condition Counts
        Dim inbornConditionCounts = dt.AsEnumerable() _
        .GroupBy(Function(row) row.Field(Of String)("inborn_conditions")) _
        .Select(Function(g) New With {.Condition = g.Key, .Count = g.Count()}) _
        .OrderByDescending(Function(i) i.Count).ToList()

        report.AppendLine("Most Prevalent Inborn Conditions:" & Environment.NewLine)
        For Each condition In inbornConditionCounts
            report.AppendLine($"- Inborn Condition: {condition.Condition}, Occurrences: {condition.Count}")
        Next
        report.AppendLine(Environment.NewLine)

        ' Separator for readability
        report.AppendLine(New String("-"c, 50))

        Return report.ToString()
    End Function


    Public Function generatePdfForMedicalReport() As String
        ' Get the medical report as a string
        Dim medicalReport As String = GenerateMedicalReport()

        ' Generate a unique file name for the PDF
        Dim timestamp As String = DateTime.Now.ToString("yyyyMMddHHmmss")
        Dim filepath As String = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "reports", $"MedicalSummaryReport_{timestamp}.pdf")

        ' Ensure the directory exists
        If Not Directory.Exists(System.IO.Directory.GetCurrentDirectory() & "\medical_reports") Then
            Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() & "\medical_reports")
        End If

        ' Configure QuestPDF license
        QuestPDF.Settings.License = LicenseType.Community

        ' Create the PDF
        QuestPDF.Fluent.Document.Create(Sub(container)
                                            container.Page(Sub(page)
                                                               page.Size(PageSizes.A4)
                                                               page.Margin(2, Unit.Centimetre)
                                                               page.PageColor(Colors.White)

                                                               page.Header().Text("Medical Report Summary").FontSize(20).Bold().AlignCenter()
                                                               page.Content() _
                                                                .PaddingVertical(1, Unit.Centimetre) _
                                                                .Text(medicalReport).FontSize(12).LineHeight(1.5)
                                                           End Sub)
                                        End Sub).GeneratePdf(filepath)

        ' Return the file path
        Return filepath
    End Function







    Private Function GeneratePDLReleaseSummaryReport() As String
        ' Fetch data from the database (assuming modDB.GetTableData retrieves the DataTable)
        Dim dt As DataTable = modDB.GetTableData("inmatereleasedetails")

        ' If no data is available, return early with a message
        If dt.Rows.Count = 0 Then
            MessageBox.Show("No inmate release data available.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return String.Empty
        End If

        ' Generate a unique filename based on the current date and time
        Dim timenow As String = DateTime.Now.ToString("yyyyMMddHHmmss")
        Dim filepath As String = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "reports", $"PDLReleaseSummaryReport_{timenow}.pdf")

        ' Ensure the directory exists
        Dim reportsDir As String = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "reports")
        If Not Directory.Exists(reportsDir) Then
            Directory.CreateDirectory(reportsDir)
        End If

        ' Aggregate data for the summary
        Dim totalReleases As Integer = dt.Rows.Count
        Dim releasesByType = dt.AsEnumerable().
                         GroupBy(Function(row) row.Field(Of String)("type_of_release")).
                         Select(Function(g) New With {.Type = g.Key, .Count = g.Count()}).ToList()

        Dim officerStats = dt.AsEnumerable().
                       GroupBy(Function(row) row.Field(Of String)("officer_name")).
                       Select(Function(g) New With {.Officer = g.Key, .Handled = g.Count()}).ToList()

        ' Configure QuestPDF license
        QuestPDF.Settings.License = LicenseType.Community

        ' PDF Generation
        QuestPDF.Fluent.Document.Create(Sub(container)
                                            container.Page(Sub(page)
                                                               page.Size(PageSizes.A4)
                                                               page.Margin(2, Unit.Centimetre)
                                                               page.PageColor(Colors.White)

                                                               page.Content().Column(Sub(col)
                                                                                         ' Report Header
                                                                                         col.Item().Text("CellBlock Central").FontSize(28)
                                                                                         col.Item().Text("This report summarizes the releases of PDLs (Persons Deprived of Liberty), including their case details, release dates, and any associated conditions or requirements. It serves as a tool for managing transitions and ensuring the effective reintegration of individuals into society.").FontSize(10).Italic()

                                                                                         col.Item().Text("Summary Report of Released PDLs").Bold().FontSize(20).AlignCenter()
                                                                                         col.Item().Text($"Date Generated: {DateTime.Now:yyyy-MM-dd}").AlignCenter()
                                                                                         col.Item().PaddingBottom(20)

                                                                                         ' Overall Statistics
                                                                                         col.Item().Text("Overall Statistics").Bold().FontSize(14).Underline()
                                                                                         col.Item().Text($"Total Releases: {totalReleases}")
                                                                                         col.Item().Text("") ' Spacer

                                                                                         ' Breakdown by Type of Release
                                                                                         col.Item().Text("Breakdown by Type of Release").Bold()
                                                                                         For Each releaseType In releasesByType
                                                                                             col.Item().Text($"- {releaseType.Type}: {releaseType.Count}")
                                                                                         Next
                                                                                         col.Item().PaddingBottom(10)

                                                                                         ' Officer Contribution
                                                                                         col.Item().Text("Officer Contribution").Bold()
                                                                                         For Each officer In officerStats
                                                                                             col.Item().Text($"- {officer.Officer}: {officer.Handled} Releases")
                                                                                         Next
                                                                                         col.Item().PaddingBottom(10)

                                                                                         ' Footer Note
                                                                                         col.Item().AlignRight().Text($"Generated on {DateTime.Now:yyyy-MM-dd HH:mm:ss}").FontSize(10)
                                                                                     End Sub)
                                                           End Sub)
                                        End Sub).GeneratePdf(filepath)

        ' Return the file path of the generated report
        Return filepath
    End Function



    Public Function GenerateRecentInmateReleaseReport(startDate As DateTime, endDate As DateTime) As String
        ' Fetch data from the database (assuming modDB.GetTableData retrieves the DataTable)
        Dim dt1 As DataTable = modDB.GetTableData("inmatereleasedetails")

        ' Safely filter data by date range (release_date)
        Dim filteredRows = dt1.AsEnumerable().Where(Function(row)
                                                        Dim releaseDate As DateTime
                                                        If DateTime.TryParse(row.Field(Of String)("release_date"), releaseDate) Then
                                                            Return releaseDate >= startDate AndAlso releaseDate <= endDate
                                                        End If
                                                        Return False
                                                    End Function)

        ' Convert the filtered rows to a DataTable
        Dim filteredData As DataTable
        If filteredRows.Any() Then
            filteredData = filteredRows.CopyToDataTable()
        Else
            MessageBox.Show("No data found for the selected date range.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return String.Empty ' Return early if no data
        End If

        ' Basic report metadata
        Dim reportDate As String = DateTime.Now.ToString("yyyy-MM-dd")
        Dim totalReleases As Integer = filteredData.Rows.Count
        Dim timenow As String = DateTime.Now.ToString("yyyyMMddHHmmss")
        Dim filepath As String = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "reports", $"RecentInmateReleaseReport_{timenow}.pdf")

        ' Ensure directory exists
        Dim reportsDir As String = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "reports")
        If Not Directory.Exists(reportsDir) Then
            Directory.CreateDirectory(reportsDir)
        End If

        ' PDF Generation
        QuestPDF.Settings.License = LicenseType.Community
        QuestPDF.Fluent.Document.Create(Sub(container)
                                            container.Page(Sub(page)
                                                               page.Size(PageSizes.A4)
                                                               page.Margin(2, Unit.Centimetre)
                                                               page.PageColor(Colors.White)

                                                               page.Content().Column(Sub(cols)
                                                                                         ' Header with title
                                                                                         cols.Item().Text("CellBlock Central").FontSize(28)

                                                                                         cols.Item().Text("This report provides a summary of recent inmate releases, outlining the reasons for release, the individual's status upon release, and any follow-up actions required. It ensures that releases are processed smoothly and in accordance with legal and correctional standards.").FontSize(10).Italic()

                                                                                         cols.Item().AlignCenter().Text("Recent Inmate Release Report").Bold().FontSize(24).FontColor(Colors.Black).Underline()
                                                                                         ' Report metadata
                                                                                         cols.Item().Text($"Report Date: {reportDate}").FontSize(12).FontColor(Colors.Black)
                                                                                         cols.Item().Text($"Total Releases: {totalReleases}").FontSize(12).FontColor(Colors.Black)
                                                                                         cols.Item().PaddingBottom(20)

                                                                                         ' Releases section
                                                                                         For Each row As DataRow In filteredData.Rows
                                                                                             ' Extract details from each row
                                                                                             Dim pdlId As String = row("pdl_id").ToString()
                                                                                             Dim firstName As String = row("first_name").ToString()
                                                                                             Dim middleName As String = row("middle_name").ToString()
                                                                                             Dim lastName As String = row("last_name").ToString()
                                                                                             Dim sex As String = row("sex").ToString()
                                                                                             Dim releaseDate As String = row("release_date").ToString()
                                                                                             Dim releaseType As String = row("type_of_release").ToString()
                                                                                             Dim releaseReason As String = row("reason_release").ToString()
                                                                                             ' Placeholder variables for officer/inmate details
                                                                                             Dim officerName As String = "Officer Placeholder"
                                                                                             Dim officerPosition As String = "Position Placeholder"
                                                                                             Dim officerSignaturePath As String = "officer-signature.png"
                                                                                             Dim officerDateSigned As String = DateTime.Now.ToString("yyyy-MM-dd")
                                                                                             Dim inmateSignaturePath As String = "inmate-signature.png"
                                                                                             Dim dateSignedByInmate As String = DateTime.Now.ToString("yyyy-MM-dd")

                                                                                             ' Add inmate details to the report
                                                                                             cols.Spacing(10)
                                                                                             cols.Item().Text($"Date of Report: {DateTime.Now:yyyy-MM-dd}")
                                                                                             cols.Item().Text("Personal Details:").Bold()
                                                                                             cols.Item().Text($"PDL ID: {pdlId}")
                                                                                             cols.Item().Text($"Name: {firstName} {middleName} {lastName}")
                                                                                             cols.Item().Text($"Sex: {sex}")

                                                                                             cols.Item().Text("Release Details:").Bold()
                                                                                             cols.Item().Text($"Release Date: {releaseDate}")
                                                                                             cols.Item().Text($"Type of Release: {releaseType}")
                                                                                             cols.Item().Text($"Reason for Release: {releaseReason}")

                                                                                             cols.Item().Text("Authorization Information:").Bold()
                                                                                             cols.Item().Text($"Officer Name: {officerName}")
                                                                                             cols.Item().Text($"Position: {officerPosition}")

                                                                                             cols.Item().Text($"Date Signed by Officer: {officerDateSigned}")

                                                                                             cols.Item().Text("Inmate Acknowledgment:").Bold()

                                                                                             cols.Item().Text($"Date Signed by Inmate: {dateSignedByInmate}")
                                                                                         Next

                                                                                         ' Footer
                                                                                         cols.Item().AlignRight().Text($"Generated on {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
                                                                                     End Sub)
                                                           End Sub)
                                        End Sub).GeneratePdf(filepath)

        ' Return the file path of the generated report
        Return filepath
    End Function

    Private Sub dtStartDate_ValueChanged(sender As Object, e As EventArgs) Handles dtStartDate.ValueChanged
        FilterAndGenerateReport()
    End Sub

    Private Sub dtEndDate_ValueChanged(sender As Object, e As EventArgs) Handles dtEndDate.ValueChanged
        FilterAndGenerateReport()
    End Sub

    Private Sub FilterAndGenerateReport()
        Dim startDate As DateTime = dtStartDate.Value
        Dim endDate As DateTime = dtEndDate.Value

    End Sub
End Class
