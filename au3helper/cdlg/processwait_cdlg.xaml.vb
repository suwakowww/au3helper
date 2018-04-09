' コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

Public NotInheritable Class processwait_cdlg
    Inherits ContentDialog

    Public addcode As String

    Private Sub ContentDialog_PrimaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
        Dim w_close As String
        If pw_waitclose.IsChecked = True Then
            w_close = "close"
        Else
            w_close = Nothing
        End If
        If pw_timeout.Text <> "" Then
            addcode = String.Format("processwait{0}(""{1}"",{2})", w_close, pw_name.Text, pw_timeout.Text)
        Else
            addcode = String.Format("processwait{0}(""{1}"")", w_close, pw_name.Text)
        End If
    End Sub

    Private Sub ContentDialog_SecondaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)

    End Sub
End Class
