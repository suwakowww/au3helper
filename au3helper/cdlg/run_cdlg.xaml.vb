' コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

Public NotInheritable Class run_cdlg
    Inherits ContentDialog

    Public addcode As String = Nothing

    Private Sub ContentDialog_PrimaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
        addcode = "run(""" + run_prog.Text + """)"
    End Sub

    Private Sub ContentDialog_SecondaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)

    End Sub
End Class
