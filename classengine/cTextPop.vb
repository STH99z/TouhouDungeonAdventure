Public Class cTextPop
    Enum TextPopType
        none = 0
        value = 1
        text = 2
    End Enum
    Protected Friend value As Int16 = 0
    Protected Friend text As String = ""

    Protected Friend sKey As String
    Protected Friend iType As TextPopType
    Protected Friend xPos As Int16, yPos As Int16
    Protected Friend iLastTime = 10

    Public Sub New(x As Int16, y As Int16, type As TextPopType, arg As Object)
        xPos = x
        yPos = y
        iType = type
        Select Case type
            Case TextPopType.value
                value = arg
            Case TextPopType.text
                text = arg
        End Select
        sKey = "T" & mDeclear.iKey
        iKey += 1
        Col_TextPop.Add(Me, sKey)
    End Sub

    Public Sub Pop()
        Select Case iType
            Case TextPopType.value
                DrawText(value, xPos + ofX(), yPos + ofY(), Color.Yellow)
            Case TextPopType.text

        End Select
        yPos -= iLastTime / 3
        iLastTime -= 1
        If iLastTime < 0 Then Col_TextPop.Remove(Me.sKey)
    End Sub
End Class
