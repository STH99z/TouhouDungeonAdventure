Public Class cLayer
    Protected Friend layerWidth As Integer, layerHeight As Integer
    Protected Friend iTileI(,) As cTileInstance

    Protected Friend bVisible As Boolean
    Protected Friend bCollisionCreature As Boolean
    Protected Friend bCollisionProjectile As Boolean

    Protected Friend Overloads Sub DrawRegion(xstart As Int16, ystart As Int16, xend As Int16, yend As Int16)

    End Sub

    Protected Friend Overloads Sub DrawUserClientRegion(Optional ByVal SeperatedRender As Boolean = True)
        Dim xstart As Int16, xend As Int16
        Dim ystart As Int16, yend As Int16
        Dim px As Int16, py As Int16
        xstart = (Cam.X - ResW / 2 - 32) / 32
        If xstart < 1 Then xstart = 1
        xend = (Cam.X + ResW / 2 + 32) / 32
        If xend > layerWidth Then xend = layerWidth
        ystart = (Cam.Y - ResH / 2 - 32) / 32
        If ystart < 1 Then ystart = 1
        yend = (Cam.Y + ResH / 2 + 32) / 32
        If yend > layerHeight Then yend = layerHeight

        If SeperatedRender Then BeginGraph()
        Sys_cTex_Tsymbol.SetCellIndex(1)
        Sys_cTex_Tsymbol.SetAlpha(64)
        For px = xstart To xend
            For py = ystart To yend
                If iTileI(px, py) Is Nothing Then
                    Sys_cTex_Tsymbol.DrawGraphR(px * 32, py * 32, False)
                Else
                    iTileI(px, py).DrawR(px, py)
                End If
            Next
        Next
        If SeperatedRender Then EndGraph()
    End Sub

    Protected Friend Sub DrawUpperTiles(ByRef Entity As cEntity)
        Dim p As Point = Entity.GetTilePos
        BeginGraph()
        For px = p.X - 1 To p.X + 1
            For py = p.Y - 1 To p.Y + 1
                If (px > 0) And (px < Curmap.mapWidth + 1) Then
                    If (py > 0) And (py < Curmap.mapHeight + 1) Then
                        If iTileI(px, py) Is Nothing Then
                            Sys_cTex_Tsymbol.DrawGraphR(px * 32, py * 32, False)
                        Else
                            If iTileI(px, py).mTile.bUpper Then
                                iTileI(px, py).DrawR(px, py)
                            End If
                        End If
                    End If
                End If

            Next
        Next
        EndGraph()
    End Sub

    Protected Friend Sub Gen_TileReplace(original_id As Int16, replace_id As Int16)
        Dim px, py
        For px = 1 To layerWidth
            For py = 1 To layerHeight
                If GetTileType(px, py) = original_id Then iTileI(px, py).mTile = Curmap.mTile(replace_id)
            Next
        Next
    End Sub

    Protected Friend Sub ATrefresh(Optional ByVal layerID As Int16 = 0)
        Dim px As Int16, py As Int16
        For py = 1 To layerHeight
            For px = 1 To layerWidth
                If Not (iTileI(px, py) Is Nothing) Then iTileI(px, py).CheckD8S(layerID, px, py)
            Next
        Next
    End Sub

    Protected Friend Sub ATrefresh(rect As Rectangle, Optional ByVal layerID As Int16 = 0)
        Dim px As Int16, py As Int16
        rect.X = limitMin(rect.X, 1)
        rect.Y = limitMin(rect.Y, 1)
        rect.Width = limitMax(rect.Width, layerWidth - rect.Left)
        rect.Height = limitMax(rect.Height, layerHeight - rect.Top)
        For py = rect.Top To rect.Bottom
            For px = rect.Left To rect.Right
                If Not (iTileI(px, py) Is Nothing) Then iTileI(px, py).CheckD8S(layerID, px, py)
            Next
        Next
    End Sub

    Protected Friend Function GetTileType(tx, ty) As Int16
        If tx < 1 Then Return -1
        If ty < 1 Then Return -1
        If tx > layerWidth Then Return -1
        If ty > layerHeight Then Return -1
        If (iTileI(tx, ty) Is Nothing) Then Return -1
        Return iTileI(tx, ty).mTile.id
    End Function

    Protected Friend Sub SetTileType(tx As Int16, ty As Int16, id As Int16)
        If (tx > 0) And (ty > 0) Then
            If (tx <= layerWidth) And (ty <= layerHeight) Then
                iTileI(tx, ty) = New cTileInstance(id)
            End If
        End If
    End Sub

    Protected Friend Function IsTileAutoTex(tx, ty, TAcenter) As Boolean
        If tx < 1 Then Return False
        If ty < 1 Then Return False
        If tx > layerWidth Then Return False
        If ty > layerHeight Then Return False

        'Dim i As Int16 = iTileI(tx, ty).mTile.id
        'Select Case i
        '    Case TAcenter, TAcenter - 1, TAcenter + 1
        '        Return True
        '    Case TAcenter - Curmap.mTexSplitX, TAcenter - Curmap.mTexSplitX - 1, TAcenter - Curmap.mTexSplitX + 1
        '        Return True
        '    Case TAcenter + Curmap.mTexSplitX, TAcenter + Curmap.mTexSplitX - 1, TAcenter + Curmap.mTexSplitX + 1
        '        Return True
        'End Select
        Return False
    End Function

    Protected Friend Sub ClearLayerData()
        layerWidth = 1
        layerHeight = 1
        ReDim iTileI(0, 0)
    End Sub

    Protected Friend Sub Init(w As Integer, h As Integer)
        layerWidth = w
        layerHeight = h
        ReDim iTileI(w, h)
    End Sub
End Class
