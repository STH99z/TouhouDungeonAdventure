Public Class ccYoumu
    Inherits cPlayer

    Protected Friend Shared mTexEff As New cTex
    Protected Friend iDashTimeLast As Int16 = 0
    Private Const iDashTime = 50

    Private sklsound As New cSound
    Private Skl1dmkcount As Int16 = 0, Skl1frame As Int16 = 0, Skl1faceto As Single

    Protected Friend Sub InitAll()
        sCharaName = "Youmu"
        mTex = New cTex
        mTex.LoadGraph(d_image & "character\chara_09.png", 3, 5)
        mTexEff.LoadGraph(d_image & "particle\eff16x2.png", 16, 2)
        sklsound.LoadA("se\se_slash.wav")
        sklsound.SetvolumeA(My.Resources.iSEVolume)
        Init(11, 4)
        iMoveSpeed = 2.6
        iMoveSpeedBase = 2.6
        iClass = CharacterClass.short_range
    End Sub

    Protected Friend Overrides Sub Shoot()
        If mInput.IsKeyDownDX(Key_shoot, True) Then
            Dim a As New cArea(9, cArea.JudgeMethod.square, True, False)
            a.iRadius = 16
            a.SetPos(xPos + Math.Cos(fDirection2Angle(iFaceTo)) * 20, yPos + Math.Sin(fDirection2Angle(iFaceTo)) * 20)
            a.mTex = mTexEff
            a.mAnim = New cAnim(17, 20, 25)
            a.mAnim.Args.fRotate = fDirection2Angle(iFaceTo) + 0.785
            a.SetTrigger(5)
            a.Register(Me.col_area)
        End If
    End Sub

    Protected Friend Overrides Sub Skill_1()
        If iDirection = 5 Then Exit Sub
        If mInput.IsKeyDownDX(Key_skill1, True) Then
            sklsound.ssound.CurrentPosition = 0
            Skl1frame = 10
            Skl1faceto = fDirection2Angle(iFaceTo)
            bIgnoreCollision_entity = True
            iMoveSpeed = iMoveSpeedBase * 5
            sklsound.PlayA()
        End If
    End Sub

    Protected Friend Overrides Sub Update()
        MyBase.Update()
        If Skl1frame > 0 Then
            Skl1frame -= 1
            Dim a As New cArea(15, cArea.JudgeMethod.square, True, False)
            a.iRadius = 16
            a.SetPos(xPos, yPos)
            a.mTex = mTexEff
            a.mAnim = New cAnim(5, 6, 25)
            a.mAnim.Args.fRotate = fDirection2Angle(iFaceTo) - 0.785
            a.SetTrigger(5)
            a.Register(Me.col_area)
            If Skl1frame = 0 Then
                bIgnoreCollision_entity = False
                iMoveSpeed = iMoveSpeedBase
            End If
        End If
    End Sub
End Class
