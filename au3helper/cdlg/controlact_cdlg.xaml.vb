' コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

Public NotInheritable Class controlact_cdlg
    Inherits ContentDialog

    Public addcode As String

    Private Sub ContentDialog_PrimaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
        Dim clickkey As String
        Select Case ca_act.SelectedIndex
            Case 0
                clickkey = "enable"
            Case 1
                clickkey = "disable"
            Case 2
                clickkey = "show"
            Case 3
                clickkey = "hide"
            Case Else
                clickkey = "focus"
        End Select
        addcode = String.Format("control{0}(""{1}"",""{2}"",{3})", clickkey, c_act_title.Text, c_act_string.Text, c_act_id.Text)
    End Sub

    Private Sub ContentDialog_SecondaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)

    End Sub

    Private Sub ContentDialog_Loaded(sender As Object, e As RoutedEventArgs)
        If Window.Current.Bounds.Height <= 372 Then
            '修复 ContentDialog 显示隐藏项目后导致无法显示所有项目的问题
            scrollbar.VerticalScrollBarVisibility = ScrollBarVisibility.Visible
            scrollbar.Height = Window.Current.Bounds.Height - 128
        Else
            '要是屏幕高度足够，则无需进行修复，并隐藏不必要的滚动条
            scrollbar.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden
        End If
    End Sub
End Class
