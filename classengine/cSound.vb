Imports Microsoft.DirectX.AudioVideoPlayback
Imports Microsoft.DirectX

Public Class cSound
    Protected ssound As Audio = Nothing
    Public StopPosition As Double
    ''' <summary>
    ''' 获取、设置音频当前位置
    ''' </summary>
    ''' <returns></returns>
    Public Property CurPos As Double
        Get
            Return ssound.CurrentPosition
        End Get
        Set(value As Double)
            ssound.CurrentPosition = value
        End Set

    End Property
    ''' <summary>
    ''' 获取、设定音量（待换算）
    ''' </summary>
    ''' <returns></returns>
    Public Property Volume As Int32
        Get
            Return ssound.Volume
        End Get
        Set(value As Int32)
            ssound.Volume = value
        End Set
    End Property

    ''' <summary>
    ''' 载入音频文件
    ''' </summary>
    ''' <param name="fn">音频文件地址，相对地址</param>
    Public Sub LoadFromPath(fn As String)
        ssound = New Audio(Application.StartupPath & "\" & fn, False)
        StopPosition = ssound.StopPosition / 10000000
    End Sub

    ''' <summary>
    ''' 啥都不做的实例化
    ''' </summary>
    Public Sub New()
    End Sub
    ''' <summary>
    ''' 顺便初始化的实例化
    ''' </summary>
    ''' <param name="fn"></param>
    Public Sub New(fn As String)
        Me.LoadFromPath(fn)
    End Sub
    ''' <summary>
    ''' 播放
    ''' </summary>
    Public Sub Play()
        ssound.Play()
    End Sub
    ''' <summary>
    ''' 强制重新播放
    ''' </summary>
    Public Sub ForcePlay()
        CurPos = 0
        ssound.Play()
    End Sub
    ''' <summary>
    ''' 就是停止播放，调用的时候就调用ClassInstance.Stop()就好
    ''' </summary>
    Public Sub [Stop]()
        ssound.Stop()
    End Sub
    ''' <summary>
    ''' 暂停
    ''' </summary>
    Public Sub Pause()
        ssound.Pause()
    End Sub
    ''' <summary>
    ''' 循环，因为没有多线程所以放在主循环里边调用，靴靴
    ''' </summary>
    Public Sub [Loop]()
        'If sBGM.State = StateFlags.Stopped Then sBGM.Play()
        'If sBGM.Stopped Then sBGM.Play()
        If IsStopped() Then ssound.Stop() : ssound.Play()
    End Sub
    ''' <summary>
    ''' 是否播放完毕
    ''' </summary>
    ''' <returns></returns>
    Public Function IsStopped() As Boolean
        Return (ssound.CurrentPosition = StopPosition)
    End Function
    ''' <summary>
    ''' ，停止播放，释放资源
    ''' </summary>
    Public Sub Dispose()
        ssound.Stop()
        ssound.Dispose()
    End Sub
End Class
