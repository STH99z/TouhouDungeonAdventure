Public Class FrmStart
    Public bFirstStart As Boolean = False

    Private Sub FrmStart_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        Me.Hide()
    End Sub

    Private Sub FrmStart_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not bFirstStart Then
            bFirstStart = True
            Me.Hide()
            If My.Application.CommandLineArgs.Count > 0 Then
                If My.Application.CommandLineArgs.Contains("-me") Then
                    Application.DoEvents()
                    FrmEdit.Show()
                End If
            Else
                Application.DoEvents()
                FrmMain.Show()
            End If
        End If
    End Sub

    Public Sub ExitApplication()
        End
    End Sub

End Class