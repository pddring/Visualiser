Imports AForge.Video
Imports AForge.Video.DirectShow

Public Class Visualiser

    Class Annotation
        Public coordinates As Rectangle
        Public Enum AnnotationType
            Rectangle
            Line
            Scribble
        End Enum
        Public Type As AnnotationType
        Public points As New List(Of Point)
        Public pen As New Pen(New SolidBrush(Color.Red), 10)

        Sub New(a As Annotation)
            Me.pen.Color = a.pen.Color
            Me.Type = a.Type
        End Sub

        Sub New()

        End Sub
    End Class

    Public currentAnnotation As Annotation = New Annotation

    Public annotations As New List(Of Annotation)

    Class ZoomArea
        Public Left As Integer = 0
        Public Right As Integer = 0
        Public Top As Integer = 0
        Public Bottom As Integer = 0
    End Class

    Enum PlayMode As Integer
        Paused
        Playing
    End Enum

    Dim status As PlayMode = PlayMode.Paused

    Dim highQuality As Boolean = False
    Dim device As VideoCaptureDevice

    Dim pad As New ZoomArea

    Dim frame As Bitmap

    Private Sub NewFrame(sender As Object, eventArgs As NewFrameEventArgs)
        Dim frame As Bitmap = New Bitmap(Width, Height, Drawing.Imaging.PixelFormat.Format32bppArgb)
        frameW = eventArgs.Frame.Width
        frameH = eventArgs.Frame.Height

        Using g As Graphics = Drawing.Graphics.FromImage(frame)
            If highQuality Then
                g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
                g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                g.CompositingMode = Drawing2D.CompositingMode.SourceOver
            End If
            Dim destRect As New Rectangle(0, 0, Width, Height)
            Dim srcRect As New Rectangle(pad.Left, pad.Top, eventArgs.Frame.Width - pad.Right - pad.Left, eventArgs.Frame.Height - pad.Bottom - pad.Top)
            g.DrawImage(eventArgs.Frame, destRect, srcRect, GraphicsUnit.Pixel)

            Try
                For Each a As Annotation In annotations
                    Select Case a.Type
                        Case Annotation.AnnotationType.Line
                            g.DrawLine(a.pen, a.coordinates.X, a.coordinates.Y, a.coordinates.Right, a.coordinates.Bottom)
                        Case Annotation.AnnotationType.Rectangle
                            g.DrawRectangle(a.pen, a.coordinates)
                        Case Annotation.AnnotationType.Scribble
                            If a.points.Count > 2 Then
                                g.DrawLines(a.pen, a.points.ToArray())
                            End If
                    End Select
                Next


                Select Case currentAnnotation.Type
                    Case Annotation.AnnotationType.Line
                        g.DrawLine(currentAnnotation.pen, currentAnnotation.coordinates.X, currentAnnotation.coordinates.Y, currentAnnotation.coordinates.Right, currentAnnotation.coordinates.Bottom)
                    Case Annotation.AnnotationType.Rectangle
                        g.DrawRectangle(currentAnnotation.pen, currentAnnotation.coordinates)
                    Case Annotation.AnnotationType.Scribble
                        If currentAnnotation.points.Count > 2 Then
                            g.DrawLines(currentAnnotation.pen, currentAnnotation.points.ToArray())
                        End If
                End Select
            Catch
            End Try
        End Using
        Me.BackgroundImage = frame
        frameCount += 1

    End Sub

    Private Function ChooseDevice() As DialogResult
        Dim d As VideoCaptureDeviceForm = New VideoCaptureDeviceForm()
        If d.ShowDialog() = DialogResult.OK Then
            Try
                device.Stop()
                RemoveHandler device.NewFrame, AddressOf Me.NewFrame
            Catch ex As Exception

            End Try
            device = d.VideoDevice

            device.Start()
            status = PlayMode.Playing

            AddHandler device.NewFrame, AddressOf Me.NewFrame

        End If
        Return d.DialogResult

    End Function

    Dim fullScreen As Boolean = False
    Dim oldSize As Rectangle

    Sub ToggleFullscreen()
        If fullScreen Then
            Bounds = oldSize
            Me.FormBorderStyle = FormBorderStyle.Sizable
            fullScreen = False
            mainMenu.Visible = True
        Else
            oldSize = Bounds
            Me.FormBorderStyle = FormBorderStyle.None
            Left = 0
            Top = 0
            Width = Screen.FromHandle(Me.Handle).Bounds.Width
            Height = Screen.FromHandle(Me.Handle).Bounds.Height
            fullScreen = True
            mainMenu.Visible = False
        End If
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown

        Select Case e.KeyCode
            Case Keys.E
                device.DisplayPropertyPage(Me.Handle)
            Case Keys.T
                statusBar.Visible = Not statusBar.Visible
            Case Keys.C
                annotations.Clear()
            Case Keys.Q
                Close()
            Case Keys.S
                currentAnnotation.Type = Annotation.AnnotationType.Scribble
            Case Keys.Z
                If e.Control Then
                    Try
                        annotations.Remove(annotations.Last)
                    Catch
                    End Try
                End If
            Case Keys.F
                ToggleFullscreen()

            Case Keys.L
                currentAnnotation.Type = Annotation.AnnotationType.Line
            Case Keys.R
                currentAnnotation.Type = Annotation.AnnotationType.Rectangle


            Case Keys.P
                Dim dlg As New ColorDialog
                dlg.FullOpen = True
                If dlg.ShowDialog = DialogResult.OK Then
                    currentAnnotation.pen.Color = dlg.Color
                End If
            Case Keys.D1
                If e.Control Then
                    ChooseDevice()
                Else
                    highQuality = Not highQuality
                End If

            Case Keys.Space
                If status = PlayMode.Paused Then
                    status = PlayMode.Playing
                    device.Start()
                Else
                    status = PlayMode.Paused
                    device.Stop()
                End If

            Case Keys.D0
                pad.Left = 0
                pad.Right = 0
                pad.Top = 0
                pad.Bottom = 0
        End Select
    End Sub

    Dim frameCount As Integer = 0
    Dim frameW As Integer = 800
    Dim frameH As Integer = 600


    Private Sub timerRefresh_Tick(sender As Object, e As EventArgs) Handles timerRefresh.Tick

        If status = PlayMode.Paused Then
            lblStatus.Text = "Paused"
        End If

        If status = PlayMode.Playing Then
            lblStatus.Text = "Playing " & frameCount & " fps"
            If highQuality Then
                lblStatus.Text &= " (High Quality)"
            End If

            lblStatus.Text &= " " & frameW & "x" & frameH
        End If
        frameCount = 0
    End Sub

    Private Sub MouseWheelScroll(sender As Object, e As MouseEventArgs)
        Dim aX As Double = frameW / 10
        Dim aY As Double = frameH / 10

        Dim x As Double = aX * (e.X / Width)
        Dim y As Double = aY * (e.Y / Height)


        If e.Delta > 0 Then
            If pad.Left + pad.Right + aX < frameW Then
                pad.Left += x
                pad.Top += y
                pad.Right += aX - x
                pad.Bottom += aY - y
            End If
        Else
            If pad.Left + pad.Right - aX > 0 Then
                pad.Left -= x
                pad.Top -= y
                pad.Bottom -= (aY - y)
                pad.Right -= (aX - x)
            End If

        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AddHandler Me.MouseWheel, AddressOf MouseWheelScroll
        If ChooseDevice() <> DialogResult.OK Then
            Close()
        End If

    End Sub

    Private Sub Visualiser_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            device.Stop()
        Catch
        End Try
    End Sub


    Dim drawingShape As Boolean = False
    Private Sub Visualiser_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
        currentAnnotation.coordinates.Location = e.Location
        drawingShape = True
    End Sub

    Private Sub Visualiser_MouseUp(sender As Object, e As MouseEventArgs) Handles MyBase.MouseUp
        drawingShape = False
        annotations.Add(currentAnnotation)
        currentAnnotation = New Annotation(currentAnnotation)

    End Sub

    Private Sub Visualiser_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
        If drawingShape Then
            currentAnnotation.coordinates.Width = e.Location.X - currentAnnotation.coordinates.X
            currentAnnotation.coordinates.Height = e.Location.Y - currentAnnotation.coordinates.Y
            If currentAnnotation.Type = Annotation.AnnotationType.Scribble Then
                currentAnnotation.points.Add(e.Location)
            End If
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub ChooseCameraCtrl1ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChooseCameraCtrl1ToolStripMenuItem.Click
        ChooseDevice()
    End Sub

    Private Sub EditPropertiesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditPropertiesToolStripMenuItem.Click
        device.DisplayPropertyPage(Me.Handle)
    End Sub

    Private Sub ToggleQuality1ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToggleQuality1ToolStripMenuItem.Click
        highQuality = Not highQuality
    End Sub

    Private Sub PlayPauseSpaceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PlayPauseSpaceToolStripMenuItem.Click
        If status = PlayMode.Paused Then
            status = PlayMode.Playing
            device.Start()
        Else
            status = PlayMode.Paused
            device.Stop()
        End If
    End Sub

    Private Sub Reset0ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Reset0ToolStripMenuItem.Click
        pad.Left = 0
        pad.Right = 0
        pad.Top = 0
        pad.Bottom = 0
    End Sub

    Private Sub FullScreenFToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FullScreenFToolStripMenuItem.Click
        ToggleFullscreen()
    End Sub

    Private Sub ToogleToolbarTToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToogleToolbarTToolStripMenuItem.Click
        statusBar.Visible = Not statusBar.Visible
    End Sub

    Private Sub UndoCtrlZToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UndoCtrlZToolStripMenuItem.Click
        Try
            annotations.Remove(annotations.Last)
        Catch
        End Try
    End Sub

    Private Sub LineLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LineLToolStripMenuItem.Click
        currentAnnotation.Type = Annotation.AnnotationType.Line
    End Sub

    Private Sub RectangleRToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RectangleRToolStripMenuItem.Click
        currentAnnotation.Type = Annotation.AnnotationType.Rectangle
    End Sub

    Private Sub ScribbleSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScribbleSToolStripMenuItem.Click
        currentAnnotation.Type = Annotation.AnnotationType.Scribble
    End Sub

    Private Sub PickColourPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PickColourPToolStripMenuItem.Click
        Dim dlg As New ColorDialog
        dlg.FullOpen = True
        If dlg.ShowDialog = DialogResult.OK Then
            currentAnnotation.pen.Color = dlg.Color
        End If
    End Sub

    Private Sub ClearCToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearCToolStripMenuItem.Click
        annotations.Clear()
    End Sub
End Class
