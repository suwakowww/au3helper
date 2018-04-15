' コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

Public NotInheritable Class df_cm_cdlg
    Inherits ContentDialog

    Public addcode As String = Nothing

    Private Sub ContentDialog_PrimaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
        Dim fifl As String = Nothing
        Dim fifl2 As String = Nothing
        Dim fifl3 As String = Nothing
        If fifl_type.SelectedIndex = 0 Then
            fifl = "file"
        Else
            fifl = "dir"
        End If
        If fifl_act.SelectedIndex = 0 Then
            fifl2 = "copy"
        Else
            fifl2 = "move"
        End If
        If fifl_rep.IsChecked = True Then
            fifl3 = ",1"
        End If
        addcode = String.Format("{0}{1}(""{2}"",""{3}""{4})", fifl, fifl2, fifl_src.Text, fifl_dst.Text, fifl3)
    End Sub

    Private Sub ContentDialog_SecondaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)

    End Sub

    Private Sub ContentDialog_Loaded(sender As Object, e As RoutedEventArgs)
        If Window.Current.Bounds.Height <= 400 Then
            '修复 ContentDialog 显示隐藏项目后导致无法显示所有项目的问题
            scrollbar.VerticalScrollBarVisibility = ScrollBarVisibility.Visible
            scrollbar.Height = Window.Current.Bounds.Height - 128
        Else
            '要是屏幕高度足够，则无需进行修复，并隐藏不必要的滚动条
            scrollbar.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden
        End If
    End Sub
End Class
