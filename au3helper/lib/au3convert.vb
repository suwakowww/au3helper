Imports System.Text.RegularExpressions

Public Class au3convert
    Public Shared Function au3convert(ByVal src As String)
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
                        result = Regex.Replace(src, "(winwaitactive|winwaitclose|winwaitnotactive|winwait)\(""(.+?)"",""(.+?)""\)", "等待带有 $3 字符串的 $2 窗口 $1", RegexOptions.IgnoreCase)
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
            ElseIf Regex.Matches(src, "send\(""(.+?)""(,1)?\)", RegexOptions.IgnoreCase).Count > 0 Then
                result = Regex.Replace(src, "send\(""(.+?)""(,1)?\)", "发送按键 $1", RegexOptions.IgnoreCase)
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

#Region "Control 控件操作函数（三参数）"
        ElseIf Regex.Matches(src, "control(disable|enable|hide|show)\(").Count > 0 Then
            If Regex.Matches(src, "control(disable|enable|hide|show)\(""(.+?)"",""(.+?)"",([0-9]+)\)", RegexOptions.IgnoreCase).Count > 0 Then
                result = Regex.Replace(src, "control(disable|enable|hide|show)\(""(.+?)"",""(.+?)"",([0-9]+)\)", "$1 带有 $3 字符串的 $2 窗口内的 $4 控件", RegexOptions.IgnoreCase)
            Else
                errorflag = True
                result = "语法错误：" + src
            End If
            If errorflag = False Then
                result = result.Replace("disable", "禁用")
                result = result.Replace("enable", "启用")
                result = result.Replace("hide", "隐藏")
                result = result.Replace("show", "显示")
            End If
#End Region

#Region "MouseMove 函数"
        ElseIf Regex.Matches(src, "mousemove\(", RegexOptions.IgnoreCase).Count > 0 Then
            '检测 MouseMove 函数
            If Regex.Matches(src, ",").Count > 0 Then
                If Regex.Matches(src, "mousemove\(([0-9]+),([0-9])+,([0-9]+)+\)", RegexOptions.IgnoreCase).Count > 0 Then
                    result = Regex.Replace(src, "mousemove\(([0-9]+),([0-9]+)+,([0-9]+)+\)", "将鼠标以 $3 的速度移动到 ($1,$2) 的位置（速度值越大越慢）", RegexOptions.IgnoreCase)
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

#Region "Win 操作函数"
        ElseIf Regex.Matches(src, "win(activate|close|kill)\(", RegexOptions.IgnoreCase).Count > 0 Then
            Select Case Regex.Matches(src, """,").Count
                Case 0
                    If Regex.Matches(src, "win(activate|close|kill)\(""(.+?)""\)", RegexOptions.IgnoreCase).Count > 0 Then
                        result = Regex.Replace(src, "win(activate|close|kill)\(""(.+?)""\)", "$1 $2 窗口")
                    Else
                        errorflag = True
                        result = "语法错误：" + src
                    End If
                Case 1
                    If Regex.Matches(src, "win(activate|close|kill)\(""(.+?)"",""(.+?)""\)", RegexOptions.IgnoreCase).Count > 0 Then
                        result = Regex.Replace(src, "win(activate|close|kill)\(""(.+?)"",""(.+?)""\)", "$1 带有 $3 字符串的 $2 窗口", RegexOptions.IgnoreCase)
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
End Class
