Public Class SystemConfig

    Dim connectionString As String
    Private Sub SystemConfig_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Ensure the form is always on top
        Me.TopMost = True
    End Sub

    Private Function GetConnectionString() As String
        ' Retrieve the values from the text fields
        Dim server As String = txtServer.Text
        Dim uid As String = txtUid.Text
        Dim password As String = txtPassword.Text
        Dim database As String = txtDatabase.Text

        ' Build the connection string
        connectionString = $"Server={server};Uid={uid};Pwd={password};Database={database};"
        Return connectionString
    End Function

    ' Example usage of the GetConnectionString method
    Private Sub btnTestConnection_Click(sender As Object, e As EventArgs) Handles btnTestConn.Click
        Try
            ' Get the connection string
            connectionString = GetConnectionString()

            ' Use the connection string to test the connection
            Using connection As New MySql.Data.MySqlClient.MySqlConnection(connectionString)
                connection.Open()
                MessageBox.Show("Connection successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Using
        Catch ex As Exception
            MessageBox.Show($"Connection failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSaveConfig_Click(sender As Object, e As EventArgs) Handles btnSaveConfig.Click
        Try
            ' Get the connection string
            connectionString = GetConnectionString()

            ' Test the connection
            Using connection As New MySql.Data.MySqlClient.MySqlConnection(connectionString)
                connection.Open()
                ' Connection successful
            End Using

            ' If successful, save the configuration
            MessageBox.Show("Configuration saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Set the global connection string or use it as needed
            strConnection = connectionString
            Me.Close()

        Catch ex As Exception
            ' Show error if the connection fails
            MessageBox.Show($"Connection failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
