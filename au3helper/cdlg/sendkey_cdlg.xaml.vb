' コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

Public NotInheritable Class sendkey_cdlg
    Inherits ContentDialog

    Public addcode As String = Nothing

    Private Sub ContentDialog_PrimaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
        Dim hotkey As String = Nothing
        If key_a.IsChecked = True Then
            hotkey = hotkey + "!"
        End If
        If key_s.IsChecked = True Then
            hotkey = hotkey + "+"
        End If
        If key_c.IsChecked = True Then
            hotkey = hotkey + "^"
        End If
        If key_w.IsChecked = True Then
            hotkey = hotkey + "#"
        End If
        If key_raw.IsOn Then
            addcode = "send(""" + insertkey.Text + """,1)"
        Else
            addcode = "send(""" + hotkey + insertkey.Text + """)"
        End If
    End Sub

    Private Sub ContentDialog_SecondaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)

    End Sub

    Private Sub key_raw_Toggled(sender As Object, e As RoutedEventArgs)
        If key_raw.IsOn Then
            key_a.IsEnabled = False
            key_c.IsEnabled = False
            key_s.IsEnabled = False
            key_w.IsEnabled = False
        Else
            key_a.IsEnabled = True
            key_c.IsEnabled = True
            key_s.IsEnabled = True
            key_w.IsEnabled = True
        End If

    End Sub

    Private Sub ContentDialog_Loaded(sender As Object, e As RoutedEventArgs)
        If Window.Current.Bounds.Height <= 600 Then
            '修复 ContentDialog 显示隐藏项目后导致无法显示所有项目的问题
            scrollbar.VerticalScrollBarVisibility = ScrollBarVisibility.Visible
            scrollbar.Height = Window.Current.Bounds.Height - 128
        Else
            '要是屏幕高度足够，则无需进行修复，并隐藏不必要的滚动条
            scrollbar.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden
        End If
    End Sub
End Class
