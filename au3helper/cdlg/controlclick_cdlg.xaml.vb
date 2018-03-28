' コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

Public NotInheritable Class controlclick_cdlg
    Inherits ContentDialog

    Public addcode As String

    Private Sub ContentDialog_PrimaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
        Dim clickkey As String = Nothing
        Select Case c_click_key.SelectedIndex
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
        If c_click_adv.IsChecked = True Then
            addcode = "controlclick(""" + c_click_title.Text + """,""" + c_click_string.Text + """," + c_click_id.Text + ",""" + clickkey + """"
            If c_click_times.Text <> "" Then
                addcode = addcode + "," + c_click_times.Text
            End If
            addcode = addcode + ")"
        Else
            addcode = "controlclick(""" + c_click_title.Text + """,""" + c_click_string.Text + """," + c_click_id.Text + ")"
        End If
    End Sub

    Private Sub ContentDialog_SecondaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)

    End Sub

    Private Sub c_click_adv_Click(sender As Object, e As RoutedEventArgs)
        If c_click_adv.IsChecked = True Then
            c_click_key.Visibility = Visibility.Visible
            c_click_key_text.Visibility = Visibility.Visible
            c_click_key_text2.Visibility = Visibility.Visible
            c_click_key_text3.Visibility = Visibility.Visible
            c_click_times_text.Visibility = Visibility.Visible
            c_click_times.Visibility = Visibility.Visible
        Else
            c_click_key.Visibility = Visibility.Collapsed
            c_click_key_text.Visibility = Visibility.Collapsed
            c_click_key_text2.Visibility = Visibility.Collapsed
            c_click_key_text3.Visibility = Visibility.Collapsed
            c_click_times_text.Visibility = Visibility.Collapsed
            c_click_times.Visibility = Visibility.Collapsed
        End If
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
