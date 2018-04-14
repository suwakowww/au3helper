Imports Windows.Storage
Imports Windows.Storage.Pickers


' 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

''' <summary>
''' それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
''' </summary>
Public NotInheritable Class SetPage
    Inherits Page

    Private Async Sub set_bkg_Click(sender As Object, e As RoutedEventArgs)
        Dim localpath As StorageFolder = ApplicationData.Current.LocalFolder
        Dim fileopen As New FileOpenPicker()
        fileopen.FileTypeFilter.Add(".jpg")
        fileopen.FileTypeFilter.Add(".png")
        fileopen.FileTypeFilter.Add(".bmp")
        Dim sfile As StorageFile = Await fileopen.PickSingleFileAsync()
        If sfile IsNot Nothing Then
            Dim filetoken As String = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(sfile)
            Dim checkname As StorageFile
            checkname = Await sfile.CopyAsync(localpath, "bkg.jpg", NameCollisionOption.ReplaceExisting)
            set_bkg.Content = "替换背景"
            del_bkg.IsEnabled = True
            del_bkg.Content = "删除背景"
            'setbkg.set_bkg(TryCast(Me, Page))
            '测试代码
        End If
    End Sub

    Private Async Sub del_bkg_Click(sender As Object, e As RoutedEventArgs)
        Dim delfile As StorageFile = Await ApplicationData.Current.LocalFolder.GetFileAsync("bkg.jpg")
        Await delfile.DeleteAsync()
        del_bkg.IsEnabled = False
        del_bkg.Content = "当前无背景"
    End Sub

    Private Async Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        If Await ApplicationData.Current.LocalFolder.TryGetItemAsync("bkg.jpg") Is Nothing Then
            del_bkg.IsEnabled = False
            del_bkg.Content = "当前无背景"
        Else
            set_bkg.Content = "替换背景"
            del_bkg.IsEnabled = True
            del_bkg.Content = "删除背景"
        End If
        Dim theme_set As String = TryCast(Window.Current.Content, Frame).RequestedTheme
        If theme_set = "0" Then
            c_dark.IsChecked = True
        Else
            c_light.IsChecked = True
        End If
    End Sub

    Private Sub c_light_dark_Checked(sender As Object, e As RoutedEventArgs)
        Dim setvalue As String = TryCast(sender, RadioButton).Name
        If setvalue = "c_light" Then
            TryCast(Window.Current.Content, Frame).RequestedTheme = ApplicationTheme.Dark
        Else
            TryCast(Window.Current.Content, Frame).RequestedTheme = ApplicationTheme.Light
        End If
    End Sub
End Class
