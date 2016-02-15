Imports Microsoft.DirectX.DirectInput
Imports Microsoft.DirectX

Module mInput
    Public mouseX As Int16
    Public mouseY As Int16
    Public mouseState As System.Windows.Forms.MouseButtons
    Public mouseStatel As System.Windows.Forms.MouseButtons
    Public mouseState2 As System.Windows.Forms.MouseButtons

    Public mouseDelta As Int16
    Public mouseLOC As System.Drawing.Point
    Public mouseLOCl As System.Drawing.Point
    Public keyData As System.Windows.Forms.Keys

    'DI
    Public Kpool As String = ""
    Public bOnFocus As Boolean = True
    Public DIkeyboard As Device
    Public DImouse As Device

    Public Sub ShowInputKeys()
        Dim k As Key
        Dim s As String = ""
        For Each k In DIkeyboard.GetPressedKeys
            s = s & k & " "
        Next
        DrawText(s, 100, 0, Color.Green)
    End Sub

    Public Function InitDXinput() As Boolean
        On Error GoTo sFailed

        DIkeyboard = New Device(SystemGuid.Keyboard)
        DIkeyboard.Acquire()
        DImouse = New Device(SystemGuid.Mouse)
        DImouse.Acquire()
        Return True

sFailed:
        Return False
    End Function

    Public Function IsKeyDownDX(Key As Key, Optional clearKey As Boolean = True) As Boolean 'DI方法检测按键，false长按
        If Not bOnFocus Then Return False
        Dim k As Key, result As Boolean = False
        For Each k In DIkeyboard.GetPressedKeys
            If Key = k Then
                result = True
                If clearKey Then
                    If InStr(Kpool, k & " ", CompareMethod.Text) > 0 Then
                        result = False
                    Else
                        Kpool = Kpool & k & " "
                    End If
                End If
            End If
        Next

        Return result
    End Function

    Public Sub RefreshKeyDX() '刷新按键，每次绘图循环必须调用一次
        If Kpool = "" Then Exit Sub
        Dim k As Key, arr() As String, i As Int16, bremove As Boolean = True
        arr = Split(Left(Kpool, Len(Kpool) - 1), " ")
        For i = 0 To UBound(arr)
            For Each k In DIkeyboard.GetPressedKeys
                If k = arr(i) Then bremove = False
            Next
            If bremove Then arr(i) = "-"
            bremove = True
        Next
        Kpool = Join(arr, " ") & " "
        Kpool = Strings.Replace(Kpool, "- ", "")

    End Sub

    Public Function GetDelta(Optional cleardelta As Boolean = True) As Int16
        Dim md As Int16 = mouseDelta
        If cleardelta Then mouseDelta = 0
        Return md
    End Function

    Public Function IsKeyDown(kdata As System.Windows.Forms.Keys, Optional clearkey As Boolean = True) As Boolean
        If bOnFocus = False Then Return False
        If (keyData = kdata) Then
            If clearkey Then keyData = Keys.None
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub GetXYfromWindow(x As Int16, y As Int16)
        mouseX = x
        mouseY = y
        mouseLOCl = mouseLOC
        mouseLOC = New Point(x, y)
    End Sub

    Public Sub GetMSfromWindow(eMState As System.Windows.Forms.MouseButtons)
        mouseStatel = mouseState
        mouseState2 = eMState
        mouseState = eMState
    End Sub

    Public Function IsMouseU1() As Boolean
        If bOnFocus = False Then Return False
        If DImouse.CurrentMouseState.GetMouseButtons(1) = 128 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function IsMouseD1() As Boolean
        If bOnFocus = False Then Return False
        If DImouse.CurrentMouseState.GetMouseButtons(0) = 128 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function IsMouseD2() As Boolean
        If bOnFocus = False Then Return False
        If DImouse.CurrentMouseState.GetMouseButtons(1) = 128 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function IsMouseD1c(Optional clearmouseevent As Boolean = True) As Boolean
        If bOnFocus = False Then Return False
        If mouseState = MouseButtons.Left Then
            If clearmouseevent Then mouseState = MouseButtons.None
            Return True
        Else
            Return False
        End If
    End Function

    Public Function IsMouseD2c(Optional clearmouseevent As Boolean = True) As Boolean
        If bOnFocus = False Then Return False
        If mouseState2 = MouseButtons.Right Then
            If clearmouseevent Then mouseState2 = MouseButtons.None
            Return True
        Else
            Return False
        End If
    End Function
End Module
