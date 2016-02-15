Public Class cTileInstance
    Public Structure dir8tilesame
        Dim s1 As Byte
        Dim s2 As Byte
        Dim s3 As Byte
        Dim s4 As Byte
        Dim s6 As Byte
        Dim s7 As Byte
        Dim s8 As Byte
        Dim s9 As Byte
    End Structure

    Protected Friend d8s As dir8tilesame
    Protected Friend mTile As cTile
    Protected Friend sParamExtra As String

    ''' <summary>
    ''' 相对摄像机位置画出Tile
    ''' </summary>
    ''' <param name="x">地图坐标系x值</param>
    ''' <param name="y">地图坐标系y值</param>
    ''' <remarks></remarks>
    Protected Friend Sub DrawR(x, y)
        If mTile.bAutoTile Then
            With d8s
                'mTile.mTex.DrawGraphR_RECT(x * 32, y * 32, New Rectangle(mTile.pAutoTile_LT, New Size(16, 16)))
                mTile.mTex.DrawGraphR_RECT(x * 32, y * 32, _
                           New Rectangle(mTile.pAutoTile_LT.X + ATofst(0, .s4, .s7, .s8, 0), _
                                         mTile.pAutoTile_LT.Y + ATofst(0, .s4, .s7, .s8, 1), _
                                         16, 16))
                mTile.mTex.DrawGraphR_RECT(x * 32 + 16, y * 32, _
                           New Rectangle(mTile.pAutoTile_LT.X + ATofst(1, .s6, .s9, .s8, 0), _
                                         mTile.pAutoTile_LT.Y + ATofst(1, .s6, .s9, .s8, 1), _
                                         16, 16))
                mTile.mTex.DrawGraphR_RECT(x * 32, y * 32 + 16, _
                           New Rectangle(mTile.pAutoTile_LT.X + ATofst(2, .s4, .s1, .s2, 0), _
                                         mTile.pAutoTile_LT.Y + ATofst(2, .s4, .s1, .s2, 1), _
                                         16, 16))
                mTile.mTex.DrawGraphR_RECT(x * 32 + 16, y * 32 + 16, _
                           New Rectangle(mTile.pAutoTile_LT.X + ATofst(3, .s6, .s3, .s2, 0), _
                                         mTile.pAutoTile_LT.Y + ATofst(3, .s6, .s3, .s2, 1), _
                                         16, 16))

            End With
        Else
            mTile.DrawR(x * 32, y * 32)
        End If
    End Sub

    Protected Friend Sub DrawTest(x, y)
        mTile.DrawTest(x * 32, y * 32)
    End Sub

    Protected Friend Sub ProcParamExtra()

    End Sub

    Public Sub CheckD8S(tlayer, tilex, tiley)
        If mTile.bAutoTile = False Then Exit Sub
        Dim t As Int16 = mTile.id
        With d8s
            .s1 = 0
            .s2 = 0
            .s3 = 0
            .s4 = 0
            .s6 = 0
            .s7 = 0
            .s8 = 0
            .s9 = 0
            If t = Curmap.mLayer(tlayer).GetTileType(tilex - 1, tiley + 1) Then .s1 = 1
            If t = Curmap.mLayer(tlayer).GetTileType(tilex, tiley + 1) Then .s2 = 1
            If t = Curmap.mLayer(tlayer).GetTileType(tilex + 1, tiley + 1) Then .s3 = 1
            If t = Curmap.mLayer(tlayer).GetTileType(tilex - 1, tiley) Then .s4 = 1
            If t = Curmap.mLayer(tlayer).GetTileType(tilex + 1, tiley) Then .s6 = 1
            If t = Curmap.mLayer(tlayer).GetTileType(tilex - 1, tiley - 1) Then .s7 = 1
            If t = Curmap.mLayer(tlayer).GetTileType(tilex, tiley - 1) Then .s8 = 1
            If t = Curmap.mLayer(tlayer).GetTileType(tilex + 1, tiley - 1) Then .s9 = 1
        End With
    End Sub

    Public Sub New(pTileIndexPointer As Int16)
        mTile = Curmap.mTile(pTileIndexPointer)
    End Sub

    Public Sub New(pTile As cTile)
        mTile = pTile
    End Sub
End Class
