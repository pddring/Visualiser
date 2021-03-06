﻿Imports System.Net
Imports System.Text.RegularExpressions
Imports Accord.Video
Imports Accord.Video.DirectShow

''' <summary>
''' Visualiser form
''' </summary>
Public Class Visualiser


    Public Enum SourceType
        LocalCamera
        RemoteCamera
    End Enum

    Public source As SourceType = SourceType.LocalCamera

    ''' <summary>
    ''' Stores details about video annotations
    ''' </summary>
    Class Annotation

        ' Position to show annotation
        Public coordinates As Rectangle

        ' Type of annotation
        Public Enum AnnotationType

            ' Unfilled rectangle defined by coordinates
            Rectangle

            ' Lines from top left to bottom right of coordinates
            Line

            ' Freehand drawing with coordinates stored in points
            Scribble
        End Enum

        Public Enabled As Boolean = False

        ' type of annotation
        Public Type As AnnotationType

        Class PointF
            Public X As Double
            Public Y As Double

            Sub New(from As Point)
                Me.X = from.X
                Me.Y = from.Y
            End Sub
        End Class

        ' list of points that define the freehand scribble
        Public points As New List(Of PointF)

        Public Function GetPoints() As Point()
            Dim points(Me.points.Count - 1) As Point
            For i = 0 To Me.points.Count - 1
                points(i).X = Me.points(i).X
                points(i).Y = Me.points(i).Y
            Next
            Return points
        End Function

        ' colour and width of annotation
        Public pen As New Pen(New SolidBrush(Color.Red), 10)

        ' Copy settings from existing annotation
        Sub New(a As Annotation)
            Me.pen.Color = a.pen.Color
            Me.Type = a.Type
            pen.StartCap = Drawing2D.LineCap.Round
            pen.EndCap = Drawing2D.LineCap.Round
            pen.LineJoin = Drawing2D.LineJoin.Round
            Enabled = True
        End Sub

        ' Default constructor
        Sub New()
            pen.StartCap = Drawing2D.LineCap.Round
            pen.EndCap = Drawing2D.LineCap.Round
            pen.LineJoin = Drawing2D.LineJoin.Round
        End Sub
    End Class

    ' inital help image
    Dim helpImage As Bitmap

    ' Annotation used whilst drawing, before it's saved
    Public currentAnnotation As Annotation = New Annotation

    ' List of saved annotations
    Public annotations As New List(Of Annotation)

    ' Set to true whilst user is dragging a shape
    Dim drawingShape As Boolean = False

    ' Defines the area cropped out of the current view. The rest is stretched to the size of the current window
    Class ZoomArea
        Public Left As Integer = 0
        Public Right As Integer = 0
        Public Top As Integer = 0
        Public Bottom As Integer = 0
    End Class

    ' Camera capture / play mode
    Enum PlayMode As Integer
        Paused
        Playing
    End Enum

    ' Start camera in play mode
    Dim status As PlayMode = PlayMode.Paused

    ' High quality mode enables anti-aliasing whilst resizing and a few other things. Didn't seem to make a noticeable impact
    ' on quality but does impede frame rate. Toggle by pressing 1
    Dim highQuality As Boolean = False

    ' Currently selected webcam device
    Dim device As VideoCaptureDevice

    ' Currently selected remote webcam device
    Dim stream As MJPEGStream
    Dim streamIP As String

    ' Area to crop in order to zoom in on part of the image
    Dim pad As New ZoomArea


    ' Keeps track of whether the app is in full screen mode or not
    Dim fullScreen As Boolean = False

    ' Used to store the app size before it went into full screen mode
    Dim oldSize As Rectangle

    ' Used to count how many frames have been displayed in the last second
    Dim frameCount As Integer = 0

    ' Used to store the dimensions of frames captured from the camera device
    Dim frameW As Integer = 800
    Dim frameH As Integer = 600

    Dim frame As Bitmap

    ''' <summary>
    ''' Handler called when a new frame is available from the video capture device
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub NewFrame(sender As Object, eventArgs As NewFrameEventArgs)
        Dim w = picPreview.Width
        If w Mod 2 <> 0 Then
            w += 1
        End If
        Dim h = picPreview.Height
        If h Mod 2 <> 0 Then
            h += 1
        End If
        ' Create a new bitmap the size of the current window for drawing
        If IsNothing(frame) Then
            frame = New Bitmap(w, h, Drawing.Imaging.PixelFormat.Format32bppArgb)
        End If

        m.WaitOne()

        If Not recording Then
            recordStartTime = Now
            If frame.Width <> w Or frame.Height <> h Then
                Dim sfX As Double = picPreview.Width / frame.Width
                Dim sfY As Double = picPreview.Height / frame.Height

                frame.Dispose()
                frame = New Bitmap(picPreview.Width, picPreview.Height, Drawing.Imaging.PixelFormat.Format32bppArgb)

                For Each a As Annotation In annotations
                    a.coordinates.X *= sfX
                    a.coordinates.Width *= sfX
                    a.coordinates.Y *= sfY
                    a.coordinates.Height *= sfY
                    If a.Type = Annotation.AnnotationType.Scribble Then
                        For Each p In a.points
                            p.X *= sfX
                            p.Y *= sfY
                        Next
                    End If
                Next
            End If
        End If

        frameW = eventArgs.Frame.Width
        frameH = eventArgs.Frame.Height

        Dim g As Graphics = Drawing.Graphics.FromImage(frame)

        ' Enable high quality settings
        If highQuality Then
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            g.CompositingMode = Drawing2D.CompositingMode.SourceOver
        End If
        g.FillRectangle(Brushes.Black, 0, 0, frame.Width, frame.Height)

        If rotate <> 0 Then
            g.TranslateTransform(frame.Width / 2, frame.Height / 2)
            g.RotateTransform(rotate)
            g.TranslateTransform(-frame.Width / 2, -frame.Height / 2)
        End If

        ' Resize captured frame
        Dim destRect As New Rectangle(-pad.Left, -pad.Top, picPreview.Width + pad.Left + pad.Right, picPreview.Height + pad.Top + pad.Bottom)
        Dim srcRect As New Rectangle(0, 0, eventArgs.Frame.Width, eventArgs.Frame.Height)
        g.DrawImage(eventArgs.Frame, destRect, srcRect, GraphicsUnit.Pixel)

        If recording Then
            videoWriter.WriteVideoFrame(frame, Now - recordStartTime)
        End If


        picPreview.Invalidate()

        ' update frame rate stats
        frameCount += 1

        m.ReleaseMutex()


    End Sub

    ''' <summary>
    ''' Show dialog letting the user choose which video capture device to use and choose resolution
    ''' </summary>
    ''' <returns>DialogResult (OK / Cancel)</returns>
    Private Function ChooseDevice() As DialogResult
        zoom = 0
        ' Show dialog
        Dim d As VideoCaptureDeviceForm = New VideoCaptureDeviceForm()
        If d.ShowDialog() = DialogResult.OK Then

            ' Stop receiving frames from the previous device
            Try
                device.Stop()
                RemoveHandler device.NewFrame, AddressOf Me.NewFrame
            Catch ex As Exception
            End Try
            Try
                stream.Stop()
            Catch ex As Exception

            End Try

            ' Set up the newly selected device
            device = d.VideoDevice
            source = SourceType.LocalCamera
            device.Start()
            status = PlayMode.Playing
            AddHandler device.NewFrame, AddressOf Me.NewFrame

        End If
        Return d.DialogResult
    End Function

    ''' <summary>
    ''' Toggles full screen mode or normal size mode
    ''' </summary>
    Sub ToggleFullscreen()

        ' Return from full screen mode into normal mode
        If fullScreen Then
            Bounds = oldSize
            Me.FormBorderStyle = FormBorderStyle.Sizable
            fullScreen = False
            mainMenu.Visible = True

            ' Go into full screen mode
        Else
            oldSize = Bounds
            Me.FormBorderStyle = FormBorderStyle.None
            Dim s As Screen = Screen.FromHandle(Me.Handle)
            Left = s.Bounds.Left
            Top = s.Bounds.Top
            Width = s.Bounds.Width
            Height = s.Bounds.Height
            fullScreen = True
            mainMenu.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' Keyboard shortcut handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown

        Select Case e.KeyCode
            Case Keys.F1
                picPreview.BackgroundImage = helpImage
                picPreview.Invalidate()

            ' E shows camera settings window
            Case Keys.E
                If source = SourceType.LocalCamera And Not IsNothing(device) Then
                    device.DisplayPropertyPage(Me.Handle)
                End If

            ' T toggles toolbar at the bottom of the screen
            Case Keys.T
                statusBar.Visible = Not statusBar.Visible

            ' C clears all annotations
            Case Keys.C
                annotations.Clear()
                picPreview.Invalidate()

            ' Q quits the program
            Case Keys.Q
                Close()

            ' S enables scribble annotation mode
            Case Keys.S
                currentAnnotation.Type = Annotation.AnnotationType.Scribble

            ' Ctrl Z removes the last added annotation
            Case Keys.Z
                If e.Control Then
                    Try
                        annotations.Remove(annotations.Last)
                        picPreview.Invalidate()
                    Catch
                    End Try
                End If

            ' F toggles fullscreen
            Case Keys.F
                ToggleFullscreen()

            ' L enables line drawing annotation mode
            Case Keys.L
                currentAnnotation.Type = Annotation.AnnotationType.Line

            ' R enables rectangle drawing annotation mode (default)
            Case Keys.R
                currentAnnotation.Type = Annotation.AnnotationType.Rectangle

            ' P shows colour picker dialog
            Case Keys.P
                chooseColour

            ' Ctrl + 1 shows camera selection dialog
            ' 1 on its own toggles high / normal quality mode
            Case Keys.D1
                If e.Control Then
                    ChooseDevice()
                Else
                    highQuality = Not highQuality
                End If

            Case Keys.Enter
                If status = PlayMode.Playing Then
                    If source = SourceType.RemoteCamera And streamIP <> "" Then
                        Dim url As String
                        url = streamIP + "cam/1/led_toggle"

                        Dim t As New Task(Sub()
                                              Try
                                                  Dim request = WebRequest.Create(url)
                                                  Using response = request.GetResponse()
                                                  End Using
                                              Catch
                                              End Try


                                          End Sub)
                        t.Start()
                    End If
                End If


            Case Keys.D2
                If e.Control Then
                    ChooseStream()
                End If

            ' Space toggles play mode (pause / play)
            Case Keys.Space
                Dim oldPlayMode = status
                Try
                    If status = PlayMode.Paused Then
                        status = PlayMode.Playing
                        If source = SourceType.LocalCamera Then
                            device.Start()
                        End If
                        If source = SourceType.RemoteCamera Then
                            stream.Start()
                        End If

                    Else
                        status = PlayMode.Paused
                        If source = SourceType.LocalCamera Then
                            device.Stop()
                        End If
                        If source = SourceType.RemoteCamera Then
                            stream.Stop()
                        End If

                    End If
                Catch
                    status = oldPlayMode
                End Try


            ' 0 resets any zoom
            Case Keys.D0
                pad.Left = 0
                pad.Right = 0
                pad.Top = 0
                pad.Bottom = 0
                zoom = 0
                rotate = 0

            Case Keys.D9
                resetBox()

        End Select
    End Sub

    ''' <summary>
    ''' Triggered every second to update the frame rate label
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub timerRefresh_Tick(sender As Object, e As EventArgs) Handles timerRefresh.Tick

        ' App is currently paused
        If status = PlayMode.Paused Then
            lblStatus.Text = "Paused"
        End If

        ' App is currently displaying live feed
        If status = PlayMode.Playing Then
            lblStatus.Text = "Playing " & frameCount & " fps"
            If highQuality Then
                lblStatus.Text &= " (High Quality)"
            End If

            ' Show captured frame resolution (not actually displayed resolution)
            lblStatus.Text &= " " & frameW & "x" & frameH

            If zoom > 1 Then
                lblStatus.Text &= " Zoom: " & zoom
            End If

            If rotate <> 0 Then
                lblStatus.Text &= " " & rotate & " deg"
            End If

            If recording Then
                lblStatus.Text &= " (REC)"
            End If
        End If

        ' Reset frame count for the next second
        frameCount = 0
    End Sub


    Dim zoom As Integer
    Dim rotate As Integer
    ''' <summary>
    ''' Mouse scroll wheel handler - allows user to zoom in and out of an area of the video
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MouseWheelScroll(sender As Object, e As MouseEventArgs)
        If My.Computer.Keyboard.ShiftKeyDown Then
            If e.Delta > 0 Then
                rotate += 5
            Else
                rotate -= 5
            End If
        ElseIf My.Computer.Keyboard.CtrlKeyDown Then
            Dim flags As CameraControlFlags
            If source = SourceType.LocalCamera And IsNothing(device) = False Then
                device.GetCameraProperty(CameraControlProperty.Zoom, zoom, flags)
                Dim amount As Integer = 0
                If e.Delta > 0 Then
                    amount = 1
                Else amount = -1
                End If
                device.SetCameraProperty(CameraControlProperty.Zoom, zoom + amount, CameraControlFlags.Manual)
            End If

            If source = SourceType.RemoteCamera And streamIP <> "" Then
                Dim url As String
                Dim zDiff = 0
                If e.Delta > 0 Then
                    url = streamIP + "cam/1/zoomin"
                    zDiff += 1
                Else
                    url = streamIP + "cam/1/zoomout"
                    If zoom > 1 Then
                        zDiff -= 1
                    End If
                End If

                Dim t As New Task(Sub()
                                      Try
                                          Dim request = WebRequest.Create(url)
                                          Using response = request.GetResponse()
                                              zoom += zDiff

                                          End Using
                                      Catch
                                          zoom = 0
                                      End Try

                                  End Sub)
                t.Start()
            End If
        Else
            ' Amount to zoom by
            Dim aX As Double = frameW / 10
            Dim aY As Double = frameH / 10

            ' Coordinates to zoom in on (after rotation has been cancelled)
            Dim x As Double = e.X - (picPreview.Width / 2)
            Dim y As Double = e.Y - (picPreview.Height / 2)
            Dim rX As Double = x * Math.Cos(-rotate * Math.PI / 180) - y * Math.Cos(-rotate * Math.PI / 180)
            Dim rY As Double = y * Math.Cos(-rotate * Math.PI / 180) + x * Math.Sin(-rotate * Math.PI / 180)
            rX += picPreview.Width / 2
            rY += picPreview.Height / 2

            ' aX is the total amount to ignore on both left and right. x is how much of aX is ignored on the left.
            x = aX * (rX / picPreview.Width)
            y = aY * (rY / picPreview.Height)

            ' Zoom in
            If e.Delta > 0 Then
                If pad.Left + pad.Right + aX < frameW Then
                    pad.Left += x
                    pad.Top += y
                    pad.Right += aX - x
                    pad.Bottom += aY - y
                End If

                ' Zoom out
            Else
                If pad.Left + pad.Right - aX > 0 Then
                    pad.Left -= x
                    pad.Top -= y
                    pad.Bottom -= (aY - y)
                    pad.Right -= (aX - x)
                End If
            End If


        End If
    End Sub

    Sub resetBox()
        lblStatus.Text = "Recovered image..."

        Me.Controls.Remove(picPreview)
        picPreview = New PictureBox()
        Me.Controls.Add(picPreview)
        picPreview.Dock = DockStyle.Fill

        If status = PlayMode.Playing Then
            picPreview.BackgroundImage = frame
        Else
            picPreview.BackgroundImage = helpImage
        End If

    End Sub

    Sub UnknownError()
        resetBox()

    End Sub

    ''' <summary>
    ''' App initialisation
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'AddHandler Application.ThreadException, AddressOf UnknownError
        AllowTransparency = False

        ' Mouse wheel handler can't be added in the GUI. Don't know why.
        AddHandler Me.MouseWheel, AddressOf MouseWheelScroll

        helpImage = picPreview.BackgroundImage

        ' Show device choose dialog on startup
        'If ChooseDevice() <> DialogResult.OK Then
        'Close()
        'End If
    End Sub

    ''' <summary>
    ''' App closing handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Visualiser_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        ' Shutdown camera if necessary
        Try
            device.Stop()
        Catch
        End Try
        Try
            stream.Stop()
        Catch ex As Exception

        End Try
    End Sub


    ''' <summary>
    ''' Mouse down handler - user is starting to draw an annotation
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Visualiser_MouseDown(sender As Object, e As MouseEventArgs) Handles picPreview.MouseDown

        ' Store current mouse location
        currentAnnotation.coordinates.Location = e.Location
        drawingShape = True
        currentAnnotation.Enabled = True
    End Sub

    ''' <summary>
    ''' Mouse up handler - user has finished drawing an annotation
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Visualiser_MouseUp(sender As Object, e As MouseEventArgs) Handles picPreview.MouseUp

        ' Store annotation
        drawingShape = False
        annotations.Add(currentAnnotation)
        currentAnnotation = New Annotation(currentAnnotation)
    End Sub

    ''' <summary>
    ''' Mouse move handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Visualiser_MouseMove(sender As Object, e As MouseEventArgs) Handles picPreview.MouseMove

        ' Is user currently drawing an annotation?
        If drawingShape Then
            currentAnnotation.coordinates.Width = e.Location.X - currentAnnotation.coordinates.X
            currentAnnotation.coordinates.Height = e.Location.Y - currentAnnotation.coordinates.Y
            If currentAnnotation.Type = Annotation.AnnotationType.Scribble Then
                currentAnnotation.points.Add(New Annotation.PointF(e.Location))
            End If
            picPreview.Invalidate()
        End If
    End Sub

    ''' <summary>
    ''' Menu: File > Exit 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click

        ' Close the program
        Close()
    End Sub

    ''' <summary>
    ''' Menu: Camera > Choose Camera
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ChooseCameraCtrl1ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChooseCameraCtrl1ToolStripMenuItem.Click

        ' Show dialog to choose camera and resolution
        ChooseDevice()
    End Sub

    ' Menu: Camera > Edit Properties
    Private Sub EditPropertiesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditPropertiesToolStripMenuItem.Click

        ' Show dialog to edit camera settings
        device.DisplayPropertyPage(Me.Handle)
    End Sub

    ''' <summary>
    ''' Menu: Camera > Toggle quality
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ToggleQuality1ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToggleQuality1ToolStripMenuItem.Click

        ' Toggle quality settings
        highQuality = Not highQuality
    End Sub

    ''' <summary>
    ''' Menu: Camera > Play / Pause
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub PlayPauseSpaceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PlayPauseSpaceToolStripMenuItem.Click

        ' Toggle play mode
        If status = PlayMode.Paused Then
            status = PlayMode.Playing
            If source = SourceType.LocalCamera Then
                device.Start()
            Else
                stream.Start()
            End If

        Else
            status = PlayMode.Paused
            If source = SourceType.LocalCamera Then
                device.Stop()
            Else
                stream.Stop()
            End If

        End If
    End Sub

    ''' <summary>
    ''' Menu: Display > Reset zoom
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Reset0ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Reset0ToolStripMenuItem.Click

        ' Remove all padding so zooming is reset to default settings
        pad.Left = 0
        pad.Right = 0
        pad.Top = 0
        pad.Bottom = 0
    End Sub

    ''' <summary>
    ''' Menu: Display > Full Screen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub FullScreenFToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FullScreenFToolStripMenuItem.Click

        ' Toggle fullscreen mode
        ToggleFullscreen()
    End Sub

    ''' <summary>
    ''' Menu: Display > Toggle toolbar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ToogleToolbarTToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToogleToolbarTToolStripMenuItem.Click

        ' Show / hide status bar
        statusBar.Visible = Not statusBar.Visible
    End Sub

    ''' <summary>
    ''' Menu: Annotations > Undo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub UndoCtrlZToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UndoCtrlZToolStripMenuItem.Click

        ' Remove last added annotation (if there is one to remove)
        Try
            annotations.Remove(annotations.Last)
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Menu: Annotations > Line
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LineLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LineLToolStripMenuItem.Click

        ' Allow user to draw a line annotation by dragging
        currentAnnotation.Type = Annotation.AnnotationType.Line
    End Sub

    ''' <summary>
    ''' Menu: Annotations > Rectangle
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RectangleRToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RectangleRToolStripMenuItem.Click

        ' Allow user to to draw a rectangle by dragging
        currentAnnotation.Type = Annotation.AnnotationType.Rectangle
    End Sub

    ''' <summary>
    ''' Menu: Annotations > Scribble
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ScribbleSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScribbleSToolStripMenuItem.Click

        ' Allow user to draw a freehand scribble by dragging
        currentAnnotation.Type = Annotation.AnnotationType.Scribble
    End Sub

    ''' <summary>
    ''' Menu: Annotation > Pick colour
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub PickColourPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PickColourPToolStripMenuItem.Click

        ' Show dialog to choose annotation colour
        Dim dlg As New ColorDialog
        dlg.FullOpen = True
        If dlg.ShowDialog = DialogResult.OK Then
            currentAnnotation.pen.Color = dlg.Color
        End If
    End Sub

    ''' <summary>
    ''' Menu: Annotations > Clear
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ClearCToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearCToolStripMenuItem.Click

        ' Clear all annotations
        annotations.Clear()
    End Sub

    ''' <summary>
    ''' Show dialog to choose remote webcam device
    ''' </summary>
    Sub ChooseStream()
        zoom = 0
        ' stop streaming from local remote devices
        Try
            RemoveHandler device.NewFrame, AddressOf NewFrame
            device.Stop()
        Catch
        End Try
        Try
            stream.Stop()
        Catch ex As Exception
        End Try

        ' Ask for connection string
        Dim src As String = InputBox("Stream source: (leave blank to scan)", "Connect to remote camera", My.Settings("LastStreamAddress"))

        Try
            ' Show scan window if connection string is empty
            If src = "" Then
                Dim dlg As New RemoteCameraScanner
                If dlg.ShowDialog() = DialogResult.OK Then
                    src = dlg.RemoteAddress
                End If
            End If

            ' save connection for next time
            My.Settings("LastStreamAddress") = src
            Dim m = Regex.Match(src, "http:\/\/\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d+\/")
            streamIP = m.Captures(0).Value
            My.Settings.Save()

            ' connect to remote device
            stream = New MJPEGStream(src)
            AddHandler stream.NewFrame, AddressOf NewFrame
            source = SourceType.RemoteCamera
            stream.Start()
            status = PlayMode.Playing
        Catch
            MsgBox("Could not connect")
        End Try
    End Sub



    ''' <summary>
    ''' Menu: Camera > Connect to stream
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ConnectToStreamCtrl2ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConnectToStreamCtrl2ToolStripMenuItem.Click
        ChooseStream()
    End Sub

    Dim m As New System.Threading.Mutex

    Private Sub picPreview_Paint(sender As Object, e As PaintEventArgs) Handles picPreview.Paint



        m.WaitOne()
        Dim g As Graphics = e.Graphics
        If Not IsNothing(frame) Then
            g.DrawImage(frame, 0, 0)
        End If

        ' Draw annotations
        For Each a As Annotation In annotations
            Select Case a.Type
                Case Annotation.AnnotationType.Line
                    g.DrawLine(a.pen, a.coordinates.X, a.coordinates.Y, a.coordinates.Right, a.coordinates.Bottom)
                Case Annotation.AnnotationType.Rectangle
                    g.DrawRectangle(a.pen, a.coordinates)
                Case Annotation.AnnotationType.Scribble
                    If a.points.Count > 2 Then
                        g.DrawLines(a.pen, a.getPoints())
                    End If
            End Select
        Next

        If currentAnnotation.Enabled Then
            ' Draw current annotation
            Select Case currentAnnotation.Type
                Case Annotation.AnnotationType.Line
                    g.DrawLine(currentAnnotation.pen, currentAnnotation.coordinates.X, currentAnnotation.coordinates.Y, currentAnnotation.coordinates.Right, currentAnnotation.coordinates.Bottom)
                Case Annotation.AnnotationType.Rectangle
                    g.DrawRectangle(currentAnnotation.pen, currentAnnotation.coordinates)
                Case Annotation.AnnotationType.Scribble
                    If currentAnnotation.points.Count > 2 Then
                        g.DrawLines(currentAnnotation.pen, currentAnnotation.GetPoints)
                    End If
            End Select
        End If
        m.ReleaseMutex()

    End Sub

    Sub ChooseColour()
        Dim dlg As New ColorDialog
        dlg.FullOpen = True
        If dlg.ShowDialog = DialogResult.OK Then
            currentAnnotation.pen.Color = dlg.Color
        End If
    End Sub


    Private Sub BlueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BlueToolStripMenuItem.Click
        currentAnnotation.pen.Color = Color.Blue
        btnChangeColour.Image = My.Resources.ResourceManager.GetObject("pen_blue")
    End Sub

    Private Sub BlackToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BlackToolStripMenuItem.Click
        currentAnnotation.pen.Color = Color.Black
        btnChangeColour.Image = My.Resources.ResourceManager.GetObject("pen_black")
    End Sub

    Private Sub RedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RedToolStripMenuItem.Click
        currentAnnotation.pen.Color = Color.Red
        btnChangeColour.Image = My.Resources.ResourceManager.GetObject("pen_red")
    End Sub

    Private Sub PickColourToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PickColourToolStripMenuItem.Click
        ChooseColour()
    End Sub

    Private Sub ScribbleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScribbleToolStripMenuItem.Click
        currentAnnotation.Type = Annotation.AnnotationType.Scribble
        btnChangeShape.Image = My.Resources.ResourceManager.GetObject("shape_scribble")
    End Sub

    Private Sub ClearToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearToolStripMenuItem.Click
        annotations.Clear()
        picPreview.Invalidate()
    End Sub

    Private Sub RectangleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RectangleToolStripMenuItem.Click
        currentAnnotation.Type = Annotation.AnnotationType.Rectangle
        btnChangeShape.Image = My.Resources.ResourceManager.GetObject("shape_rectangle")
    End Sub

    Private Sub LineToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LineToolStripMenuItem.Click
        currentAnnotation.Type = Annotation.AnnotationType.Line
        btnChangeShape.Image = My.Resources.ResourceManager.GetObject("shape_line")
    End Sub

    Sub SaveVideo()
        If recording = False Then
            Dim dlg As New SaveFileDialog
            dlg.Title = "Save video"
            dlg.FileName = "Video" & Now.ToString("dd mm yyyy H-mm") & ".mp4"
            If dlg.ShowDialog = DialogResult.OK Then
                Dim w = picPreview.Width
                If w Mod 2 <> 0 Then
                    w += 1
                End If
                Dim h = picPreview.Height
                If h Mod 2 <> 0 Then
                    h += 1
                End If
                videoWriter.Open(dlg.FileName, w, h)
                recording = True
                recordStartTime = Now
            End If
        Else
            recording = False
            videoWriter.Close()
        End If
    End Sub

    Sub SaveImage()
        If Not IsNothing(frame) Then
            Dim dlg As New SaveFileDialog
            dlg.Title = "Save image"
            dlg.FileName = "Snapshot " & Now.ToString("dd mm yyyy H-mm") & ".jpg"
            If dlg.ShowDialog = DialogResult.OK Then
                m.WaitOne()
                frame.Save(dlg.FileName)
                m.ReleaseMutex()
            End If
        End If
    End Sub

    Private Sub SaveImageToolStbripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveImageToolStripMenuItem.Click
        SaveImage()
    End Sub

    Dim videoWriter As New FFMPEG.VideoFileWriter
    Dim recordStartTime As DateTime
    Dim recording As Boolean = False
    Private Sub SavevideoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SavevideoToolStripMenuItem.Click
        SaveVideo()
    End Sub

    Private Sub SaveVideoToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SaveVideoToolStripMenuItem1.Click
        SaveVideo()
    End Sub

    Private Sub SaveImageToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SaveImageToolStripMenuItem1.Click
        SaveImage()
    End Sub
End Class
