<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Visualiser
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Visualiser))
        Me.statusBar = New System.Windows.Forms.StatusStrip()
        Me.lblStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.timerRefresh = New System.Windows.Forms.Timer(Me.components)
        Me.mainMenu = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CameraToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChooseCameraCtrl1ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConnectToStreamCtrl2ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditPropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToggleQuality1ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PlayPauseSpaceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AnnotationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UndoCtrlZToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LineLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RectangleRToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ScribbleSToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PickColourPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClearCToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ZoomToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Reset0ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FullScreenFToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToogleToolbarTToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.picPreview = New System.Windows.Forms.PictureBox()
        Me.btnChangeColour = New System.Windows.Forms.ToolStripDropDownButton()
        Me.PickColourToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BlueToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BlackToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnChangeShape = New System.Windows.Forms.ToolStripDropDownButton()
        Me.ClearToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RectangleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LineToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ScribbleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.statusBar.SuspendLayout()
        Me.mainMenu.SuspendLayout()
        CType(Me.picPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'statusBar
        '
        Me.statusBar.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.statusBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatus, Me.btnChangeColour, Me.btnChangeShape})
        Me.statusBar.Location = New System.Drawing.Point(0, 649)
        Me.statusBar.Name = "statusBar"
        Me.statusBar.Size = New System.Drawing.Size(854, 38)
        Me.statusBar.TabIndex = 0
        Me.statusBar.Text = "StatusStrip1"
        '
        'lblStatus
        '
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(120, 33)
        Me.lblStatus.Text = "Status bar"
        '
        'timerRefresh
        '
        Me.timerRefresh.Enabled = True
        Me.timerRefresh.Interval = 1000
        '
        'mainMenu
        '
        Me.mainMenu.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.mainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.CameraToolStripMenuItem, Me.AnnotationsToolStripMenuItem, Me.ZoomToolStripMenuItem})
        Me.mainMenu.Location = New System.Drawing.Point(0, 0)
        Me.mainMenu.Name = "mainMenu"
        Me.mainMenu.Size = New System.Drawing.Size(854, 40)
        Me.mainMenu.TabIndex = 1
        Me.mainMenu.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(64, 36)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(190, 38)
        Me.ExitToolStripMenuItem.Text = "E&xit (Q)"
        '
        'CameraToolStripMenuItem
        '
        Me.CameraToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ChooseCameraCtrl1ToolStripMenuItem, Me.ConnectToStreamCtrl2ToolStripMenuItem, Me.EditPropertiesToolStripMenuItem, Me.ToggleQuality1ToolStripMenuItem, Me.PlayPauseSpaceToolStripMenuItem})
        Me.CameraToolStripMenuItem.Name = "CameraToolStripMenuItem"
        Me.CameraToolStripMenuItem.Size = New System.Drawing.Size(108, 36)
        Me.CameraToolStripMenuItem.Text = "&Camera"
        '
        'ChooseCameraCtrl1ToolStripMenuItem
        '
        Me.ChooseCameraCtrl1ToolStripMenuItem.Name = "ChooseCameraCtrl1ToolStripMenuItem"
        Me.ChooseCameraCtrl1ToolStripMenuItem.Size = New System.Drawing.Size(414, 38)
        Me.ChooseCameraCtrl1ToolStripMenuItem.Text = "&Choose Camera (Ctrl + 1)"
        '
        'ConnectToStreamCtrl2ToolStripMenuItem
        '
        Me.ConnectToStreamCtrl2ToolStripMenuItem.Name = "ConnectToStreamCtrl2ToolStripMenuItem"
        Me.ConnectToStreamCtrl2ToolStripMenuItem.Size = New System.Drawing.Size(414, 38)
        Me.ConnectToStreamCtrl2ToolStripMenuItem.Text = "Connect to Stream (Ctrl + 2)"
        '
        'EditPropertiesToolStripMenuItem
        '
        Me.EditPropertiesToolStripMenuItem.Name = "EditPropertiesToolStripMenuItem"
        Me.EditPropertiesToolStripMenuItem.Size = New System.Drawing.Size(414, 38)
        Me.EditPropertiesToolStripMenuItem.Text = "&Edit properties (E)"
        '
        'ToggleQuality1ToolStripMenuItem
        '
        Me.ToggleQuality1ToolStripMenuItem.Name = "ToggleQuality1ToolStripMenuItem"
        Me.ToggleQuality1ToolStripMenuItem.Size = New System.Drawing.Size(414, 38)
        Me.ToggleQuality1ToolStripMenuItem.Text = "&Toggle quality (1)"
        '
        'PlayPauseSpaceToolStripMenuItem
        '
        Me.PlayPauseSpaceToolStripMenuItem.Name = "PlayPauseSpaceToolStripMenuItem"
        Me.PlayPauseSpaceToolStripMenuItem.Size = New System.Drawing.Size(414, 38)
        Me.PlayPauseSpaceToolStripMenuItem.Text = "&Play / Pause (Space)"
        '
        'AnnotationsToolStripMenuItem
        '
        Me.AnnotationsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UndoCtrlZToolStripMenuItem, Me.LineLToolStripMenuItem, Me.RectangleRToolStripMenuItem, Me.ScribbleSToolStripMenuItem, Me.PickColourPToolStripMenuItem, Me.ClearCToolStripMenuItem})
        Me.AnnotationsToolStripMenuItem.Name = "AnnotationsToolStripMenuItem"
        Me.AnnotationsToolStripMenuItem.Size = New System.Drawing.Size(156, 36)
        Me.AnnotationsToolStripMenuItem.Text = "&Annotations"
        '
        'UndoCtrlZToolStripMenuItem
        '
        Me.UndoCtrlZToolStripMenuItem.Name = "UndoCtrlZToolStripMenuItem"
        Me.UndoCtrlZToolStripMenuItem.Size = New System.Drawing.Size(274, 38)
        Me.UndoCtrlZToolStripMenuItem.Text = "&Undo (Ctrl + Z)"
        '
        'LineLToolStripMenuItem
        '
        Me.LineLToolStripMenuItem.Name = "LineLToolStripMenuItem"
        Me.LineLToolStripMenuItem.Size = New System.Drawing.Size(274, 38)
        Me.LineLToolStripMenuItem.Text = "&Line (L)"
        '
        'RectangleRToolStripMenuItem
        '
        Me.RectangleRToolStripMenuItem.Name = "RectangleRToolStripMenuItem"
        Me.RectangleRToolStripMenuItem.Size = New System.Drawing.Size(274, 38)
        Me.RectangleRToolStripMenuItem.Text = "&Rectangle (R)"
        '
        'ScribbleSToolStripMenuItem
        '
        Me.ScribbleSToolStripMenuItem.Name = "ScribbleSToolStripMenuItem"
        Me.ScribbleSToolStripMenuItem.Size = New System.Drawing.Size(274, 38)
        Me.ScribbleSToolStripMenuItem.Text = "&Scribble (S)"
        '
        'PickColourPToolStripMenuItem
        '
        Me.PickColourPToolStripMenuItem.Name = "PickColourPToolStripMenuItem"
        Me.PickColourPToolStripMenuItem.Size = New System.Drawing.Size(274, 38)
        Me.PickColourPToolStripMenuItem.Text = "&Pick Colour (P)"
        '
        'ClearCToolStripMenuItem
        '
        Me.ClearCToolStripMenuItem.Name = "ClearCToolStripMenuItem"
        Me.ClearCToolStripMenuItem.Size = New System.Drawing.Size(274, 38)
        Me.ClearCToolStripMenuItem.Text = "&Clear (C)"
        '
        'ZoomToolStripMenuItem
        '
        Me.ZoomToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Reset0ToolStripMenuItem, Me.FullScreenFToolStripMenuItem, Me.ToogleToolbarTToolStripMenuItem})
        Me.ZoomToolStripMenuItem.Name = "ZoomToolStripMenuItem"
        Me.ZoomToolStripMenuItem.Size = New System.Drawing.Size(104, 36)
        Me.ZoomToolStripMenuItem.Text = "&Display"
        '
        'Reset0ToolStripMenuItem
        '
        Me.Reset0ToolStripMenuItem.Name = "Reset0ToolStripMenuItem"
        Me.Reset0ToolStripMenuItem.Size = New System.Drawing.Size(303, 38)
        Me.Reset0ToolStripMenuItem.Text = "&Reset zoom (0)"
        '
        'FullScreenFToolStripMenuItem
        '
        Me.FullScreenFToolStripMenuItem.Name = "FullScreenFToolStripMenuItem"
        Me.FullScreenFToolStripMenuItem.Size = New System.Drawing.Size(303, 38)
        Me.FullScreenFToolStripMenuItem.Text = "&Full screen (F)"
        '
        'ToogleToolbarTToolStripMenuItem
        '
        Me.ToogleToolbarTToolStripMenuItem.Name = "ToogleToolbarTToolStripMenuItem"
        Me.ToogleToolbarTToolStripMenuItem.Size = New System.Drawing.Size(303, 38)
        Me.ToogleToolbarTToolStripMenuItem.Text = "&Toogle toolbar (T)"
        '
        'picPreview
        '
        Me.picPreview.BackgroundImage = Global.Visualiser.My.Resources.Resources.Help
        Me.picPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.picPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.picPreview.Location = New System.Drawing.Point(0, 40)
        Me.picPreview.Name = "picPreview"
        Me.picPreview.Size = New System.Drawing.Size(854, 609)
        Me.picPreview.TabIndex = 2
        Me.picPreview.TabStop = False
        '
        'btnChangeColour
        '
        Me.btnChangeColour.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnChangeColour.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PickColourToolStripMenuItem, Me.BlueToolStripMenuItem, Me.BlackToolStripMenuItem, Me.RedToolStripMenuItem})
        Me.btnChangeColour.Image = Global.Visualiser.My.Resources.Resources.pen_red
        Me.btnChangeColour.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnChangeColour.Name = "btnChangeColour"
        Me.btnChangeColour.Size = New System.Drawing.Size(54, 36)
        Me.btnChangeColour.Text = "ToolStripDropDownButton1"
        '
        'PickColourToolStripMenuItem
        '
        Me.PickColourToolStripMenuItem.Name = "PickColourToolStripMenuItem"
        Me.PickColourToolStripMenuItem.Size = New System.Drawing.Size(230, 38)
        Me.PickColourToolStripMenuItem.Text = "Pick colour"
        '
        'BlueToolStripMenuItem
        '
        Me.BlueToolStripMenuItem.Name = "BlueToolStripMenuItem"
        Me.BlueToolStripMenuItem.Size = New System.Drawing.Size(230, 38)
        Me.BlueToolStripMenuItem.Text = "Blue"
        '
        'BlackToolStripMenuItem
        '
        Me.BlackToolStripMenuItem.Name = "BlackToolStripMenuItem"
        Me.BlackToolStripMenuItem.Size = New System.Drawing.Size(230, 38)
        Me.BlackToolStripMenuItem.Text = "Black"
        '
        'RedToolStripMenuItem
        '
        Me.RedToolStripMenuItem.Name = "RedToolStripMenuItem"
        Me.RedToolStripMenuItem.Size = New System.Drawing.Size(230, 38)
        Me.RedToolStripMenuItem.Text = "Red"
        '
        'btnChangeShape
        '
        Me.btnChangeShape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnChangeShape.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ClearToolStripMenuItem, Me.RectangleToolStripMenuItem, Me.LineToolStripMenuItem, Me.ScribbleToolStripMenuItem})
        Me.btnChangeShape.Image = Global.Visualiser.My.Resources.Resources.shape_rectangle
        Me.btnChangeShape.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnChangeShape.Name = "btnChangeShape"
        Me.btnChangeShape.Size = New System.Drawing.Size(54, 36)
        Me.btnChangeShape.Text = "ToolStripDropDownButton1"
        '
        'ClearToolStripMenuItem
        '
        Me.ClearToolStripMenuItem.Name = "ClearToolStripMenuItem"
        Me.ClearToolStripMenuItem.Size = New System.Drawing.Size(268, 38)
        Me.ClearToolStripMenuItem.Text = "Clear"
        '
        'RectangleToolStripMenuItem
        '
        Me.RectangleToolStripMenuItem.Name = "RectangleToolStripMenuItem"
        Me.RectangleToolStripMenuItem.Size = New System.Drawing.Size(268, 38)
        Me.RectangleToolStripMenuItem.Text = "Rectangle"
        '
        'LineToolStripMenuItem
        '
        Me.LineToolStripMenuItem.Name = "LineToolStripMenuItem"
        Me.LineToolStripMenuItem.Size = New System.Drawing.Size(268, 38)
        Me.LineToolStripMenuItem.Text = "Line"
        '
        'ScribbleToolStripMenuItem
        '
        Me.ScribbleToolStripMenuItem.Name = "ScribbleToolStripMenuItem"
        Me.ScribbleToolStripMenuItem.Size = New System.Drawing.Size(268, 38)
        Me.ScribbleToolStripMenuItem.Text = "Scribble"
        '
        'Visualiser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(854, 687)
        Me.Controls.Add(Me.picPreview)
        Me.Controls.Add(Me.statusBar)
        Me.Controls.Add(Me.mainMenu)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.mainMenu
        Me.Name = "Visualiser"
        Me.Text = "Visualiser"
        Me.TransparencyKey = System.Drawing.Color.Transparent
        Me.statusBar.ResumeLayout(False)
        Me.statusBar.PerformLayout()
        Me.mainMenu.ResumeLayout(False)
        Me.mainMenu.PerformLayout()
        CType(Me.picPreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents statusBar As StatusStrip
    Friend WithEvents lblStatus As ToolStripStatusLabel
    Friend WithEvents timerRefresh As Timer
    Friend WithEvents mainMenu As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CameraToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ChooseCameraCtrl1ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditPropertiesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AnnotationsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UndoCtrlZToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LineLToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RectangleRToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ScribbleSToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PickColourPToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ClearCToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ZoomToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Reset0ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToggleQuality1ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FullScreenFToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToogleToolbarTToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PlayPauseSpaceToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConnectToStreamCtrl2ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents picPreview As PictureBox
    Friend WithEvents btnChangeColour As ToolStripDropDownButton
    Friend WithEvents PickColourToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BlueToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BlackToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RedToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents btnChangeShape As ToolStripDropDownButton
    Friend WithEvents ClearToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RectangleToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LineToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ScribbleToolStripMenuItem As ToolStripMenuItem
End Class
