Imports Microsoft.DirectX
Imports Microsoft.DirectX.Direct3D

Public Class FrmEdit
    Public bFrmStartLoaded As Boolean = False

    Public Shared fnt20 As Direct3D.Font
    Public Shared fnt10 As Direct3D.Font
    Public Shared Emap As New cMap
    Public Euipage As Byte = 0
    Public iLayerTarget As Int16 = 0
    Public bCamMoving As Boolean = False
    Public pMouseDown As Point
    Public pCamMoveStart As Point
    Public pMousePointing As Point
    Public bSelected As Boolean = False
    Public bMouseDown As Boolean = False
    Public bMouseDownRight As Boolean = False
    Public Shared rectFill As Rectangle
    Public Shared rectSelected As Rectangle
    Public Shared rectATdirect As Rectangle
    Public Ecam(3) As cCamera

    Private Sub FrmEdit_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        If Not bFrmStartLoaded Then
            bFrmStartLoaded = True
            Me.Focus()
            ResW *= 3
            ResH *= 3
            Me.SetClientSizeCore(ResW, ResH)
            mGraph.InitDXBasic(Me, ResW, ResH)
            mInput.InitDXinput()
            mFile.LoadGameData()

            fnt10 = mGraph.CreateFont("Arial", 10, FontStyle.Regular)
            fnt20 = mGraph.CreateFont("Arial", 20, FontStyle.Regular)

            Do Until mInput.IsKeyDownDX(DirectInput.Key.O, False) Or _
                     mInput.IsKeyDownDX(DirectInput.Key.N, False) Or _
                     mInput.IsKeyDownDX(DirectInput.Key.T, False)

                Application.DoEvents()
                BeginDevice()
                mGraph.ClearDevice(Color.Black)
                mGraph.DrawTextA("O -> 打开地图", 20, 20, Color.White, fnt20)
                mGraph.DrawTextA("N -> 新建地图", 20, 60, Color.White, fnt20)
                mGraph.DrawTextA("T -> 测试操作", 20, 100, Color.White, fnt20)
                'mGraph.Drawline(0, 0, 10, 10, Color.White)
                EndDevice()
                RefreshKeyDX()
            Loop

            If mInput.IsKeyDownDX(DirectInput.Key.N) Then
                Dim mwid As Int16, mhei As Int16
                Dim mtilefile As String
                Dim mlayercount As Int16
                Dim mfilename As String
                Dim mmapname As String
                mmapname = InputBox("地图名称？")
                mfilename = InputBox("地图文件名称？")
                mwid = InputBox("地图 宽 格数？", , 100)
                mhei = InputBox("地图 高 格数？", , 60)
                mlayercount = InputBox("地图 图层数？", , 1)
                Dim ofd As New OpenFileDialog
                ofd.Multiselect = False
                ofd.Filter = "TDA tile file|*.txt"
                ofd.InitialDirectory = AppPath & d_data
                If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    mtilefile = ofd.FileName
                    Emap.iLayerCount = mlayercount
                    Emap.LoadTiles(mtilefile)
                    Emap.mapHeight = mhei
                    Emap.mapWidth = mwid
                    Emap.ReDimLayer()
                    Emap.Register()
                    Cam.FocusOn(New Point(0, 0))
                End If
            ElseIf mInput.IsKeyDownDX(DirectInput.Key.T) Then
                Dim mwid As Int16 = 60, mhei As Int16 = 40
                Dim mtilefile As String = AppPath & d_data & "map_tile_mansion.txt"
                Dim mlayercount As Int16 = 2
                Dim mmapname As String
                mmapname = "TestMap"
                Emap.sMapName = mmapname
                Emap.iLayerCount = mlayercount
                Emap.LoadTiles(mtilefile)
                Emap.mapHeight = mhei
                Emap.mapWidth = mwid
                Emap.ReDimLayer()
                Emap.Register()
                Cam.FocusOn(New Point(0, 0))
            ElseIf mInput.IsKeyDownDX(DirectInput.Key.O) Then
                Dim ofd As New OpenFileDialog
                ofd.Multiselect = False
                ofd.Filter = "TDA map file|*.map"
                ofd.InitialDirectory = AppPath & d_data & "maps\"
                If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Emap.MapLoad(ofd.FileName)
                End If
            End If

            Dim i As Int16
            For i = 0 To 3
                Ecam(i) = Cam.copy
            Next

            p_editmain()

        End If
    End Sub

    Private Sub p_editmain()
        Dim tx As Int16, ty As Int16

        Do Until bStop
            Application.DoEvents()

            mGraph.ClearDevice(Color.Black)
            mGraph.BeginDevice()

            If mInput.IsKeyDownDX(DirectInput.Key.Z) Then
                Ecam(Euipage) = Cam.copy
                Euipage = 0
                Cam = Ecam(Euipage).copy
            End If
            If mInput.IsKeyDownDX(DirectInput.Key.X) Then
                Ecam(Euipage) = Cam.copy
                Euipage = 1
                Cam = Ecam(Euipage).copy
            End If
            If mInput.IsKeyDownDX(DirectInput.Key.C) Then
                Ecam(Euipage) = Cam.copy
                Euipage = 2
                Cam = Ecam(Euipage).copy
            End If
            If mInput.IsKeyDownDX(DirectInput.Key.V) Then
                Ecam(Euipage) = Cam.copy
                Euipage = 3
                Cam = Ecam(Euipage).copy
            End If


            If bCamMoving Then
                Cam.FocusOn(New Point(pCamMoveStart.X - mInput.mouseLOC.X + pMouseDown.X, _
                                      pCamMoveStart.Y - mInput.mouseLOC.Y + pMouseDown.Y))
                If mInput.IsKeyDownDX(DirectInput.Key.Space, False) = False Or (mInput.IsMouseD1 = False) Then
                    bCamMoving = False
                End If
            End If
            pMousePointing = New Point((mouseLOC.X - 16 - ofX()) / 32, (mouseLOC.Y - 16 - ofY()) / 32)

            Select Case Euipage
                Case 0 'edit---------------------
                    pMousePointing.X = limitMin(pMousePointing.X, 1)
                    pMousePointing.X = limitMax(pMousePointing.X, Curmap.mapWidth)
                    pMousePointing.Y = limitMin(pMousePointing.Y, 1)
                    pMousePointing.Y = limitMax(pMousePointing.Y, Curmap.mapHeight)
                    ClearDevice(Color.Gray)

                    If mInput.IsKeyDownDX(DirectInput.Key.UpArrow, True) Then
                        iLayerTarget += 1
                        iLayerTarget = limitMax(iLayerTarget, Emap.iLayerCount - 1)
                    End If
                    If mInput.IsKeyDownDX(DirectInput.Key.DownArrow, True) Then
                        iLayerTarget -= 1
                        iLayerTarget = limitMin(iLayerTarget, 0)
                    End If

                    If bSelected Then
                        If Not bCamMoving Then
                            If mInput.IsMouseD1 Then '画笔
                                For tx = rectSelected.Left To rectSelected.Right
                                    For ty = rectSelected.Top To rectSelected.Bottom
                                        Emap.mLayer(iLayerTarget).SetTileType( _
                                            pMousePointing.X + tx - rectSelected.Left, _
                                            pMousePointing.Y + ty - rectSelected.Top, _
                                            tx + ty * Emap.mTexSplitX)
                                    Next
                                Next
                                tx = pMousePointing.X
                                ty = pMousePointing.Y
                                Emap.mLayer(iLayerTarget).ATrefresh( _
                                                    New Rectangle(tx - 1, ty - 1, _
                                                    rectSelected.Width + 2, rectSelected.Height + 2), iLayerTarget)
                            End If
                            If bMouseDownRight Then '矩形
                                If mInput.IsMouseD2 Then '拖拽
                                    rectFill = New Rectangle(rectFill.X, rectFill.Y, _
                                                             pMousePointing.X - rectFill.X, _
                                                             pMousePointing.Y - rectFill.Y)
                                Else '右键松开
                                    bMouseDownRight = False
                                    For tx = rectFill.Left To rectFill.Right
                                        For ty = rectFill.Top To rectFill.Bottom
                                            Emap.mLayer(iLayerTarget).SetTileType( _
                                                    tx, ty, rectSelected.X + rectSelected.Y * Emap.mTexSplitX)
                                            Emap.mLayer(iLayerTarget).ATrefresh( _
                                                    New Rectangle(rectFill.Left - 1, rectFill.Top - 1, _
                                                    rectFill.Right + 1, rectFill.Bottom + 1), iLayerTarget)
                                        Next
                                    Next
                                End If
                            Else
                                If mInput.IsMouseD2 Then '右键
                                    bMouseDownRight = True
                                    rectFill = New Rectangle(pMousePointing, New Size(0, 0))
                                End If
                            End If

                        End If
                    End If
                    If mInput.IsKeyDownDX(DirectInput.Key.LeftControl, False) Then
                        If mInput.IsKeyDownDX(DirectInput.Key.S, False) Then
                            If mInput.IsKeyDownDX(DirectInput.Key.LeftShift, False) Then '另存为

                            Else '保存
                                Emap.MapSave(d_data & "maps\" & Emap.sMapName & ".map")
                                MsgBox("Saved")
                            End If
                        End If
                    End If

                    mGraph.DrawRectF(32 + ofX(), 32 + ofY(), Emap.mapWidth * 32 + 32 + ofX(), Emap.mapHeight * 32 + 32 + ofY(), Color.Black)
                    mGraph.DrawRect(32 + ofX(), 32 + ofY(), Emap.mapWidth * 32 + 32 + ofX(), Emap.mapHeight * 32 + 32 + ofY(), Color.Wheat)
                    'mGraph.Drawline(0, 10, 10, 10, Color.White)
                    Emap.DrawMap()
                    If bSelected Then
                        If bMouseDownRight Then
                            With rectFill
                                mGraph.DrawRect(.Left * 32 + ofX(), .Top * 32 + ofY(), _
                                                .Right * 32 + ofX() + 32, .Bottom * 32 + ofY() + 32, _
                                                Color.Yellow)
                            End With
                        End If
                    End If
                Case 1 'tile select---------------------
                    Emap.TestTiles(0, 0)
                    pMousePointing.X = limitMin(pMousePointing.X, 0)
                    pMousePointing.X = limitMax(pMousePointing.X, Curmap.mTexSplitX - 1)
                    pMousePointing.Y = limitMin(pMousePointing.Y, 0)
                    pMousePointing.Y = limitMax(pMousePointing.Y, Curmap.mTexSplitY - 1)

                    If Not bCamMoving Then '图块选取
                        If mInput.IsMouseD2 Then
                            bSelected = False
                        End If
                        If bMouseDown = False Then
                            If mInput.IsMouseD1 Then
                                bSelected = True
                                bMouseDown = True
                                rectSelected = New Rectangle(pMousePointing, New Size(0, 0))
                            End If
                        End If
                        If bMouseDown Then '鼠标托选
                            If mInput.IsMouseD1 Then
                                rectSelected = New Rectangle(rectSelected.X, rectSelected.Y, _
                                                             pMousePointing.X - rectSelected.X, _
                                                             pMousePointing.Y - rectSelected.Y)
                            Else '鼠标松开
                                bMouseDown = False
                            End If
                        End If
                    End If


                    If bSelected Then
                        With rectSelected
                            mGraph.DrawRect(.X * 32 + ofX(), .Y * 32 + ofY(), _
                                            .Right * 32 + 32 + ofX(), .Bottom * 32 + 32 + ofY(), Color.FromArgb(255, 0, 64, 128))
                            mGraph.DrawRect(.X * 32 + 1 + ofX(), .Y * 32 + 1 + ofY(), _
                                            .Right * 32 + 32 - 1 + ofX(), .Bottom * 32 + 32 - 1 + ofY(), Color.FromArgb(255, 0, 64, 128))
                            mGraph.DrawRectF(.X * 32 + ofX(), .Y * 32 + ofY(), _
                                            .Right * 32 + 32 + ofX(), .Bottom * 32 + 32 + ofY(), Color.FromArgb(64, 0, 64, 128))
                        End With
                    End If
                Case 2 'tile collision edit---------------------
                    Emap.TestTiles_Collision(0, 0)
                    pMousePointing.X = limitMin(pMousePointing.X, 0)
                    pMousePointing.X = limitMax(pMousePointing.X, Curmap.mTexSplitX - 1)
                    pMousePointing.Y = limitMin(pMousePointing.Y, 0)
                    pMousePointing.Y = limitMax(pMousePointing.Y, Curmap.mTexSplitY - 1)

                    With pMousePointing
                        DrawText(Emap.mTile(.X + .Y * Emap.mTexSplitX).mAnim.Args.iIndex, 5, ResH / 2, Color.Wheat)
                        If Not bCamMoving Then
                            If mInput.IsMouseD1c Then
                                Emap.mTile(.X + .Y * Emap.mTexSplitX).bCollisionCreature = Not Emap.mTile(.X + .Y * Emap.mTexSplitX).bCollisionCreature
                            End If
                            If mInput.IsMouseD2c Then
                                Emap.mTile(.X + .Y * Emap.mTexSplitX).bCollisionProjectile = Not Emap.mTile(.X + .Y * Emap.mTexSplitX).bCollisionProjectile
                            End If
                        End If
                    End With

                    If mInput.IsKeyDownDX(DirectInput.Key.LeftControl, False) Then
                        If mInput.IsKeyDownDX(DirectInput.Key.S, False) Then
                            Emap.TileSave(Emap.mTexDataPath)
                            MsgBox("TileData Saved")
                        End If
                    End If
                Case 3 'tile ATlink edit---------------------
                    Emap.TestTiles_AutoTile(0, 0)
                    pMousePointing.X = limitMin(pMousePointing.X, 0)
                    pMousePointing.X = limitMax(pMousePointing.X, Curmap.mTexSplitX - 1)
                    pMousePointing.Y = limitMin(pMousePointing.Y, 0)
                    pMousePointing.Y = limitMax(pMousePointing.Y, Curmap.mTexSplitY - 1)
                    With pMousePointing
                        DrawText(Emap.mTile(.X + .Y * Emap.mTexSplitX).mAnim.Args.iIndex, 5, ResH / 2, Color.Wheat)
                        If Not bCamMoving Then
                            If mInput.IsMouseD1c Then
                                Emap.mTile(.X + .Y * Emap.mTexSplitX).bAutoTile = Not Emap.mTile(.X + .Y * Emap.mTexSplitX).bAutoTile
                                Emap.mTile(.X + .Y * Emap.mTexSplitX).pAutoTile_LT.X = rectATdirect.X * 32
                                Emap.mTile(.X + .Y * Emap.mTexSplitX).pAutoTile_LT.Y = rectATdirect.Y * 32
                            End If
                            If mInput.IsMouseD2 Then
                                rectATdirect = New Rectangle(pMousePointing, New Size(0, 0))
                                mGraph.DrawRectF(rectATdirect.X * 32 + ofX(), _
                                                rectATdirect.Y * 32 + ofY(), _
                                                rectATdirect.X * 32 + ofX() + 64, _
                                                rectATdirect.Y * 32 + ofY() + 96, _
                                                Color.FromArgb(128, Color.Green))
                            End If
                            If mInput.IsKeyDownDX(DirectInput.Key.U) Then
                                Emap.mTile(.X + .Y * Emap.mTexSplitX).bUpper = Not Emap.mTile(.X + .Y * Emap.mTexSplitX).bUpper
                            End If
                        End If
                        If Emap.mTile(.X + .Y * Emap.mTexSplitX).bAutoTile Then
                            mGraph.Drawline(.X * 32 + ofX(), .Y * 32 + ofY(), _
                                            Emap.mTile(.X + .Y * Emap.mTexSplitX).pAutoTile_LT.X + ofX(), _
                                            Emap.mTile(.X + .Y * Emap.mTexSplitX).pAutoTile_LT.Y + ofY(), _
                                            Color.Green)
                            mGraph.DrawRectF(Emap.mTile(.X + .Y * Emap.mTexSplitX).pAutoTile_LT.X + ofX(), _
                                            Emap.mTile(.X + .Y * Emap.mTexSplitX).pAutoTile_LT.Y + ofY(), _
                                            Emap.mTile(.X + .Y * Emap.mTexSplitX).pAutoTile_LT.X + ofX() + 64, _
                                            Emap.mTile(.X + .Y * Emap.mTexSplitX).pAutoTile_LT.Y + ofY() + 96, _
                                            Color.FromArgb(128, Color.Green))
                        End If
                    End With

                    If mInput.IsKeyDownDX(DirectInput.Key.LeftControl, False) Then
                        If mInput.IsKeyDownDX(DirectInput.Key.S, False) Then
                            Emap.TileSave(Emap.mTexDataPath)
                            MsgBox("TileData Saved")
                        End If
                    End If
            End Select
            With pMousePointing
                mGraph.DrawText("CamPos: " & Cam.X & " " & Cam.Y, 5, 5, Color.White)
                mGraph.DrawText("TilePos: " & .X & " " & .Y, 5, 16, Color.White)
                mGraph.DrawText("LayerTarget: " & iLayerTarget & " ↑↓换层", 5, 27, Color.White)
                mGraph.DrawText("Ctrl + S: 保存", 5, 38, Color.White)
                mGraph.DrawText("Ctrl + Shift + S: 另存", 5, 49, Color.White)
                mGraph.DrawRect(.X * 32 + ofX(), .Y * 32 + ofY(), .X * 32 + ofX() + 32, .Y * 32 + ofY() + 32, Color.White)
                mGraph.DrawRectF(.X * 32 + ofX(), .Y * 32 + ofY(), .X * 32 + ofX() + 32, .Y * 32 + ofY() + 32, Color.FromArgb(64, 255, 255, 255))
            End With
            Select Case Euipage
                Case 0
                    mGraph.DrawText("当前模式：地图绘制 Space_ML_MR", 5, ResH - 20, Color.White)
                Case 1
                    mGraph.DrawText("当前模式：图块选择 Space_ML_MR", 5, ResH - 20, Color.White)
                Case 2
                    mGraph.DrawText("当前模式：图块碰撞设定 Space_ML_MR", 5, ResH - 20, Color.White)
                Case 3
                    mGraph.DrawText("当前模式：自动图块设定 Space_ML_MR_U", 5, ResH - 20, Color.White)
            End Select
            EndDevice()
            mInput.RefreshKeyDX()
        Loop
    End Sub

    Private Sub FrmEdit_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        FrmStart.ExitApplication()
    End Sub

    Private Sub FrmEdit_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        bStop = True
        UnloadDXengine()
    End Sub

    Private Sub FrmEdit_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        GetMSfromWindow(e.Button)
        GetXYfromWindow(e.X, e.Y)
        If mInput.IsKeyDownDX(DirectInput.Key.Space, False) And (e.Button = Windows.Forms.MouseButtons.Left) Then
            bCamMoving = True
            pMouseDown = New Point(e.X, e.Y)
            pCamMoveStart = New Point(Cam.X, Cam.Y)
        End If

    End Sub

    Private Sub FrmEdit_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        GetMSfromWindow(e.Button)
        GetXYfromWindow(e.X, e.Y)
    End Sub

    Private Sub FrmEdit_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        GetMSfromWindow(e.Button)
        GetXYfromWindow(e.X, e.Y)
    End Sub

    Private Sub FrmEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class