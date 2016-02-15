''' <summary>
''' 所有非生物实体的类
''' </summary>
''' <remarks></remarks>
Public Class cProjectile
    Inherits cEntity
    ''' <summary>
    ''' 是否按直角坐标运算
    ''' </summary>
    ''' <remarks></remarks>
    Protected Friend bProcVec
    ''' <summary>
    ''' 是否按极坐标运算
    ''' </summary>
    ''' <remarks></remarks>
    Protected Friend bProcRotVec

    Protected Friend Ax As Single = 0
    Protected Friend Ay As Single = 0
    Protected Friend Vx As Single = 0
    Protected Friend Vy As Single = 0

    Protected Friend Vscale As Single = 0 'V大小
    Protected Friend Ascale As Single = 0 'V加速度
    Protected Friend Vrot As Single = 0 'V方向
    Protected Friend Arot As Single = 0 '角加速度

    Protected Friend iLifeTime As Int16 = -1

    Protected Friend Sub Init(xPos As Single, yPos As Single, Optional ByVal iRad As Int16 = 3)
        With Me
            .bProcVec = True
            .bProcRotVec = False
            .xPos = xPos
            .yPos = yPos
            .iRadius = iRad
        End With
    End Sub

    Protected Friend Sub Init(xPos As Single, yPos As Single, Vx As Single, Vy As Single, Optional ByVal iRad As Int16 = 3)
        With Me
            .bProcVec = True
            .bProcRotVec = False
            .xPos = xPos
            .yPos = yPos
            .Vx = Vx
            .Vy = Vy
            .iRadius = iRad
        End With
    End Sub

    Protected Friend Sub Init(xPos As Single, yPos As Single, Vx As Single, Vy As Single, Ax As Single, Ay As Single, Optional ByVal iRad As Int16 = 3)
        With Me
            .bProcVec = True
            .bProcRotVec = False
            .xPos = xPos
            .yPos = yPos
            .Vx = Vx
            .Vy = Vy
            .Ax = Ax
            .Ay = Ay
            .iRadius = iRad
        End With
    End Sub

    Protected Friend Sub InitR(xPos As Single, yPos As Single, Vscale As Single, Vrot As Single, Optional ByVal Ascale As Single = 0, Optional Arot As Single = 0, Optional ByVal iRad As Int16 = 3)
        With Me
            .bProcVec = False
            .bProcRotVec = True
            .xPos = xPos
            .yPos = yPos
            .Vscale = Vscale
            .Ascale = Ascale
            .Vrot = Vrot
            .Arot = Arot
            .iRadius = iRad
        End With
    End Sub

    Protected Friend Sub ProcPosition()
        If bProcVec Then
            Me.SetPos(xPos + Vx, yPos + Vy)
            Vx += Ax
            Vy += Ay
        End If
        If bProcRotVec Then
            Me.SetPos(xPos + Math.Cos(Vrot) * Vscale, yPos + Math.Sin(Vrot) * Vscale)
            Vscale += Ascale
            Vrot += Arot
        End If
    End Sub

    Protected Friend Function GetCollision() As Boolean
        Dim pos As Point = Me.GetTilePos
        Return Curmap.GetTileCP(pos.X, pos.Y)
    End Function

    Protected Friend Shadows Function IsCollideWith(e As cEntity) As Boolean
        Dim d As Int16 = Me.iRadius + e.iRadius
        If Math.Abs(xPos - e.xPos) <= d Then '优化1
            If Math.Abs(yPos - e.yPos) <= d Then '优化2
                If d >= Me.GetDistance(e) Then Return True '精准判断
            End If
        End If
        Return False
    End Function
End Class
