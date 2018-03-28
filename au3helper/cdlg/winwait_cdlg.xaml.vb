' コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

Public NotInheritable Class winwait_cdlg
    Inherits ContentDialog

    Public addcode As String = Nothing

    Private Sub ContentDialog_PrimaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
        Dim clickkey As String = Nothing
        Select Case ww_set.SelectedIndex
            Case 0
                clickkey = "winwait"
            Case 1
                clickkey = "winwaitactive"
            Case 2
                clickkey = "winwaitclose"
            Case 3
                clickkey = "winwaitnotactive"
        End Select
        addcode = clickkey + "(""" + ww_title.Text + """,""" + ww_string.Text + """"
        If ww_timeout.Text <> "" Then
            addcode = addcode + "," + ww_timeout.Text
        End If
        addcode = addcode + ")"
    End Sub

    Private Sub ContentDialog_SecondaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)

    End Sub

    Private Sub ContentDialog_Loaded(sender As Object, e As RoutedEventArgs)
        If Window.Current.Bounds.Height <= 600 Then
            '修复 ContentDialog 显示隐藏项目后导致无法显示所有项目的问题
            ScrollBar.VerticalScrollBarVisibility = ScrollBarVisibility.Visible
            ScrollBar.Height = Window.Current.Bounds.Height - 128
        Else
            '要是屏幕高度足够，则无需进行修复，并隐藏不必要的滚动条
            ScrollBar.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden
        End If
    End Sub
End Class
