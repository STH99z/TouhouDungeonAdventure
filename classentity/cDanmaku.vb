Public Class cDanmaku
    Inherits cProjectile

    Protected Friend bTargetEnemy As Boolean = True
    Protected Friend bTargetCharacter As Boolean = False
    Protected Friend bIgnoreCollision As Boolean = False
    Protected Friend iDamage As Int16 = 0
    Protected Friend mAnim As cAnim
    Protected Friend sKey As String
    Protected Friend cBelongTo As Collection

    Protected Friend Overrides Sub Draw()
        mAnim.Args.fRotate = Vrot + 1.57
        mAnim.SendArgs(mTex)
        mTex.DrawGraphC(xPos + ofX(), yPos + ofY())
    End Sub

    Protected Friend Sub New(ByVal key As String, ByVal dmg As Int16, ByVal btenemy As Boolean, ByVal btchara As Boolean)
        bTargetCharacter = btchara
        bTargetEnemy = btenemy
        iDamage = dmg
        sKey = key
    End Sub

    Protected Friend Sub New(ByVal dmg As Int16, Optional ByVal btenemy As Boolean = False, Optional ByVal btchara As Boolean = True)
        bTargetCharacter = btchara
        bTargetEnemy = btenemy
        iDamage = dmg
        sKey = "K" & iKey
        iKey += 1
        If iKey = 100000 Then iKey = 0
    End Sub

    ''' <summary>
    ''' 注册弹幕到弹幕池
    ''' </summary>
    ''' <param name="dmkCollection">弹幕池Collection，必须用Shared声明。</param>
    ''' <remarks></remarks>
    Protected Friend Sub Register(ByRef dmkCollection As Collection)
        dmkCollection.Add(Me, Me.sKey)
        cBelongTo = dmkCollection
    End Sub

    ''' <summary>
    ''' 自我消除
    ''' </summary>
    ''' <remarks></remarks>
    Protected Friend Sub Dispose()
        Me.mTex = Nothing
        cBelongTo.Remove(Me.sKey)
    End Sub

    Protected Friend Sub IgnoreCollision()
        bIgnoreCollision = True
    End Sub

    Protected Friend Sub SetTexAnim(ByRef tex As cTex, ByVal anim As cAnim)
        mTex = tex
        mAnim = anim
    End Sub

    Protected Friend Sub Move()
        ProcPosition()
        If Not bIgnoreCollision Then
            If GetCollision() Then
                iLifeTime = 0
            End If
        End If
    End Sub

    Protected Friend Sub Update()
        If iLifeTime = 0 Then
            Me.Dispose()
        ElseIf iLifeTime > 0 Then
            iLifeTime -= 1
            If bTargetCharacter Then
                For Each e As cCreature In Col_Chara
                    If Me.IsCollideWith(e) Then
                        e.Damaged(iDamage)
                        CreateDmgText(e.xPos - 10, e.yPos - 20, iDamage)
                        Me.iLifeTime = 0
                        Exit For
                    End If
                Next
            ElseIf bTargetEnemy Then
                For Each e As cCreature In Col_Enemy
                    If Me.IsCollideWith(e) Then
                        e.Damaged(iDamage)
                        CreateDmgText(e.xPos - 10, e.yPos - 20, iDamage)
                        Me.iLifeTime = 0
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    'Protected Friend Sub CheckHit()

    'End Sub
End Class
