1. Flyout

本来挺好的界面的，结果总是有些 bug，不得不用

界面（XAML）：
```xaml
......
<Page.Resources>
......
<Flyout x:Name="sample_fly">
......
</Flyout>
......
</Page.Resources>
......
<button x:Name="sample_fly_trigger" Content="显示的字符" Click="sample_fly_trigger_click"/>
......
```
代码（VB）：
```vbnet
......
Private Sub sample_fly_trigger_click(sender As Object, e As RoutedEventArgs)
......
sample_fly.Showat(sample_fly_trigger)
......
End Sub
......
```

2. ContentDialog

对话框式界面，挺好用的，有两种方法进行显现

* 纯代码显现

这种方法可以比较简单制作对话框，但比较难实现较为复杂的功能

代码（VB）：
```vbnet
......
Private Async Sub sample_button_Click(sender As Object, e As RoutedEventArgs)
Dim sample_contentdialog As New ContentDialog With
{
    .Title = "标题",
    .Content = "内容",
    .PrimaryButtonText = "主要按钮文本",
    .SecondaryButtonText = "次要按钮文本",
    '更多的设置项可以参考上述内容进行设置
}
......
'定义按钮事件，目前仅限于 PrimaryButtonClick 跟 SecondryButtonClick 可进行定义
AddHandler sample_contentdialog.PrimaryButtonClick, AddressOf primarybuttonclick
......
'显示 contentdialog
Await sample_contentdialog.ShowAsync()
......
End Sub
......
'对上述按钮，开新方法执行
Private Sub primarybuttonclick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
'需要的代码自行输入
......
End Sub
......
```

