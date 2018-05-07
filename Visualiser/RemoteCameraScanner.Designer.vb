<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RemoteCameraScanner
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RemoteCameraScanner))
        Me.lstIPAddresses = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnScan = New System.Windows.Forms.Button()
        Me.txtPort = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lstFound = New System.Windows.Forms.ListBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.progressBar = New System.Windows.Forms.ProgressBar()
        Me.timerUpdateProgress = New System.Windows.Forms.Timer(Me.components)
        Me.txtAddressFormat = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lstIPAddresses
        '
        Me.lstIPAddresses.FormattingEnabled = True
        Me.lstIPAddresses.ItemHeight = 25
        Me.lstIPAddresses.Location = New System.Drawing.Point(12, 37)
        Me.lstIPAddresses.Name = "lstIPAddresses"
        Me.lstIPAddresses.Size = New System.Drawing.Size(305, 204)
        Me.lstIPAddresses.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(197, 25)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Local IP Addresses"
        '
        'btnScan
        '
        Me.btnScan.Location = New System.Drawing.Point(17, 320)
        Me.btnScan.Name = "btnScan"
        Me.btnScan.Size = New System.Drawing.Size(300, 45)
        Me.btnScan.TabIndex = 2
        Me.btnScan.Text = "Scan"
        Me.btnScan.UseVisualStyleBackColor = True
        '
        'txtPort
        '
        Me.txtPort.Location = New System.Drawing.Point(17, 283)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(300, 31)
        Me.txtPort.TabIndex = 3
        Me.txtPort.Text = "4747"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 255)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(135, 25)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Remote port:"
        '
        'lstFound
        '
        Me.lstFound.FormattingEnabled = True
        Me.lstFound.ItemHeight = 25
        Me.lstFound.Location = New System.Drawing.Point(332, 37)
        Me.lstFound.Name = "lstFound"
        Me.lstFound.Size = New System.Drawing.Size(305, 279)
        Me.lstFound.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(336, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(209, 25)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Discovered Devices:"
        '
        'progressBar
        '
        Me.progressBar.Location = New System.Drawing.Point(332, 320)
        Me.progressBar.Name = "progressBar"
        Me.progressBar.Size = New System.Drawing.Size(305, 45)
        Me.progressBar.TabIndex = 7
        '
        'timerUpdateProgress
        '
        Me.timerUpdateProgress.Enabled = True
        '
        'txtAddressFormat
        '
        Me.txtAddressFormat.Location = New System.Drawing.Point(17, 424)
        Me.txtAddressFormat.Name = "txtAddressFormat"
        Me.txtAddressFormat.Size = New System.Drawing.Size(311, 31)
        Me.txtAddressFormat.TabIndex = 8
        Me.txtAddressFormat.Text = "http://[ip]:[port]/mjpegfeed"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 396)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(163, 25)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Address format:"
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(498, 424)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(139, 47)
        Me.btnOK.TabIndex = 10
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(341, 424)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(139, 47)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'RemoteCameraScanner
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(650, 499)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtAddressFormat)
        Me.Controls.Add(Me.progressBar)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lstFound)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtPort)
        Me.Controls.Add(Me.btnScan)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lstIPAddresses)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "RemoteCameraScanner"
        Me.Text = "RemoteCameraScanner"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lstIPAddresses As ListBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnScan As Button
    Friend WithEvents txtPort As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents lstFound As ListBox
    Friend WithEvents Label3 As Label
    Friend WithEvents progressBar As ProgressBar
    Friend WithEvents timerUpdateProgress As Timer
    Friend WithEvents txtAddressFormat As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents btnOK As Button
    Friend WithEvents btnCancel As Button
End Class
