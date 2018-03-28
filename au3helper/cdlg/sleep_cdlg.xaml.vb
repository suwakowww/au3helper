' コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

Public NotInheritable Class sleep_cdlg

    Inherits ContentDialog

    Public addcode As String = Nothing  '设置个公共变量方便其他位置调用


    Private Sub ContentDialog_PrimaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
        '遗留代码
        'Public addcode As String = Nothing
        addcode = "sleep(" + sleep_times.Text + ")"
        '遗留代码 x2
        'MainPage.rawcode.Text = rawcode.text + addcode.Trim() + vbCrLf
        'sleep_times.Text = ""
    End Sub

    Private Sub ContentDialog_SecondaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)

    End Sub
End Class
