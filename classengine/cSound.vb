Imports Microsoft.DirectX.AudioVideoPlayback
Imports Microsoft.DirectX

Public Class cSound
    Public ssound As Audio = Nothing
    Public StopPosition As Double

    Public Function CurPos() As Double
        Return ssound.CurrentPosition
    End Function

    Public Sub SetvolumeA(vol As Int32)
        'ssound.Volume = (vol - 100) * 40
        ssound.Volume = (vol - 100) * 100
    End Sub

    Public Sub LoadA(fn As String)
        ssound = New Audio(Application.StartupPath & "\" & fn, False)
        StopPosition = ssound.StopPosition / 10000000
    End Sub

    Public Sub PlayA()
        ssound.Play()
    End Sub

    Public Sub StopA()
        ssound.Stop()
    End Sub

    Public Sub PauseA()
        ssound.Pause()
    End Sub

    Public Sub LoopA()
        'If sBGM.State = StateFlags.Stopped Then sBGM.Play()
        'If sBGM.Stopped Then sBGM.Play()
        If IsStopped() Then ssound.Stop() : ssound.Play()
    End Sub

    Public Function IsStopped() As Boolean
        Return (ssound.CurrentPosition = StopPosition)
    End Function

    Public Sub Dispose()
        ssound.Stop()
        ssound.Dispose()
    End Sub
End Class
