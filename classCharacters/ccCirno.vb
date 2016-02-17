Public Class ccCirno
    Inherits cPlayer

    'Protected Friend Shared mTexEff As New cTex
    Protected Friend iDashTimeLast As Int16 = 0
    Private Const iDashTime = 50
    Private sklsound As cSound
    Private shootsound As cSound

    Private Skl1dmkcount As Int16 = 0, Skl1frame As Int16 = 0, Skl1faceto As Single

    Protected Friend Sub InitAll()
        sCharaName = "Cirno"
        mTex = New cTex
        mTex.LoadGraph(d_image & "character\chara_07.png", 3, 5)
        shootsound = New cSound(d_se & "se_tan00.wav")
        sklsound = New cSound(d_se & "se_tan00.wav", 0.01, 0.026)

        'sklsound.SetvolumeA(My.Resources.iSEVolume)
        'mTexEff.LoadGraph(d_image & "particle\eff16x2.png", 16, 2)
        Init(11, 4)
        iMoveSpeed = 2.3
        iClass = CharacterClass.support
    End Sub

    Protected Friend Overrides Sub Shoot()
        If mInput.IsKeyDownDX(Key_shoot, True) Then
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
    End Sub

    Protected Friend Overrides Sub Skill_1()
        If mInput.IsKeyDownDX(Key_skill1, True) Then
            '如果还在放skl1那么就不设定frame和方向
            If Skl1frame > 0 Then
                Exit Sub
            End If
            Skl1frame = 100
            Skl1faceto = fDirection2Angle(iFaceTo)
            sklsound.ForcePlay()
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
                sklsound.Loop()
            Next
        End If
    End Sub

End Class
