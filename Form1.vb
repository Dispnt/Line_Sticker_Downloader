Imports System.Net
Imports System.IO
Imports System.Net.NetworkInformation
Imports System.Text.RegularExpressions

Public Class Form1
    Dim pageno As Integer, pageend As Integer, dlpath As String

    Private Sub path_Click(sender As Object, e As EventArgs) Handles path.Click
        FolderBrowserDialog1.ShowDialog()
        path.Text = FolderBrowserDialog1.SelectedPath
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            status.ForeColor = Color.Crimson
            status.Text = "Connecting to line.me..."
            Dim urladdress As String = source.Text
            Dim streamb As IO.Stream = WebRequest.Create(urladdress).GetResponse().GetResponseStream() 'urladress为网页地址。
            Dim srb As StreamReader = New StreamReader(streamb, System.Text.Encoding.UTF8)
            Dim startno As String = Regex.Match(srb.ReadToEnd, "background-image:url[\s\S]{120}").ToString
            srb.Dispose()
            beginfrom.Text = Regex.Match(startno, "[1-9][0-9]{5,}").ToString
            ''从...开始结束
            Dim streame As IO.Stream = WebRequest.Create(urladdress).GetResponse().GetResponseStream() 'urladress为网页地址。
            Dim sre As StreamReader = New StreamReader(streame, System.Text.Encoding.UTF8)
            Dim endno As String = Regex.Match(sre.ReadToEnd, "background-image:url[\s\S]{120}", RegexOptions.RightToLeft).ToString
            sre.Dispose()
            endto.Text = Regex.Match(endno, "[1-9][0-9]{4,}").ToString
            '''''自动获取完成
            status.Text = "Downloading From server(lagging)..."
            pageno = beginfrom.Text
            pageend = endto.Text
            Do While pageno <= pageend
                My.Computer.Network.DownloadFile(server.Text & pageno & "/android/sticker.png", path.Text & pageno & ".png")

                ListView1.Items.Add(pageno & ".png")
                pageno = pageno + 1
                status.Text = "Downloading " & pageno & ".png..."
                Threading.Thread.Sleep(200)
                Application.DoEvents()
            Loop
        Catch ex As Exception

            status.Text = "Error!"
            MsgBox(ex.Message)
            'MsgBox("if photo exists in target path , Check internet connection and whether source URL is available" & Chr(10) & " 检查网络连接,路径下是否已存在部分图片,网址是否正确(https) ")
        End Try
        status.Text = "Done"
        status.ForeColor = Color.MediumSeaGreen
        PictureBox1.Visible = True
        ifconnected.Visible = False

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Process.Start(PictureBox1.ImageLocation)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Enabled = True
    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim p As Ping = New Ping()
        Dim pr As PingReply
        pr = p.Send("line.me")
        If (pr.Status <> IPStatus.Success) Then
            ifconnected.Checked = False 'fail
        Else
            ifconnected.Checked = True 'success

        End If
        Timer1.Enabled = False
        status.Text = "Done"
        status.ForeColor = Color.MediumSeaGreen
    End Sub

    Private Sub source_Click(sender As Object, e As EventArgs) Handles source.Click
        source.Focus()        '文本框获得焦点  
        source.SelectionStart = 0     '选取的起始位置设为0，即从头开始进行文本框内容的选取  
        source.SelectionLength = Len(source.Text)
    End Sub

    Private Sub GroupBox2_Enter(sender As Object, e As EventArgs) Handles GroupBox2.Enter

    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        FolderBrowserDialog1.ShowDialog()
        path.Text = FolderBrowserDialog1.SelectedPath
    End Sub


    Private Sub ListView1_Click(sender As Object, e As EventArgs) Handles ListView1.Click
        Try
            PictureBox1.ImageLocation = path.Text & ListView1.SelectedItems.Item(0).Text
        Catch ex As Exception
        End Try
    End Sub

End Class
'45-84!!!
'https://sdl-stickershop.line.naver.jp/stickershop/v1/sticker/******/ANDROID/sticker.png
