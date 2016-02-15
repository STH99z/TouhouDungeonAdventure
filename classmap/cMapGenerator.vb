Public Class cMapGenerator
    Protected Friend tMap As cMap

    Protected Friend Sub GenerateMansion(ByRef map As cMap)
        tMap = map
        With map
            .mapWidth = 125
            .mapHeight = 95
            .iLayerCount = 1
            ReDim .mLayer(.iLayerCount - 1)
            Dim i As Int16
            For i = 0 To .iLayerCount - 1
                .mLayer(i) = New cLayer
                .mLayer(i).layerWidth = 125
                .mLayer(i).layerHeight = 95
                .mLayer(i).Init(125, 95)
            Next

        End With
        With map.mLayer(0)
            Dim px As Int16, py As Int16
            Dim wid As Int16 = Int(Rnd() * 3) + 4
            Dim intxpos As Int16 = Int(Rnd() * (.layerWidth - 20) + 10)
            Dim intylen As Int16 = Int(Rnd() * 10) + 20
            FillRegion(intxpos - wid, 95 - wid - intylen, intxpos + wid, 95 - wid, 0, 2)
            FillRegionAuto(intxpos - wid + 2, 95 - wid - intylen + 2, intxpos + wid - 2, 95 - wid - 2, 0, 41)
            For py = .layerHeight To 1 Step -1
                For px = 1 To .layerWidth
                    If IsTileEmpty(px, py, 0) Then SetTile(0, px, py, 6)
                Next
            Next
            map.mLayer(0).ATrefresh(0)
            FrmMain.p1.SetPos(intxpos * 32 + 16, 95 * 32 - wid * 32 + 16)
        End With

    End Sub

    Protected Friend Sub FillFrame(l, t, r, b, layer, idcenter)
        Dim px As Int16, py As Int16
        px = l
        For py = t + 1 To b - 1
            SetTile(layer, px, py, idcenter - 1)
        Next
        px = r
        For py = t + 1 To b - 1
            SetTile(layer, px, py, idcenter + 1)
        Next
        py = t
        For px = l + 1 To r - 1
            SetTile(layer, px, py, idcenter - tMap.mTexSplitX)
        Next
        py = b
        For px = l + 1 To r - 1
            SetTile(layer, px, py, idcenter + tMap.mTexSplitX)
        Next
        px = l
        py = t
        SetTile(layer, px, py, idcenter - tMap.mTexSplitX - 1)
        px = r
        py = t
        SetTile(layer, px, py, idcenter - tMap.mTexSplitX + 1)
        px = l
        py = b
        SetTile(layer, px, py, idcenter + tMap.mTexSplitX - 1)
        px = r
        py = b
        SetTile(layer, px, py, idcenter + tMap.mTexSplitX + 1)
    End Sub

    Protected Friend Sub FillRegion(l, t, r, b, layer, id)
        For px = l To r
            For py = t To b
                SetTile(layer, px, py, id)
            Next
        Next
    End Sub

    Protected Friend Sub FillRegionAuto(l, t, r, b, layer, idcenter)
        FillFrame(l, t, r, b, layer, idcenter)
        FillRegion(l + 1, t + 1, r - 1, b - 1, layer, idcenter)
    End Sub

    Protected Friend Function IsTileEmpty(x, y, layer) As Boolean
        Return (tMap.mLayer(layer).iTileI(x, y) Is Nothing)
    End Function
    Protected Friend Sub SetTile(tlayer, tx, ty, tTileID)
        If tx < 1 Then tx = 1
        If ty < 1 Then ty = 1
        If tx > tMap.mapWidth Then tx = tMap.mapWidth
        If ty > tMap.mapHeight Then ty = tMap.mapHeight
        tMap.mLayer(tlayer).iTileI(tx, ty) = New cTileInstance(tMap.mTile(tTileID))
    End Sub
    Protected Friend Sub SetTileAuto(tlayer, tx, ty, tTileCenterID)
        '设置元件图形地图块到格，ID要求是中心位置的，右边 没有 凹角时的自动元件
        '咕~~(╯﹏╰)b 待编写
        If tx < 1 Then tx = 1
        If ty < 1 Then ty = 1
        If tx > tMap.mapWidth Then tx = tMap.mapWidth
        If ty > tMap.mapHeight Then ty = tMap.mapHeight

        tMap.mLayer(tlayer).iTileI(tx, ty) = New cTileInstance(tMap.mTile(tTileCenterID))
    End Sub

    Protected Friend Sub SetTileAuto_Stripe(tlayer As Int16, tx As Int16, ty As Int16, tTileIDlist() As Int16)
        '设置元件图形地图块到格，ID要求是中心位置的，主要用于条状墙体

    End Sub

    Protected Friend Sub SetTileAuto_Concave(tlayer, tx, ty, tTileCenterID)
        '设置元件图形地图块到格，ID要求是中心位置的，右边 跟着 凹角时的自动元件
        '咕~~(╯﹏╰)b 待编写
        If tx < 1 Then tx = 1
        If ty < 1 Then ty = 1
        If tx > tMap.mapWidth Then tx = tMap.mapWidth
        If ty > tMap.mapHeight Then ty = tMap.mapHeight
        tMap.mLayer(tlayer).iTileI(tx, ty) = New cTileInstance(tMap.mTile(tTileCenterID))
    End Sub
End Class
