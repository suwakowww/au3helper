' コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

Public NotInheritable Class mousemove_cdlg
    Inherits ContentDialog

    Public addcode As String = Nothing

    Private Sub ContentDialog_PrimaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
        If move_adv.IsChecked = True Then
            addcode = "mousemove(" + move_x.Text + "," + move_y.Text + "," + (Math.Abs(move_s.Value).ToString) + ")"
        Else
            addcode = "mousemove(" + move_x.Text + "," + move_y.Text + ")"
        End If
        'rawcode.Text = rawcode.Text + addcode.Trim() + vbCrLf
        '执行完成之后恢复初始设置
        move_x.Text = ""
        move_y.Text = ""
        move_s.Value = -10
    End Sub

    Private Sub ContentDialog_SecondaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)

    End Sub

    Private Sub move_adv_Click(sender As Object, e As RoutedEventArgs)
        If move_adv.IsChecked = True Then
            move_s.Visibility = Visibility.Visible
            move_text_s.Visibility = Visibility.Visible
        Else
            move_s.Visibility = Visibility.Collapsed
            move_text_s.Visibility = Visibility.Collapsed
        End If

        If Window.Current.Bounds.Height <= 600 AndAlso move_adv.IsChecked = True Then
            '修复 ContentDialog 显示隐藏项目后导致无法显示所有项目的问题
            scrollbar.VerticalScrollBarVisibility = ScrollBarVisibility.Visible
            scrollbar.Height = Window.Current.Bounds.Height - 128
        Else
            '要是屏幕高度足够，则无需进行修复，并隐藏不必要的滚动条
            ScrollBar.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden
        End If
    End Sub

End Class
