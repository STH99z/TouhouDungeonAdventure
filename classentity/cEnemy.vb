Public Class cEnemy
	Inherits cCreature

	''' <summary>
	''' 轮番检测时只按这样检测，假设共10个
	''' 1st:1,4,7,10
	''' 2nd:2,5,8
	''' 3rd:3,6,9
	''' </summary>
	Protected Friend Shared iCheckNoticeOffset As Int16 = 0
	''' <summary>
	''' 分iCheckNoticeLoop份检测
	''' </summary>
	Protected Friend Shared iCheckNoticeLoop As Int16 = 3

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

	Protected Friend pAtTilePos As Point
	Protected Friend vCollisionOffset As PointF = New PointF(0, 0)

	Public Sub New()

	End Sub

	Protected Friend Overrides Sub DrawC()
		mAnim.SendArgs(mTex)
		mTex.DrawGraphR(xPos - mTex.cellW / 2, yPos - mTex.cellH + 5)

	End Sub

	''' <summary>
	''' 绘制全部敌人
	''' </summary>
	''' <remarks></remarks>
	Public Shared Sub DrawEnemy()
		For Each ce As cEnemy In Col_Enemy
			ce.DrawC()
		Next
	End Sub

	Protected Friend Sub Active(ByRef eFocus As cEntity)
        eFocusOn = eFocus
        'Dim a = 0
    End Sub

    Protected Friend Sub Deactive()
        eFocusOn = Nothing
    End Sub

    Protected Friend Sub CheckNotice()
        'Dim dist As Single = iNoticeRange + 1
        Dim disttemp As Single
        For Each e As cPlayer In Col_Chara
            disttemp = e.GetDistance(Me)
			If disttemp <= iNoticeRange Then
				'If disttemp < dist Then
				'    dist = disttemp
				'    Active(e)
				'End If
				Active(e)
			ElseIf disttemp >= iNoticeRange * 1.3 Then
				Deactive()
			End If
        Next
    End Sub
	''' <summary>
	''' 清空每个Enemy的碰撞偏移
	''' </summary>
	Protected Friend Shared Sub CollideBuffer_Clear()
		Dim e As cEnemy
		For Each e In Col_Enemy
			e.vCollisionOffset = New PointF()
		Next
	End Sub

	Protected Friend Shared Sub CollideBuffer_Calc()
		If Col_Enemy.Count = 0 Then Exit Sub
		For Each e As cEnemy In Col_Enemy
			e.CollideWithAll_PreCalc()
		Next
	End Sub

	Protected Friend Shared Sub CollideBuffer_Visualize()

		Dim e As cEnemy
		For Each e In Col_Enemy
			mGraph.Drawline(e.xPos + ofX(), e.yPos + ofY(),
							e.xPos + e.vCollisionOffset.X * 10 + ofX(),
							e.yPos + e.vCollisionOffset.Y * 10 + ofY(),
							Color.Gray)
		Next
	End Sub
	''' <summary>
	''' 单个与所有检测碰撞，并把碰撞结果存储在数组中
	''' </summary>
	''' <remarks>最优</remarks>
	Protected Friend Sub CollideWithAll_PreCalc()
		Dim ix As Int16, iy As Int16
		Dim xresult As Double, yresult As Double
		Dim dist As Single
		'debuging
		Dim TotalEnemyProcessed As Int16 = 0
		Dim IgnoredThroughRect As Int16 = 0
		Dim IgnoredThroughEquals As Int16 = 0

		'vCollisionOffset = New PointF(0, 0)
		For ix = -1 To 1
			For iy = -1 To 1
				TotalEnemyProcessed += htTileContains(pAtTilePos.X + ix, pAtTilePos.Y + iy).Values.Count
				For Each e As cEnemy In htTileContains(pAtTilePos.X + ix, pAtTilePos.Y + iy).Values
					If e.Equals(Me) Then
						IgnoredThroughEquals += 1
						Continue For
					End If
					If IsCollideWith_rect(e) Then
						dist = GetDistance(e)
						xresult = (e.xPos - xPos) * e.iMoveSpeed / dist
						yresult = (e.yPos - yPos) * e.iMoveSpeed / dist
						With vCollisionOffset
							.X -= xresult
							.Y -= yresult
						End With
						'With e.vCollisionOffset
						'	.X += xresult
						'	.Y += yresult
						'End With
					Else
						IgnoredThroughRect += 1
					End If
				Next
			Next
		Next
	End Sub
	'''' <summary>
	'''' 单个与所有检测碰撞，并把碰撞结果存储在数组中
	'''' </summary>
	'''' <remarks>已优化</remarks>
	'Protected Friend Sub CollideWithAll_PreCalc(MyIndex As Integer)
	'    Dim e As cEnemy
	'    Dim dist As Single
	'    For i As Integer = MyIndex + 1 To Col_Enemy.Count
	'        e = Col_Enemy.Item(i)
	'        If Me.IsCollideWith_rect(e) Then
	'            dist = GetDistance(e)
	'            With vEnemyCollideBuffer(i)
	'                .X += (e.xPos - xPos) * e.iMoveSpeed / dist
	'                .Y += (e.yPos - yPos) * e.iMoveSpeed / dist
	'            End With
	'            With vEnemyCollideBuffer(MyIndex)
	'                .X -= (e.xPos - xPos) * iMoveSpeed / dist
	'                .Y -= (e.yPos - yPos) * iMoveSpeed / dist
	'            End With
	'        End If
	'    Next
	'End Sub
	'''' <summary>
	'''' 单个与所有检测碰撞，并把碰撞结果存储在数组中
	'''' </summary>
	'''' <remarks>未优化，保证各CPU处理量相同</remarks>
	'Protected Friend Shared Sub CollideWithAll_PreCalc_single(MyIndex As Integer)
	'    Dim e As cEnemy, e2 As cEnemy
	'    Dim dist As Single
	'    For i As Integer = MyIndex + 1 To Col_Enemy.Count
	'        e = Col_Enemy.Item(i)
	'        e2 = Col_Enemy.Item(MyIndex)
	'        If e2.IsCollideWith_rect(e) Then
	'            dist = e2.GetDistance(e)
	'            With vEnemyCollideBuffer(i)
	'                .X += (e.xPos - e2.xPos) * e.iMoveSpeed / dist
	'                .Y += (e.yPos - e2.yPos) * e.iMoveSpeed / dist
	'            End With
	'            With vEnemyCollideBuffer(MyIndex)
	'                .X -= (e.xPos - e2.xPos) * e2.iMoveSpeed / dist
	'                .Y -= (e.yPos - e2.yPos) * e2.iMoveSpeed / dist
	'            End With
	'        End If
	'    Next
	'End Sub
	'''' <summary>
	'''' 单个与所有检测碰撞，并把碰撞结果存储在数组中
	'''' </summary>
	'''' <remarks>复杂，需要考虑公用数组的引用，单帧碰撞引用次数≈敌人总数^2，待优化</remarks>
	'Protected Friend Sub CollideWithAll_PreCalc(Myskey As String)
	'    Dim e As cEnemy
	'    Dim dist As Single
	'    For i As Integer = 1 To Col_Enemy.Count
	'        e = Col_Enemy.Item(i)
	'        If e.sKey <> Myskey Then
	'            If Me.IsCollideWith_rect(e) Then
	'                dist = GetDistance(e)
	'                With vEnemyCollideBuffer(i)
	'                    .X -= (e.xPos - xPos) * iMoveSpeed / dist * 3
	'                    .Y -= (e.yPos - yPos) * iMoveSpeed / dist * 3
	'                End With
	'            End If
	'        End If
	'    Next
	'End Sub
	'''' <summary>
	'''' 调用多线程分配用过程
	'''' </summary>
	'''' <remarks></remarks>
	'Protected Friend Shared Sub CollideWithAll_PreCalc_alloc(CPUid As Int16)
	'    Dim st As Integer, ed As Integer, cores As Integer
	'    cores = System.Environment.ProcessorCount
	'    st = Int(Col_Enemy.Count * (CPUid - 1) / cores) + 1
	'    ed = Int(Col_Enemy.Count * CPUid / cores)
	'    For i As Integer = st To ed

	'    Next
	'End Sub
	'''' <summary>
	'''' 释放缓冲区，将碰撞产生的偏移作用于实体坐标
	'''' </summary>
	'''' <param name="MyIndex"></param>
	'''' <remarks></remarks>
	'   Protected Friend Sub CollideWithAll_ReleaseBuffer(MyIndex As Integer)
	'	With vEnemyCollideBuffer(MyIndex)
	'		SetPos(xPos + .X, yPos + .Y)
	'	End With
	'End Sub
	''' <summary>
	''' 适用于最优方法的释放缓冲区
	''' </summary>
	''' <param name="EnemyObjRef">需要释放偏移的Enemy对象</param>
	Protected Friend Shared Sub CollideWithAll_ReleaseBuffer(EnemyObjRef As cEnemy)
		EnemyObjRef.CollideWithAll_ReleaseBuffer()
	End Sub
	''' <summary>
	''' 适用于最优方法的释放缓冲区，释放对象自己的偏移量
	''' </summary>
	Protected Friend Sub CollideWithAll_ReleaseBuffer()
		SetPos(xPos + vCollisionOffset.X, yPos + vCollisionOffset.Y)
	End Sub
    '''' <summary>
    '''' 释放缓冲区，将碰撞产生的偏移作用于实体坐标
    '''' </summary>
    '''' <param name="MysKey"></param>
    '''' <remarks></remarks>
    'Protected Friend Sub CollideWithAll_ReleaseBuffer(MysKey As String)
    '    With vEnemyCollideBuffer(MysKey)
    '        SetPos(xPos + .X, yPos + .Y)
    '    End With
    'End Sub
    ''' <summary>
    ''' 更新全部敌人状态 
    ''' </summary>
    ''' <remarks></remarks>
    Protected Friend Shared Sub EnemyCollection_Update()
		If Col_Enemy.Count = 0 Then Exit Sub

		'原版的检测碰撞
		'Dim ce As cEnemy
		'      iEnemyArrayProc = 1
		'      iEnemyCollideBufferProc = 1
		'iEnemyArrayCount = Col_Enemy.Count
		'Do While iEnemyArrayProc <= iEnemyArrayCount
		'	If Col_Enemy.Count = 0 Then Exit Do
		'	ce = Col_Enemy.Item(iEnemyArrayProc)
		'	ce.Update() '敌人更新（掉血配色）
		'          ce.CollideWithAll_ReleaseBuffer(iEnemyCollideBufferProc) '释放碰撞
		'          ce.Process() '敌人消除&移动
		'          iEnemyArrayProc += 1
		'	iEnemyCollideBufferProc += 1
		'Loop

		'新版的检测碰撞
		cEnemy.CollideBuffer_Clear()
		cEnemy.CollideBuffer_Calc()
		Dim ce As cEnemy
		For Each ce In Col_Enemy
			ce.Update() '敌人更新（掉血配色）
			ce.CollideWithAll_ReleaseBuffer() '释放碰撞
			ce.Process() '敌人消除&移动
		Next

		'敌人洞察玩家-旧版
		'妈的这里我要干嘛来着？
		''If Col_Enemy.Count = 0 Then Exit Sub
		''      Dim e As cEnemy
		''      For i As Int16 = 0 To 4
		''          If (iCheckNoticeProc + i) > Col_Enemy.Count Then
		''              iCheckNoticeProc = 1 - i
		''          End If
		''          e = Col_Enemy.Item(iCheckNoticeProc + i)
		''	e.CheckNotice()
		''Next
		''      iCheckNoticeProc += 5

		'敌人洞察玩家
		Dim e As cEnemy
		For i As Int32 = 1 + iCheckNoticeOffset To Col_Enemy.Count Step iCheckNoticeLoop
			e = Col_Enemy.Item(i)
			e.CheckNotice()
		Next
		iCheckNoticeOffset += 1
		If iCheckNoticeOffset = iCheckNoticeLoop Then
			iCheckNoticeOffset = 0
		End If
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
		'注册到小格点HashTable
		pAtTilePos = GetTilePos()
		htTileContains(pAtTilePos.X, pAtTilePos.Y).Add(sKey, Me)
	End Sub

	Protected Friend Overrides Sub Dispose()
		MyBase.Dispose()
		Me.col_area.Clear()
		Me.col_dmk.Clear()
	End Sub

	Protected Friend Overrides Sub OnDeath()
		MyBase.OnDeath()
		FrmMain.killCount += 1
		If FrmMain.p1.sCharaName = "Youmu" Then
			With FrmMain.p1
				.MP += 2.5
				If .MP > .MPmax Then
					.MP = MPmax
				End If
			End With
		End If
		htTileContains(pAtTilePos.X, pAtTilePos.Y).Remove(sKey)
		Try
			Col_Enemy.Remove(Me.sKey)
		Catch ex As Exception
			MsgBox(ex.Message + vbNewLine + ShowDetail())
		End Try
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
		'检测碰撞 扣Chara血
		If IsCollideWith_rect(FrmMain.p1) Then
			With FrmMain.p1
				.HP -= 4
			End With
			Me.OnDeath()
		End If
	End Sub

	Protected Friend Overrides Sub Move()
		'更新碰撞HashTable
		Dim pos As Point = GetTilePos()
		If pos <> pAtTilePos Then
			'需要更新
			htTileContains(pAtTilePos.X, pAtTilePos.Y).Remove(sKey)
			htTileContains(pos.X, pos.Y).Add(sKey, Me)
			pAtTilePos = pos
		End If

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
