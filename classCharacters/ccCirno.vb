Public Class ccCirno
    Inherits cPlayer

    'Protected Friend Shared mTexEff As New cTex
    Protected Friend iDashTimeLast As Int16 = 0
    Private Const iDashTime = 50
    Private sklsound As New cSound

    Private Skl1dmkcount As Int16 = 0, Skl1frame As Int16 = 0, Skl1faceto As Single

    Protected Friend Sub InitAll()
        sCharaName = "Cirno"
        mTex = New cTex
        mTex.LoadGraph(d_image & "character\chara_07.png", 3, 5)
        sklsound.LoadA(d_se & "se_kira02.wav")
        sklsound.SetvolumeA(My.Resources.iSEVolume)
        'mTexEff.LoadGraph(d_image & "particle\eff16x2.png", 16, 2)
        Init(11, 4)
        iMoveSpeed = 2.3
        iClass = CharacterClass.support
    End Sub

    Protected Friend Overrides Sub Shoot()
        If mInput.IsKeyDownDX(Key_shoot, True) Then
            'Dim a As New cArea(9, cArea.JudgeMethod.rect, True, False)
            'a.iRadius = 16
            'a.SetPos(xPos + Math.Cos(fDirection2Angle(iFaceTo)) * 20, yPos + Math.Sin(fDirection2Angle(iFaceTo)) * 20)
            'a.mAnim = New cAnim(17, 20, 25)
            'a.mAnim.Args.fRotate = fDirection2Angle(iFaceTo) + 0.785
            'a.iLifeTime = 5
            'a.Register(Me.col_area)
            Dim b As New cDanmaku(5, True, False)
            b.InitR(xPos, yPos, 4, fDirection2Angle(iFaceTo), 0, 0, 3)
            b.mTex = Sys_cTex_Dmk8
            b.mAnim = New cAnim(72, 72, 9999)
            b.iLifeTime = 60
            b.Register(Me.col_dmk)
            b = New cDanmaku(5, True, False)
            b.InitR(xPos, yPos, 4, fDirection2Angle(iFaceTo) + 0.2, 0, 0, 3)
            b.mTex = Sys_cTex_Dmk8
            b.mAnim = New cAnim(72, 72, 9999)
            b.iLifeTime = 60
            b.Register(Me.col_dmk)
            b = New cDanmaku(5, True, False)
            b.InitR(xPos, yPos, 4, fDirection2Angle(iFaceTo) - 0.2, 0, 0, 3)
            b.mTex = Sys_cTex_Dmk8
            b.mAnim = New cAnim(72, 72, 9999)
            b.iLifeTime = 60
            b.Register(Me.col_dmk)
        End If
    End Sub

    Protected Friend Overrides Sub Skill_1()
        If mInput.IsKeyDownDX(Key_skill1, True) Then
            Skl1frame = 100
            Skl1faceto = fDirection2Angle(iFaceTo)
        End If
    End Sub

    Protected Friend Overrides Sub Update()
        MyBase.Update()
        If Skl1frame > 0 Then
            Dim b As cDanmaku
            For i As Int16 = 0 To 5
                sklsound.ssound.CurrentPosition = 0
                b = New cDanmaku(8, True, False)
                b.InitR(xPos, yPos, 4, Skl1faceto + (100 - Skl1frame) + 0.03 * i, 0, 0, 3)
                b.mTex = Sys_cTex_Dmk8
                b.mAnim = New cAnim(72, 72, 9999)
                b.iLifeTime = 60
                b.Register(Me.col_dmk)
                Skl1frame -= 1
                sklsound.PlayA()
            Next
        End If
    End Sub

End Class
