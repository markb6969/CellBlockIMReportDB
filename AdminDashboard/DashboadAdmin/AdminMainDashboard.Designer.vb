<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AdminMainDashboard
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblTotalText = New System.Windows.Forms.Label()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnMainDB = New System.Windows.Forms.Button()
        Me.btnAddEntity = New System.Windows.Forms.Button()
        Me.btnReport = New System.Windows.Forms.Button()
        Me.btnManageInmate = New System.Windows.Forms.Button()
        Me.btnManageUser = New System.Windows.Forms.Button()
        Me.btnSettings = New System.Windows.Forms.Button()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.dgvCellblockInfo = New System.Windows.Forms.DataGridView()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.Panel4.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FlowLayoutPanel1.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvCellblockInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Controls.Add(Me.dgvCellblockInfo)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(4, 339)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(4, 4, 133, 62)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1159, 269)
        Me.Panel2.TabIndex = 1
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.PictureBox2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4, 4, 9, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(566, 319)
        Me.Panel1.TabIndex = 0
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.Panel1, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Panel3, 1, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(4, 4)
        Me.TableLayoutPanel2.Margin = New System.Windows.Forms.Padding(4, 4, 133, 4)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(1159, 327)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.White
        Me.Panel3.Controls.Add(Me.PictureBox3)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(583, 4)
        Me.Panel3.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(572, 319)
        Me.Panel3.TabIndex = 1
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel2, 0, 1)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(4, 4)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1296, 670)
        Me.TableLayoutPanel1.TabIndex = 25
        '
        'tlpMain
        '
        Me.tlpMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.ColumnCount = 1
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.Controls.Add(Me.TableLayoutPanel1, 0, 0)
        Me.tlpMain.Location = New System.Drawing.Point(341, 256)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 1
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.Size = New System.Drawing.Size(1304, 678)
        Me.tlpMain.TabIndex = 30
        '
        'lblTotal
        '
        Me.lblTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Font = New System.Drawing.Font("Poppins", 39.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.Location = New System.Drawing.Point(1297, 50)
        Me.lblTotal.Margin = New System.Windows.Forms.Padding(4, 0, 67, 0)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(93, 118)
        Me.lblTotal.TabIndex = 31
        Me.lblTotal.Text = "0"
        Me.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 48.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(320, 34)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(437, 91)
        Me.Label1.TabIndex = 32
        Me.Label1.Text = "Dashboard"
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.PictureBox1)
        Me.Panel4.Location = New System.Drawing.Point(0, 4)
        Me.Panel4.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(333, 255)
        Me.Panel4.TabIndex = 0
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = Global.CellBlockIM.My.Resources.Resources.cellblock_central__1_
        Me.PictureBox1.Location = New System.Drawing.Point(63, 11)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(212, 201)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 19
        Me.PictureBox1.TabStop = False
        '
        'lblTotalText
        '
        Me.lblTotalText.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotalText.AutoSize = True
        Me.lblTotalText.Font = New System.Drawing.Font("Poppins", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalText.Location = New System.Drawing.Point(1171, 145)
        Me.lblTotalText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotalText.Name = "lblTotalText"
        Me.lblTotalText.Size = New System.Drawing.Size(339, 30)
        Me.lblTotalText.TabIndex = 34
        Me.lblTotalText.Text = "Total Persons Deprived of Liberty (PDL)"
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.CustomFormat = "MMMM, dd, yyyy"
        Me.DateTimePicker1.Enabled = False
        Me.DateTimePicker1.Font = New System.Drawing.Font("Poppins", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker1.Location = New System.Drawing.Point(345, 145)
        Me.DateTimePicker1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(265, 32)
        Me.DateTimePicker1.TabIndex = 33
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.Panel4)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnMainDB)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnAddEntity)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnReport)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnManageInmate)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnManageUser)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnSettings)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(337, 937)
        Me.FlowLayoutPanel1.TabIndex = 35
        '
        'btnMainDB
        '
        Me.btnMainDB.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.btnMainDB.FlatAppearance.BorderSize = 0
        Me.btnMainDB.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMainDB.Image = Global.CellBlockIM.My.Resources.Resources.grid__1_
        Me.btnMainDB.Location = New System.Drawing.Point(0, 267)
        Me.btnMainDB.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnMainDB.Name = "btnMainDB"
        Me.btnMainDB.Size = New System.Drawing.Size(333, 92)
        Me.btnMainDB.TabIndex = 1
        Me.btnMainDB.UseVisualStyleBackColor = False
        '
        'btnAddEntity
        '
        Me.btnAddEntity.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.btnAddEntity.FlatAppearance.BorderSize = 0
        Me.btnAddEntity.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAddEntity.Image = Global.CellBlockIM.My.Resources.Resources.plus
        Me.btnAddEntity.Location = New System.Drawing.Point(0, 367)
        Me.btnAddEntity.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnAddEntity.Name = "btnAddEntity"
        Me.btnAddEntity.Size = New System.Drawing.Size(333, 92)
        Me.btnAddEntity.TabIndex = 1
        Me.btnAddEntity.UseVisualStyleBackColor = False
        '
        'btnReport
        '
        Me.btnReport.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.btnReport.FlatAppearance.BorderSize = 0
        Me.btnReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReport.Image = Global.CellBlockIM.My.Resources.Resources.report
        Me.btnReport.Location = New System.Drawing.Point(0, 467)
        Me.btnReport.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnReport.Name = "btnReport"
        Me.btnReport.Size = New System.Drawing.Size(333, 92)
        Me.btnReport.TabIndex = 1
        Me.btnReport.UseVisualStyleBackColor = False
        '
        'btnManageInmate
        '
        Me.btnManageInmate.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.btnManageInmate.FlatAppearance.BorderSize = 0
        Me.btnManageInmate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnManageInmate.Image = Global.CellBlockIM.My.Resources.Resources.inmate__1_
        Me.btnManageInmate.Location = New System.Drawing.Point(0, 567)
        Me.btnManageInmate.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnManageInmate.Name = "btnManageInmate"
        Me.btnManageInmate.Size = New System.Drawing.Size(333, 92)
        Me.btnManageInmate.TabIndex = 1
        Me.btnManageInmate.UseVisualStyleBackColor = False
        '
        'btnManageUser
        '
        Me.btnManageUser.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.btnManageUser.FlatAppearance.BorderSize = 0
        Me.btnManageUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnManageUser.Image = Global.CellBlockIM.My.Resources.Resources.setting__3_
        Me.btnManageUser.Location = New System.Drawing.Point(0, 667)
        Me.btnManageUser.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnManageUser.Name = "btnManageUser"
        Me.btnManageUser.Size = New System.Drawing.Size(333, 92)
        Me.btnManageUser.TabIndex = 1
        Me.btnManageUser.UseVisualStyleBackColor = False
        '
        'btnSettings
        '
        Me.btnSettings.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.btnSettings.FlatAppearance.BorderSize = 0
        Me.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSettings.Image = Global.CellBlockIM.My.Resources.Resources.setting__2_
        Me.btnSettings.Location = New System.Drawing.Point(0, 767)
        Me.btnSettings.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnSettings.Name = "btnSettings"
        Me.btnSettings.Size = New System.Drawing.Size(333, 92)
        Me.btnSettings.TabIndex = 1
        Me.btnSettings.UseVisualStyleBackColor = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = Global.CellBlockIM.My.Resources.Resources.Screenshot_2024_12_05_133723
        Me.PictureBox2.Location = New System.Drawing.Point(88, 23)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(390, 273)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox2.TabIndex = 1
        Me.PictureBox2.TabStop = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox3.Image = Global.CellBlockIM.My.Resources.Resources.Screenshot_2024_12_05_133742
        Me.PictureBox3.Location = New System.Drawing.Point(91, 23)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(390, 273)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox3.TabIndex = 1
        Me.PictureBox3.TabStop = False
        '
        'dgvCellblockInfo
        '
        Me.dgvCellblockInfo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvCellblockInfo.BackgroundColor = System.Drawing.Color.WhiteSmoke
        Me.dgvCellblockInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCellblockInfo.Location = New System.Drawing.Point(42, 29)
        Me.dgvCellblockInfo.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvCellblockInfo.Name = "dgvCellblockInfo"
        Me.dgvCellblockInfo.ReadOnly = True
        Me.dgvCellblockInfo.RowHeadersWidth = 51
        Me.dgvCellblockInfo.Size = New System.Drawing.Size(1074, 211)
        Me.dgvCellblockInfo.TabIndex = 23
        '
        'AdminMainDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1645, 937)
        Me.Controls.Add(Me.tlpMain)
        Me.Controls.Add(Me.lblTotalText)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.Label1)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "AdminMainDashboard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.tlpMain.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvCellblockInfo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents lblTotal As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel4 As Panel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents btnMainDB As Button
    Friend WithEvents btnAddEntity As Button
    Friend WithEvents btnReport As Button
    Friend WithEvents lblTotalText As Label
    Friend WithEvents DateTimePicker1 As DateTimePicker
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents btnManageInmate As Button
    Friend WithEvents btnManageUser As Button
    Friend WithEvents btnSettings As Button
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents dgvCellblockInfo As DataGridView
    Friend WithEvents PictureBox3 As PictureBox
End Class
