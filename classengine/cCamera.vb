Public Class cCamera
    Public X As Integer, Y As Integer

    Public Sub FocusOn(entity As cEntity)
        X = entity.xPos
        Y = entity.yPos
        LimitScreen()
    End Sub

    Public Sub LimitScreen()
        X = limitMin(X, ResW / 2 + 32)
        X = limitMax(X, Curmap.mapWidth * 32 + 32 - ResW / 2)
        Y = limitMin(Y, ResH / 2 + 32)
        Y = limitMax(Y, Curmap.mapHeight * 32 + 32 - ResH / 2)
    End Sub

    Public Sub FocusOn(p As Point)
        X = p.X
        Y = p.Y
    End Sub

    Public Sub Reset()
        X = 0
        Y = 0
    End Sub

    Public Function copy() As cCamera
        Dim c As New cCamera
        c.X = X
        c.Y = Y
        Return c
    End Function

End Class
