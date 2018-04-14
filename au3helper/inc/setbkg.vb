Imports Windows.Storage
Imports Windows.Storage.Streams

Public Class setbkg
    Public Shared Async Sub set_bkg(ByVal sender As Page)
        Dim bkg As ImageBrush = New ImageBrush()
        Dim bkg2 As BitmapImage = New BitmapImage()
        Dim file As StorageFile = Await ApplicationData.Current.LocalFolder.GetFileAsync("bkg.jpg")
        Using ms As IRandomAccessStream = Await file.OpenAsync(FileAccessMode.Read)
            bkg2.SetSource(ms)
        End Using
        '(New Uri("ms-appdata:///local/bkg.jpg", UriKind.Absolute))
        bkg.ImageSource = bkg2
        bkg.Stretch = Stretch.UniformToFill
        bkg.Opacity = 0.5
        sender.Background = bkg
        'maingrid.Background = Nothing
        'main_topbar.Background.Opacity = 0.5
        'mainsplit.PaneBackground.Opacity = 0.8
        'If Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily <> "Windows.Mobile" Then
        '    del_background.Visibility = Visibility.Visible
        'End If
    End Sub
End Class
