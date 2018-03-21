' 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください
Imports System.Text.RegularExpressions

''' <summary>
''' それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page


    Private Sub rawcode_LostFocus(sender As Object, e As RoutedEventArgs)
        converting.IsActive = True
        Dim codeconvert As String
        Dim perlinetext As String()
        codeconvert = rawcode.Text
        If codeconvert <> "" Then
            perlinetext = codeconvert.Split(vbCrLf)
            converted.Text = ""
            For i = 0 To perlinetext.Count - 1
                converted.Text = converted.Text + au3convert(perlinetext(i))
            Next
        Else
            converted.Text = ""
        End If
        converting.IsActive = False
    End Sub

    Public Function au3convert(ByVal src As String)
        Dim result As String = Nothing
        Dim errorflag As Boolean = False
        src = src.Trim()    '清理空白字符
        If Regex.Matches(src, "run\(", RegexOptions.IgnoreCase).Count > 0 Then
            '检测 Run 函数
            result = Regex.Replace(src, "run\((\'|"")(.+?)(\'|"")\)", "运行 $2 程序", RegexOptions.IgnoreCase)

#Region "WinWait 系列函数"
        ElseIf Regex.Matches(src, "winwait(active|close|notactive)?\(", RegexOptions.IgnoreCase).Count > 0 Then
            '检测 WinWait 系列函数
            Select Case Regex.Matches(src, """,").Count
                Case 2
                    If Regex.Matches(src, "(winwaitactive|winwaitclose|winwaitnotactive|winwait)\(""(.+?)"",""(.+?)"",([0-9]+)\)", RegexOptions.IgnoreCase).Count > 0 Then
                        result = Regex.Replace(src, "(winwaitactive|winwaitclose|winwaitnotactive|winwait)\(""(.+?)"",""(.+?)"",([0-9]+)\)", "等待带有 $3 字符串的 $2 窗口 $1，如无则 $4 秒后继续往下执行", RegexOptions.IgnoreCase)
                    Else
                        errorflag = True
                        result = "语法错误：" + src
                    End If
                Case 1
                    If Regex.Matches(src, "(winwaitactive|winwaitclose|winwaitnotactive|winwait)\(""(.+?)"",""(.+?)""\)", RegexOptions.IgnoreCase).Count > 0 Then
                        result = Regex.Replace(src, "(winwaitactive|winwaitclose|winwaitnotactive|winwait)\(""(.+?)"",""(.+?)""\)", "等待带有 $2 字符串的 $1 窗口出现", RegexOptions.IgnoreCase)
                    Else
                        errorflag = True
                        result = "语法错误：" + src
                    End If
                Case Else
                    If Regex.Matches(src, "(winwaitactive|winwaitclose|winwaitnotactive|winwait)\(""(.+?)""\)", RegexOptions.IgnoreCase).Count > 0 Then
                        result = Regex.Replace(src, "(winwaitactive|winwaitclose|winwaitnotactive|winwait)\(""(.+?)""\)", "等待 $1 窗口出现", RegexOptions.IgnoreCase)
                    Else
                        errorflag = True
                        result = "语法错误：" + src
                    End If
            End Select
            If errorflag = False Then
                result = result.Replace("winwaitnotactive", "非激活")
                result = result.Replace("winwaitclose", "关闭")
                result = result.Replace("winwaitactive", "激活")
                result = result.Replace("winwait", "出现")
            End If
#End Region

#Region "Send 函数"
        ElseIf Regex.Matches(src, "send\(", RegexOptions.IgnoreCase).Count > 0 Then
            If Regex.Matches(src, "\!|\+|\^|\#").Count > 0 AndAlso Regex.Matches(src, "send\(""(.+?)""(,0)?\)", RegexOptions.IgnoreCase).Count > 0 Then
                result = Regex.Replace(src, "send\(""(.+?)""(,0)?\)", "发送 $1 组合键", RegexOptions.IgnoreCase)
                result = result.Replace("+", "Shift+")  '因为后面的加号会影响
                result = result.Replace("!", "Alt+")
                result = result.Replace("^", "Ctrl+")
                result = result.Replace("#", "Win+")
            ElseIf Regex.matches(src, "send\(""(.+?)""(,1)?\)", RegexOptions.IgnoreCase).Count > 0 Then
                result = Regex.Replace(src, "send\(""(.+?)""(,1)?\)", "发送按键 $1", RegexOptions.IgnoreCase)
            Else
                result = "语法错误：" + src
            End If
#End Region

        ElseIf Regex.Matches(src, "sleep\(", RegexOptions.IgnoreCase).Count > 0 Then
            '检测 Sleep 函数
            result = Regex.Replace(src, "sleep\(([0-9]+)\)", "等待 $1 毫秒后继续往下执行", RegexOptions.IgnoreCase)

        ElseIf src = "" Then
            '防止下面的检测首字符导致崩溃
            result = src

        ElseIf src.Substring(0, 1) = ";" Then
            '检测注释
            result = Regex.Replace(src, ";(.+)", "<注释：$1>")

#Region "ControlClick 函数"
        ElseIf Regex.Matches(src, "controlclick\("， RegexOptions.IgnoreCase).Count > 0 Then
            Select Case Regex.Matches(src, ",").Count
                Case 2
                    If Regex.Matches(src, "controlclick\(""(.+?)"",""(.+?)"",([0-9]+)\)", RegexOptions.IgnoreCase).Count > 0 Then
                        result = Regex.Replace(src, "controlclick\(""(.+?)"",""(.+?)"",([0-9]+)\)", "在带有 $2 字符串的 $1 窗口内点击控件 ID 为 $3 的按钮", RegexOptions.IgnoreCase)
                    Else
                        result = "语法错误：" + src
                    End If
                Case 3
                    If Regex.Matches(src, "controlclick\(""(.+?)"",""(.+?)"",([0-9]+),""(.+?)""\)", RegexOptions.IgnoreCase).Count > 0 Then
                        result = Regex.Replace(src, "controlclick\(""(.+?)"",""(.+?)"",([0-9]+),""(.+?)""\)", "在带有 $2 字符串的 $1 窗口内用鼠标 $4 键点击控件 ID 为 $3 的按钮", RegexOptions.IgnoreCase)
                    Else
                        result = "语法错误：" + src
                    End If
                Case 4
                    If Regex.Matches(src, "controlclick\(""(.+?)"",""(.+?)"",([0-9]+),""(.+?)"",([0-9]+)\)", RegexOptions.IgnoreCase).Count > 0 Then
                        result = Regex.Replace(src, "controlclick\(""(.+?)"",""(.+?)"",([0-9]+),""(.+?)"",([0-9]+)\)", "在带有 $2 字符串的 $1 窗口内用鼠标 $4 键点击 $5 次控件 ID 为 $3 的按钮", RegexOptions.IgnoreCase)
                    Else
                        result = "语法错误" + src
                    End If
                Case Else
                    result = "参数错误：" + src
            End Select
            'result = Regex.Replace(src, "controlclick\(""(.+?)"",""(.+?)"",([0-9]+)\)", "在带有 $2 字符串的 $1 窗口内点击控件 ID 为 $3 的按钮", RegexOptions.IgnoreCase)
#End Region

#Region "MouseMove 函数"
        ElseIf Regex.Matches(src, "mousemove\(", RegexOptions.IgnoreCase).Count > 0 Then
            '检测 MouseMove 函数
            If Regex.Matches(src, ",").Count > 0 Then
                If Regex.Matches(src, "mousemove\(([0-9]+),([0-9])+,([0-9]+)+\)", RegexOptions.IgnoreCase).Count > 0 Then
                    result = Regex.Replace(src, "mousemove\(([0-9]+),([0-9])+,([0-9]+)+\)", "将鼠标以 $3 的速度移动到 ($1,$2) 的位置（速度值越大越慢）", RegexOptions.IgnoreCase)
                Else
                    result = "语法错误：" + src
                End If
            Else
                If Regex.Matches(src, "mousemove\(([0-9]+),([0-9]+)\)", RegexOptions.IgnoreCase).Count > 0 Then
                    result = Regex.Replace(src, "mousemove\(([0-9]+),([0-9]+)\)", "将鼠标移动到 ($1,$2) 的位置", RegexOptions.IgnoreCase)
                Else
                    result = "语法错误：" + src
                End If
            End If
#End Region

#Region "MouseClick 函数"
        ElseIf Regex.Matches(src, "mouseclick\(", RegexOptions.IgnoreCase).Count > 0 Then
            If Regex.Matches(src, ",").Count > 0 Then
                Select Case Regex.Matches(src, ",").Count
                    Case 2
                        If Regex.Matches(src, "mouseclick\(""(left|right|middle|main|menu|primary|secondary)"",([0-9]+),([0-9]+)\)", RegexOptions.IgnoreCase).Count > 0 Then
                            result = Regex.Replace(src, "mouseclick\(""(left|right|middle|main|menu|primary|secondary)"",([0-9]+),([0-9]+)\)", "在 ($2,$3) 上点击鼠标 $1 键", RegexOptions.IgnoreCase)
                        Else
                            errorflag = True
                            result = "语法错误：" + src
                        End If
                    Case 3
                        If Regex.Matches(src, "mouseclick\(""(left|right|middle|main|menu|primary|secondry)"",([0-9]+),([0-9]+),([0-9]+)\)", RegexOptions.IgnoreCase).Count > 0 Then
                            result = Regex.Replace(src, "mouseclick\(""(left|right|middle|main|menu|primary|secondry)"",([0-9]+),([0-9]+),([0-9]+)\)", "在 ($2,$3) 上点击 $4 次鼠标 $1 键", RegexOptions.IgnoreCase)
                        Else
                            errorflag = True
                            result = "语法错误：" + src
                        End If
                    Case 4
                        If Regex.Matches(src, "mouseclick\(""(left|right|middle|main|menu|primary|secondry)"",([0-9]+),([0-9]+),([0-9]+),([0-9]+)\)", RegexOptions.IgnoreCase).Count > 0 Then
                            result = Regex.Replace(src, "mouseclick\(""(left|right|middle|main|menu|primary|secondry)"",([0-9]+),([0-9]+),([0-9]+),([0-9]+)\)", "以 $5 的速度移动到 ($2,$3) 并点击 $4 次鼠标 $1 键（速度值越大越慢）", RegexOptions.IgnoreCase)
                        Else
                            errorflag = True
                            result = "语法错误：" + src
                        End If
                    Case Else
                        errorflag = True
                        result = "语法错误：" + src
                End Select
            Else
                If Regex.Matches(src, "mouseclick\(""(left|right|middle|main|menu|primary|secondry)""\)", RegexOptions.IgnoreCase).Count > 0 Then
                    result = Regex.Replace(src, "mouseclick\(""(left|right|middle|main|menu|primary|secondry)""\)", "点击鼠标 $1 键")
                Else
                    errorflag = True
                    result = "语法错误：" + src
                End If
            End If
#End Region

                Else
                '语法错误，或者不支持，则原样输出
                result = src
        End If
        result = result + vbLf
        Return result
    End Function

#Region "生成 Send 代码"
    Private Sub btn_insertkey_Click(sender As Object, e As RoutedEventArgs)
        Dim addcode As String = Nothing
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
        rawcode.Text = rawcode.Text + addcode.Trim() + vbCrLf
    End Sub
#End Region

#Region "键盘原始输入开关"
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
#End Region

    Private Sub m_move_Click(sender As Object, e As RoutedEventArgs)
        mousemove_fly.ShowAt(m_move)
    End Sub

#Region "生成 MouseMove 代码"
    Private Sub btn_insertmmove_Click(sender As Object, e As RoutedEventArgs)
        Dim addcode As String = Nothing
        If move_adv.IsChecked = True Then
            addcode = "mousemove(" + move_x.Text + "," + move_y.Text + "," + (Math.Abs(move_s.Value).ToString) + ")"
        Else
            addcode = "mousemove(" + move_x.Text + "," + move_y.Text + ")"
        End If
        rawcode.Text = rawcode.Text + addcode.Trim() + vbCrLf
        '执行完成之后恢复初始设置
        move_x.Text = ""
        move_y.Text = ""
        move_s.Value = -10
    End Sub
#End Region

    Private Sub m_click_Click(sender As Object, e As RoutedEventArgs)
        mouseclick_fly.ShowAt(m_click)
    End Sub

#Region "鼠标移动高级选项显示/隐藏"
    Private Sub move_adv_Click(sender As Object, e As RoutedEventArgs)
        If move_adv.isChecked = True Then
            move_s.Visibility = Visibility.Visible
            move_text_s.Visibility = Visibility.Visible
        Else
            move_s.Visibility = Visibility.Collapsed
            move_text_s.Visibility = Visibility.Collapsed
        End If
    End Sub
#End Region

#Region "鼠标点击高级选项显示/隐藏"
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
    End Sub
#End Region

#Region "生成 MouseClick 代码"
    Private Sub btn_insertclick_click_Click(sender As Object, e As RoutedEventArgs)
        Dim addcode As String = Nothing
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
        rawcode.Text = rawcode.Text + addcode.Trim() + vbCrLf
        '执行完成之后恢复初始设置
        click_key.SelectedIndex = 3
        click_x.Text = ""
        click_y.Text = ""
        click_times.Text = ""
        click_move_s.Value = -10
    End Sub
#End Region

    Private Sub send_key_Click(sender As Object, e As RoutedEventArgs)
        sendkey_fly.ShowAt(send_key)
    End Sub

    Private Sub c_click_Click(sender As Object, e As RoutedEventArgs)
        controlclick_fly.ShowAt(c_click)
    End Sub

    Private Sub sleep_Click(sender As Object, e As RoutedEventArgs)
        sleep_fly.ShowAt(sleep)
    End Sub

#Region "控件点击高级选项显示/隐藏"
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

#End Region

#Region "生成 ControlClick 代码"
    Private Sub btn_controlclick_Click(sender As Object, e As RoutedEventArgs)
        Dim addcode As String = Nothing
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
        rawcode.Text = rawcode.Text + addcode.Trim() + vbCrLf
        c_click_title.Text = ""
        c_click_string.Text = ""
        c_click_id.Text = ""
        c_click_key.SelectedIndex = 3
        c_click_times.Text = ""
    End Sub
#End Region

#Region "生成 Sleep 代码"
    Private Sub btn_sleep_Click(sender As Object, e As RoutedEventArgs)
        Dim addcode As String = Nothing
        addcode = "sleep(" + sleep_times.Text + ")"
        rawcode.text = rawcode.text + addcode.Trim() + vbCrLf
        sleep_times.Text = ""
    End Sub
#End Region

    Private Sub w_wait_Click(sender As Object, e As RoutedEventArgs)
        winwait_fly.ShowAt(w_wait)
    End Sub

#Region "生成 Winwait 系列代码"
    Private Sub btn_winwait_Click(sender As Object, e As RoutedEventArgs)
        Dim addcode As String = Nothing
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
        rawcode.Text = rawcode.Text + addcode.Trim() + vbCrLf
        ww_set.SelectedIndex = 0
        ww_title.Text = ""
        ww_string.Text = ""
        ww_timeout.Text = ""
    End Sub
#End Region

    Private Sub run_exec_Click(sender As Object, e As RoutedEventArgs)
        run_fly.ShowAt(run_exec)
    End Sub

    Private Async Sub btn_about_Click(sender As Object, e As RoutedEventArgs)
        Dim about_dlg As New ContentDialog With
        {
            .Title = "关于",
            .Content = "还没写好" + vbCrLf + "设备：" + Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily,
            .PrimaryButtonText = "没写好",
            .SecondaryButtonText = "关闭"
        }
        AddHandler about_dlg.PrimaryButtonClick, AddressOf repo_clicked
        Await about_dlg.ShowAsync()
    End Sub

    Private Async Sub repo_clicked(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
        Await Windows.System.Launcher.LaunchUriAsync(New Uri("https://www.baidu.com"))
    End Sub

    Private Sub btn_run_exec_Click(sender As Object, e As RoutedEventArgs)
        Dim addcode As String = Nothing
        addcode = "run(""" + run_prog.Text + """)"
        rawcode.Text = rawcode.Text + addcode.Trim() + vbCrLf
        run_prog.Text = ""
    End Sub

    Private Sub Mainpage_Loaded(sender As Object, e As RoutedEventArgs)
        If Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily = "Windows.Mobile" Then
            winevt.Visibility = Visibility.Collapsed
            mousekey.Visibility = Visibility.Collapsed
            clickbtn.Visibility = Visibility.Collapsed
            cclickbtn.Visibility = Visibility.Collapsed
            timerbtn.Visibility = Visibility.Collapsed
            runbtn.Visibility = Visibility.Collapsed
        End If
        If Window.Current.Bounds.Width < Window.Current.Bounds.Height Then
            '先想想
        End If
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
