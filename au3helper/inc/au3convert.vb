Imports System.Text.RegularExpressions

Public Class au3convert
    Public Shared Function au3convert(ByVal src As String)
        Dim result As String = Nothing
        Dim errorflag As Boolean = False
        src = src.Trim()    '清理空白字符

#Region "Run 函数"
        If Regex.Matches(src, "run\(", RegexOptions.IgnoreCase).Count > 0 Then
            '检测 Run 函数
            result = Regex.Replace(src, "run\((\'|"")(.+?)(\'|"")\)", "运行 $2 程序", RegexOptions.IgnoreCase)
#End Region

#Region "ProcessWait 函数"
        ElseIf Regex.Matches(src, "process(wait|waitclose)\(", RegexOptions.IgnoreCase).Count > 0 Then
            '分离正则表达式
            Dim pw_regestr As String() = New String() _
                {
                "process(wait|waitclose)\(""(.+?)"",([0-9]+)\)",
                "process(wait|waitclose)\(""(.+?)""\)"
                }
            '这里第一条为二参数版（带等待时间，单位为秒），第二条为一参数版（不带等待时间）

            If src.Contains(""",") = True Then
                If Regex.Matches(src, pw_regestr(0), RegexOptions.IgnoreCase).Count > 0 Then
                    result = Regex.Replace(src, pw_regestr(0), "等待 $2 进程 $1，如无则 $3 秒后继续往下执行")
                Else
                    errorflag = True
                    result = "语法错误：" + src
                End If
            Else
                If Regex.Matches(src, pw_regestr(1), RegexOptions.IgnoreCase).Count > 0 Then
                    result = Regex.Replace(src, pw_regestr(1), "等待 $2 进程 $1", RegexOptions.IgnoreCase)
                Else
                    errorflag = True
                    result = "语法错误：" + src
                End If
            End If
            If errorflag = False Then
                result = result.Replace("waitclose", "结束")
                result = result.Replace("wait", "出现")
            End If
#End Region

#Region "WinWait 系列函数"
        ElseIf Regex.Matches(src, "winwait(active|close|notactive)?\(", RegexOptions.IgnoreCase).Count > 0 Then
            '分离正则表达式
            Dim ww_regestr As String() = New String() _
                {
                "(winwaitactive|winwaitclose|winwaitnotactive|winwait)\(""(.+?)"",""(.+?)"",([0-9]+)\)",
                "(winwaitactive|winwaitclose|winwaitnotactive|winwait)\(""(.+?)"",""(.+?)""\)",
                "(winwaitactive|winwaitclose|winwaitnotactive|winwait)\(""(.+?)""\)"
                }

            '检测 WinWait 系列函数
            Select Case Regex.Matches(src, """,").Count
                Case 2
                    If Regex.Matches(src, ww_regestr(0), RegexOptions.IgnoreCase).Count > 0 Then
                        result = Regex.Replace(src, ww_regestr(0), "等待带有 $3 字符串的 $2 窗口 $1，如无则 $4 秒后继续往下执行", RegexOptions.IgnoreCase)
                    Else
                        errorflag = True
                        result = "语法错误：" + src
                    End If
                Case 1
                    If Regex.Matches(src, ww_regestr(1), RegexOptions.IgnoreCase).Count > 0 Then
                        result = Regex.Replace(src, ww_regestr(1), "等待带有 $3 字符串的 $2 窗口 $1", RegexOptions.IgnoreCase)
                    Else
                        errorflag = True
                        result = "语法错误：" + src
                    End If
                Case Else
                    If Regex.Matches(src, ww_regestr(2), RegexOptions.IgnoreCase).Count > 0 Then
                        result = Regex.Replace(src, ww_regestr(2), "等待 $2 窗口 $1", RegexOptions.IgnoreCase)
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
            Dim s_regestr As String() = New String() _
                {
                "send\(""(.+?)""(,0)?\)",
                "send\(""(.+?)""(,1)?\)"
                }
            If Regex.Matches(src, "\!|\+|\^|\#").Count > 0 AndAlso Regex.Matches(src, s_regestr(0), RegexOptions.IgnoreCase).Count > 0 Then
                result = Regex.Replace(src, s_regestr(0), "发送 $1 组合键", RegexOptions.IgnoreCase)
                result = result.Replace("+", "Shift+")  '因为后面的加号会影响
                result = result.Replace("!", "Alt+")
                result = result.Replace("^", "Ctrl+")
                result = result.Replace("#", "Win+")
            ElseIf Regex.Matches(src, s_regestr(1), RegexOptions.IgnoreCase).Count > 0 Then
                result = Regex.Replace(src, s_regestr(1), "发送按键 $1", RegexOptions.IgnoreCase)
            Else
                result = "语法错误：" + src
            End If
#End Region

#Region "Sleep 函数"
        ElseIf Regex.Matches(src, "sleep\(", RegexOptions.IgnoreCase).Count > 0 Then
            '检测 Sleep 函数
            result = Regex.Replace(src, "sleep\(([0-9]+)\)", "等待 $1 毫秒后继续往下执行", RegexOptions.IgnoreCase)
#End Region

#Region "单行注释"
        ElseIf src = "" Then
            '防止下面的检测首字符导致崩溃
            result = src

        ElseIf src.Substring(0, 1) = ";" Then
            '检测注释
            result = Regex.Replace(src, ";(.+)", "<注释：$1>")
#End Region

#Region "ControlClick 函数"
        ElseIf Regex.Matches(src, "controlclick\("， RegexOptions.IgnoreCase).Count > 0 Then
            Dim cc_regestr As String() = New String() _
                {
                "controlclick\(""(.+?)"",""(.+?)"",([0-9]+)\)",
                "controlclick\(""(.+?)"",""(.+?)"",([0-9]+),""(.+?)""\)",
                "controlclick\(""(.+?)"",""(.+?)"",([0-9]+),""(.+?)"",([0-9]+)\)"
                }
            Select Case Regex.Matches(src, ",").Count
                Case 2
                    If Regex.Matches(src, cc_regestr(0), RegexOptions.IgnoreCase).Count > 0 Then
                        result = Regex.Replace(src, cc_regestr(0), "在带有 $2 字符串的 $1 窗口内点击控件 ID 为 $3 的按钮", RegexOptions.IgnoreCase)
                    Else
                        result = "语法错误：" + src
                    End If
                Case 3
                    If Regex.Matches(src, cc_regestr(1), RegexOptions.IgnoreCase).Count > 0 Then
                        result = Regex.Replace(src, cc_regestr(1), "在带有 $2 字符串的 $1 窗口内用鼠标 $4 键点击控件 ID 为 $3 的按钮", RegexOptions.IgnoreCase)
                    Else
                        result = "语法错误：" + src
                    End If
                Case 4
                    If Regex.Matches(src, cc_regestr(2), RegexOptions.IgnoreCase).Count > 0 Then
                        result = Regex.Replace(src, cc_regestr(2), "在带有 $2 字符串的 $1 窗口内用鼠标 $4 键点击 $5 次控件 ID 为 $3 的按钮", RegexOptions.IgnoreCase)
                    Else
                        result = "语法错误" + src
                    End If
                Case Else
                    result = "参数错误：" + src
            End Select
            'result = Regex.Replace(src, "controlclick\(""(.+?)"",""(.+?)"",([0-9]+)\)", "在带有 $2 字符串的 $1 窗口内点击控件 ID 为 $3 的按钮", RegexOptions.IgnoreCase)
#End Region

#Region "Control 控件操作函数（三参数）"
        ElseIf Regex.Matches(src, "control(disable|enable|hide|show|focus)\(", RegexOptions.IgnoreCase).Count > 0 Then
            Dim c_regestr As String = "control(disable|enable|hide|show|focus)\(""(.+?)"",""(.+?)"",([0-9]+)\)"
            If Regex.Matches(src, c_regestr, RegexOptions.IgnoreCase).Count > 0 Then
                result = Regex.Replace(src, c_regestr, "$1 带有 $3 字符串的 $2 窗口内的 $4 控件", RegexOptions.IgnoreCase)
            Else
                errorflag = True
                result = "语法错误：" + src
            End If
            If errorflag = False Then
                result = result.Replace("disable", "禁用")
                result = result.Replace("enable", "启用")
                result = result.Replace("hide", "隐藏")
                result = result.Replace("show", "显示")
                result = result.Replace("focus", "定位到")
            End If
#End Region

#Region "MouseMove 函数"
        ElseIf Regex.Matches(src, "mousemove\(", RegexOptions.IgnoreCase).Count > 0 Then
            Dim mm_regestr As String() = New String() _
                {
                "mousemove\(([0-9]+),([0-9])+,([0-9]+)+\)",
                "mousemove\(([0-9]+),([0-9]+)\)"
                }
            '检测 MouseMove 函数
            If Regex.Matches(src, ",").Count > 1 Then
                If Regex.Matches(src, mm_regestr(0), RegexOptions.IgnoreCase).Count > 0 Then
                    result = Regex.Replace(src, mm_regestr(0), "将鼠标以 $3 的速度移动到 ($1,$2) 的位置（速度值越大越慢）", RegexOptions.IgnoreCase)
                Else
                    result = "语法错误：" + src
                End If
            Else
                If Regex.Matches(src, mm_regestr(1), RegexOptions.IgnoreCase).Count > 0 Then
                    result = Regex.Replace(src, mm_regestr(1), "将鼠标移动到 ($1,$2) 的位置", RegexOptions.IgnoreCase)
                Else
                    result = "语法错误：" + src
                End If
            End If
#End Region

#Region "MouseClick 函数"
        ElseIf Regex.Matches(src, "mouseclick\(", RegexOptions.IgnoreCase).Count > 0 Then
            Dim mc_regestr As String() = New String() _
                {
                "mouseclick\(""(left|right|middle|main|menu|primary|secondary)"",([0-9]+),([0-9]+)\)",
                "mouseclick\(""(left|right|middle|main|menu|primary|secondry)"",([0-9]+),([0-9]+),([0-9]+)\)",
                "mouseclick\(""(left|right|middle|main|menu|primary|secondry)"",([0-9]+),([0-9]+),([0-9]+),([0-9]+)\)",
                "mouseclick\(""(left|right|middle|main|menu|primary|secondry)""\)"
                }
            If Regex.Matches(src, ",").Count > 0 Then
                Select Case Regex.Matches(src, ",").Count
                    Case 2
                        If Regex.Matches(src, mc_regestr(0), RegexOptions.IgnoreCase).Count > 0 Then
                            result = Regex.Replace(src, mc_regestr(0), "在 ($2,$3) 上点击鼠标 $1 键", RegexOptions.IgnoreCase)
                        Else
                            errorflag = True
                            result = "语法错误：" + src
                        End If
                    Case 3
                        If Regex.Matches(src, mc_regestr(1), RegexOptions.IgnoreCase).Count > 0 Then
                            result = Regex.Replace(src, mc_regestr(1), "在 ($2,$3) 上点击 $4 次鼠标 $1 键", RegexOptions.IgnoreCase)
                        Else
                            errorflag = True
                            result = "语法错误：" + src
                        End If
                    Case 4
                        If Regex.Matches(src, mc_regestr(2), RegexOptions.IgnoreCase).Count > 0 Then
                            result = Regex.Replace(src, mc_regestr(2), "以 $5 的速度移动到 ($2,$3) 并点击 $4 次鼠标 $1 键（速度值越大越慢）", RegexOptions.IgnoreCase)
                        Else
                            errorflag = True
                            result = "语法错误：" + src
                        End If
                    Case Else
                        errorflag = True
                        result = "语法错误：" + src
                End Select
            Else
                If Regex.Matches(src, mc_regestr(3), RegexOptions.IgnoreCase).Count > 0 Then
                    result = Regex.Replace(src, mc_regestr(3), "点击鼠标 $1 键")
                Else
                    errorflag = True
                    result = "语法错误：" + src
                End If
            End If
            If errorflag = False Then
                result = result.Replace("left", "左")
                result = result.Replace("right", "右")
                result = result.Replace("middle", "中")
                result = result.Replace("main", "主要")
                result = result.Replace("menu", "次要")
                result = result.Replace("primary", "主要")
                result = result.Replace("secondary", "次要")
            End If
#End Region

#Region "Win 操作函数（二参数）"
        ElseIf Regex.Matches(src, "win(activate|close|kill)\(", RegexOptions.IgnoreCase).Count > 0 Then
            Dim w_other_regestr As String() = New String() _
                {
                "win(activate|close|kill)\(""(.+?)""\)",
                "win(activate|close|kill)\(""(.+?)"",""(.+?)""\)"
                }
            Select Case Regex.Matches(src, """,").Count
                Case 0
                    If Regex.Matches(src, w_other_regestr(0), RegexOptions.IgnoreCase).Count > 0 Then
                        result = Regex.Replace(src, w_other_regestr(0), "$1 $2 窗口")
                    Else
                        errorflag = True
                        result = "语法错误：" + src
                    End If
                Case 1
                    If Regex.Matches(src, w_other_regestr(1), RegexOptions.IgnoreCase).Count > 0 Then
                        result = Regex.Replace(src, w_other_regestr(1), "$1 带有 $3 字符串的 $2 窗口", RegexOptions.IgnoreCase)
                    Else
                        errorflag = True
                        result = "语法错误：" + src
                    End If
                Case Else
                    errorflag = True
                    result = "语法错误：" + src
            End Select
            If errorflag = False Then
                result = result.Replace("activate", "激活")
                result = result.Replace("close", "关闭")
                result = result.Replace("kill", "强制关闭")
            End If
#End Region

        Else
            '语法错误，或者不支持，则原样输出
            result = src
        End If
        result = result + vbLf
        Return result
    End Function

    ''' <summary>
    ''' 代码错误检测
    ''' </summary>
    ''' <param name="errsrc">错误源码</param>
    ''' <returns></returns>
    Public Shared Function errorcheck(ByVal errsrc As String)
        Dim errormsg As String = Nothing
        Return errormsg
    End Function
End Class
