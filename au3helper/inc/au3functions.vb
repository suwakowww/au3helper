Imports System.Collections.Generic
Imports au3helper

Public Class au3functions
    Public Property au3funccat As String
    Public Property au3funcs As String
    Public Property au3fcicon As String

End Class


Public Class au3func
    Public Shared Function get_funcs()
        '由于使用了FontAwesome，然而无法像 XAML 那样可以写字符的16进制值，就只能这样写了
        '复制下列图标即可
        '（U+F2D0），窗口
        '（U+F017），时钟
        '（U+F04B），播放
        '（U+F245），鼠标
        '（U+F11C），键盘
        '（U+F1DE），设置（三栏）
        '（U+F04C），暂停
        '（U+F120），运行
        Dim func_list As New List(Of au3functions)()
        func_list.Add(New au3functions With {.au3funccat = "Windows 窗口", .au3funcs = "WinWait", .au3fcicon = ""})
        func_list.Add(New au3functions With {.au3funccat = "Windows 窗口", .au3funcs = "WinWaitActive", .au3fcicon = ""})
        func_list.Add(New au3functions With {.au3funccat = "Windows 窗口", .au3funcs = "WinWaitClose", .au3fcicon = ""})
        func_list.Add(New au3functions With {.au3funccat = "Windows 窗口", .au3funcs = "WinWaitNotActive", .au3fcicon = ""})
        func_list.Add(New au3functions With {.au3funccat = "Windows 窗口", .au3funcs = "WinActive", .au3fcicon = ""})
        func_list.Add(New au3functions With {.au3funccat = "Windows 窗口", .au3funcs = "WinClose", .au3fcicon = ""})
        func_list.Add(New au3functions With {.au3funccat = "Windows 窗口", .au3funcs = "WinKill", .au3fcicon = ""})
        func_list.Add(New au3functions With {.au3funccat = "鼠标", .au3funcs = "MouseMove", .au3fcicon = ""})
        func_list.Add(New au3functions With {.au3funccat = "鼠标", .au3funcs = "MouseClick", .au3fcicon = ""})
        func_list.Add(New au3functions With {.au3funccat = "Windows 控件", .au3funcs = "ControlClick", .au3fcicon = ""})
        func_list.Add(New au3functions With {.au3funccat = "键盘", .au3funcs = "Send", .au3fcicon = ""})
        func_list.Add(New au3functions With {.au3funccat = "计时与延迟", .au3funcs = "Sleep", .au3fcicon = ""})
        func_list.Add(New au3functions With {.au3funccat = "进程", .au3funcs = "Run", .au3fcicon = ""})
        '18.04.18添加
        func_list.Add(New au3functions With {.au3funccat = "Windows 控件", .au3funcs = "ControlEnable", .au3fcicon = ""})
        func_list.Add(New au3functions With {.au3funccat = "Windows 控件", .au3funcs = "ControlDisable", .au3fcicon = ""})
        func_list.Add(New au3functions With {.au3funccat = "Windows 控件", .au3funcs = "ControlHide", .au3fcicon = ""})
        func_list.Add(New au3functions With {.au3funccat = "Windows 控件", .au3funcs = "ControlShow", .au3fcicon = ""})
        func_list.Add(New au3functions With {.au3funccat = "Windows 控件", .au3funcs = "ControlFocus", .au3fcicon = ""})
        func_list.Add(New au3functions With {.au3funccat = "进程", .au3funcs = "ProcessWait", .au3fcicon = ""})
        func_list.Add(New au3functions With {.au3funccat = "进程", .au3funcs = "ProcessWaitClose", .au3fcicon = ""})

        '排序
        func_list.Sort(AddressOf sortlist)
        Return func_list
    End Function

    Private Shared Function sortlist(x As au3functions, y As au3functions) As Integer
        '根据函数类型排序
        If x.au3funccat > y.au3funccat Then
            Return 1
        ElseIf x.au3funccat = y.au3funccat Then
            '如果函数类型相同，根据函数名排序
            If x.au3funcs > y.au3funcs Then
                Return 1
            ElseIf x.au3funcs = y.au3funcs Then
                Return 0
            Else
                Return -1
            End If
        Else
            Return -1
        End If
    End Function
End Class