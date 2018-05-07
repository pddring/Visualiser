Imports System.Net
Imports System.Net.Sockets

Public Class RemoteCameraScanner
    Public RemoteAddress As String = ""

    Private Sub RemoteCameraScanner_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim host = Dns.GetHostEntry(Dns.GetHostName)
        For Each ip In host.AddressList
            If ip.GetAddressBytes.Count = 4 Then
                lstIPAddresses.Items.Add(ip.ToString)
            End If
        Next
    End Sub

    Dim tasks As New List(Of Task)

    Private Sub btnScan_Click(sender As Object, e As EventArgs) Handles btnScan.Click
        Dim localIP As String = lstIPAddresses.SelectedItem
        Dim parts As String() = localIP.Split(".")
        lstFound.Items.Clear()
        tasks.Clear()

        For c As Integer = 1 To 254

            parts(3) = c
            Dim remoteIP As String = String.Join(".", parts)

            Dim ta As New Task(Sub()
                                   Dim success As Boolean = False
                                   Try
                                       Using client As New TcpClient
                                           If client.ConnectAsync(remoteIP, txtPort.Text).Wait(500) Then
                                               success = True
                                           End If

                                       End Using
                                   Catch ex As Exception
                                   End Try
                                   If InvokeRequired Then
                                       Invoke(Sub()
                                                  If success Then
                                                      lstFound.Items.Add(remoteIP)
                                                  Else
                                                      'lstFound.Items.Add("X " & remoteIP)
                                                  End If

                                              End Sub)

                                   Else
                                       If success Then
                                           lstFound.Items.Add(remoteIP)
                                       Else
                                           lstFound.Items.Add("X " & remoteIP)
                                       End If
                                   End If


                               End Sub)
            tasks.Add(ta)
            ta.Start()

        Next
    End Sub

    Private Sub timerUpdateProgress_Tick(sender As Object, e As EventArgs) Handles timerUpdateProgress.Tick
        progressBar.Minimum = 0
        progressBar.Maximum = 254

        Dim completed As Integer = 0
        For Each t In tasks
            If t.IsCompleted Then completed += 1
        Next
        progressBar.Value = completed
    End Sub

    Private Sub lstFound_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstFound.SelectedIndexChanged
        RemoteAddress = txtAddressFormat.Text.Replace("[ip]", lstFound.SelectedItem).Replace("[port]", txtPort.Text)
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        DialogResult = DialogResult.OK
        Close()
    End Sub
End Class