Public Class FrmMain

    Public bFrmMainLoaded As Boolean = False
    Public Shared c2 As New cTex, ctex_tama As New cTex
	Public Shared p1 As cPlayer
	Public Shared e1 As cEnemy
	Public Shared map As New cMap()

	Public Shared Col_charaDmkA As New Collection
    Public Shared Col_enemyDmkA As New Collection

    Private Sub FrmMain_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        If Not bFrmMainLoaded Then
            Me.Focus()
            Me.SetClientSizeCore(ResW * My.Resources.windowScaleW, ResH * My.Resources.windowScaleH)
            Me.SetClientSizeCore(ResW * 2, ResH * 2)
            Me.Left = Screen.PrimaryScreen.Bounds.Width / 2 - Me.Width / 2
            Me.Top = Screen.PrimaryScreen.Bounds.Height / 2 - Me.Height / 2
            mGraph.InitDXBasic(Me, ResW, ResH)
            mInput.InitDXinput()
            mFile.LoadGameData()

            Col_charaDmk = Col_charaDmkA
            Col_enemyDmk = Col_enemyDmkA

            c2.LoadGraph(d_image & "character\chara_09.png", 3, 5)
			ctex_tama.LoadGraph(d_image & "character\nekotama.png", 4, 4)

			p1 = New ccCirno()
			p1.SetPos(42 * 32, 21 * 32)
			p1.Register()

			e1 = New cEnemy()
			e1.iRadius = 5
            e1.SetTexAnim(ctex_tama, New cAnim(5, 8, 250))
            e1.SetPos(46 * 32, 21 * 32)
            e1.iNoticeRange = 200
            e1.Register()


            map.Register()
			map.MapLoad(d_maps & "Shrine01.map")


			bFrmMainLoaded = True
            bStop = False

            If My.Resources.bJumpOP = False Then p_TitleScreen()

            p_MainGame()
        End If

    End Sub

    Private Sub FrmMain_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        FrmStart.ExitApplication()
    End Sub

    Private Sub FrmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        bStop = True
        UnloadDXengine()
    End Sub

    Private Sub FrmMain_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        'p1.SetPos(e.X, e.Y)
    End Sub

    Private Sub FrmMain_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        mInput.GetXYfromWindow(e.X, e.Y)
    End Sub

    Public Sub p_MainGame()
#Region "初始化+循环开始"
        Dim fi As Integer = 0
        Do
            My.Application.DoEvents()
#End Region

#Region "绘制+计算"
            '————————绘制部分————————
            mGraph.ClearDevice(Color.Purple)
            mGraph.BeginDevice()

            map.DrawMap()

            BeginGraph_Forced()

            DrawEnemy()
            p1.DrawC()
            p1.DrawArea()
			p1.DrawDmk()
			'DrawRect(0, 0, 10, 10, Color.FromArgb(0, 0, 0, 0))

			EndGraph_Forced()

            map.DrawMap_UpperLayer(p1)
			'cEnemy.CollideBuffer_Visualize()

			DrawTextPoped()

            If mInput.IsKeyDownDX(Microsoft.DirectX.DirectInput.Key.NumPad0, False) Then
                DrawRectF(0, ResH - 50, ResW, ResH, Color.FromArgb(196, Color.Black))
                DrawText(p1.GetTilePos.ToString, 2, ResH - 48, Color.White)
            End If

			If mInput.IsKeyDownDX(Microsoft.DirectX.DirectInput.Key.B, False) Then
				mISrenderer.InitISrenderer_test()
				mISrenderer.DrawBagSlots()
			End If

			p1.DrawHPbar(400 - 90, 300 - 40, 80, 10)
			p1.DrawMPbar(400 - 90, 300 - 20, 80, 10)

			DrawText("FPS: " & mGraph.fFPS, 10, ResH - 22, Color.White)
            DrawText("TotalFrames: " & fi.ToString(), 10, ResH - 42, Color.White)
            DrawText("Col_Enemy.Count: " & Col_Enemy.Count, 10, ResH - 62, Color.White)

            mGraph.EndDevice(False)
			'————————绘制部分结束————————
#End Region

#Region "按键检测"
			'按键检测
			If mInput.IsKeyDownDX(Microsoft.DirectX.DirectInput.Key.F5) Then
				Dim ce As cEnemy
				For i As Single = 0.5 To 6.28 Step 6.28 / 60
					ce = New cEnemy
					ce.iRadius = 7
					ce.HP = 20
					ce.SetTexAnim(ctex_tama, New cAnim(5, 8, 250))
					ce.SetPos(42 * 32 + Math.Cos(i) * 120, 23 * 32 + Math.Sin(i) * 120)
					ce.Register()
				Next
			End If
			If mInput.IsKeyDownDX(Microsoft.DirectX.DirectInput.Key.Escape) Then
				bStop = True
				UnloadDXengine()
				Application.Exit()
				Exit Sub
			End If
#End Region
			p1.Move()
			p1.ProcessDmk()
            p1.ProcessArea()
            p1.Shoot()
			p1.Skill_1()
			p1.Skill_2()
			p1.Update()
            cEnemy.CollideBuffer_Clear()
            cEnemy.CollideBuffer_Calc()
            cEnemy.EnemyCollection_Update()

            Cam.FocusOn(p1)

            RefreshKeyDX()


#Region "帧控制"
			'帧控制
			If False Then
                Do Until mInput.IsKeyDownDX(Microsoft.DirectX.DirectInput.Key.DownArrow, False) Or bStop
                    Application.DoEvents()
                Loop
            End If
            If False Then
                Dim ti As Long = timeGetTime()
                Do Until (timeGetTime - ti) > 999 Or bStop
                    Application.DoEvents()
                Loop
            End If
            fi += 1
#End Region

#Region "呈递"
            '呈递
            device.Present()
        Loop Until bStop
#End Region
    End Sub

    Private Sub FrmMain_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        If bFrmMainLoaded Then
            If Me.WindowState = FormWindowState.Maximized Then
                bStop = True
                UnloadDXengine()
            Else
                With mGraph.presentParams
                    'ResW = Me.ClientSize.Width
                    'ResH = Me.ClientSize.Height
                    'If (ResW Mod 2 = 1) Or (ResH Mod 2 = 1) Then
                    '    ResW += 1
                    '    ResH += 1
                    'End If
                    .BackBufferWidth = ResW
                    .BackBufferHeight = ResH
                End With
                device.Reset(mGraph.presentParams)
            End If
        End If
    End Sub
End Class
