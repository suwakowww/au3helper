' コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

Public NotInheritable Class winact_cdlg
    Inherits ContentDialog

    Public addcode As String = Nothing

    Private Sub ContentDialog_PrimaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
        Dim clickkey As String
        Select Case wa_act.SelectedIndex
            Case 0
                clickkey = "winactivate"
            Case 1
                clickkey = "winclose"
            Case Else
                clickkey = "winkill"
        End Select
        If wa_string.Text <> "" Then
            addcode = clickkey + "(""" + wa_title.Text + """,""" + wa_string.Text + """)"
        Else
            addcode = clickkey + "(""" + wa_title.Text + """)"
        End If
    End Sub

    Private Sub ContentDialog_SecondaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)

    End Sub
End Class
