Public Class FrmStart
    Public bFirstStart As Boolean = False

    Private Sub FrmStart_Activated(sender As Object, e As EventArgs) Handles Me.Activated

	End Sub

    Private Sub FrmStart_Load(sender As Object, e As EventArgs) Handles Me.Load

	End Sub

    Public Sub ExitApplication()
        End
    End Sub

	Private Sub FrmStart_Shown(sender As Object, e As EventArgs) Handles Me.Shown
		Text = "Loading..."
		If Not bFirstStart Then
			bFirstStart = True

			If My.Application.CommandLineArgs.Count > 0 Then
				If My.Application.CommandLineArgs.Contains("-me") Then
					Me.Hide()
					FrmEdit.Show()
				End If
			Else
				Me.Hide()
				FrmMain.Show()
			End If
		End If
	End Sub
End Class