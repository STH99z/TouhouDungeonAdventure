Public Class cCreature
    Inherits cEntity
	Protected Friend HP As Single = 1
	Protected Friend HPmax As Single = 1
	Protected Friend MP As Single = 0
	Protected Friend MPmax As Single = 0
	Protected Friend Lv As Int16
    Protected Friend iWidthHalf As Int16, iHeightHalf As Int16
    Protected Friend iMoveSpeedBase As Single = 1
    Protected Friend iMoveSpeed As Single = 1
    Protected Friend iDirection As Byte = 5
    Protected Friend xSpeed As Single = 0
    Protected Friend ySpeed As Single = 0
    Protected Friend iBuff() As Int16 = {}
    Protected Friend mAnim As cAnim
    Protected Friend bCollision As Boolean = True

    Protected iUnderAttack As Int16 = 0

    Public Sub New()
        MyBase.New()
    End Sub
    'Protected  Friend iCarry As citem

    Protected Friend Overridable Sub Move()
        '(@﹏@)~ 待优化
        '大概思路就是player.RECT.四角->检测碰撞
        If bIgnoreCollision_map Then
            If iDirection > 6 Then
                yPos -= iMoveSpeed
            End If
            If iDirection < 4 Then
                yPos -= iMoveSpeed
            End If
            Select Case iDirection
                Case 1, 4, 7
                    xPos -= iMoveSpeed
                Case 3, 6, 9
                    xPos += iMoveSpeed
            End Select
            Exit Sub
        End If

        Dim tpo As Point = Me.GetTilePos()
        Dim tpo4 As Point = cEntity.GetTilePos(xPos - iWidthHalf, yPos)
        Dim tpo6 As Point = cEntity.GetTilePos(xPos + iWidthHalf, yPos)
        Dim tpo8 As Point = cEntity.GetTilePos(xPos, yPos - iHeightHalf)
        Dim tpo2 As Point = cEntity.GetTilePos(xPos, yPos + iHeightHalf)
        If iDirection > 6 Then
            yPos -= iMoveSpeed
            If bCollision Then
                If Curmap.GetTileCC(tpo.X, tpo.Y - 1) Then
                    yPos = limitMin(yPos, tpo.Y * 32 + iHeightHalf)
                End If
                If xPos Mod 32 - iWidthHalf < 0 Then '已优化
                    If Curmap.GetTileCC(tpo4.X, tpo4.Y - 1) Then
                        yPos = limitMin(yPos, tpo.Y * 32 + iHeightHalf)
                    End If
                End If
                If xPos Mod 32 + iWidthHalf > 32 Then
                    If Curmap.GetTileCC(tpo6.X, tpo6.Y - 1) Then
                        yPos = limitMin(yPos, tpo.Y * 32 + iHeightHalf)
                    End If
                End If
            End If

        End If
        If iDirection < 4 Then
            yPos += iMoveSpeed
            If bCollision Then
                If Curmap.GetTileCC(tpo.X, tpo.Y + 1) Then
                    yPos = limitMax(yPos, tpo.Y * 32 + 32 - iHeightHalf)
                End If
                If xPos Mod 32 - iWidthHalf < 0 Then '已优化
                    If Curmap.GetTileCC(tpo4.X, tpo4.Y + 1) Then
                        yPos = limitMax(yPos, tpo.Y * 32 + 32 - iHeightHalf)
                    End If
                End If
                If xPos Mod 32 + iWidthHalf > 32 Then
                    If Curmap.GetTileCC(tpo6.X, tpo6.Y + 1) Then
                        yPos = limitMax(yPos, tpo.Y * 32 + 32 - iHeightHalf)
                    End If
                End If
            End If

        End If
        Select Case iDirection
            Case 1, 4, 7
                xPos -= iMoveSpeed
                If bCollision Then
                    If Curmap.GetTileCC(tpo.X - 1, tpo.Y) Then
                        xPos = limitMin(xPos, tpo.X * 32 + iWidthHalf)
                    End If
                    If yPos Mod 32 - iHeightHalf < 0 Then
                        If Curmap.GetTileCC(tpo8.X - 1, tpo8.Y) Then
                            xPos = limitMin(xPos, tpo.X * 32 + iWidthHalf)
                        End If
                    End If
                    If yPos Mod 32 + iHeightHalf > 32 Then
                        If Curmap.GetTileCC(tpo2.X - 1, tpo2.Y) Then
                            xPos = limitMin(xPos, tpo.X * 32 + iWidthHalf)
                        End If
                    End If
                End If


            Case 3, 6, 9
                xPos += iMoveSpeed
                If bCollision Then
                    If Curmap.GetTileCC(tpo.X + 1, tpo.Y) Then
                        xPos = limitMax(xPos, tpo.X * 32 + 32 - iWidthHalf)
                    End If
                    If yPos Mod 32 - iHeightHalf < 0 Then
                        If Curmap.GetTileCC(tpo8.X + 1, tpo8.Y) Then
                            xPos = limitMax(xPos, tpo.X * 32 + 32 - iWidthHalf)
                        End If
                    End If
                    If yPos Mod 32 + iHeightHalf > 32 Then
                        If Curmap.GetTileCC(tpo2.X + 1, tpo2.Y) Then
                            xPos = limitMax(xPos, tpo.X * 32 + 32 - iWidthHalf)
                        End If
                    End If
                End If


        End Select
    End Sub
    Protected Friend Overridable Sub Death()

    End Sub

    Protected Friend Overridable Sub Damaged(ByVal dmgvalue As Int16)
        HP -= dmgvalue
        iUnderAttack = 11
    End Sub

	Protected Friend Overridable Sub Update()
		If iUnderAttack > 0 Then
			iUnderAttack -= 1
			mAnim.Args.iB = 255 - 25.5 * iUnderAttack
			mAnim.Args.iG = 255 - 25.5 * iUnderAttack
		End If
	End Sub



End Class
