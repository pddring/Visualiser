Imports System.Net
Imports System.Net.Sockets

Public Class RemoteCameraScanner
    ' selected remote webcam address
    Public RemoteAddress As String = ""

    ' Each IP address is scanned with a different task
    Dim tasks As New List(Of Task)

    ''' <summary>
    ''' Form load handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RemoteCameraScanner_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Get local IPv4 addresses
        Dim host = Dns.GetHostEntry(Dns.GetHostName)
        For Each ip In host.AddressList
            If ip.GetAddressBytes.Count = 4 Then
                lstIPAddresses.Items.Add(ip.ToString)
            End If
        Next
    End Sub


    ''' <summary>
    ''' Scan button click handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnScan_Click(sender As Object, e As EventArgs) Handles btnScan.Click

        ' get details of selected local IP address
        Dim localIP As String = lstIPAddresses.SelectedItem
        Dim parts As String() = localIP.Split(".")

        ' Clear list of remote devices that've been previously scanned
        lstFound.Items.Clear()
        tasks.Clear()

        ' Loop through all devices on the subnet of the selected local IP
        For c As Integer = 1 To 254

            ' Set remote IP
            parts(3) = c
            Dim remoteIP As String = String.Join(".", parts)

            ' Create a new task to scan remote IP on a different thread
            Dim ta As New Task(Sub()

                                   ' assume the remote IP isn't live unless we get a response
                                   Dim success As Boolean = False
                                   Try

                                       ' Connect asynchronously - timeout after 500ms
                                       Using client As New TcpClient
                                           If client.ConnectAsync(remoteIP, txtPort.Text).Wait(500) Then

                                               ' connection succeeded so assume that this remoteIP is live
                                               success = True
                                           End If

                                       End Using
                                   Catch ex As Exception
                                   End Try

                                   ' You can only update the listbox on the main GUI thread
                                   If InvokeRequired Then
                                       Invoke(Sub()

                                                  ' add the found remote IP to the list
                                                  If success Then
                                                      lstFound.Items.Add(remoteIP)
                                                  End If

                                              End Sub)

                                   End If
                               End Sub)
            ' Start the scan on this IP
            tasks.Add(ta)
            ta.Start()

        Next
    End Sub

    ''' <summary>
    ''' Timer to update the progress bar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub timerUpdateProgress_Tick(sender As Object, e As EventArgs) Handles timerUpdateProgress.Tick

        ' set bounds - 254 IPs to scan
        progressBar.Minimum = 0
        progressBar.Maximum = 254

        ' When a task completes, the matching IP address is either live or it timed out
        Dim completed As Integer = 0
        For Each t In tasks
            If t.IsCompleted Then completed += 1
        Next
        progressBar.Value = completed
    End Sub

    ''' <summary>
    ''' User selects a found IP address
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub lstFound_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstFound.SelectedIndexChanged

        ' Set the webcam address based on the details in the text box
        RemoteAddress = txtAddressFormat.Text.Replace("[ip]", lstFound.SelectedItem).Replace("[port]", txtPort.Text)
        txtAddress.Text = RemoteAddress
    End Sub

    ''' <summary>
    ''' OK button click handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        DialogResult = DialogResult.OK
        Close()
    End Sub

    ''' <summary>
    ''' Override text box change handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub txtAddress_TextChanged(sender As Object, e As EventArgs) Handles txtAddress.TextChanged
        RemoteAddress = txtAddress.Text
    End Sub
End Class