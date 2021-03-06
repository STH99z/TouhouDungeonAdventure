﻿Public Class ccCirno
    Inherits cPlayer

	'Protected Friend Shared mTexEff As New cTex
	Private sklsound1 As cSound
	Private sklsound2 As cSound
	Private shootsound As cSound

	Private Skl1dmkcount As Int16 = 0, Skl1frame As Int16 = 0, Skl1faceto As Single

	Protected Friend Sub New()
		MyBase.New()
		SetMAXhp(100)
		SetMAXmp(100)
		HPregen = 0.025
		MPregen = 0.025
		sCharaName = "Cirno"
		mTex = New cTex()
		mTex.LoadGraph(d_image & "character\chara_07.png", 3, 5)
		shootsound = cSound.GetSE("tan00")
		sklsound1 = cSound.GetSE("tan00")
		sklsound1.SetLoopTiming(0.01, 0.026)

		InitCollisionSetting(11, 4)
		iMoveSpeed = 2.3
		iClass = CharacterClass.support
	End Sub

	Protected Friend Overrides Sub Shoot()
		If mInput.IsKeyDownDX(Key_shoot, True) Then
			If MP >= 1 Then
				MP -= 1
				Dim b As cDanmaku
				Dim i As Integer
				For i = -1 To 1
					b = New cDanmaku(5, True, False)
					b.InitR(xPos, yPos, 4, fDirection2Angle(iFaceTo) + i * 0.2, 0, 0, 3)
					b.mTex = Sys_cTex_Dmk8
					b.mAnim = New cAnim(72, 72, 9999)
					b.iLifeTime = 60
					b.Register(Me.col_dmk)
				Next
				shootsound.ForcePlay()
			End If

		End If
	End Sub

	Protected Friend Overrides Sub Skill_1()
		If mInput.IsKeyDownDX(Key_skill1, True) Then
            '如果还在放skl1那么就不设定frame和方向
            If Skl1frame > 0 Then
				Exit Sub
			End If
			'检测MP量
			Const MPcost As Int16 = 15
			If MP >= MPcost Then
				MP -= MPcost
				Skl1frame = 100
				Skl1faceto = fDirection2Angle(iFaceTo)
				sklsound1.ForcePlay()
			End If
		End If
	End Sub

	Protected Friend Overrides Sub Skill_2()
		If mInput.IsKeyDownDX(Key_skill2, True) Then
			MP = MPmax
		End If
	End Sub

	Protected Friend Overrides Sub Update()
		MyBase.Update()
		If Skl1frame > 0 Then
			Dim b As cDanmaku
			For i As Int16 = 0 To 5
				b = New cDanmaku(8, True, False)
				b.InitR(xPos, yPos, 4, Skl1faceto + (100 - Skl1frame) + 0.03 * i, 0, 0, 3)
				b.mTex = Sys_cTex_Dmk8
				b.mAnim = New cAnim(72, 72, 9999)
				b.iLifeTime = 60
				b.Register(Me.col_dmk)
				Skl1frame -= 1
				sklsound1.Loop()
			Next
        End If
    End Sub

End Class
