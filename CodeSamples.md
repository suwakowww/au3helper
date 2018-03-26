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

* 外部 XAML

这种方法可以实现较为复杂的功能（比如做个表单），但是方法较前者麻烦。

首先要新建一个 ContentDialog 的项，然后在里面输入需要的代码，但一些界面上的内容则直接在 XAML 文件上进行书写，代码大致如下：

```xaml
<ContentDialog
    x:Class="sampleapp.samplecdlg_cdlg"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:sampleapp"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    mc:Ignorable="d"
    Title="标题"
    PrimaryButtonText="主要按钮文本"
    SecondaryButtonText="次要按钮文本"
    PrimaryButtonClick="ContentDialog_PrimaryButtonClick"
    SecondaryButtonClick="ContentDialog_SecondaryButtonClick">
    <Grid>
    ......
    </Grid>
</ContentDialog>
```

除了 `<Grid>` 到 `</Grid>` 的之外，还可以对 `Title`、`PrimaryButtonText`、`SecondaryButtonText` 等等进行修改。

需要注意的是，ContentDialog 的底部的两个按钮在对应的 .vb 文件内进行书写：

```vbnet
......
Private Sub ContentDialog_PrimaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
'比如这里是 PrimaryButton 按钮的代码
End Sub
......
```

如果需要传值，则在上述 vb 文件内定义一个 `Public` 变量，放在 `Class` 之内、各个 `Sub` 或者 `Function` 之外即可被调用。

写完之后，需要在页面进行调用，则在页面对应的 vb 文件内书写：

```vbnet
Private Async Sub sample_button_Click(sender As Object, e As RoutedEventArgs)
'需要使用 Async 进行异步操作
    Dim sample_result As ContentDialogResult
    '这个为接收 ContentDialog 点击的按键，如果不需要进行传值，可删去
    Dim sample_cdlg As New sample_dlg
    '这个为实例化自定义的 ContentDialog
    sample_result = Await sample_cdlg.ShowAsync()
    '如果不需要接收点击按键，则不需要进行赋值
    '由于 ContentDialog 弹窗相当于异步操作，所以这里需要使用 Await，当然后面的 ShowAsync() 则代表异步方式进行显示
    ......
End Sub
```
