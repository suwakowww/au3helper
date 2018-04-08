' 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください
Imports System.Text.RegularExpressions
Imports Windows.Storage
Imports Windows.Storage.Pickers
Imports Windows.Storage.Streams
Imports Windows.UI.Popups

''' <summary>
''' それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    Private Sub rawcode_LostFocus(sender As Object, e As RoutedEventArgs)
        converting.IsActive = True
        src2dst()
        converting.IsActive = False
    End Sub

    Private Sub src2dst()
        Dim codeconvert As String
        Dim perlinetext As String()
        Dim result As String = Nothing
        codeconvert = rawcode.Text
        If codeconvert <> "" Then
            perlinetext = codeconvert.Split(vbCrLf)
            converted.Text = ""
            For i = 0 To perlinetext.Count - 1
                result = result + au3convert.au3convert(perlinetext(i))
            Next
            converted.Text = result
            m_converted.Text = result
        Else
            converted.Text = ""
        End If
    End Sub

    Private Sub src2dst_m()
        Dim codeconvert As String
        Dim perlinetext As String()
        Dim result As String = Nothing
        codeconvert = m_rawcode.Text
        If codeconvert <> "" Then
            perlinetext = codeconvert.Split(vbCrLf)
            converted.Text = ""
            For i = 0 To perlinetext.Count - 1
                result = result + au3convert.au3convert(perlinetext(i))
            Next
            m_converted.Text = result
        Else
            m_converted.Text = ""
        End If
    End Sub

    Private Async Sub btn_about_Click(sender As Object, e As RoutedEventArgs)
        Dim dlg_content As String
        dlg_content = "一个写 Au3 脚本的工具。" + vbCrLf + "系统平台：" + Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily + vbCrLf
        dlg_content = dlg_content + "分辨率（虚拟）：" + Window.Current.Bounds.Width.ToString + " x " + Window.Current.Bounds.Height.ToString
        Dim about_dlg As New ContentDialog With
        {
            .Title = "关于",
            .Content = dlg_content,
            .PrimaryButtonText = "源代码",
            .SecondaryButtonText = "关闭"
        }
        AddHandler about_dlg.PrimaryButtonClick, AddressOf repo_clicked
        Await about_dlg.ShowAsync()
    End Sub

    Private Async Sub repo_clicked(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
        Await Windows.System.Launcher.LaunchUriAsync(New Uri("https://github.com/suwakowww/au3helper"))
    End Sub

    Private Sub Mainpage_Loaded(sender As Object, e As RoutedEventArgs)
        If Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily = "Windows.Mobile" Then

        End If
        checkwidth()
        additems()
        Dim menulist As New List(Of au3functions)()
        menulist = au3func.get_funcs()
        funclist.ItemsSource = menulist
    End Sub

#Region "检测屏幕宽度"
    Private Sub checkwidth()
        Select Case Window.Current.Bounds.Width
            Case Is <= 320
                mainsplit.DisplayMode = SplitViewDisplayMode.Overlay
                mainsplit.IsPaneOpen = False
                left_menu.Visibility = Visibility.Visible
                low_width.Visibility = Visibility.Visible
                desktop_ana.Visibility = Visibility.Collapsed
                desktop_src.Visibility = Visibility.Collapsed
                mobile_src_ana.Visibility = Visibility.Visible
            Case Is < 640
                mainsplit.DisplayMode = SplitViewDisplayMode.Overlay
                mainsplit.IsPaneOpen = False
                left_menu.Visibility = Visibility.Visible
                low_width.Visibility = Visibility.Collapsed
                desktop_ana.Visibility = Visibility.Collapsed
                desktop_src.Visibility = Visibility.Collapsed
                mobile_src_ana.Visibility = Visibility.Visible
            Case Is < 768
                mainsplit.DisplayMode = SplitViewDisplayMode.Inline
                mainsplit.IsPaneOpen = True
                left_menu.Visibility = Visibility.Collapsed
                low_width.Visibility = Visibility.Collapsed
                desktop_ana.Visibility = Visibility.Collapsed
                desktop_src.Visibility = Visibility.Collapsed
                mobile_src_ana.Visibility = Visibility.Visible
            Case Is < 1280
                mainsplit.DisplayMode = SplitViewDisplayMode.Overlay
                mainsplit.IsPaneOpen = False
                left_menu.Visibility = Visibility.Visible
                low_width.Visibility = Visibility.Collapsed
                desktop_ana.Visibility = Visibility.Visible
                desktop_src.Visibility = Visibility.Visible
                mobile_src_ana.Visibility = Visibility.Collapsed
            Case Else
                mainsplit.DisplayMode = SplitViewDisplayMode.Inline
                mainsplit.IsPaneOpen = True
                left_menu.Visibility = Visibility.Collapsed
                low_width.Visibility = Visibility.Collapsed
                desktop_ana.Visibility = Visibility.Visible
                desktop_src.Visibility = Visibility.Visible
                mobile_src_ana.Visibility = Visibility.Collapsed
        End Select
        'If Window.Current.Bounds.Width < 640 Then
        '    '由于这种水平分辨率太小，隐藏这些功能
        '    mainsplit.DisplayMode = SplitViewDisplayMode.Overlay
        '    left_menu.Visibility = Visibility.Visible
        '    If Window.Current.Bounds.Width <= 320 Then
        '        low_width.Visibility = Visibility.Visible
        '    End If
        'ElseIf Window.Current.Bounds.Width >= 1280 Then
        '    '由于这种水平分辨率太小，隐藏这些功能
        '    mainsplit.DisplayMode = SplitViewDisplayMode.Inline
        '    mainsplit.IsPaneOpen = True
        '    left_menu.Visibility = Visibility.Collapsed
        'Else
        '    mainsplit.DisplayMode = SplitViewDisplayMode.Overlay
        '    left_menu.Visibility = Visibility.Visible
        'End If
        'If Window.Current.Bounds.Width < 768 Then
        '    desktop_src.Visibility = Visibility.Collapsed
        '    desktop_ana.Visibility = Visibility.Collapsed
        '    mobile_src_ana.Visibility = Visibility.Visible
        'Else
        '    desktop_src.Visibility = Visibility.Visible
        '    desktop_ana.Visibility = Visibility.Visible
        '    mobile_src_ana.Visibility = Visibility.Collapsed
        'End If
    End Sub
#End Region

#Region "低分辨率提示"
    Private Async Sub low_width_Click(sender As Object, e As RoutedEventArgs)
        Dim low_width_dlg As New ContentDialog With
            {
                .Title = "一部分功能已禁用",
                .Content = "由于屏幕宽度太低，无法完整显示所有内容，故禁用了一部分功能。" + vbCrLf + "请考虑降低 DPI 设置使用。" + vbCrLf + "（可能需要重启）",
                .PrimaryButtonText = "显示设置",
                .SecondaryButtonText = "关闭"
            }
        AddHandler low_width_dlg.PrimaryButtonClick, AddressOf to_display_settings
        Await low_width_dlg.ShowAsync()
    End Sub
#End Region

    Private Async Sub to_display_settings(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
        Await Windows.System.Launcher.LaunchUriAsync(New Uri("ms-settings:display"))
    End Sub

    Private Sub left_menu_Click(sender As Object, e As RoutedEventArgs)
        mainsplit.IsPaneOpen = Not mainsplit.IsPaneOpen
    End Sub

    Private Async Sub s_save_Tapped(sender As Object, e As TappedRoutedEventArgs)
        '保存文件
        s_save.IsSelected = False
        Dim write_data As String = rawcode.Text
        Dim savefile As New FileSavePicker
        Dim filetype As New Object
        filetype = {".au3"}
        savefile.FileTypeChoices.Add("Autoit 3 Script", filetype)
        s_save.IsSelected = False
        Dim tofile As StorageFile = Await savefile.PickSaveFileAsync()
        If tofile IsNot Nothing Then
            Using transaction As StorageStreamTransaction = Await tofile.OpenTransactedWriteAsync
                Using textwrite As DataWriter = New DataWriter(transaction.Stream)
                    textwrite.WriteString(write_data)
                    transaction.Stream.Size = Await textwrite.StoreAsync()
                    Await transaction.CommitAsync()
                End Using
            End Using
        End If
        mainsplit.IsPaneOpen = Not mainsplit.IsPaneOpen
    End Sub

    Private Async Sub s_open_Tapped(sender As Object, e As TappedRoutedEventArgs)
        '读取文件
        s_open.IsSelected = False
        Dim fileopen As New FileOpenPicker()
        fileopen.FileTypeFilter.Add(".au3")
        Dim sfile As StorageFile = Await fileopen.PickSingleFileAsync()
        If sfile IsNot Nothing Then
            Using readstream As IRandomAccessStream = Await sfile.OpenAsync(FileAccessMode.Read)
                Using textread As DataReader = New DataReader(readstream)
                    Dim size As UInt64 = readstream.Size
                    If size <= UInt32.MaxValue Then
                        Dim numbytesloaded As UInt32 = Await textread.LoadAsync(Convert.ToInt32(size))
                        Dim filecontent As String = textread.ReadString(numbytesloaded)
                        rawcode.Text = filecontent
                    End If
                End Using
            End Using
        End If
        mainsplit.IsPaneOpen = Not mainsplit.IsPaneOpen
    End Sub

    Private Async Sub s_new_Tapped(sender As Object, e As TappedRoutedEventArgs)
        '新建文件（清空）
        s_new.IsSelected = False
        If rawcode.Text <> "" Then
            Dim clearwarn As New MessageDialog("确定要新建文件吗？", "警告")
            clearwarn.Commands.Add((New UICommand("确定", AddressOf clearcode, commandId:=0)))
            clearwarn.Commands.Add((New UICommand("取消", AddressOf clearcode, commandId:=1)))
            Dim result As Object = Await clearwarn.ShowAsync()
        End If
        mainsplit.IsPaneOpen = Not mainsplit.IsPaneOpen
    End Sub

    Private Sub clearcode(command As IUICommand)
        'Throw New NotImplementedException()
        If command.Id = 0 Then
            rawcode.Text = ""
        End If
    End Sub

    Private Sub Mainpage_SizeChanged(sender As Object, e As SizeChangedEventArgs)
        checkwidth()
    End Sub

    Private Sub mobile_src_ana_p_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        src2dst_m()
    End Sub

    Public Sub additems()
        'Dim mainitem As List(Of au3functions) = New List(Of au3functions)()
        'mainitem.Add(New au3functions With {.au3funcs = "WinWait", .au3funccat = "Windows 窗口"})
        'Dim items As List(Of au3funcgrp) = (From item In mainitem
        '                                    Group item By item.au3funccat Into g
        '                                    Select New au3funcgrp With {.key = g.key, .itemcontent = g.ToList()}).tolist()
    End Sub

    '重写的函数列表，原菜单删除
    Private Async Sub funclist_ItemClick(sender As Object, e As ItemClickEventArgs)
        Dim dlg_r As ContentDialogResult
        Select Case DirectCast(e.ClickedItem, au3helper.au3functions).au3funcs
            Case Is = "WinWait", Is = "WinWaitActive", Is = "WinWaitClose", Is = "WinWaitNotActive"
                Dim winwait_dlg As New winwait_cdlg
                dlg_r = Await winwait_dlg.ShowAsync()
                If dlg_r = ContentDialogResult.Primary Then
                    rawcode.Text = rawcode.Text + winwait_dlg.addcode.Trim() + vbCrLf
                End If
            Case Is = "WinActive", Is = "WinClose", Is = "WinKill"
                Dim winact_dlg As New winact_cdlg
                dlg_r = Await winact_dlg.ShowAsync()
                If dlg_r = ContentDialogResult.Primary Then
                    rawcode.Text = rawcode.Text + winact_dlg.addcode.Trim() + vbCrLf
                End If
            Case Is = "MouseMove"
                Dim mousemove_dlg As New mousemove_cdlg
                dlg_r = Await mousemove_dlg.ShowAsync()
                If dlg_r = ContentDialogResult.Primary Then
                    rawcode.Text = rawcode.Text + mousemove_dlg.addcode.Trim() + vbCrLf
                End If
            Case Is = "MouseClick"
                Dim mouseclick_dlg As New mouseclick_cdlg
                dlg_r = Await mouseclick_dlg.ShowAsync()
                If dlg_r = ContentDialogResult.Primary Then
                    rawcode.Text = rawcode.Text + mouseclick_dlg.addcode.Trim() + vbCrLf
                End If
            Case Is = "ControlClick"
                Dim controlclick_dlg As New controlclick_cdlg
                dlg_r = Await controlclick_dlg.ShowAsync()
                If dlg_r = ContentDialogResult.Primary Then
                    rawcode.Text = rawcode.Text + controlclick_dlg.addcode.Trim() + vbCrLf
                End If
            Case Is = "Send"
                Dim sendkey_dlg As New sendkey_cdlg
                dlg_r = Await sendkey_dlg.ShowAsync()
                If dlg_r = ContentDialogResult.Primary Then
                    rawcode.Text = rawcode.Text + sendkey_dlg.addcode.Trim() + vbCrLf
                End If
            Case Is = "Sleep"
                Dim sleep_dlg As New sleep_cdlg
                dlg_r = Await sleep_dlg.ShowAsync()
                If dlg_r = ContentDialogResult.Primary Then
                    rawcode.Text = rawcode.Text + sleep_dlg.addcode.Trim() + vbCrLf
                End If
            Case "Run"
                Dim run_dlg As New run_cdlg
                dlg_r = Await run_dlg.ShowAsync()
                If dlg_r = ContentDialogResult.Primary Then
                    rawcode.Text = rawcode.Text + run_dlg.addcode.Trim() + vbCrLf
                End If
            Case Else
                Dim unsupport_dlg As New ContentDialog With
                    {
                    .Title = "未支持的函数",
                    .Content = "本函数尚未支持生成。",
                    .PrimaryButtonText = "关闭"
                    }
                Await unsupport_dlg.ShowAsync()
        End Select
    End Sub

    'Private Sub l_d_toggle_Click(sender As Object, e As RoutedEventArgs)
    'If CType(Window.Current.Content, Frame).RequestedTheme = ApplicationTheme.Light Then
    'CType(Window.Current.Content, Frame).RequestedTheme = ApplicationTheme.Dark
    'Else
    'CType(Window.Current.Content, Frame).RequestedTheme = ApplicationTheme.Light
    'End If
    'If Application.Current.RequestedTheme = ApplicationTheme.Light Then
    'Application.Current.RequestedTheme = ApplicationTheme.Dark
    'Else
    'Application.Current.RequestedTheme = ApplicationTheme.Light
    'End If
    'End Sub
End Class
