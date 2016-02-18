Public Class cEnemy
    Inherits cCreature

    Delegate Function DlgProc()

    Protected Friend sKey As String

    Protected Friend col_dmk As New Collection
    Protected Friend col_area As New Collection

    Protected Friend iFaceTo As Byte = 2
    Protected Friend iBaseDamage As Int16 = 1
    Protected Friend bActivated As Boolean = False
    Protected Friend iShootInterval As Int16
    Protected Friend iShootCountdown As Int16

    Protected Friend iNoticeRange As Int16 = 120
    Protected Friend eFocusOn As cEntity

    Protected Friend Overrides Sub DrawC()
        mAnim.SendArgs(mTex)
        mTex.DrawGraphR(xPos - mTex.cellW / 2, yPos - mTex.cellH + 5)

    End Sub

    Protected Friend Sub Active(ByRef eFocus As cEntity)
        eFocusOn = eFocus
        Dim a = 0
    End Sub

    Protected Friend Sub Deactive()
        eFocusOn = Nothing
    End Sub

    Protected Friend Sub CheckNotice()
        Dim dist As Single = iNoticeRange + 1
        Dim disttemp As Single
        For Each e As cPlayer In Col_Chara
            disttemp = e.GetDistance(Me)
            If disttemp <= iNoticeRange Then
                If disttemp < dist Then
                    dist = disttemp
                    Active(e)
                End If
            End If
        Next
    End Sub

    Protected Friend Shared Sub CollideBuffer_Clear()
        ReDim vEnemyCollideBuffer(Col_Enemy.Count)
        For i As Integer = 1 To Col_Enemy.Count
            vEnemyCollideBuffer(i) = New PointF(0, 0)
        Next
    End Sub

    Protected Friend Shared Sub CollideBuffer_Calc()
        If Col_Enemy.Count = 0 Then Exit Sub
        Dim e As cEnemy
        For i As Integer = 1 To Col_Enemy.Count
            e = Col_Enemy.Item(i)
            e.CollideWithAll_PreCalc(i)
        Next
    End Sub

    Protected Friend Shared Sub CollideBuffer_Visualize()
        If Col_Enemy.Count = 0 Then Exit Sub
        Dim e As cEnemy
        Dim i As Integer
        For i = 1 To Col_Enemy.Count
            e = Col_Enemy.Item(i)
            mGraph.Drawline(e.xPos + ofX(), e.yPos + ofY(), e.xPos + vEnemyCollideBuffer(i).X + ofX(), e.yPos + vEnemyCollideBuffer(i).Y + ofY(), Color.Blue)
        Next
        mGraph.DrawText(vEnemyCollideBuffer(1).ToString, 1, 1, Color.White)
    End Sub
    ''' <summary>
    ''' 单个与所有检测碰撞，并把碰撞结果存储在数组中
    ''' </summary>
    ''' <remarks>已优化</remarks>
    Protected Friend Sub CollideWithAll_PreCalc(MyIndex As Integer)
        Dim e As cEnemy
        Dim dist As Single
        For i As Integer = MyIndex + 1 To Col_Enemy.Count
            e = Col_Enemy.Item(i)
            If Me.IsCollideWith_rect(e) Then
                dist = GetDistance(e)
                With vEnemyCollideBuffer(i)
                    .X += (e.xPos - xPos) * e.iMoveSpeed / dist
                    .Y += (e.yPos - yPos) * e.iMoveSpeed / dist
                End With
                With vEnemyCollideBuffer(MyIndex)
                    .X -= (e.xPos - xPos) * iMoveSpeed / dist
                    .Y -= (e.yPos - yPos) * iMoveSpeed / dist
                End With
            End If
        Next
    End Sub
    ''' <summary>
    ''' 单个与所有检测碰撞，并把碰撞结果存储在数组中
    ''' </summary>
    ''' <remarks>未优化，保证各CPU处理量相同</remarks>
    Protected Friend Shared Sub CollideWithAll_PreCalc_single(MyIndex As Integer)
        Dim e As cEnemy, e2 As cEnemy
        Dim dist As Single
        For i As Integer = MyIndex + 1 To Col_Enemy.Count
            e = Col_Enemy.Item(i)
            e2 = Col_Enemy.Item(MyIndex)
            If e2.IsCollideWith_rect(e) Then
                dist = e2.GetDistance(e)
                With vEnemyCollideBuffer(i)
                    .X += (e.xPos - e2.xPos) * e.iMoveSpeed / dist
                    .Y += (e.yPos - e2.yPos) * e.iMoveSpeed / dist
                End With
                With vEnemyCollideBuffer(MyIndex)
                    .X -= (e.xPos - e2.xPos) * e2.iMoveSpeed / dist
                    .Y -= (e.yPos - e2.yPos) * e2.iMoveSpeed / dist
                End With
            End If
        Next
    End Sub
    ''' <summary>
    ''' 单个与所有检测碰撞，并把碰撞结果存储在数组中
    ''' </summary>
    ''' <remarks>复杂，需要考虑公用数组的引用，单帧碰撞引用次数≈敌人总数^2，待优化</remarks>
    Protected Friend Sub CollideWithAll_PreCalc(Myskey As String)
        Dim e As cEnemy
        Dim dist As Single
        For i As Integer = 1 To Col_Enemy.Count
            e = Col_Enemy.Item(i)
            If e.sKey <> Myskey Then
                If Me.IsCollideWith_rect(e) Then
                    dist = GetDistance(e)
                    With vEnemyCollideBuffer(i)
                        .X -= (e.xPos - xPos) * iMoveSpeed / dist * 3
                        .Y -= (e.yPos - yPos) * iMoveSpeed / dist * 3
                    End With
                End If
            End If
        Next
    End Sub
    ''' <summary>
    ''' 调用多线程分配用过程
    ''' </summary>
    ''' <remarks></remarks>
    Protected Friend Shared Sub CollideWithAll_PreCalc_alloc(CPUid As Int16)
        Dim st As Integer, ed As Integer, cores As Integer
        cores = System.Environment.ProcessorCount
        st = Int(Col_Enemy.Count * (CPUid - 1) / cores) + 1
        ed = Int(Col_Enemy.Count * CPUid / cores)
        For i As Integer = st To ed

        Next
    End Sub
    ''' <summary>
    ''' 释放缓冲区，将碰撞产生的偏移作用于实体坐标
    ''' </summary>
    ''' <param name="MyIndex"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CollideWithAll_ReleaseBuffer(MyIndex As Integer)
        With vEnemyCollideBuffer(MyIndex)
            SetPos(xPos + .X, yPos + .Y)
        End With
    End Sub
    ''' <summary>
    ''' 释放缓冲区，将碰撞产生的偏移作用于实体坐标
    ''' </summary>
    ''' <param name="MysKey"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CollideWithAll_ReleaseBuffer(MysKey As String)
        With vEnemyCollideBuffer(MysKey)
            SetPos(xPos + .X, yPos + .Y)
        End With
    End Sub
    ''' <summary>
    ''' 更新全部敌人状态 
    ''' </summary>
    ''' <remarks></remarks>
    Protected Friend Shared Sub EnemyCollection_Update()
        If Col_Enemy.Count = 0 Then Exit Sub
        Dim ce As cEnemy
        iEnemyArrayProc = 1
        iEnemyCollideBufferProc = 1
        iEnemyArrayCount = Col_Enemy.Count
        Do While iEnemyArrayProc <= iEnemyArrayCount
            If Col_Enemy.Count = 0 Then Exit Do
            ce = Col_Enemy.Item(iEnemyArrayProc)
            ce.Update() '敌人更新（掉血配色）
            ce.CollideWithAll_ReleaseBuffer(iEnemyCollideBufferProc) '释放碰撞
            ce.Process() '敌人消除&移动
            iEnemyArrayProc += 1
            iEnemyCollideBufferProc += 1
        Loop
        '敌人洞察玩家
        If Col_Enemy.Count = 0 Then Exit Sub
        Dim e As cEnemy
        For i As Int16 = 0 To 4
            If (iCheckNoticeProc + i) > Col_Enemy.Count Then
                iCheckNoticeProc = 1 - i
            End If
            e = Col_Enemy.Item(iCheckNoticeProc + i)
            e.CheckNotice()
        Next
        iCheckNoticeProc += 5
    End Sub

    Protected Friend Sub SetShootInterval(ByVal interval As Int16)
        iShootInterval = interval
        iShootCountdown = interval
    End Sub

    Protected Friend Sub SetTexAnim(tex As cTex, anim As cAnim)
        mTex = tex
        mAnim = anim
    End Sub

    Protected Friend Sub Register()
        Me.sKey = "e" & iKey
        iKey += 1
        Col_Enemy.Add(Me, Me.sKey)
    End Sub

	Protected Friend Overrides Sub Dispose()
		MyBase.Dispose()
		Me.col_area.Clear()
		Me.col_dmk.Clear()
	End Sub

	Protected Friend Overrides Sub OnDeath()
		MyBase.OnDeath()
		Col_Enemy.Remove(Me.sKey)
		Me.Dispose()
	End Sub

	Protected Friend Sub Process()
		If HP <= 0 Then
			Me.OnDeath()
			iEnemyArrayCount -= 1
            iEnemyArrayProc -= 1
        Else
            Me.Move()
        End If
    End Sub

    Protected Friend Overrides Sub Move()
        If Not (eFocusOn Is Nothing) Then
            Dim dist As Single = GetDistance(eFocusOn)
            If dist > iRadius Then
                SetPos(xPos + (eFocusOn.xPos - xPos) * iMoveSpeed / dist, yPos + (eFocusOn.yPos - yPos) * iMoveSpeed / dist)
            End If
        Else
            MyBase.Move()
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
            a.Judge()
        Next
        If Me.col_area.Count = 0 Then
            col_area.Clear()
            col_area = New Collection
        End If
    End Sub
End Class
