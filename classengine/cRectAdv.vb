Public Class cRectAdv
    Public Left As Single, Top As Single, Width As Single, Height As Single

    Public ReadOnly Property Right() As Single
        Get
            Return Left + Width
        End Get
    End Property
    Public ReadOnly Property Bottom As Single
        Get
            Return Top + Height
        End Get
    End Property

    Public Sub New(x As Single, y As Single, w As Single, h As Single)
        Left = x
        Top = y
        Width = w
        Height = h
    End Sub

    Public Function Contains(p As PointF) As Boolean
        If p.X >= Left Then
            If p.X < Left + Width Then
                If p.Y >= Top Then
                    If p.Y < Top + Height Then
                        Return True
                    End If
                    Return False
                End If
                Return False
            End If
            Return False
        End If
        Return False
    End Function

    Public Function Contains(x As Single, y As Single) As Boolean
        If x >= Left Then
            If x < Left + Width Then
                If y >= Top Then
                    If y < Top + Height Then
                        Return True
                    End If
                    Return False
                End If
                Return False
            End If
            Return False
        End If
        Return False
    End Function
End Class
