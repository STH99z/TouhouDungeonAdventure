'EZD2D引擎，作者STH99。
'声明，你可以任意拷贝本引擎代码，但必须确保其完整性。
'本引擎引用DX SDK中的Microsoft.DirectX.dll;Microsoft.DirectX.Direct3D.dll;Microsoft.DirectX.Direct3DX.dll
'你不能使用本引擎做商业用途！
'本人邮箱765624429@qq.com，欢迎发来bug报告，我会尽快处理。
'希望看到你的作品哦~能发给我一份吗？
'PS：对本引擎的修改和添加请写在其他模块中。不要改动本模块。

Imports Microsoft.DirectX
Imports Microsoft.DirectX.Direct3D

Module mGraph

    Public presentParams As PresentParameters = New PresentParameters
    Public Const pi = 3.14159
    Public Declare Function timeGetTime Lib "winmm.dll" () As Long '引用系统获取时间的函数
    Public device As Device '声明，Device指显卡适配器，一个显卡至少有一个适配器
    Public d3dfont As Microsoft.DirectX.Direct3D.Font, d3dfont2 As Microsoft.DirectX.Direct3D.Font
    'Public m_Font As Microsoft.DirectX.Direct3D.Font '文字
    Public m_Sprite As Microsoft.DirectX.Direct3D.Sprite '精灵，这个接口对于显示2D图像和文字比较方便
    Public m_Sprite_text As Microsoft.DirectX.Direct3D.Sprite
    Public lTimeLast As Long = -1 '计算fps用
    Public LTimeNow As Long = -1 '计算fps用
    Public fFPSbuffer As Integer '避免溢出报错
    Public fFPS As Integer = 0 'fps值，外部可调用
    Public bSpriteBegined As Boolean = False

    '初始化DX9设备
    Public Sub InitDXBasic(ByRef frm As Form, w As Int16, h As Int16)
        Randomize()
        ATofst_fill()
        'frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        presentParams.Windowed = True  '窗口模式 
        presentParams.SwapEffect = SwapEffect.Discard  '交换
        presentParams.BackBufferFormat = Format.Unknown
        presentParams.BackBufferWidth = w
        presentParams.BackBufferHeight = h
        presentParams.MultiSample = MultiSampleType.None  '多重采样=0

        'presentParams.EnableAutoDepthStencil = True
        'presentParams.AutoDepthStencilFormat = DepthFormat.D16
        device = New Device(0, DeviceType.Hardware, frm, CreateFlags.SoftwareVertexProcessing, presentParams)
        device.VertexFormat = CustomVertex.TransformedColored.Format
        m_Sprite = New Sprite(device)
        m_Sprite_text = New Sprite(device)
        '为了能响应键盘事件，该属性必须设为true         
        frm.KeyPreview = True

        '创建D字体对象，显示字符用,这两句不能放在场景中，否则会很慢 
        Dim winFont As New System.Drawing.Font("Arial", 9, FontStyle.Regular)
        d3dfont = New Microsoft.DirectX.Direct3D.Font(device, winFont)
        Dim winFont2 As New System.Drawing.Font("Arial", 20, FontStyle.Regular)
        d3dfont2 = New Microsoft.DirectX.Direct3D.Font(device, winFont2)
    End Sub

    Public Function CreateFont(ftname As String, ftsize As Int16, ftstyle As FontStyle) As Direct3D.Font
        Dim winFont As New System.Drawing.Font(ftname, ftsize, ftstyle)
        Return New Microsoft.DirectX.Direct3D.Font(device, winFont)
    End Function

    Public Sub UnloadDXengine()
        device.Dispose()
        d3dfont.Dispose()
        d3dfont2.Dispose()
        'm_Font.Dispose()
        m_Sprite.Dispose()
    End Sub

    '创建一个顶点结构，模块内部使用
    Private Function CCV(x As Single, y As Single, c As Color) As CustomVertex.TransformedColored
        CCV.Position = New Vector4(x, y, 0, 1.0F)
        CCV.Color = c.ToArgb
    End Function

    Private Function CCV3(x As Single, y As Single, z As Single, c As Color) As CustomVertex.TransformedColored
        CCV3.Position = New Vector4(x, y, z, 1.0F)
        CCV3.Color = c.ToArgb
    End Function

    '画点
    Public Sub DrawPoint(x As Integer, y As Integer, c As Color)
        Dim v As CustomVertex.TransformedColored
        v = CCV(x, y, c)
        device.DrawUserPrimitives(PrimitiveType.PointList, 1, v)
    End Sub

    '画线
    Public Sub Drawline(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, c As Color)
        Dim v(1) As CustomVertex.TransformedColored
        v(0) = CCV(x1, y1, c)
        v(1) = CCV(x2, y2, c)
        device.DrawUserPrimitives(PrimitiveType.LineList, 1, v)
    End Sub

    '画三角形,实心
    Public Sub DrawTriangle(x1, y1, x2, y2, x3, y3, c)
        Dim v(2) As CustomVertex.TransformedColored
        v(0) = CCV(x1, y1, c)
        v(1) = CCV(x2, y2, c)
        v(2) = CCV(x3, y3, c)
        device.DrawUserPrimitives(PrimitiveType.TriangleList, 1, v)
    End Sub

    '画三角形,实心渐变
    Public Sub DrawTriangleG(x1, y1, x2, y2, x3, y3, c1, c2, c3)
        Dim v(2) As CustomVertex.TransformedColored
        v(0) = CCV(x1, y1, c1)
        v(1) = CCV(x2, y2, c2)
        v(2) = CCV(x3, y3, c3)
        device.DrawUserPrimitives(PrimitiveType.TriangleList, 1, v)
    End Sub

    '画长方形，空心
    Public Sub DrawRect(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, c As Color)
        Drawline(x1, y1, x2, y1, c)
        Drawline(x1, y1, x1, y2, c)
        Drawline(x2, y1, x2, y2, c)
        Drawline(x1, y2, x2, y2, c)
    End Sub

    '画长方形，实心
    Public Sub DrawRectF(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, c As Color)
        Dim v(3) As CustomVertex.TransformedColored
        Dim temp As Integer
        If x1 > x2 Then
            temp = x2
            x2 = x1
            x1 = temp
        End If
        If y1 > y2 Then
            temp = y2
            y2 = y1
            y1 = temp
        End If
        v(0) = CCV(x1, y1, c)
        v(1) = CCV(x2, y1, c)
        v(2) = CCV(x1, y2, c)
        v(3) = CCV(x2, y2, c)
        device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, v)
    End Sub

    '画长方形，实心渐变
    Public Sub DrawRectG(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, _
                         c1 As Color, c2 As Color, c3 As Color, c4 As Color)
        Dim v(3) As CustomVertex.TransformedColored
        Dim temp As Integer
        If x1 > x2 Then
            temp = x2
            x2 = x1
            x1 = temp
        End If
        If y1 > y2 Then
            temp = y2
            y2 = y1
            y1 = temp
        End If
        v(0) = CCV(x1, y1, c1)
        v(1) = CCV(x2, y1, c2)
        v(2) = CCV(x1, y2, c3)
        v(3) = CCV(x2, y2, c4)
        device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, v)
    End Sub

    '画圆
    Public Sub DrawCircle(x As Integer, y As Integer, r As Integer, c As Color)
        Dim v(36) As CustomVertex.TransformedColored '精度为36的圆
        Dim i As Byte = 0
        Dim a As Double = 0
        For i = 0 To 35
            a = 6.28 / 36 * i
            v(i) = CCV(x + Math.Cos(a) * r, y + Math.Sin(a) * r, c)
        Next
        v(36) = v(0)
        device.DrawUserPrimitives(PrimitiveType.LineStrip, 36, v)
    End Sub

    '输出文本
    Public Sub DrawText(ByVal stext As String, ByVal x As Integer, ByVal y As Integer, color As System.Drawing.Color)
        d3dfont.DrawText(Nothing, stext, x, y, color)
    End Sub

    Public Sub DrawTextA(ByVal stext As String, ByVal x As Integer, ByVal y As Integer, color As System.Drawing.Color, font As Microsoft.DirectX.Direct3D.Font)
        font.DrawText(Nothing, stext, x, y, color)
    End Sub

    Public Sub DrawTextMulti(ByVal stext As String, ByVal x As Integer, ByVal y As Integer, color As System.Drawing.Color, font As Microsoft.DirectX.Direct3D.Font)
        If font Is Nothing Then
            d3dfont.DrawText(m_Sprite_text, stext, x, y, color)
        Else
            font.DrawText(m_Sprite_text, stext, x, y, color)
        End If
    End Sub

    Public Sub DrawTextMulti_Flush()
        m_Sprite_text.Begin(SpriteFlags.AlphaBlend)
        m_Sprite_text.Flush()
        m_Sprite_text.End()
    End Sub

    '准备输出精灵图片
    Public Sub BeginGraph()
        If Not bSpriteBegined Then m_Sprite.Begin(SpriteFlags.AlphaBlend + SpriteFlags.SortDepthFrontToBack)
    End Sub

    '结束
    Public Sub EndGraph()
        If Not bSpriteBegined Then m_Sprite.End()
    End Sub

    '准备输出精灵图片
    Public Sub BeginGraph_Forced()
        bSpriteBegined = True
        m_Sprite.Begin(SpriteFlags.AlphaBlend + SpriteFlags.SortDepthBackToFront)
    End Sub

    '结束
    Public Sub EndGraph_Forced()
        m_Sprite.End()
        bSpriteBegined = False
    End Sub

    Public Sub _Test()
        Dim v(8) As CustomVertex.TransformedColored
        v(0) = CCV3(ResW / 2, ResH / 2, 0, Color.White)
        v(1) = CCV3(ResW / 2 + 50, ResH / 2, 0, Color.White)
        v(2) = CCV3(ResW / 2, ResH / 2 + 50, 0, Color.White)

        v(3) = CCV3(ResW / 2, ResH / 2, 0.5F, Color.White)
        v(4) = CCV3(ResW / 2 - 50, ResH / 2, 0.5F, Color.White)
        v(5) = CCV3(ResW / 2, ResH / 2 - 50, 0.5F, Color.White)

        v(6) = CCV3(ResW / 2, ResH / 2, -0.5F, Color.White)
        v(7) = CCV3(ResW / 2 + 50, ResH / 2, -0.5F, Color.White)
        v(8) = CCV3(ResW / 2, ResH / 2 - 50, -0.5F, Color.White)
        device.DrawUserPrimitives(PrimitiveType.TriangleList, 3, v)
    End Sub

    '准备设备绘制
    Public Sub BeginDevice()
        lTimeLast = LTimeNow  '计算FPS准备
        If lTimeLast = -1 Then lTimeLast = timeGetTime - 1
        device.BeginScene()
        device.SetRenderState(RenderStates.ZEnable, True)
        'device.SetRenderState(RenderStates.ZBufferWriteEnable, True)
        'device.SetRenderState(RenderStates.ZBufferFunction, 4)
        device.SetRenderState(RenderStates.CullMode, Cull.None)
		device.SetRenderState(RenderStates.Lighting, False)
        device.SetRenderState(RenderStates.AlphaBlendEnable, True)
        device.SetRenderState(RenderStates.SourceBlend, Blend.SourceAlpha)
        device.SetRenderState(RenderStates.DestinationBlend, Blend.InvSourceAlpha)
        device.SetRenderState(RenderStates.AlphaTestEnable, False)
        'device.SetRenderState(RenderStates.MultisampleAntiAlias, False)
        'device.SetRenderState(RenderStates.AntialiasedLineEnable, False)
    End Sub

    '结束
    Public Sub EndDevice(Optional presentnow As Boolean = True)
		device.EndScene()
		If presentnow Then device.Present()
		LTimeNow = timeGetTime()
		fFPSbuffer = LTimeNow - lTimeLast
		If fFPSbuffer = 0 Then fFPSbuffer = 1 '避免计算过快导致fFPS溢出
        fFPS = Int(1000 / fFPSbuffer) '计算FPS
        'If continueRendering Then
        '    If FrmMain.WindowState <> FormWindowState.Minimized Then
        '        FrmMain.Invalidate()
        '    End If
        'End If
    End Sub

    '清理屏幕
    Public Sub ClearDevice(color As System.Drawing.Color)
        device.Clear(ClearFlags.Target, color, 1.0F, 0)
    End Sub

    '--------------------cTex部分AT拓展--------------------
    ''' <summary>
    ''' ■AutoTile绘制时16x16小格的横向和纵向偏移设置。
    ''' ■参数对应：
    ''' ■参1：0-1-2-3>>角方向7-9-1-3
    ''' ■参2：参1角方向对应横向方向是否为同种方块
    ''' ■参3：参1角方向对应斜向方向是否为同种方块
    ''' ■参4：参1角方向对应纵向方向是否为同种方块
    ''' ■参5：偏移中取横纵哪个数据，0=横距，1=纵距
    ''' </summary>
    ''' <remarks></remarks>
    Public ATofst(3, 1, 1, 1, 1) As Int16
    Private Sub ATofst_fill()
        'Corner7
        ATofst(0, 0, 0, 0, 0) = 0
        ATofst(0, 0, 0, 0, 1) = 32
        ATofst(0, 0, 1, 0, 0) = 0
        ATofst(0, 0, 1, 0, 1) = 32

        ATofst(0, 1, 0, 0, 0) = 32
        ATofst(0, 1, 0, 0, 1) = 32
        ATofst(0, 1, 1, 0, 0) = 32
        ATofst(0, 1, 1, 0, 1) = 32

        ATofst(0, 0, 0, 1, 0) = 0
        ATofst(0, 0, 0, 1, 1) = 64
        ATofst(0, 0, 1, 1, 0) = 0
        ATofst(0, 0, 1, 1, 1) = 64

        ATofst(0, 1, 0, 1, 0) = 32
        ATofst(0, 1, 0, 1, 1) = 0

        ATofst(0, 1, 1, 1, 0) = 32
        ATofst(0, 1, 1, 1, 1) = 64
        'Corner9
        ATofst(1, 0, 0, 0, 0) = 48
        ATofst(1, 0, 0, 0, 1) = 32
        ATofst(1, 0, 1, 0, 0) = 48
        ATofst(1, 0, 1, 0, 1) = 32

        ATofst(1, 1, 0, 0, 0) = 16
        ATofst(1, 1, 0, 0, 1) = 32
        ATofst(1, 1, 1, 0, 0) = 16
        ATofst(1, 1, 1, 0, 1) = 32

        ATofst(1, 0, 0, 1, 0) = 48
        ATofst(1, 0, 0, 1, 1) = 64
        ATofst(1, 0, 1, 1, 0) = 48
        ATofst(1, 0, 1, 1, 1) = 64

        ATofst(1, 1, 0, 1, 0) = 48
        ATofst(1, 1, 0, 1, 1) = 0

        ATofst(1, 1, 1, 1, 0) = 16
        ATofst(1, 1, 1, 1, 1) = 64
        'Corner1
        ATofst(2, 0, 0, 0, 0) = 0
        ATofst(2, 0, 0, 0, 1) = 80
        ATofst(2, 0, 1, 0, 0) = 0
        ATofst(2, 0, 1, 0, 1) = 80

        ATofst(2, 1, 0, 0, 0) = 32
        ATofst(2, 1, 0, 0, 1) = 80
        ATofst(2, 1, 1, 0, 0) = 32
        ATofst(2, 1, 1, 0, 1) = 80

        ATofst(2, 0, 0, 1, 0) = 0
        ATofst(2, 0, 0, 1, 1) = 48
        ATofst(2, 0, 1, 1, 0) = 0
        ATofst(2, 0, 1, 1, 1) = 48

        ATofst(2, 1, 0, 1, 0) = 32
        ATofst(2, 1, 0, 1, 1) = 16

        ATofst(2, 1, 1, 1, 0) = 32
        ATofst(2, 1, 1, 1, 1) = 48
        'Corner3
        ATofst(3, 0, 0, 0, 0) = 48
        ATofst(3, 0, 0, 0, 1) = 80
        ATofst(3, 0, 1, 0, 0) = 48
        ATofst(3, 0, 1, 0, 1) = 80

        ATofst(3, 1, 0, 0, 0) = 16
        ATofst(3, 1, 0, 0, 1) = 80
        ATofst(3, 1, 1, 0, 0) = 16
        ATofst(3, 1, 1, 0, 1) = 80

        ATofst(3, 0, 0, 1, 0) = 48
        ATofst(3, 0, 0, 1, 1) = 48
        ATofst(3, 0, 1, 1, 0) = 48
        ATofst(3, 0, 1, 1, 1) = 48

        ATofst(3, 1, 0, 1, 0) = 48
        ATofst(3, 1, 0, 1, 1) = 16

        ATofst(3, 1, 1, 1, 0) = 16
        ATofst(3, 1, 1, 1, 1) = 48
    End Sub

End Module
