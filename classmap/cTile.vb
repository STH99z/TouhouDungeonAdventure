Public Class cTile
    Public id As Int16
    Public tag As String = ""
    Public mTex As cTex
    Public mAnim As cAnim
    Public bAutoTile As Boolean = False
    Public bUpper As Boolean = False
    Public pAutoTile_LT As New Point
    Protected Friend bCollisionCreature As Boolean
    Protected Friend bCollisionProjectile As Boolean

    Protected Friend Sub Init()

    End Sub
    Protected Friend Sub DrawTest(x, y)
        'If Not bAutoTile Then mAnim.SendArgs(mTex)
        mAnim.SendArgs(mTex)
        mTex.DrawGraph(x, y)
        mGraph.DrawText(id, x + 1, y, Color.White)
        'mGraph.DrawText(mTex.index, x + 1, y + 10, Color.White)
        mGraph.DrawText(tag, x + 1, y + 10, Color.White)
    End Sub
    Protected Friend Sub DrawTest_NoText(x, y)
        mAnim.SendArgs(mTex)
        mTex.DrawGraph(x, y)
    End Sub

    Protected Friend Function HaveTag(tagname As String) As Boolean
        If InStr(tag, tagname, CompareMethod.Text) > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Protected Friend Sub DrawR(x, y)
        If Not bAutoTile Then
            mAnim.SendArgs(mTex)
            mTex.DrawGraphR(x, y, False)
        End If

    End Sub
End Class
