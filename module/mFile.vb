Module mFile
    Public Const d_data = "RES\data\"
    Public Const d_maps = "RES\data\maps\"
    Public Const f_mtm = "map_tile_mansion.txt"

    Public Const d_image = "RES\image\"
    Public Const f_iccf = "image_chara_centeroffset.txt"
    Public Const f_icwi = "image_chara_walkindex.txt"
    Public Const f_wfs = "image_chara_walkframespeed.txt"

    Public Const d_se = "RES\se\"

    Public ReadOnly AppPath As String = Application.StartupPath & "\"
    Public Line As String = ""
    Public args As String()
    Public argsInput As String()

    Private sReader(0) As IO.StreamReader
    Private sWriter(0) As IO.StreamWriter
    Private iCurR As Int16 = 0
    Private iCurW As Int16 = 0

    Public Sub FileOpen_Read(relatedpath As String)
        If IO.File.Exists(relatedpath) = False Then
            MsgBox("FILE NOT FOUND!" & vbCrLf & relatedpath)
            End
        End If
        If Not (sReader(iCurR) Is Nothing) Then
            iCurR += 1
            ReDim Preserve sReader(iCurR)
        End If
        sReader(iCurR) = New IO.StreamReader(relatedpath, System.Text.Encoding.Default)
    End Sub

    Public Sub FileOpen_Write(relatedpath As String, Optional ByVal write_append As Boolean = False)
        If Not (sWriter(iCurW) Is Nothing) Then
            iCurW += 1
            ReDim Preserve sWriter(iCurW)
        End If
        sWriter(iCurW) = New IO.StreamWriter(relatedpath, write_append, System.Text.Encoding.Default)
    End Sub

    Public Function GetLine() As String
        'If sReader=nothing Then Return "*Empty"
        If sReader(iCurR).EndOfStream Then
            Return "*EOF"
        Else
            Line = sReader(iCurR).ReadLine
            args = Split(Line, ",")
            Return Line
        End If
    End Function

    Public Sub WriteLine(line As String)
        If sWriter Is Nothing Then Exit Sub
        sWriter(iCurW).WriteLine(line)
    End Sub

    Public Sub VarsInput(ByRef obj As Object)
        'MsgBox(Line)
        If Line = "" Then Exit Sub
        If Strings.Left(Line, 2) = "//" Then Exit Sub
        If Strings.Left(Line, 1) = "_" Then
            'Call过程
            argsInput = Split(Strings.Mid(Line, Strings.InStr(Line, ",", CompareMethod.Text) + 1), ",")
            Dim procName As String = Strings.Right(Line, Line.Length - 1)
            CallByName(obj, procName, CallType.Method)
            'MsgBox("OK called proc")
        Else
            'Call变量
            argsInput = Split(Strings.Mid(Line, Strings.InStr(Line, ",", CompareMethod.Text) + 1), ",")
            CallByName(obj, args(0), CallType.Let, argsInput)
            '被Call的Method/Var尼玛币必须是Public声明哒！
            'MsgBox(CallByName(obj, args(0), CallType.Get))
        End If

    End Sub

    Public Sub FileClose_Read()
        sReader(iCurR).Close()
        sReader(iCurR).Dispose()
        iCurR -= 1
        If iCurR < 0 Then
            iCurR = 0
            ReDim sReader(0)
        Else
            ReDim Preserve sReader(iCurR)
        End If

    End Sub

    Public Sub FileClose_Write()
        sWriter(iCurW).Close()
        sWriter(iCurW).Dispose()
        iCurW -= 1
        If iCurW < 0 Then
            iCurW = 0
            ReDim sWriter(0)
        Else
            ReDim Preserve sWriter(iCurW)
        End If

    End Sub

    Public Sub LoadGameData()
        FileOpen_Read(AppPath & d_data & f_iccf)
        GetLine()
        chara_centeroffset = New Point(args(0), args(1))

        FileClose_Read()
        Sys_cTex_Tsymbol = New cTex
        Sys_cTex_Tsymbol.LoadGraph(d_image & "system\tile_symbol.png", 8, 1)
        Sys_cTex_Dmk16 = New cTex
        Sys_cTex_Dmk16.LoadGraph(d_image & "particle\dmk16.png", 16, 12)
        Sys_cTex_Dmk8 = New cTex
        Sys_cTex_Dmk8.LoadGraph(d_image & "particle\dmk8.png", 16, 12)
        Sys_cTex_Dmk5 = New cTex
        Sys_cTex_Dmk5.LoadGraph(d_image & "particle\dmk5.png", 16, 8)

        LoadAllSE()
    End Sub

    Public Sub LoadAllSE()
        SEnames = New Hashtable()
        Try
            Dim sr As New IO.StreamReader(Application.StartupPath & "\" & d_data & "se_list.txt")
            Dim i As Int16
            For i = 1 To 48
                Dim s As String
                s = sr.ReadLine()
                SE(i) = New cSound(d_se & s)
                SEnames.Add(s, SE(i))
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Function EOF_Read() As Boolean
        Return sReader(iCurR).EndOfStream
    End Function
End Module
