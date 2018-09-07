Imports System.Net
Imports System.ComponentModel

' 개발자 정보
' 개발자명 : 이예찬(LeeYeChan)
' E-mail : ehn1225@naver.com
' 제작년일 : 20180731
' 학습의 목적을 벗어난 영리행위 및 무단 수정, 배포를 금지합니다.

Public Class Form1
    Dim Winhttp As New WinHttp.WinHttpRequest
    Dim wb As New WebClient
    Public WithEvents download As WebClient
    Dim Url As String = "http://radibrary.tistory.com/category/%EC%A7%84%ED%96%89%20%EC%A4%91/%EC%9B%94%EC%9A%94%EC%9D%BC"
    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        Process.Start("http://radibrary.tistory.com/category")
    End Sub
    Private Sub download_DownloadFileCompleted(sender As Object, e As AsyncCompletedEventArgs) Handles download.DownloadFileCompleted
        downloadalert.Text = "다운로드 상태 : 다운로드 완료"
    End Sub
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            TextBox3.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        downloadalert.Text = "다운로드 상태 : 다운로드 대기"

        Select Case ComboBox1.SelectedIndex
            Case 0
                Url = "http://radibrary.tistory.com/category/%EC%A7%84%ED%96%89%20%EC%A4%91/%EC%9B%94%EC%9A%94%EC%9D%BC"
            Case 1
                Url = "http://radibrary.tistory.com/category/%EC%A7%84%ED%96%89%20%EC%A4%91/%ED%99%94%EC%9A%94%EC%9D%BC"
            Case 2
                Url = "http://radibrary.tistory.com/category/%EC%A7%84%ED%96%89%20%EC%A4%91/%EC%88%98%EC%9A%94%EC%9D%BC"
            Case 3
                Url = "http://radibrary.tistory.com/category/%EC%A7%84%ED%96%89%20%EC%A4%91/%EB%AA%A9%EC%9A%94%EC%9D%BC"
            Case 4
                Url = "http://radibrary.tistory.com/category/%EC%A7%84%ED%96%89%20%EC%A4%91/%EA%B8%88%EC%9A%94%EC%9D%BC"
            Case 5
                Url = "http://radibrary.tistory.com/category/%EC%A7%84%ED%96%89%20%EC%A4%91/%ED%86%A0%EC%9A%94%EC%9D%BC"
            Case 6
                Url = "http://radibrary.tistory.com/category/%EC%A7%84%ED%96%89%20%EC%A4%91/%EC%9D%BC%EC%9A%94%EC%9D%BC"
        End Select
        Roadpage(Url)
        pageLabel.Text = 1
    End Sub
    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        pageLabel.Text = Int(pageLabel.Text) + 1
        Roadpage(Url & "?page=" & pageLabel.Text)
    End Sub
    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        If Not pageLabel.Text = 1 Then
            pageLabel.Text = Int(pageLabel.Text) - 1
            Roadpage(Url & "?page=" & pageLabel.Text)
        End If
    End Sub
    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = Chr(13) Then
            downloadalert.Text = "다운로드 상태 : 다운로드 대기"
            Url = "http://radibrary.tistory.com/search/" & TextBox2.Text
            Roadpage(Url)
            pageLabel.Text = 1
        End If
    End Sub

    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs) Handles ListView1.DoubleClick '다운로드
        Dim Name, Temp, ResultTemp As String
        Dim countnotice As Integer
        downloadalert.Text = "다운로드 상태 : 다운로드 준비"

        Winhttp.Open("GET", "http://radibrary.tistory.com/" & ListView1.Items(ListView1.FocusedItem.Index).SubItems(2).Text) '선택아이템 주소 접속
        Winhttp.Send()
        ResultTemp = Split(Split(Winhttp.ResponseText, "toggleMoreLess")(1), "</div>")(0) '리스트 부분만 남기고 불필요항 부분 제거
        countnotice = UBound(Split(ResultTemp, "https://t1.daumcdn.net/tistory_admin/blogs/image/extension/unknown.gif")) '항목수를 받아옴.
        If Not countnotice = 0 And Not TextBox3.Text = "" Then
            downloadalert.Text = "다운로드 상태 : 다운로드 중"
            For i = 1 To countnotice
                Temp = Split(Split(ResultTemp, "<a href=""")(i), """><img")(0) 'Temp에 다운로드 주소를 대입
                Name = Split(Split(ResultTemp, "style=""vertical-align: middle;"" />")(i), "</a><")(0) '다운로드 항목의 이름
                download = New WebClient()
                download.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)")
                download.DownloadFileTaskAsync(New Uri(Temp), TextBox3.Text & "\" & Name)
            Next
        Else
            downloadalert.Text = "다운로드 상태 : 오류발생"
            MessageBox.Show("해당 게시글에서 파일을 다운로드할 수 없습니다." & Chr(13) & "다운로드 경로가 올바른지 확인하십시오.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub Roadpage(Url As String)
        ListView1.Items.Clear()

        Dim countnotice As Integer
        Dim ResultTemp, Temp As String

        Winhttp.Open("GET", Url)
        Winhttp.Send()
        ResultTemp = Split(Split(Winhttp.ResponseText, "<div class=""list_title"">")(1), "<div class=""index_title"">")(0) '리스트 부분만 남기고 불필요항 부분 제거
        countnotice = UBound(Split(Winhttp.ResponseText, "<div class=""list_content"">")) '항목수를 받아옴.

        If Not countnotice = 0 Then
            For i = 1 To countnotice
                Temp = Split(Split(Winhttp.ResponseText, "<div class=""list_content"">")(i), "</div>")(0) 'temp에게시글을 받아옴.
                ListView1.Items.Add(Split(Split(Temp, ">")(2), "<")(0))  '라디오 이름 추가
                ListView1.Items(i - 1).SubItems.Add(Split(Split(Temp, ">")(9), "<")(0))  '업로드일 추가
                ListView1.Items(i - 1).SubItems.Add(Split(Split(Temp, "<a href=""")(1), """")(0)) '아이템 주소
            Next
        Else
            MessageBox.Show("해당주소에 접근할 수 없습니다.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            downloadalert.Text = "다운로드 상태 : 오류발생"
        End If
        
    End Sub
End Class
