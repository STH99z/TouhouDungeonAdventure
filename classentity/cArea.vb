Public Class cArea
    Inherits cEntity
    Protected Friend bTargetEnemy As Boolean = True
    Protected Friend bTargetCharacter As Boolean = False
    Protected Friend iDamage As Int16 = 0
    Protected Friend mAnim As cAnim
    Protected Friend sKey As String
    Protected Friend cBelongTo As Collection

    Protected Friend iWidth As Int16, iHeight As Int16
    Protected Friend iLifeTime As Int16
    Protected Friend iJudgeFrequency As Int16
    Protected Friend iJudgeMethod As JudgeMethod = 0
    Protected Friend Rect As Rectangle
    Protected Friend AngStart As Single, AngEnd As Single
    'Protected Friend mTexRot As Single

    Enum JudgeMethod
        square = 0
        round = 1
        fan = 2
        rect = 4
    End Enum

    Protected Friend Overrides Sub Draw()
        'mAnim.Args.fRotate = mTexRot
        mAnim.SendArgs(mTex)
        mTex.DrawGraphC(xPos + ofX(), yPos + ofY())
        'DrawRectF(xPos - iRadius + ofX(), yPos - iRadius + ofY(), _
        '          xPos + iRadius + ofX(), yPos + iRadius + ofY(), _
        '          Color.FromArgb(32, Color.White))
    End Sub

    Protected Friend Sub InitArea()
        Select Case iJudgeMethod
            Case cArea.JudgeMethod.square
                Rect = New Rectangle(xPos - iRadius, yPos - iRadius, iRadius * 2, iRadius * 2)
            Case cArea.JudgeMethod.round

            Case cArea.JudgeMethod.fan

            Case cArea.JudgeMethod.rect
                Rect = New Rectangle(xPos - iHeight / 2, yPos - iWidth / 2, iWidth, iHeight)
        End Select
    End Sub

    Protected Friend Sub New(ByVal key As String, ByVal dmg As Int16, JudgeMethod As JudgeMethod, Optional ByVal btenemy As Boolean = False, Optional ByVal btchara As Boolean = True)
        bTargetCharacter = btchara
        bTargetEnemy = btenemy
        iDamage = dmg
        iJudgeMethod = JudgeMethod
        sKey = key
    End Sub

    Protected Friend Sub New(ByVal dmg As Int16, JudgeMethod As JudgeMethod, Optional ByVal btenemy As Boolean = False, Optional ByVal btchara As Boolean = True)
        bTargetCharacter = btchara
        bTargetEnemy = btenemy
        iDamage = dmg
        iJudgeMethod = JudgeMethod
        sKey = "A" & iKey
        iKey += 1
        If iKey = 100000 Then iKey = 0
    End Sub

    Protected Friend Sub SetTrigger(ByVal lifetime As Int16, Optional ByVal trigfrequency As Int16 = -1)
        iLifeTime = lifetime
        If trigfrequency = -1 Then
            iJudgeFrequency = lifetime
        Else
            iJudgeFrequency = trigfrequency
        End If
    End Sub

    Protected Friend Function IsTimeToJudge() As Boolean
        If (iLifeTime Mod iJudgeFrequency) = 0 Then Return True
        Return False
    End Function

    ''' <summary>
    ''' 注册弹幕到弹幕池
    ''' </summary>
    ''' <param name="areaCollection">判定域池Collection，必须用Shared声明。</param>
    ''' <remarks></remarks>
    Protected Friend Sub Register(ByRef areaCollection As Collection)
        InitArea()
        areaCollection.Add(Me, Me.sKey)
        cBelongTo = areaCollection
    End Sub

    ''' <summary>
    ''' 自我消除
    ''' </summary>
    ''' <remarks></remarks>
    Protected Friend Sub Dispose()
        Me.mTex = Nothing
        'Me.mAnim = Nothing
        cBelongTo.Remove(Me.sKey)
    End Sub

    Protected Friend Sub SetJudgeMethod(jm As JudgeMethod)
        iJudgeMethod = jm
    End Sub

    Protected Friend Sub SetTexAnim(ByRef tex As cTex, ByVal anim As cAnim)
        mTex = tex
        mAnim = anim
    End Sub

    Protected Friend Sub Judge()
        Select Case iJudgeMethod
            Case JudgeMethod.rect
                For Each ce As cEnemy In Col_Enemy
                    If Rect.Contains(ce.xPos, ce.yPos) Then
                        ce.OnHit(iDamage)
                        CreateDmgText(ce.xPos - 10, ce.yPos - 20, iDamage)
                    End If
                Next
            Case JudgeMethod.round

            Case JudgeMethod.fan

            Case JudgeMethod.square
                For Each ce As cEnemy In Col_Enemy
                    If Rect.Contains(ce.xPos, ce.yPos) Then
                        ce.OnHit(iDamage)
                        CreateDmgText(ce.xPos - 10, ce.yPos - 20, iDamage)
                    End If
                Next
        End Select
    End Sub

    Protected Friend Sub Update()
        If iLifeTime = 0 Then
            Me.Dispose()
        ElseIf iLifeTime > 0 Then
            If IsTimeToJudge() Then Me.Judge()
            iLifeTime -= 1
        End If
    End Sub
End Class
