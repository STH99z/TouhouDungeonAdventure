Imports Microsoft.DirectX
Imports Microsoft.DirectX.Direct3D

Public Class cTex
    Private m_Tex As Microsoft.DirectX.Direct3D.Texture
    Public ii As ImageInformation
    Public width As Int16
    Public height As Int16
    Public cellX As Int16, cellY As Int16
    Public cellW As Int16, cellH As Int16
    Private cellRect As Rectangle
    Public index As Int16
    Private ix As Int16 = 0, iy As Int16 = 0
    Private bIsMulti As Boolean = False '该纹理是否有多个格子
    Private rotA As Single = 0.0F
    Private rotC As Vector2
    Private sptC As Vector2
    Private sptS As Vector2 = New Vector2(1, 1)
    Private drR As Byte = 255
    Private drG As Byte = 255
    Private drB As Byte = 255
    Private drA As Byte = 255
    Private drW As Double = 1
    Private drH As Double = 1
    Private drColor As System.Drawing.Color = Color.White

    Public Sub LoadGraph(filename As String, Optional ByVal splitX As Int16 = 1, Optional ByVal splitY As Int16 = 1)
        ii = TextureLoader.ImageInformationFromFile(Application.StartupPath + "\" + filename)
        m_Tex = TextureLoader.FromFile(device, Application.StartupPath + "\" + filename, ii.Width, ii.Height, 1, Usage.None, Format.Unknown, Pool.Managed, Filter.None, Filter.None, 0)
        width = ii.Width
        height = ii.Height
        rotC = New Vector2(ii.Width / 2, ii.Height / 2)
        sptC = New Vector2(ii.Width / 2, ii.Height / 2)
        sptS = New Vector2(1, 1)
        cellX = splitX
        cellY = splitY
        cellW = width / cellX
        cellH = height / cellY
        SetCellIndex(1)
        If (cellX > 1) Or (cellY > 1) Then bIsMulti = True
    End Sub

    Public Sub DrawGraph(x As Integer, y As Integer)
        BeginGraph()
        If bIsMulti Then
            '分格图像绘制
            Dim trans As Vector2 = New Vector2(x, y)
            Dim mat As Matrix
            mat = Matrix.Transformation2D(sptC, 0, sptS, rotC, rotA, trans)
            m_Sprite.Transform = mat
            m_Sprite.Draw(m_Tex, New Rectangle(ix * cellW, iy * cellH, cellW, cellH), Nothing, Nothing, drColor)
        Else
            '普通图像绘制
            Dim trans As Vector2 = New Vector2(x + ii.Width * (drW - 1) / 2, y + ii.Height * (drH - 1) / 2)
            Dim mat As Matrix
            'Calculating:scaling centre, scaling rotation, scaling, rotation centre, rotation, translation
            mat = Matrix.Transformation2D(sptC, 0, sptS, sptC, rotA, trans)
            m_Sprite.Transform = mat
            m_Sprite.Draw(m_Tex, Nothing, Nothing, drColor.ToArgb)
        End If
        EndGraph()
    End Sub

    Public Sub DrawGraphR_RECT(x As Integer, y As Integer, RECT_LTWH As Rectangle)
        'BeginGraph()
        '分格图像绘制
        Dim trans As Vector2 = New Vector2(x - Cam.X + ResW \ 2, y - Cam.Y + ResH \ 2)
        Dim Pcenter As New Vector2(RECT_LTWH.Width \ 2, RECT_LTWH.Height \ 2)
        Dim mat As Matrix
        mat = Matrix.Transformation2D(Pcenter, 0, sptS, Pcenter, rotA, trans)
        m_Sprite.Transform = mat
        m_Sprite.Draw(m_Tex, RECT_LTWH, Nothing, Nothing, drColor)
        'EndGraph()
    End Sub

    Public Sub DrawGraphR(x As Integer, y As Integer, Optional ByVal bgeg As Boolean = True)
        If bgeg Then BeginGraph()
        If bIsMulti Then
            '分格图像绘制
            Dim trans As Vector2 = New Vector2(x - Cam.X + ResW / 2, y - Cam.Y + ResH / 2)
            Dim mat As Matrix
            mat = Matrix.Transformation2D(sptC, 0, sptS, rotC, rotA, trans)
            m_Sprite.Transform = mat
            m_Sprite.Draw(m_Tex, New Rectangle(ix * cellW, iy * cellH, cellW, cellH), Nothing, New Vector3(0, 0, y / &HFFFF), drColor)
        Else
            '普通图像绘制
            Dim trans As Vector2 = New Vector2(x + ii.Width * (drW - 1) / 2, y + ii.Height * (drH - 1) / 2)
            Dim mat As Matrix
            'Calculating:scaling centre, scaling rotation, scaling, rotation centre, rotation, translation
            mat = Matrix.Transformation2D(sptC, 0, sptS, sptC, rotA, trans)
            m_Sprite.Transform = mat
            m_Sprite.Draw(m_Tex, Nothing, Nothing, drColor.ToArgb)
        End If
        If bgeg Then EndGraph()
    End Sub

    Public Sub DrawGraphC(x As Integer, y As Integer)
        BeginGraph()
        If bIsMulti Then
            '分格图像绘制
            Dim trans As Vector2 = New Vector2(x - cellW / 2, y - cellH / 2)
            Dim mat As Matrix
            mat = Matrix.Transformation2D(sptC, 0, sptS, rotC, rotA, trans)
            m_Sprite.Transform = mat
            m_Sprite.Draw(m_Tex, New Rectangle(ix * cellW, iy * cellH, cellW, cellH), Nothing, Nothing, drColor)
        Else
            '普通图像绘制
            Dim trans As Vector2 = New Vector2(x - ii.Width / 2, y - ii.Height / 2)
            Dim mat As Matrix
            'Calculating:scaling centre, scaling rotation, scaling, rotation centre, rotation, translation
            mat = Matrix.Transformation2D(sptC, 0, sptS, sptC, rotA, trans)
            m_Sprite.Transform = mat
            m_Sprite.Draw(m_Tex, Nothing, Nothing, drColor.ToArgb)
        End If
        EndGraph()
    End Sub

    Public Sub SetCellIndex(ByVal i As Int16)
        If Not bIsMulti Then Exit Sub
        index = i
        ix = (i - 1) Mod cellX
        iy = (i - 1 - ix) \ cellX
        If i > 0 Then
            rotC = New Vector2(cellW / 2, cellH / 2)
            sptC = New Vector2(cellW / 2, cellH / 2)
        ElseIf i = 0 Then
            rotC = New Vector2(ii.Width / 2, ii.Height / 2)
            sptC = New Vector2(ii.Width / 2, ii.Height / 2)
        End If

    End Sub

    Public Sub SetRotate(ByVal angle As Single)
        rotA = angle
    End Sub

    Public Sub SetScale(w As Double, h As Double)
        drW = w
        drH = h
        sptS = New Vector2(drW, drH)
    End Sub

    Public Sub SetMirror(lrflip As Boolean, udflip As Boolean)
        If lrflip Then drW = Math.Abs(drW) * (-1) Else drW = Math.Abs(drW)
        If udflip Then drH = Math.Abs(drH) * (-1) Else drH = Math.Abs(drH)
        sptS = New Vector2(drW, drH)
    End Sub

    Public Sub SetColor(r, g, b)
        drR = r
        drG = g
        drB = b
        drColor = Color.FromArgb(drA, drR, drG, drB)
    End Sub

    Public Sub SetAlpha(a)
        drA = a
        drColor = Color.FromArgb(drA, drR, drG, drB)
    End Sub

    Public Sub Dispose()
        m_Tex.Dispose()
        ii = Nothing
    End Sub

End Class