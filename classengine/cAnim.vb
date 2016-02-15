Public Class cAnim
    Protected Friend Structure TexArgs
        Public iType As Byte
        Public iIndex As Int16
        Public idFrom As Int16, idTo As Int16
        Public idList() As Int16
        Public xScale As Single, yScale As Single
        Public xMirror As Boolean, yMirror As Boolean
        Public fRotate As Single
        Public iR As Byte, iG As Byte, iB As Byte
        Public iFrameInterval As Long
    End Structure
    Protected Friend Args As TexArgs
    Protected Friend iLastTick As Long

    Protected Friend Sub SendArgs(ByRef tex As cTex)
        Tick()
        With Args
            Select Case .iType
                Case 0
                    tex.SetCellIndex(.iIndex)
                Case 1
                    tex.SetCellIndex(.idList(.iIndex))
                Case 3
                    tex.SetCellIndex(.iIndex)
            End Select
            tex.SetScale(.xScale, .yScale)
            tex.SetMirror(.xMirror, .yMirror)
            tex.SetRotate(.fRotate)
            tex.SetColor(.iR, .iG, .iB)
        End With
    End Sub

    Public Sub New(indexfrom As Int16, indexto As Int16, interval As Int16)
        If indexfrom = indexto Then
            With Args
                .iType = 3
                .iIndex = indexfrom
                .idFrom = indexfrom
                .idTo = indexto
                .xScale = 1
                .yScale = 1
                .xMirror = False
                .yMirror = False
                .iR = 255
                .iG = 255
                .iB = 255
                .fRotate = 0
                .iFrameInterval = 3600000
            End With
        Else
            With Args
                .iType = 0
                .iIndex = indexfrom
                .idFrom = indexfrom
                .idTo = indexto
                .xScale = 1
                .yScale = 1
                .xMirror = False
                .yMirror = False
                .iR = 255
                .iG = 255
                .iB = 255
                .fRotate = 0
                .iFrameInterval = interval
            End With
        End If
        iLastTick = timeGetTime()
    End Sub

    Public Sub New(list() As Int16, interval As Int16)
        With Args
            .iType = 1
            .iIndex = 0
            .idList = list
            .xScale = 1
            .yScale = 1
            .xMirror = False
            .yMirror = False
            .iR = 255
            .iG = 255
            .iB = 255
            .fRotate = 0
            .iFrameInterval = interval
        End With
        iLastTick = timeGetTime()
    End Sub

    Private Sub Tick()
        If (timeGetTime() - iLastTick) > Args.iFrameInterval Then
            iLastTick = timeGetTime()
            With Args
                Select Case .iType
                    Case 0
                        If .iIndex = .idTo Then
                            .iIndex = .idFrom
                        Else
                            .iIndex += 1
                        End If

                    Case 1
                        If .iIndex = .idList.Length - 1 Then
                            .iIndex = 0
                        Else
                            .iIndex += 1
                        End If

                End Select

            End With
        End If
    End Sub

End Class
