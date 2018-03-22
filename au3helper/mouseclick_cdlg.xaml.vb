' コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

Public NotInheritable Class mouseclick_cdlg
    Inherits ContentDialog

    Public addcode As String = Nothing

    Private Sub ContentDialog_PrimaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
        'Dim addcode As String = Nothing
        Dim clickkey As String = Nothing
        Select Case click_key.SelectedIndex
            Case 0
                clickkey = "left"
            Case 1
                clickkey = "middle"
            Case 2
                clickkey = "right"
            Case 3
                clickkey = "primary"
            Case Else
                clickkey = "secondary"
        End Select
        If click_adv.IsChecked = True Then
            addcode = "mouseclick(""" + clickkey + """"
            If click_x.Text <> "" AndAlso click_y.Text <> "" Then
                addcode = addcode + "," + click_x.Text + "," + click_y.Text
                If click_times.Text <> "" Then
                    addcode = addcode + "," + click_times.Text
                    addcode = addcode + "," + (Math.Abs(click_move_s.Value).ToString())
                End If
            End If
            addcode = addcode + ")"
        Else
            addcode = "mouseclick(""" + clickkey + """)"
        End If
        'rawcode.Text = rawcode.Text + addcode.Trim() + vbCrLf
        '执行完成之后恢复初始设置
        click_key.SelectedIndex = 3
        click_x.Text = ""
        click_y.Text = ""
        click_times.Text = ""
        click_move_s.Value = -10

    End Sub

    Private Sub ContentDialog_SecondaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)

    End Sub

    Private Sub click_adv_Click(sender As Object, e As RoutedEventArgs)
        If click_adv.IsChecked = True Then
            click_x.Visibility = Visibility.Visible
            click_x_text.Visibility = Visibility.Visible
            click_y.Visibility = Visibility.Visible
            click_y_text.Visibility = Visibility.Visible
            click_times_text.Visibility = Visibility.Visible
            click_times.Visibility = Visibility.Visible
            click_move_s_text.Visibility = Visibility.Visible
            click_move_s.Visibility = Visibility.Visible
        Else
            click_x.Visibility = Visibility.Collapsed
            click_x_text.Visibility = Visibility.Collapsed
            click_y.Visibility = Visibility.Collapsed
            click_y_text.Visibility = Visibility.Collapsed
            click_times_text.Visibility = Visibility.Collapsed
            click_times.Visibility = Visibility.Collapsed
            click_move_s_text.Visibility = Visibility.Collapsed
            click_move_s.Visibility = Visibility.Collapsed
        End If

        If Window.Current.Bounds.Height <= 600 AndAlso click_adv.IsChecked = True Then
            '修复 ContentDialog 显示隐藏项目后导致无法显示所有项目的问题
            scrollbar.VerticalScrollBarVisibility = ScrollBarVisibility.Visible
            scrollbar.Height = Window.Current.Bounds.Height - 128
        Else
            '要是屏幕高度足够，则无需进行修复，并隐藏不必要的滚动条
            scrollbar.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden
        End If
    End Sub
End Class
