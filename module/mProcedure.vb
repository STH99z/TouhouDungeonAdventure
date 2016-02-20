Module mProcedure
    ''' <summary>
    ''' 相对摄像机位置修正X
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ofX() As Integer
        Return -Cam.X + ResW / 2
    End Function
    ''' <summary>
    ''' 相对摄像机位置修正Y
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ofY() As Integer
        Return -Cam.Y + ResH / 2
    End Function
    ''' <summary>
    ''' 限制最小值
    ''' </summary>
    ''' <param name="var"></param>
    ''' <param name="min"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function limitMin(ByVal var As Integer, ByVal min As Integer) As Integer
        If var < min Then
            Return min
        Else
            Return var
        End If
    End Function
    ''' <summary>
    ''' 限制最大值
    ''' </summary>
    ''' <param name="var"></param>
    ''' <param name="max"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function limitMax(ByVal var As Integer, ByVal max As Integer) As Integer
        If var > max Then
            Return max
        Else
            Return var
        End If
    End Function
    ''' <summary>
    ''' 检测值是否存在于值域
    ''' </summary>
    ''' <param name="var"></param>
    ''' <param name="min"></param>
    ''' <param name="max"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BelongTo(var, min, max) As Boolean
        If var < min Then Return False
        If var > max Then Return False
        Return True
    End Function
    ''' <summary>
    ''' 检查统一标示号上限
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub iKeyCheck()
        If iKey = 100000 Then iKey = 0
    End Sub

    Public Sub CreateDmgText(atX As Int16, atY As Int16, Dmg As Int16) '创建弹出文本
        Dim dt As New cTextPop(atX, atY, cTextPop.TextPopType.value, Dmg)
    End Sub
    ''' <summary>
    ''' 绘制全部弹出文本
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub DrawTextPoped()
		For Each tp As cTextPop In Col_TextPop
			tp.Pop()
		Next
	End Sub
    ''' <summary>
    ''' 算法测试：矩形检测包含点
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Algorithm_SpeedTest_RectContainsP()
        Randomize()
        Dim p As Point
        Dim rect As New Rectangle(25, 25, 50, 50)
        Dim b As Boolean
        MsgBox(".NET2.0 default algorithm")
        Dim ti As Long = timeGetTime()
        For i As Int64 = 1 To 10000000
            p = New Point(Rnd() * 100, Rnd() * 100)
            b = rect.Contains(p) '.NET2.0 default algorithm
        Next
        MsgBox("Time:" & (timeGetTime - ti).ToString)

        MsgBox("My algorithm")
        ti = timeGetTime()
        For i As Int64 = 1 To 10000000
            p = New Point(Rnd() * 100, Rnd() * 100)
            'our algorithm
            b = False
            If p.X < 25 + 50 Then
                If p.X >= 25 Then
                    If p.Y < 25 + 50 Then
                        If p.Y >= 25 Then
                            b = True
                        End If
                    End If
                End If
            End If
        Next
        MsgBox("Time:" & (timeGetTime - ti).ToString)
    End Sub
    ''' <summary>
    ''' 调取动画OP
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub p_TitleScreen()
        Dim tex_mt As New cTex
        Dim tex_sk0 As New cTex
        Dim tex_sk1 As New cTex
        Dim tex_shrine(1) As cTex
        Dim tex_reimu As New cTex
        Dim tex_marisa As New cTex
        Dim tex_jinja As New cTex
        Dim tex_korindo As New cTex
        Dim tex_sc4 As New cTex
        Dim tex_ykl(2) As cTex
        Dim tex_petal As New cTex, tex_petalbg As New cTex
        Dim tex_walk As New cTex, anim_walk As New cAnim({1, 2, 3, 2}, 415)

        Dim tex_press As New cTex
        Dim ptc As cParticle, ptccol As New Collection
        Dim sun As New cParticle
        Dim chara(1) As cParticle
        Dim bgm As New cSound
        Dim pos As Double
        Dim frmCount As Int16 = 0
        Dim barx As Int16 = 400
        tex_shrine(0) = New cTex
        tex_shrine(1) = New cTex
        tex_ykl(0) = New cTex
        tex_ykl(1) = New cTex
        tex_ykl(2) = New cTex
        tex_walk.LoadGraph(d_image & "scene\walk.png", 3)
        tex_petal.LoadGraph(d_image & "scene\petal.png")
        tex_petalbg.LoadGraph(d_image & "scene\petalbg.png")
        tex_mt.LoadGraph(d_image & "scene\sc0mt.png")
        tex_sk0.LoadGraph(d_image & "scene\sc0sk0.png")
        tex_sk1.LoadGraph(d_image & "scene\sc0sk1.png")
        tex_shrine(0).LoadGraph(d_image & "scene\sc1.png")
        tex_shrine(1).LoadGraph(d_image & "scene\sc1_02.png")
        tex_reimu.LoadGraph(d_image & "scene\reimu.png")
        tex_marisa.LoadGraph(d_image & "scene\marisa.png")
        tex_jinja.LoadGraph(d_image & "scene\sc02.png")
        tex_korindo.LoadGraph(d_image & "scene\sc03.png")
        tex_sc4.LoadGraph(d_image & "scene\sc04.png")
        tex_ykl(0).LoadGraph(d_image & "scene\y1.png")
        tex_ykl(1).LoadGraph(d_image & "scene\y2.png")
        tex_ykl(2).LoadGraph(d_image & "scene\y3.png")

        tex_press.LoadGraph(d_image & "scene\press.png")

        sun.mTex = New cTex
        sun.mTex.LoadGraph(d_image & "scene\sun.png")
        sun.InitR(170, 250, 150 / 3.733 / 60, -1.05)
        chara(0) = New cParticle
        chara(1) = New cParticle
        chara(0).mTex = tex_reimu
        chara(1).mTex = tex_marisa
        chara(0).InitR(440, 0, 15, 3.14, -0.25, 0)
        chara(1).InitR(-250, 0, 15, 0, -0.25, 0)
		bgm.LoadFromPath(d_bgm & "sc0.mp3")
		bgm.Volume = 100
        Cam.Reset()
        'Do Until mInput.IsKeyDownDX(Microsoft.DirectX.DirectInput.Key.S, True)
        '    My.Application.DoEvents()
        'Loop
        bgm.Play()
        'bgm.ssound.CurrentPosition = 10.6


        Do
            My.Application.DoEvents()

            If mInput.IsKeyDownDX(Microsoft.DirectX.DirectInput.Key.Escape, False) Then bStop = True

            pos = bgm.CurPos + 0.1
            If pos < 3.733 Then sun.ProcPosition()
            If pos < 0.733 Then tex_sk1.SetAlpha(255 * (pos / 0.733) ^ 2)
            If BelongTo(pos, 3.733, 4) Then tex_shrine(0).SetAlpha(255 * (pos - 3.733) / 0.267)

            mGraph.BeginDevice()
            mGraph.ClearDevice(Color.Black)

            If pos < 3.733 + 0.3 Then 'Mountain
                tex_sk0.DrawGraph(0, 0)
                tex_sk1.DrawGraph(0, 0)
                sun.DrawC()
                tex_mt.DrawGraph(0, pos * 5)

                If pos < 0.733 Then
                    mGraph.DrawRectF(0, 0, 400, 300, Color.FromArgb(128 - pos / 0.733 * 128, Color.Black))
                ElseIf pos < 3.733 Then
                    mGraph.DrawRectF(0, 0, 400, 300, Color.FromArgb(216 - (pos - 0.733) / 3 * 216, Color.White))
                End If
            End If
            If BelongTo(pos, 3.733, 7) Then 'Shrine
                tex_shrine(0).DrawGraph(0, -15 * (pos - 3.733) / 3.267)
                If pos > 4.17 Then
                    'tex_shrine(1).SetAlpha(255 - (pos - 4.17) / 2.83 * 128)
                    tex_shrine(1).DrawGraph(0, -15 * (pos - 3.733) / 3.267)
                    If pos < 6 Then
                        mGraph.DrawRectF(0, 0, 400, 300, Color.FromArgb(128 - (pos - 4.17) / 1.83 * 128, Color.White))
                    End If
                End If
            End If
            If BelongTo(pos, 7, 8.7) Then 'Reimu
                tex_jinja.DrawGraph(-20 + (pos - 7) / 1.7 * 20, 0)
                chara(0).Draw()
                If chara(0).Vscale >= 0 Then chara(0).ProcPosition()
            End If
            If BelongTo(pos, 8.7, 10.6) Then 'Marisa
                tex_korindo.DrawGraph(20 - (pos - 7) / 1.9 * 20, 0)
                chara(1).Draw()
                If chara(1).Vscale >= 0 Then chara(1).ProcPosition()
            End If
            If BelongTo(pos, 10.6, 15.3) Then 'Yukari
                tex_sc4.DrawGraph(0, -60 + 60 * (pos - 10.6) / 4.7)
                If pos < 11.8 Then
                    tex_ykl(0).DrawGraph(0, -60 + 60 * (pos - 10.6) / 4.7)
                ElseIf pos < 12.6 Then
                    tex_ykl(1).DrawGraph(0, -60 + 60 * (pos - 10.6) / 4.7)
                Else
                    tex_ykl(2).DrawGraph(0, -60 + 60 * (pos - 10.6) / 4.7)
                End If
                'DrawRectF(0, 0, 400, 300, Color.FromArgb((pos - 10.6) / 5 * 255, Color.White))
            End If
            If BelongTo(pos, 13.0, 14.5) Then 'Sakura/ShiftScreen
                If frmCount = 0 Then 'add leaf
                    For i = 1 To 3
                        ptc = New cParticle
                        ptc.mTex = tex_petal
                        ptc.Init(Rnd() * 420 + 70, 0, -Rnd() * 3 - 2, 2 + Rnd() * 3.5, 0, -Rnd() * 0.01)
                        ptccol.Add(ptc, "ptc" & iKey)
                        iKey += 1
                        iKeyCheck()
                    Next
                End If
                frmCount += 1
                If frmCount = 1 Then frmCount = 0

            End If
            If BelongTo(pos, 13.0, 20) Then 'Sakura/ShiftScreen Draw
                For Each p As cParticle In ptccol
                    p.ProcPosition()
                    p.DrawC()
                Next
                If pos < 15.3 Then
                    tex_petalbg.SetAlpha((pos - 13) / 2.3 * 254)
                    tex_petalbg.DrawGraph(0, 0)
                End If
            End If
            If BelongTo(pos, 14.5, 15.3) Then
                barx = barx * 0.9
                DrawRectF(barx, 0, 400, 37.5, Color.Black)
                DrawRectF(0, 262.5, 400 - barx, 300, Color.Black)
            End If
            If BelongTo(pos, 15.3, bgm.StopPosition) Then 'Press Start
                ClearDevice(Color.White)
                DrawRectF(0, 0, 400, 37.5, Color.Black)
                DrawRectF(0, 262.5, 400, 300, Color.Black)
                anim_walk.SendArgs(tex_walk)
                tex_walk.DrawGraphC(130, 150)
                tex_press.DrawGraphC(210, 258)
            End If

            mGraph.EndDevice()
            RefreshKeyDX()
        Loop Until bStop

        bgm.Stop()
        bgm.Dispose()
        ptccol.Clear()
        tex_walk.Dispose()
        tex_petal.Dispose()
        tex_petalbg.Dispose()
        tex_mt.Dispose()
        tex_sk0.Dispose()
        tex_sk1.Dispose()
        tex_shrine(0).Dispose()
        tex_shrine(1).Dispose()
        tex_reimu.Dispose()
        tex_marisa.Dispose()
        tex_jinja.Dispose()
        tex_korindo.Dispose()
        tex_sc4.Dispose()
        tex_ykl(0).Dispose()
        tex_ykl(1).Dispose()
        tex_ykl(2).Dispose()

        bStop = False
    End Sub
End Module
