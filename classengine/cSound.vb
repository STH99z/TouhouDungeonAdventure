Imports Microsoft.DirectX.AudioVideoPlayback
Imports Microsoft.DirectX

Public Class cSound
    Enum eLoopMode
        normal = 0
        alreadyset = 1
    End Enum

    Protected ssound As Audio = Nothing
    Public StopPosition As Double
    Public LoopMode As eLoopMode
    Public LoopStart As Double, LoopEnd As Double

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
        LoopMode = eLoopMode.normal
    End Sub
    ''' <summary>
    ''' 顺便初始化的实例化
    ''' </summary>
    ''' <param name="fn"></param>
    Public Sub New(fn As String)
        Me.LoadFromPath(fn)
        LoopMode = eLoopMode.normal
    End Sub
    ''' <summary>
    ''' 初始化，加载，顺便设定循环部分
    ''' </summary>
    ''' <param name="fn"></param>
    ''' <param name="loopstart">循环开始时间</param>
    ''' <param name="loopend">循环结束时间</param>
    Public Sub New(ByVal fn As String, Optional ByVal loopstart As Double = 0, Optional ByVal loopend As Double = 0)
        LoadFromPath(fn)
        SetLoopTiming(loopstart, loopend)
    End Sub
    ''' <summary>
    ''' 设定循环部分
    ''' </summary>
    ''' <param name="loopstart">循环开始时间</param>
    ''' <param name="loopend">循环结束时间</param>
    Public Sub SetLoopTiming(loopstart As Double, ByVal loopend As Double)
        LoopMode = eLoopMode.alreadyset
        Me.LoopStart = loopstart
        If (loopend <> 0) Or (loopend > Me.StopPosition) Then
            Me.LoopEnd = loopend
        Else
            Me.LoopEnd = Me.StopPosition
        End If
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
        Select Case LoopMode
            Case eLoopMode.normal
                If IsStopped() Then
                    Me.Stop()
                    Play()
                End If
            Case eLoopMode.alreadyset
                If CurPos >= LoopEnd Then
                    CurPos = LoopStart
                    Play()
                End If
        End Select
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
