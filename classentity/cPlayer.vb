Public Class cPlayer
	Inherits cCreature
	Protected Friend MPregen As Single = 0
	Protected Friend HPregen As Single = 0
	Private HPpointer As Single
	Private MPpointer As Single

	Protected Friend iInvincibleTime As Int16

	Protected Friend iStatus As Int16 = 0
    Protected Friend iLastDirection As Byte = 0
    Protected Friend iFaceTo As Byte = 2

    Protected Friend col_dmk As New Collection
    Protected Friend col_area As New Collection

    Protected Friend iBaseDamage As Int16 = 1
    Protected Friend iMaxShoot As Int16 = 1
    Protected Friend iCurShoot As Int16 = 0

    Protected Friend sKey As String
    Protected Friend sCharaName As String = "None"
    Protected Friend iClass As CharacterClass

	Public Sub New()
		MyBase.New()
        'SetBasicAnimation
        mAnim = New cAnim({13, 14, 15, 14}, 150)
	End Sub

	Protected Friend Sub SetMAXhp(hp As Int16)
		Me.HP = hp
		Me.HPmax = hp
		HPpointer = hp
	End Sub

	Protected Friend Sub SetMAXmp(mp As Int16)
		Me.MP = mp
		Me.MPmax = mp
		MPpointer = mp
	End Sub

	Protected Friend Sub HPMPregen()
		HP += HPregen
		If HP > HPmax Then HP = HPmax
		MP += MPregen
		If MP > MPmax Then MP = MPmax
	End Sub

	Enum CharacterClass
        none = 0
        short_range = 1
        long_range = 2
        priest = 3
        support = 4
        unknown = -1
    End Enum

    Protected Friend Sub InitCollisionSetting(Optional ByVal pRadius As Int16 = 4)
        iRadius = pRadius
        iWidthHalf = iRadius
        iHeightHalf = iRadius
    End Sub

    Protected Friend Sub InitCollisionSetting(Optional ByVal pWidth As Int16 = 11, Optional ByVal pHeight As Int16 = 4)
        iRadius = (pWidth + pHeight) / 2
        iWidthHalf = pWidth
        iHeightHalf = pHeight
    End Sub

    Protected Friend Sub Register()
        Me.sKey = "c" & iKey
        iKey += 1
        Col_Chara.Add(Me, Me.sKey)
    End Sub

    Protected Friend Function CanShoot() As Boolean
        If Me.col_dmk.Count < iMaxShoot Then
            Return True
        Else
            Return False
        End If
    End Function

    Protected Friend Overridable Sub Shoot()
        If mInput.IsKeyDownDX(Key_shoot, True) Then
            If CanShoot() Then
                Dim d As cDanmaku
                Dim r As Single
                Dim v As Single = 4
                Dim anim As New cAnim(80, 80, 0)
                d = New cDanmaku(5, True, False)
                r = fDirection2Angle(iFaceTo) + Rnd() * 0.314 - 0.157
                anim.Args.fRotate = r - 1.57
                d.InitR(Me.xPos, Me.yPos, v, r, 0.05)
                d.iLifeTime = 20
                d.iDamage = iBaseDamage
                d.SetTexAnim(Sys_cTex_Dmk8, anim)
                d.Register(Me.col_dmk)

                d = New cDanmaku(5, True, False)
                r = fDirection2Angle(iFaceTo) + Rnd() * 0.314 - 0.157
                anim.Args.fRotate = r - 1.57
                d.InitR(Me.xPos, Me.yPos, v, r, 0.05)
                d.iLifeTime = 20
                d.iDamage = iBaseDamage
                d.SetTexAnim(Sys_cTex_Dmk8, anim)
                d.Register(Me.col_dmk)

                d = New cDanmaku(5, True, False)
                r = fDirection2Angle(iFaceTo) + Rnd() * 0.314 - 0.157
                anim.Args.fRotate = r - 1.57
                d.InitR(Me.xPos, Me.yPos, v, r, 0.05)
                d.iLifeTime = 20
                d.iDamage = iBaseDamage
                d.SetTexAnim(Sys_cTex_Dmk8, anim)
                d.Register(Me.col_dmk)
            End If
        End If
    End Sub

    Protected Friend Overridable Sub Skill_1()
        If mInput.IsKeyDownDX(Key_skill1, False) Then

        End If
    End Sub

	Protected Friend Overrides Sub Dispose()
		MyBase.Dispose()
		Me.col_area.Clear()
		Me.col_dmk.Clear()
		Col_Chara.Remove(Me.sKey)
	End Sub

	Protected Friend Overrides Sub Update()
		MyBase.Update()
		HPMPregen()
	End Sub

	Protected Friend Overrides Sub Move()
		If mInput.IsKeyDownDX(Key_fixDirection, False) = False Then
			iDirection = 5
			If IsKeyDownDX(Key_left, False) Then
				iDirection -= 1
			End If
			If IsKeyDownDX(Key_right, False) Then
				iDirection += 1
			End If
			If IsKeyDownDX(Key_up, False) Then
				iDirection += 3
			End If
			If IsKeyDownDX(Key_down, False) Then
				iDirection -= 3
			End If
			If iDirection <> 5 Then iFaceTo = iDirection
			If iLastDirection <> iDirection Then
				If iDirection = 5 Then
					mAnim = New cAnim(mAnim.Args.idList(1), mAnim.Args.idList(1), 200)
					Select Case iLastDirection
						Case 1, 4, 7
							mAnim.Args.xMirror = True
					End Select
				Else
					Select Case iDirection
						Case 8
							mAnim = New cAnim({1, 2, 3, 2}, 150)
						Case 7, 9
							mAnim = New cAnim({4, 5, 6, 5}, 150)
						Case 4, 6
							mAnim = New cAnim({7, 8, 9, 8}, 150)
						Case 1, 3
							mAnim = New cAnim({10, 11, 12, 11}, 150)
						Case 2
							mAnim = New cAnim({13, 14, 15, 14}, 150)
					End Select
					Select Case iDirection
						Case 1, 4, 7
							mAnim.Args.xMirror = True
					End Select
				End If
				iLastDirection = iDirection
			End If
			MyBase.Move()
			If Not bIgnoreCollision_entity Then
				For Each ce As cEnemy In Col_Enemy
					ColideWith(ce, iDirection)
				Next
			End If
		Else
			Dim idrct As Byte = iDirection
			iDirection = 5
			If IsKeyDownDX(Key_left, False) Then
				iDirection -= 1
			End If
			If IsKeyDownDX(Key_right, False) Then
				iDirection += 1
			End If
			If IsKeyDownDX(Key_up, False) Then
				iDirection += 3
			End If
			If IsKeyDownDX(Key_down, False) Then
				iDirection -= 3
			End If
			MyBase.Move()
			If Not bIgnoreCollision_entity Then
				For Each ce As cEnemy In Col_Enemy
					ColideWith(ce, iDirection)
				Next
			End If
			iDirection = idrct
		End If

	End Sub

	Protected Friend Sub ProcessDmk()
		If Me.col_dmk.Count = 0 Then Exit Sub
		Dim d As cDanmaku
		For Each d In Me.col_dmk
			d.Move()
			d.Update()
		Next
		If Me.col_dmk.Count = 0 Then
			col_dmk.Clear()
			col_dmk = New Collection
		End If
	End Sub

	Protected Friend Sub ProcessArea()
		If Me.col_area.Count = 0 Then Exit Sub
		Dim a As cArea
		For Each a In Me.col_area
			a.Update()
		Next
		If Me.col_area.Count = 0 Then
			col_area.Clear()
			col_area = New Collection
		End If
	End Sub

	Protected Friend Sub DrawDmk()
		If Me.col_dmk.Count = 0 Then Exit Sub
		Dim d As cDanmaku
		For Each d In Me.col_dmk
			d.Draw()
		Next
	End Sub

	Protected Friend Sub DrawHPbar(x As Int16, y As Int16, width As Int16, height As Int16)
		HPpointer += (HP - HPpointer) / 30
		Dim pos As Single = x + width * HP / HPmax
		Dim pos2 As Single = x + width * HPpointer / HPmax
		DrawRectF(x, y, pos, y + height, colorHP)
		If pos2 > pos Then
			DrawRectF(pos, y, pos2, y + height, Color.FromArgb(128, colorHP))
		End If
	End Sub

	Protected Friend Sub DrawMPbar(x As Int16, y As Int16, width As Int16, height As Int16)
		MPpointer += (MP - MPpointer) / 30
		Dim pos As Single = x + width * MP / MPmax
		Dim pos2 As Single = x + width * MPpointer / MPmax
		DrawRectF(x, y, pos, y + height, colorMP)
		If pos2 > pos Then
			DrawRectF(pos, y, pos2, y + height, Color.FromArgb(128, colorMP))
		End If
	End Sub

	Protected Friend Sub DrawArea()
		If Me.col_area.Count = 0 Then Exit Sub
		Dim a As cArea
		For Each a In Me.col_area
			a.Draw()
		Next
	End Sub

	Protected Friend Overrides Sub DrawC()
		mAnim.SendArgs(mTex)
		mTex.DrawGraphR(xPos - chara_centeroffset.X, yPos - chara_centeroffset.Y)
	End Sub

	Protected Friend Shadows Function IsColideWith(ent As cEntity, thisMoveDirection As Byte) As Boolean
		Dim rtar As Rectangle = New Rectangle(ent.xPos - ent.iRadius, ent.yPos - ent.iRadius,
											 ent.iRadius * 2 + 1, ent.iRadius * 2 + 1)
		Dim rtag As Point, rtag2 As Point
		rtag = New Point(xPos, yPos)

		If thisMoveDirection < 4 Then
			rtag.Y += iRadius
		End If
		If thisMoveDirection > 6 Then
			rtag.Y -= iRadius
		End If
		Select Case thisMoveDirection
			Case 1, 4, 7
				rtag.X -= iRadius
			Case 3, 6, 9
				rtag.X += iRadius
		End Select

		If thisMoveDirection < 4 Then
			If rtar.Contains(rtag) Then Return True
			rtag2 = rtag
			rtag2.Offset(iRadius, 0)
			If rtar.Contains(rtag2) Then Return True
			rtag2 = rtag
			rtag2.Offset(-iRadius, 0)
			If rtar.Contains(rtag2) Then Return True
		End If
		If thisMoveDirection > 6 Then
			If rtar.Contains(rtag) Then Return True
			rtag2 = rtag
			rtag2.Offset(iRadius, 0)
			If rtar.Contains(rtag2) Then Return True
			rtag2 = rtag
			rtag2.Offset(-iRadius, 0)
			If rtar.Contains(rtag2) Then Return True
		End If
		Select Case thisMoveDirection
			Case 1, 4, 7
				If rtar.Contains(rtag) Then Return True
				rtag2 = rtag
				rtag2.Offset(0, iRadius)
				If rtar.Contains(rtag2) Then Return True
				rtag2 = rtag
				rtag2.Offset(0, -iRadius)
				If rtar.Contains(rtag2) Then Return True
			Case 3, 6, 9
				If rtar.Contains(rtag) Then Return True
				rtag2 = rtag
				rtag2.Offset(0, iRadius)
				If rtar.Contains(rtag2) Then Return True
				rtag2 = rtag
				rtag2.Offset(0, -iRadius)
				If rtar.Contains(rtag2) Then Return True
		End Select
		Return False
	End Function

	Protected Friend Sub ColideWith(ent As cEntity, thisMoveDirection As Byte)
		Dim rtar As Rectangle = New Rectangle(ent.xPos - ent.iRadius, ent.yPos - ent.iRadius,
											 ent.iRadius * 2 + 1, ent.iRadius * 2 + 1)
		Dim rtag As Point, rtag2 As Point
		Dim boc As Boolean = False
		rtag = New Point(xPos, yPos)

		If thisMoveDirection < 4 Then
			rtag.Y += iRadius
		End If
		If thisMoveDirection > 6 Then
			rtag.Y -= iRadius
		End If
		Select Case thisMoveDirection
			Case 1, 4, 7
				rtag.X -= iRadius
			Case 3, 6, 9
				rtag.X += iRadius
		End Select

        '直向
        Select Case thisMoveDirection
			Case 8
				If rtar.Contains(rtag) Then SetPos(xPos, yPos + iMoveSpeed)
				rtag2 = rtag
				rtag2.Offset(iRadius, 0)
				If rtar.Contains(rtag2) Then SetPos(xPos - iMoveSpeed, yPos)
				rtag2 = rtag
				rtag2.Offset(-iRadius, 0)
				If rtar.Contains(rtag2) Then SetPos(xPos + iMoveSpeed, yPos)
			Case 2
				If rtar.Contains(rtag) Then SetPos(xPos, yPos - iMoveSpeed)
				rtag2 = rtag
				rtag2.Offset(iRadius, 0)
				If rtar.Contains(rtag2) Then SetPos(xPos - iMoveSpeed, yPos)
				rtag2 = rtag
				rtag2.Offset(-iRadius, 0)
				If rtar.Contains(rtag2) Then SetPos(xPos + iMoveSpeed, yPos)
			Case 4
				If rtar.Contains(rtag) Then SetPos(xPos + iMoveSpeed, yPos)
				rtag2 = rtag
				rtag2.Offset(0, iRadius)
				If rtar.Contains(rtag2) Then SetPos(xPos, yPos - iMoveSpeed)
				rtag2 = rtag
				rtag2.Offset(0, -iRadius)
				If rtar.Contains(rtag2) Then SetPos(xPos, yPos + iMoveSpeed)
			Case 6
				If rtar.Contains(rtag) Then SetPos(xPos - iMoveSpeed, yPos)
				rtag2 = rtag
				rtag2.Offset(0, iRadius)
				If rtar.Contains(rtag2) Then SetPos(xPos, yPos - iMoveSpeed)
				rtag2 = rtag
				rtag2.Offset(0, -iRadius)
				If rtar.Contains(rtag2) Then SetPos(xPos, yPos + iMoveSpeed)
		End Select
        '斜向
        Select Case thisMoveDirection
			Case 7
				If rtar.Contains(rtag) Then SetPos(xPos + iMoveSpeed, yPos + iMoveSpeed)
				rtag2 = rtag
				rtag2.Offset(iRadius, -iRadius)
				If rtar.Contains(rtag2) Then SetPos(xPos, yPos + iMoveSpeed)
				rtag2 = rtag
				rtag2.Offset(-iRadius, iRadius)
				If rtar.Contains(rtag2) Then SetPos(xPos + iMoveSpeed, yPos)
			Case 3
				If rtar.Contains(rtag) Then SetPos(xPos - iMoveSpeed, yPos - iMoveSpeed)
				rtag2 = rtag
				rtag2.Offset(iRadius, -iRadius)
				If rtar.Contains(rtag2) Then SetPos(xPos - iMoveSpeed, yPos)
				rtag2 = rtag
				rtag2.Offset(-iRadius, iRadius)
				If rtar.Contains(rtag2) Then SetPos(xPos, yPos - iMoveSpeed)
			Case 9
				If rtar.Contains(rtag) Then SetPos(xPos - iMoveSpeed, yPos + iMoveSpeed)
				rtag2 = rtag
				rtag2.Offset(-iRadius, -iRadius)
				If rtar.Contains(rtag2) Then SetPos(xPos, yPos + iMoveSpeed)
				rtag2 = rtag
				rtag2.Offset(iRadius, iRadius)
				If rtar.Contains(rtag2) Then SetPos(xPos - iMoveSpeed, yPos)
			Case 1
				If rtar.Contains(rtag) Then SetPos(xPos + iMoveSpeed, yPos - iMoveSpeed)
				rtag2 = rtag
				rtag2.Offset(-iRadius, -iRadius)
				If rtar.Contains(rtag2) Then SetPos(xPos + iMoveSpeed, yPos)
				rtag2 = rtag
				rtag2.Offset(iRadius, iRadius)
				If rtar.Contains(rtag2) Then SetPos(xPos, yPos - iMoveSpeed)
		End Select
	End Sub

End Class
