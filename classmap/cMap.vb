Public Class cMap
    Public sMapName As String = "None"
    Public mGen As New cMapGenerator
    Public mLayer() As cLayer
    Public iLayerCount As Int16
    Public mTile() As cTile
    Public mTexPath As String
    Public mTexDataPath As String
    Public mTexSplitX As Int16
    Public mTexSplitY As Int16
    Public iTileCount As Int16
    Public iTileReadNow As Int16 = 0
    Public sTag As String
    Public mTileAnim As cAnim
    Public iTileIndex As Int16
    Public iTileRNstart As Int16
    Public iTileRNend As Int16
    Public bTileCC As Boolean = False
    Public bTileCP As Boolean = False
    Public bTileAT As Boolean = False
    Public bTileUP As Boolean = False
    Public iTileAT_l As Int16 = 0
    Public iTileAT_t As Int16 = 0
    Public mapWidth As Integer, mapHeight As Integer
    Public bBeginReadMP As Boolean = False
    Public Shared mTex As New cTex

    Public Sub Register()
        Curmap = Me
    End Sub

    Public Sub ReDimLayer()
        ReDim mLayer(iLayerCount - 1)
        Dim i As Int16
        For i = 0 To iLayerCount - 1
            mLayer(i) = New cLayer
            mLayer(i).layerWidth = mapWidth
            mLayer(i).layerHeight = mapHeight
            mLayer(i).Init(mapWidth, mapHeight)
        Next
    End Sub

    Public Sub DrawMap(Optional ByVal SeperatedRender As Boolean = True)
        Dim pl As Int16
        For pl = 0 To iLayerCount - 1
            mLayer(pl).DrawUserClientRegion(SeperatedRender)
        Next
    End Sub

    Public Sub DrawMap_UpperLayer(Entity As cEntity)
        Dim pl As Int16
        For pl = 0 To iLayerCount - 1
            mLayer(pl).DrawUpperTiles(Entity)
        Next
    End Sub

    Public Function GetTileCC(tx, ty) As Boolean
        If tx < 1 Then Return True
        If ty < 1 Then Return True
        If tx > mapWidth Then Return True
        If ty > mapHeight Then Return True
        Dim i As Int16, b As Boolean = False
        For i = 0 To iLayerCount - 1
            b = b Or mLayer(i).iTileI(tx, ty).mTile.bCollisionCreature
        Next
        Return b
    End Function

    Public Function GetTileCP(tx, ty) As Boolean
        If tx < 1 Then Return True
        If ty < 1 Then Return True
        If tx > mapWidth Then Return True
        If ty > mapHeight Then Return True
        Dim i As Int16, b As Boolean = False
        For i = 0 To iLayerCount - 1
            b = b Or mLayer(i).iTileI(tx, ty).mTile.bCollisionProjectile
        Next
        Return b
    End Function

    Public Sub MapGen()
        mGen.GenerateMansion(Me)
    End Sub

    Public Sub LoadTiles(Optional ByVal filename As String = "")
        If filename <> "" Then
            mTexDataPath = filename
        End If
        'This suppose to be the relative path!
        mFile.FileOpen_Read(Application.StartupPath & "\" & mTexDataPath)
        Do Until mFile.EOF_Read
            mFile.GetLine()
            mFile.VarsInput(Me)
        Loop
        mFile.FileClose_Read()
    End Sub

    Public Sub FlushDataInTile()
        mTileAnim = New cAnim(iTileIndex, iTileIndex, 0)
        mTile(iTileReadNow) = New cTile
        With mTile(iTileReadNow)
            .id = iTileReadNow
            .tag = sTag
            .bCollisionCreature = bTileCC
            .bCollisionProjectile = bTileCP
            .bUpper = bTileUP
            .mAnim = mTileAnim
            .mTex = mTex
            'MsgBox("")
        End With
    End Sub

    Public Sub FDIT()
        If bTileAT Then
            FDIT_A()
        Else
            iTileIndex = iTileReadNow + 1
            FlushDataInTile()
        End If

    End Sub

    ''' <summary>
    ''' 载入地图块，该地图块位自动元件。
    ''' </summary>
    ''' <remarks>方法自动填入对应ID的mTile，涉及参数如下。
    ''' iTileReadNow
    ''' sTag
    ''' bTileAT
    ''' iTileAT_l
    ''' iTileAT_t
    ''' bTileCC
    ''' bTileCP
    ''' </remarks>
    Public Sub FDIT_A()
        iTileIndex = iTileReadNow + 1
        mTileAnim = New cAnim(iTileIndex, iTileIndex, 0)
        mTile(iTileReadNow) = New cTile
        With mTile(iTileReadNow)
            .id = iTileReadNow
            .tag = sTag
            .bAutoTile = bTileAT
            .bCollisionCreature = bTileCC
            .bCollisionProjectile = bTileCP
            .bUpper = bTileUP
            .pAutoTile_LT = New Point(iTileAT_l, iTileAT_t)
            .mTex = mTex
            .mAnim = mTileAnim
            'MsgBox("")
        End With
    End Sub

    Public Sub FDITmulti()
        Dim i As Int16
        For i = iTileRNstart To iTileRNend
            iTileReadNow = i
            FDIT()
        Next
    End Sub

    Public Sub ReDimTile()
        ReDim mTile(iTileCount - 1)
    End Sub

    Public Sub AutoLoad()
        Dim i As Int16
        For i = 0 To iTileCount - 1
            mTileAnim = New cAnim(i + 1, i + 1, 0)
            mTile(i) = New cTile
            With mTile(i)
                .id = i
                .bCollisionCreature = bTileCC
                .bCollisionProjectile = bTileCP
                .mAnim = mTileAnim
                .mTex = mTex
                'MsgBox("")
            End With
        Next

    End Sub

    Public Sub LoadTex()
        mTex.LoadGraph(mTexPath, mTexSplitX, mTexSplitY)
    End Sub

    Public Sub ClearMapData()
        ReDim mLayer(0)
        ReDim mTile(0)
        mapWidth = 1
        mapHeight = 1
    End Sub

    Public Sub TestTiles(Optional ByVal ofstx As Int16 = 0, Optional ByVal ofsty As Int16 = 0)
        Dim i As Int16
        For i = 0 To iTileCount - 1
            mTile(i).DrawTest((i Mod mTexSplitX) * 32 + ofstx + ofX(), (i \ mTexSplitX) * 32 + ofsty + ofY())
        Next
    End Sub

    Public Sub TestTiles_Collision(Optional ByVal ofstx As Int16 = 0, Optional ByVal ofsty As Int16 = 0)
        Dim i As Int16
        Sys_cTex_Tsymbol.SetAlpha(255)
        For i = 0 To iTileCount - 1
            mTile(i).DrawTest_NoText((i Mod mTexSplitX) * 32 + ofstx + ofX(), (i \ mTexSplitX) * 32 + ofsty + ofY())
            If mTile(i).bCollisionCreature Then
                Sys_cTex_Tsymbol.SetCellIndex(2)
                Sys_cTex_Tsymbol.DrawGraphR((i Mod mTexSplitX) * 32 + ofstx, (i \ mTexSplitX) * 32 + ofsty)
            End If
            If mTile(i).bCollisionProjectile Then
                Sys_cTex_Tsymbol.SetCellIndex(3)
                Sys_cTex_Tsymbol.DrawGraphR((i Mod mTexSplitX) * 32 + ofstx, (i \ mTexSplitX) * 32 + 16 + ofsty)
            End If
        Next
    End Sub

    Public Sub TestTiles_AutoTile(Optional ByVal ofstx As Int16 = 0, Optional ByVal ofsty As Int16 = 0)
        Dim i As Int16
        Sys_cTex_Tsymbol.SetAlpha(255)
        For i = 0 To iTileCount - 1
            mTile(i).DrawTest_NoText((i Mod mTexSplitX) * 32 + ofstx + ofX(), (i \ mTexSplitX) * 32 + ofsty + ofY())
            If mTile(i).bAutoTile Then
                Sys_cTex_Tsymbol.SetCellIndex(4)
                Sys_cTex_Tsymbol.DrawGraphR((i Mod mTexSplitX) * 32 + ofstx, (i \ mTexSplitX) * 32 + ofsty)
            End If
            If mTile(i).bUpper Then
                Sys_cTex_Tsymbol.SetCellIndex(5)
                Sys_cTex_Tsymbol.DrawGraphR((i Mod mTexSplitX) * 32 + ofstx, (i \ mTexSplitX) * 32 + 16 + ofsty)
            End If
        Next
    End Sub

    Public Sub MapLoad(ByVal filename As String) ''''''''''''''''''''''''''''''''''''
        Dim ix As Int16, iy As Int16
        Dim il As Int16
        mFile.FileOpen_Read(filename)
        '妈的 改
        '改循环结构
        Do Until mFile.EOF_Read
            mFile.GetLine()
            If bBeginReadMP Then
                For il = 0 To iLayerCount - 1
                    For iy = 1 To mapHeight
                        For ix = 0 To mapWidth - 1
                            mLayer(il).iTileI(ix + 1, iy) = New cTileInstance(mFile.args(ix))
                        Next
                        mFile.GetLine()
                    Next
                    mFile.GetLine()
                Next
            Else
                mFile.VarsInput(Me)
                If mFile.Line = "bBeginReadMP,True" Then
                    Me.LoadTiles()
                    Me.ReDimLayer()
                    Me.Register()
                End If
            End If
        Loop

        mFile.FileClose_Read()
		For il = 0 To iLayerCount - 1
			For iy = 1 To mapHeight
				For ix = 0 To mapWidth - 1
					mLayer(il).iTileI(ix + 1, iy).CheckD8S(il, ix + 1, iy)
				Next
			Next
		Next

		'初始化TileContains
		ReDim htTileContains(mapWidth, mapHeight)
		For ix = 1 To mapWidth
			For iy = 1 To mapHeight
				htTileContains(ix, iy) = New Hashtable()
			Next
		Next
	End Sub

    Public Sub TileSave(ByVal filename As String)
        mFile.FileOpen_Write(filename)
        mFile.WriteLine("mTexPath," & mTexPath)
        mFile.WriteLine("mTexSplitX," & mTexSplitX)
        mFile.WriteLine("mTexSplitY," & mTexSplitY)
        mFile.WriteLine("iTileCount," & iTileCount)
        mFile.WriteLine("_LoadTex")
        mFile.WriteLine("_ReDimTile")
        mFile.WriteLine("_AutoLoad")
        mFile.WriteLine("")
        Dim ix As Int16, iy As Int16
        For iy = 0 To mTexSplitY - 1
            For ix = 0 To mTexSplitX - 1
                iTileReadNow = ix + iy * mTexSplitX
                With mTile(iTileReadNow)
                    mFile.WriteLine("iTileReadNow," & iTileReadNow)
                    mFile.WriteLine("sTag," & .tag)
                    mFile.WriteLine("bTileAT," & .bAutoTile)
                    mFile.WriteLine("bTileUP," & .bUpper)
                    mFile.WriteLine("bTileCC," & .bCollisionCreature)
                    mFile.WriteLine("bTileCP," & .bCollisionProjectile)
                    If .bAutoTile Then
                        mFile.WriteLine("iTileAT_l," & .pAutoTile_LT.X)
                        mFile.WriteLine("iTileAT_t," & .pAutoTile_LT.Y)
                        mFile.WriteLine("_FDIT_A")
                    Else
                        mFile.WriteLine("_FDIT")
                    End If
                End With
            Next
        Next
        mFile.FileClose_Write()
    End Sub

    Public Sub MapSave(ByVal filename As String) ''''''''''''''''''''''''''''''''''''
        mFile.FileOpen_Write(filename)
        mFile.WriteLine("sMapName," & sMapName)
        mFile.WriteLine("iLayerCount," & iLayerCount)
        mFile.WriteLine("mTexPath," & mTexPath)
        mFile.WriteLine("mTexDataPath," & mTexDataPath)
        mFile.WriteLine("mTexSplitX," & mTexSplitX)
        mFile.WriteLine("mTexSplitY," & mTexSplitY)
        mFile.WriteLine("iTileCount," & iTileCount)
        mFile.WriteLine("mapWidth," & mapWidth)
        mFile.WriteLine("mapHeight," & mapHeight)
        mFile.WriteLine("bBeginReadMP," & "True")

        Dim ix As Int16, iy As Int16
        Dim il As Int16
        Dim s As String = ""
        For il = 0 To iLayerCount - 1
            For iy = 1 To mapHeight
                For ix = 1 To mapWidth
                    If mLayer(il).iTileI(ix, iy) Is Nothing Then
                        s = s & "0" & ","
                    Else
                        s = s & mLayer(il).iTileI(ix, iy).mTile.id & ","
                    End If
                Next
                mFile.WriteLine(s)
                s = ""
            Next
            mFile.WriteLine("")
        Next
        mFile.FileClose_Write()
    End Sub

End Class
