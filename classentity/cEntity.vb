Public Class cEntity
    Protected Friend xPos As Single = 0
    Protected Friend yPos As Single = 0
    Protected Friend iRadius As Int16 = 5
    Protected Friend mTex As cTex

    Protected Friend bIgnoreCollision_entity As Boolean = False
    Protected Friend bIgnoreCollision_map As Boolean = False

    Public Sub New()

    End Sub

    Protected Friend Shared Function fDirection2Angle(ByVal direction As Byte) As Single
        Select Case direction
            Case 1
                Return 2.355
            Case 2
                Return 1.57
            Case 3
                Return 0.785
            Case 4
                Return 3.14
            Case 5
                Return 0
            Case 6
                Return 0
            Case 7
                Return 3.925
            Case 8
                Return 4.71
            Case 9
                Return 5.495
            Case Else
                Return 0
        End Select
    End Function

    Protected Friend Shared Function fAngle2Direction(ByVal angle As Single) As Byte
        '待检查
        Select Case Math.Atan(angle) + 0.785
            Case Is < 1.57
                Return 6
            Case Is < 1.57 + 0.785
                Return 9
            Case Is < 3.14
                Return 8
            Case Is < 3.14 + 0.785
                Return 7
            Case Is < 4.71
                Return 4
            Case Is < 4.71 + 0.785
                Return 1
            Case Is < 6.28
                Return 2
            Case Is < 6.28 + 0.785
                Return 3
            Case Else
                Return 5
        End Select
    End Function

    Protected Friend Overridable Sub SetPos(x As Single, y As Single)
        xPos = x
        yPos = y
    End Sub

    Protected Friend Overridable Sub Draw()
        mTex.DrawGraph(xPos - Cam.X, yPos - Cam.Y)
    End Sub

    Protected Friend Overridable Sub DrawC()
        mTex.DrawGraphC(xPos - Cam.X, yPos - Cam.Y)
    End Sub

    Protected Friend Overridable Function GetDistance(ent As cEntity) As Single
        Return Math.Sqrt((Me.xPos - ent.xPos) ^ 2 + (Me.yPos - ent.yPos) ^ 2)
    End Function

    Protected Friend Overridable Function GetTilePos() As Point
        Dim p As New Point(xPos \ 32, yPos \ 32)
        If p.X < 1 Then p.X = 1
        If p.Y < 1 Then p.Y = 1
        If p.X > Curmap.mapWidth Then p.X = Curmap.mapWidth
        If p.Y > Curmap.mapHeight Then p.Y = Curmap.mapHeight
        Return p
    End Function
    ''' <summary>
    ''' 返回所在地图位置
    ''' </summary>
    ''' <param name="ixpos"></param>
    ''' <param name="iypos"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Friend Shared Function GetTilePos(ixpos, iypos) As Point
        Dim p As New Point(ixpos \ 32, iypos \ 32)
        If p.X < 1 Then p.X = 1
        If p.Y < 1 Then p.Y = 1
        If p.X > Curmap.mapWidth Then p.X = Curmap.mapWidth
        If p.Y > Curmap.mapHeight Then p.Y = Curmap.mapHeight
        Return p
    End Function
    ''' <summary>
    ''' 检测实体与实体是否碰撞，有效判定为圆形
    ''' </summary>
    ''' <param name="ent">对象实体</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Friend Overridable Function IsCollideWith_round(ent As cEnemy) As Boolean
        Return (Me.GetDistance(ent) <= (Me.iRadius + ent.iRadius))
    End Function
    ''' <summary>
    ''' 检测实体与实体是否碰撞，有效判定为圆形
    ''' </summary>
    ''' <param name="ent">对象实体</param>
    ''' <param name="calculated_distance">已经计算过的距离，为了优化计算次数</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Friend Overridable Function IsCollideWith_round(ent As cEnemy, calculated_distance As Single) As Boolean
        Return (Me.GetDistance(ent) <= (Me.iRadius + ent.iRadius))
    End Function
    ''' <summary>
    ''' 检测实体与实体是否碰撞，有效判定视为正方形
    ''' </summary>
    ''' <param name="ent">对象实体</param>
    ''' <returns>是否碰撞</returns>
    ''' <remarks>算法已优化3次</remarks>
    Protected Friend Overridable Function IsCollideWith_rect(ent As cEntity) As Boolean
        '————优化x1
        'Dim rtar As cRectAdv = New cRectAdv(ent.xPos - ent.iRadius, ent.yPos - ent.iRadius, _
        '                                      ent.iRadius * 2, ent.iRadius * 2)
        'Dim rtag As PointF
        'rtag = New PointF(xPos + iRadius, yPos + iRadius)
        'If rtar.Contains(rtag) Then Return True
        'rtag = New PointF(xPos + iRadius, yPos - iRadius)
        'If rtar.Contains(rtag) Then Return True
        'rtag = New PointF(xPos - iRadius, yPos + iRadius)
        'If rtar.Contains(rtag) Then Return True
        'rtag = New PointF(xPos - iRadius, yPos - iRadius)
        'If rtar.Contains(rtag) Then Return True
        'Return False

        '————优化x2
        'If Math.Abs(Me.yPos - ent.yPos) < (Me.iRadius + ent.iRadius) Then
        '    If Math.Abs(Me.xPos - ent.xPos) < (Me.iRadius + ent.iRadius) Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        'Else
        '    Return False
        'End If

        '————优化x3
        Dim rad As Single = iRadius + ent.iRadius
        Dim r As New cRectAdv(xPos - rad, yPos - rad, rad * 2, rad * 2)
        Dim p As New PointF(ent.xPos, ent.yPos)
        Return r.Contains(p)
    End Function
    ''' <summary>
    ''' 检测实体与实体是否碰撞，有效判定视为矩形
    ''' </summary>
    ''' <param name="ent">对象实体（角色）</param>
    ''' <param name="charaWhalf">角色宽度一半</param>
    ''' <param name="charaHhalf">角色高度一半</param>
    ''' <returns>是否碰撞</returns>
    ''' <remarks>算法已优化1次</remarks>
    Protected Friend Overridable Function IsCollideWith_rect(ent As cEntity, charaWhalf As Int16, charaHhalf As Int16) As Boolean
        Dim rtar As cRectAdv = New cRectAdv(ent.xPos - ent.iRadius, ent.yPos - ent.iRadius, _
                                              ent.iRadius * 2, ent.iRadius * 2)
        Dim rtag As PointF
        rtag = New PointF(xPos + charaWhalf, yPos + charaHhalf)
        If rtar.Contains(rtag) Then Return True
        rtag = New PointF(xPos + charaWhalf, yPos - charaHhalf)
        If rtar.Contains(rtag) Then Return True
        rtag = New PointF(xPos - charaWhalf, yPos + charaHhalf)
        If rtar.Contains(rtag) Then Return True
        rtag = New PointF(xPos - charaWhalf, yPos - charaHhalf)
        If rtar.Contains(rtag) Then Return True
        Return False
    End Function

    Protected Friend Overridable Function IsInEntityRange_rect(ent As cEntity, RectHalfRange As Int16) As Boolean
        If xPos >= ent.xPos - RectHalfRange Then
            If xPos <= ent.xPos + RectHalfRange Then
                If yPos >= ent.yPos - RectHalfRange Then
                    If yPos <= ent.yPos + RectHalfRange Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Protected Friend Overridable Function IsInEntityRange_circle(ent As cEntity, EntRadius As Int16) As Boolean
        If GetDistance(ent) <= EntRadius Then Return True Else Return False
    End Function

    Protected Friend Sub DrawEntityArea()
        mGraph.DrawRectF(xPos - iRadius + ofX(), yPos - iRadius + ofY(), _
                         xPos + iRadius + ofX(), yPos + iRadius + ofY(), _
                         Color.FromArgb(128, Color.Yellow))
    End Sub

End Class