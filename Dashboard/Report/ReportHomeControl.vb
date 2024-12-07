Imports QuestPDF.Fluent
Imports QuestPDF.Helpers
Imports QuestPDF.Infrastructure
Imports System.IO
Imports System.Text
Imports DocumentFormat.OpenXml.Wordprocessing
Imports System.Runtime.InteropServices.ComTypes


Public Class ReportHomeControl
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
                Case "PDL Release Report"
                    savePath = GeneratePDLReleaseReport()
                Case "Staff Population Summary Report"
                    savePath = GenerateStaffPopulationReport()
                Case "Recent Crimes"
                    Dim startDate As DateTime = dtStartDate.Value
                    Dim endDate As DateTime = dtEndDate.Value
                    savePath = GenerateRecentCrimesReport(startDate, endDate)
                Case "Incident Crime Summary Report"
                    savePath = GenerateIncidentReport()
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
            Case "PDL Release Report"
                Return GeneratePDLReleaseReport()
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

    Private Function GeneratePdfForPDLPopulationReport(dt As DataTable) As String

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
                                                                                         cols.Item().AlignCenter().Text("PDL Population Report").Bold().FontSize(24)
                                                                                         cols.Item().Text($"Report Date: {reportDate}").FontSize(12)
                                                                                         cols.Item().Text($"Total PDLs: {totalPDLs}").FontSize(12)

                                                                                         ' Section: Sex
                                                                                         cols.Item().Text("Most Common Sex:").Bold().FontSize(14)
                                                                                         For Each sex In sexCounts
                                                                                             cols.Item().Text($"Sex: {sex.Key}, Count: {sex.Count}").FontSize(12)
                                                                                         Next

                                                                                         ' Section: Civil Status
                                                                                         cols.Item().Text("Most Common Civil Status:").Bold().FontSize(14)
                                                                                         For Each status In civilStatusCounts
                                                                                             cols.Item().Text($"Status: {status.Key}, Count: {status.Count}").FontSize(12)
                                                                                         Next

                                                                                         ' Section: Country
                                                                                         cols.Item().Text("Most Common Country:").Bold().FontSize(14)
                                                                                         For Each country In countryCounts
                                                                                             cols.Item().Text($"Country: {country.Key}, Count: {country.Count}").FontSize(12)
                                                                                         Next

                                                                                         ' Section: Municipality
                                                                                         cols.Item().Text("Most Common Municipality:").Bold().FontSize(14)
                                                                                         For Each municipality In municipalityCounts
                                                                                             cols.Item().Text($"Municipality: {municipality.Key}, Count: {municipality.Count}").FontSize(12)
                                                                                         Next

                                                                                         ' Section: Hair Color
                                                                                         cols.Item().Text("Most Common Hair Colors:").Bold().FontSize(14)
                                                                                         For Each hair In hairColorCounts
                                                                                             cols.Item().Text($"Hair Color: {hair.Key}, Count: {hair.Count}").FontSize(12)
                                                                                         Next

                                                                                         ' Footer
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
                                                                                         cols.Item().AlignCenter().Text("Staff Population Report").Bold().FontSize(24)
                                                                                         cols.Item().Text($"Report Date: {reportDate}").FontSize(12)
                                                                                         cols.Item().Text($"Total Staff: {totalStaff}").FontSize(12)

                                                                                         ' Section: Municipality
                                                                                         cols.Item().Text("Most Common Municipality:").Bold().FontSize(14)
                                                                                         For Each municipality In municipalityCounts
                                                                                             cols.Item().Text($"Municipality: {municipality.Key}, Count: {municipality.Count}").FontSize(12)
                                                                                         Next

                                                                                         ' Section: City
                                                                                         cols.Item().Text("Most Common City:").Bold().FontSize(14)
                                                                                         For Each city In cityCounts
                                                                                             cols.Item().Text($"City: {city.Key}, Count: {city.Count}").FontSize(12)
                                                                                         Next

                                                                                         ' Section: Region
                                                                                         cols.Item().Text("Most Common Region:").Bold().FontSize(14)
                                                                                         For Each regionItem In regionCounts
                                                                                             cols.Item().Text($"Region: {regionItem.Key}, Count: {regionItem.Count}").FontSize(12)
                                                                                         Next

                                                                                         ' Section: Gender
                                                                                         cols.Item().Text("Most Common Gender:").Bold().FontSize(14)
                                                                                         For Each gender In genderCounts
                                                                                             cols.Item().Text($"Gender: {gender.Key}, Count: {gender.Count}").FontSize(12)
                                                                                         Next

                                                                                         ' Section: Status
                                                                                         cols.Item().Text("Most Common Status:").Bold().FontSize(14)
                                                                                         For Each status In statusCounts
                                                                                             cols.Item().Text($"Status: {status.Key}, Count: {status.Count}").FontSize(12)
                                                                                         Next

                                                                                         ' Section: Position
                                                                                         cols.Item().Text("Most Common Position:").Bold().FontSize(14)
                                                                                         For Each position In positionCounts
                                                                                             cols.Item().Text($"Position: {position.Key}, Count: {position.Count}").FontSize(12)
                                                                                         Next

                                                                                         ' Section: Authority
                                                                                         cols.Item().Text("Most Common Authority:").Bold().FontSize(14)
                                                                                         For Each authority In authorityCounts
                                                                                             cols.Item().Text($"Authority: {authority.Key}, Count: {authority.Count}").FontSize(12)
                                                                                         Next

                                                                                         ' Footer
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

    Private Function GenerateIncidentReport(dt As DataTable) As String
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
                        .Select(Function(g) New With {.Crime = g.Key, .Count = g.Count()}) _
                        .OrderByDescending(Function(x) x.Count) ' Sort by the count of each crime, most common first

        ' Get the recent incidents by crime_id (sorting by crime_id assuming higher ID = more recent)
        Dim recentIncidents = dt.AsEnumerable() _
                            .OrderByDescending(Function(row) row.Field(Of Integer)("crime_id")) _
                            .Take(5) ' Limit to the top 5 recent incidents (can adjust this as needed)

        ' PDF Generation
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community
        QuestPDF.Fluent.Document.Create(Sub(container)
                                            container.Page(Sub(page)
                                                               page.Size(PageSizes.A4)
                                                               page.Margin(2, Unit.Centimetre)
                                                               page.PageColor(Colors.White)

                                                               ' Content
                                                               page.Content().Column(Sub(cols)
                                                                                         cols.Item().AlignCenter().Text("Recent Incident Report").Bold().FontSize(24)
                                                                                         cols.Item().Text($"Report Date: {reportDate}").FontSize(12)
                                                                                         cols.Item().Text($"Total Incidents: {totalIncidents}").FontSize(12)

                                                                                         ' Section: Most Common Crimes
                                                                                         cols.Item().Text("Most Crimes:").Bold().FontSize(14)
                                                                                         For Each crimes In crimeCounts
                                                                                             cols.Item().Text($"Crime: {crimes.Crime}, Count: {crimes.Count}").FontSize(12)
                                                                                         Next

                                                                                         ' Footer
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
                                                                                         cols.Item().AlignCenter().Text("Criminal Case Report").Bold().FontSize(24).FontColor(Colors.Black).Underline()
                                                                                         ' Report metadata

                                                                                         cols.Item().Text($"Report Date: {reportDate}").FontSize(12).FontColor(Colors.Black)
                                                                                         cols.Item().Text($"Total Cases: {totalCases}").FontSize(12).FontColor(Colors.Black)
                                                                                         cols.Item().PaddingBottom(20)

                                                                                         ' Most Common Offenses Section
                                                                                         cols.Item().Text("Most Common Offenses:").Bold().FontColor(Colors.Black)
                                                                                         cols.Item().PaddingBottom(10)
                                                                                         Dim offenseCounts = dt1.AsEnumerable().
                                                                                                        GroupBy(Function(row) row.Field(Of String)("offence_charge")).
                                                                                                        Select(Function(g) New With {.Offense = g.Key, .Count = g.Count()}).
                                                                                                        OrderByDescending(Function(o) o.Count)
                                                                                         Dim totalOffenses = offenseCounts.Sum(Function(o) o.Count)
                                                                                         For Each offense In offenseCounts
                                                                                             Dim percentage = (offense.Count / totalOffenses) * 100
                                                                                             cols.Item().Text($"Offense: {offense.Offense}, Occurrences: {offense.Count} ({percentage:F2}%)").FontSize(12)
                                                                                         Next
                                                                                         cols.Item().PaddingBottom(20)

                                                                                         ' Most Common Modus Operandi Section
                                                                                         cols.Item().Text("Most Common Modus Operandi:").Bold().FontColor(Colors.Black)
                                                                                         cols.Item().PaddingBottom(10)
                                                                                         Dim modusCounts = dt1.AsEnumerable().
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
        Dim dt As DataTable = modDB.GetTableData("medical")
        Dim report As New StringBuilder("Medical Report (Summary)" & Environment.NewLine & Environment.NewLine)

        ' Grouping and counting occurrences of common data for each field

        ' Blood Type
        Dim bloodTypeCounts = dt.AsEnumerable() _
        .GroupBy(Function(row) row.Field(Of String)("blood_type")) _
        .Select(Function(g) New With {.BloodType = g.Key, .Count = g.Count()}) _
        .OrderByDescending(Function(b) b.Count).ToList()

        ' Chronic Illnesses
        Dim chronicIllnessCounts = dt.AsEnumerable() _
        .GroupBy(Function(row) row.Field(Of String)("chronic_illnesses")) _
        .Select(Function(g) New With {.Illness = g.Key, .Count = g.Count()}) _
        .OrderByDescending(Function(c) c.Count).ToList()

        ' Allergies
        Dim allergyCounts = dt.AsEnumerable() _
        .GroupBy(Function(row) row.Field(Of String)("allergies")) _
        .Select(Function(g) New With {.Allergy = g.Key, .Count = g.Count()}) _
        .OrderByDescending(Function(a) a.Count).ToList()

        ' Mental Health Status
        Dim mentalHealthCounts = dt.AsEnumerable() _
        .GroupBy(Function(row) row.Field(Of String)("mental_health_status")) _
        .Select(Function(g) New With {.Status = g.Key, .Count = g.Count()}) _
        .OrderByDescending(Function(m) m.Count).ToList()

        ' Psychiatric Treatment Required
        Dim psychiatricTreatmentCounts = dt.AsEnumerable() _
        .GroupBy(Function(row) row.Field(Of String)("psychiatric_treatment_required")) _
        .Select(Function(g) New With {.Treatment = g.Key, .Count = g.Count()}) _
        .OrderByDescending(Function(p) p.Count).ToList()

        ' Insurance Provider
        Dim insuranceProviderCounts = dt.AsEnumerable() _
        .GroupBy(Function(row) row.Field(Of String)("insurance_provider")) _
        .Select(Function(g) New With {.Provider = g.Key, .Count = g.Count()}) _
        .OrderByDescending(Function(i) i.Count).ToList()

        ' Inborn Conditions
        Dim inbornConditionCounts = dt.AsEnumerable() _
        .GroupBy(Function(row) row.Field(Of String)("inborn_conditions")) _
        .Select(Function(g) New With {.Condition = g.Key, .Count = g.Count()}) _
        .OrderByDescending(Function(i) i.Count).ToList()

        ' Most common data for each field
        report.AppendLine("Most Common Blood Types:")
        For Each blood In bloodTypeCounts
            report.AppendLine($"Blood Type: {blood.BloodType}, Occurrences: {blood.Count}")
        Next

        report.AppendLine(Environment.NewLine & "Most Common Chronic Illnesses:")
        For Each illness In chronicIllnessCounts
            report.AppendLine($"Illness: {illness.Illness}, Occurrences: {illness.Count}")
        Next

        report.AppendLine(Environment.NewLine & "Most Common Allergies:")
        For Each allergy In allergyCounts
            report.AppendLine($"Allergy: {allergy.Allergy}, Occurrences: {allergy.Count}")
        Next

        report.AppendLine(Environment.NewLine & "Most Common Mental Health Status:")
        For Each status In mentalHealthCounts
            report.AppendLine($"Mental Health Status: {status.Status}, Occurrences: {status.Count}")
        Next

        report.AppendLine(Environment.NewLine & "Most Common Psychiatric Treatment Required:")
        For Each treatment In psychiatricTreatmentCounts
            report.AppendLine($"Treatment Required: {treatment.Treatment}, Occurrences: {treatment.Count}")
        Next

        report.AppendLine(Environment.NewLine & "Most Common Insurance Providers:")
        For Each provider In insuranceProviderCounts
            report.AppendLine($"Insurance Provider: {provider.Provider}, Occurrences: {provider.Count}")
        Next

        report.AppendLine(Environment.NewLine & "Most Common Inborn Conditions:")
        For Each condition In inbornConditionCounts
            report.AppendLine($"Inborn Condition: {condition.Condition}, Occurrences: {condition.Count}")
        Next

        ' Separator for readability
        report.AppendLine(New String("-"c, 50))

        Return report.ToString()
    End Function

    Public Function generatePdfForMedicalReport() As String
        ' Get the medical report as a string
        Dim medicalReport As String = GenerateMedicalReport()

        ' Generate a unique file name for the PDF
        Dim timestamp As String = DateTime.Now.ToString("yyyyMMddHHmmss")
        Dim filePath As String = System.IO.Directory.GetCurrentDirectory() & "\medical_reports\medical_report_" & timestamp & ".pdf"

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
                                        End Sub).GeneratePdf(filePath)

        ' Return the file path
        Return filePath
    End Function


    Private Function GeneratePDLReleaseReport() As String
        ' Fetch data from the database (example: GetTableData method should be implemented elsewhere)
        Dim dt As DataTable = modDB.GetTableData("inmatereleasedetails")

        ' Generate a unique filename based on the current date and time
        Dim timenow As String = DateTime.Now.ToString("yyyyMMddHHmmss")
        Dim filepath As String = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "reports", $"PDLReleaseReport_{timenow}.pdf")

        ' Ensure directory exists
        Dim reportsDir As String = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "reports")
        If Not Directory.Exists(reportsDir) Then
            Directory.CreateDirectory(reportsDir)
        End If

        ' Configure QuestPDF license
        QuestPDF.Settings.License = LicenseType.Community

        ' PDF Generation
        QuestPDF.Fluent.Document.Create(Sub(container)
                                            container.Page(Sub(page)
                                                               page.Size(PageSizes.A4)
                                                               page.Margin(2, Unit.Centimetre)
                                                               page.PageColor(Colors.White)

                                                               ' Add report title
                                                               page.Content().Column(Sub(col)
                                                                                         col.Item().Text("Released PDLs").Bold().FontSize(16)
                                                                                         col.Item().Text("") ' Space between sections

                                                                                         ' Iterate through each row in the DataTable to add details
                                                                                         For Each row As DataRow In dt.Rows
                                                                                             col.Item().Text($"Release ID: {row("release_id")}")
                                                                                             col.Item().Text($"PDL ID: {row("pdl_id")}")
                                                                                             col.Item().Text($"Name: {row("first_name")} {row("middle_name")} {row("last_name")}")
                                                                                             col.Item().Text($"Sex: {row("sex")}")
                                                                                             col.Item().Text($"Release Date: {row("release_date")}")
                                                                                             col.Item().Text($"Type of Release: {row("type_of_release")}")
                                                                                             col.Item().Text($"Reason for Release: {row("reason_release")}")
                                                                                             col.Item().Text($"Officer Name: {row("officer_name")}")
                                                                                             col.Item().Text($"Officer Position: {row("officer_position")}")
                                                                                             col.Item().Text(New String("-"c, 50)) ' Separator for readability
                                                                                         Next
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