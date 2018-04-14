Imports Windows.UI.Core
''' <summary>
''' 既定の Application クラスを補完するアプリケーション固有の動作を提供します。
''' </summary>
NotInheritable Class App
    Inherits Application

    ''' <summary>
    ''' アプリケーションがエンド ユーザーによって正常に起動されたときに呼び出されます。他のエントリ ポイントは、
    ''' アプリケーションが特定のファイルを開くために起動されたときに
    ''' 検索結果やその他の情報を表示するために使用されます。
    ''' </summary>
    ''' <param name="e">起動の要求とプロセスの詳細を表示します。</param>
    Protected Overrides Sub OnLaunched(e As Windows.ApplicationModel.Activation.LaunchActivatedEventArgs)
        Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)

        ' ウィンドウに既にコンテンツが表示されている場合は、アプリケーションの初期化を繰り返さずに、
        ' ウィンドウがアクティブであることだけを確認してください

        '返回键事件
        AddHandler Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested, AddressOf BackRequested


        If rootFrame Is Nothing Then
            ' ナビゲーション コンテキストとして動作するフレームを作成し、最初のページに移動します
            rootFrame = New Frame()

            AddHandler rootFrame.NavigationFailed, AddressOf OnNavigationFailed

            If e.PreviousExecutionState = ApplicationExecutionState.Terminated Then
                ' TODO: 以前中断したアプリケーションから状態を読み込みます
            End If
            ' フレームを現在のウィンドウに配置します
            Window.Current.Content = rootFrame
        End If

        If e.PrelaunchActivated = False Then
            If rootFrame.Content Is Nothing Then
                ' ナビゲーション スタックが復元されない場合は、最初のページに移動します。
                ' このとき、必要な情報をナビゲーション パラメーターとして渡して、新しいページを
                '構成します
                rootFrame.Navigate(GetType(MainPage), e.Arguments)
            End If

            ' 現在のウィンドウがアクティブであることを確認します
            Window.Current.Activate()

            '返回键的显示 / 隐藏
            If rootFrame.CanGoBack Then
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible
            Else
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed
            End If

            AddHandler rootFrame.Navigated, AddressOf OnNavigated
        End If
    End Sub

    '导航后返回键处理
    Private Sub OnNavigated(sender As Object, e As NavigationEventArgs)
        If TryCast(sender, Frame).CanGoBack = True Then
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible
        Else
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed
        End If
    End Sub

    '点击返回键处理
    Private Sub BackRequested(sender As Object, e As BackRequestedEventArgs)
        Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
        If rootFrame Is Nothing Then
            Return
        End If
        If rootFrame.CanGoBack AndAlso e.Handled = False Then
            e.Handled = True
            rootFrame.GoBack()
        End If
        Select Case TryCast(Window.Current.Content, Frame)
            Case Else

        End Select
    End Sub

    ''' <summary>
    ''' 特定のページへの移動が失敗したときに呼び出されます
    ''' </summary>
    ''' <param name="sender">移動に失敗したフレーム</param>
    ''' <param name="e">ナビゲーション エラーの詳細</param>
    Private Sub OnNavigationFailed(sender As Object, e As NavigationFailedEventArgs)
        Throw New Exception("Failed to load Page " + e.SourcePageType.FullName)
    End Sub

    ''' <summary>
    ''' アプリケーションの実行が中断されたときに呼び出されます。
    ''' アプリケーションが終了されるか、メモリの内容がそのままで再開されるかに
    ''' かかわらず、アプリケーションの状態が保存されます。
    ''' </summary>
    ''' <param name="sender">中断要求の送信元。</param>
    ''' <param name="e">中断要求の詳細。</param>
    Private Sub OnSuspending(sender As Object, e As SuspendingEventArgs) Handles Me.Suspending
        Dim deferral As SuspendingDeferral = e.SuspendingOperation.GetDeferral()
        ' TODO: アプリケーションの状態を保存してバックグラウンドの動作があれば停止します
        deferral.Complete()
    End Sub


End Class
