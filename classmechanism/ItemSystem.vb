Public Class cItem
    Public Shared sItemName() As String = _
        {
            "Nothing",
            "Yen",
            "Item01",
            "Item02"
        }
	Public Shared iItemMaxStack() As Int32 =
		{
			1,
			Integer.MaxValue,
			99,
			99
		}
	Protected Friend sName As String
    Protected Friend iID As Int32

    Public Sub New()
        iID = 0
        sName = sItemName(0)
    End Sub

    Public Sub New(ItemID As Int32)
        iID = ItemID
        sName = sItemName(iID)
    End Sub

End Class

Public Class cBag
    Protected Friend iSlotsCount As Int32
    Protected Friend mSlot() As cItemSlot
    Public Enum ItemDeleteResult
        Failed = 0
        AllClear = 1
        PartClear = 2
    End Enum
    Public Enum ItemAddResult
        Failed = 0
        AllIn = 1
        PartIn = 2
    End Enum

    Public Sub New(ByVal bEmptyBag As Boolean, Optional ByVal slots As Int32 = 40)
        If bEmptyBag Then
            iSlotsCount = slots
            ReDim mSlot(slots)
            For i As Int32 = 1 To slots
                mSlot(i) = New cItemSlot
            Next
        End If
    End Sub

    Protected Friend Function GetNextEmptySlot() As Int32
        Dim s As Int32 = -1
        For i As Int32 = 1 To iSlotsCount
            If mSlot(i).bEmpty = True Then
                s = i
                Exit For
            End If
        Next
        Return s
    End Function

    Protected Friend Function ItemAdd(ItemID As Int32, Amount As Int32) As Boolean
        Dim i As Int32 = 1
        Dim b As ItemAddResult
        Dim d As Int32, start As Int32 = Amount
        Do While (i <= iSlotsCount) And (Amount > 0)
            If mSlot(i).bEmpty Then
                If cItem.iItemMaxStack(ItemID) <= Amount Then
                    mSlot(i) = New cItemSlot(ItemID, cItem.iItemMaxStack(ItemID))
                    Amount -= cItem.iItemMaxStack(ItemID)
                Else
                    mSlot(i) = New cItemSlot(ItemID, Amount)
                    Amount = 0
                End If
            Else
                If mSlot(i).mItem.iID = ItemID Then
                    If Not mSlot(i).IsMaxStack Then
                        d = cItem.iItemMaxStack(ItemID) - mSlot(i).iAmount
                        If d <= Amount Then
                            mSlot(i).iAmount = cItem.iItemMaxStack(ItemID)
                            Amount -= d
                        Else
                            mSlot(i).iAmount += Amount
                            Amount = 0
                        End If
                    End If
                End If
            End If
            i += 1
        Loop
        If Amount < start Then
            If Amount = 0 Then
                b = ItemAddResult.AllIn
            Else
                b = ItemAddResult.PartIn
            End If
        Else
            b = ItemAddResult.Failed
        End If
        Return b
    End Function

    Protected Friend Function ItemDelete(ItemID As Int32, Amount As Int32) As ItemDeleteResult
        Dim i As Int32 = 1
        Dim b As ItemDeleteResult = ItemDeleteResult.Failed
        Do While (i <= iSlotsCount) And (Amount > 0)
            If Not mSlot(i).bEmpty Then
                If mSlot(i).mItem.iID = ItemID Then
                    If mSlot(i).iAmount > Amount Then
                        mSlot(i).iAmount -= Amount
                        b = ItemDeleteResult.AllClear
                        Amount = 0
                    ElseIf mSlot(i).iAmount = Amount Then
                        mSlot(i).Clear()
                        b = ItemDeleteResult.AllClear
                        Amount = 0
                    Else
                        Amount -= mSlot(i).iAmount
                        mSlot(i).Clear()
                        b = ItemDeleteResult.PartClear
                    End If
                End If
            End If
            i += 1
        Loop
        Return b
    End Function

    Protected Friend Function ItemExist(ItemID As Int32) As Boolean
        Dim i As Int32 = 1
        Do While i <= iSlotsCount
            If Not mSlot(i).bEmpty Then
                If mSlot(i).mItem.iID = ItemID Then
                    Return True
                End If
            End If
            i += 1
        Loop
        Return False
    End Function

    Protected Friend Function GetItemAppendSlot(ItemID As Int32) As Int32
        Dim i As Int32 = 1
        Do While i <= iSlotsCount
            If mSlot(i).bEmpty Then
                Return i
            Else
                If mSlot(i).mItem.iID = ItemID Then
                    If Not mSlot(i).IsMaxStack Then
                        Return i
                    End If
                End If
            End If
            i += 1
        Loop
        Return -1
    End Function
End Class

Public Class cItemSlot
    Protected Friend bEmpty As Boolean
    Protected Friend mItem As cItem
    Protected Friend iAmount As Int32

    Public Sub New()
        bEmpty = True
        iAmount = 0
    End Sub

    Public Sub New(ItemID As Int32, Amount As Int32)
        bEmpty = False
        mItem = New cItem(ItemID)
        iAmount = Amount
    End Sub

    Protected Friend Function IsMaxStack() As Boolean
        Return (iAmount = cItem.iItemMaxStack(mItem.iID))
    End Function

    Protected Friend Sub Clear()
        bEmpty = True
        mItem = Nothing
        iAmount = 0
    End Sub
End Class

Public Class cISrenderer
    Protected Friend iISdispX As Int16, iISdispY As Int16
    Protected Friend iISdispXt As Int16, iISdispYt As Int16
    Protected Friend bISrendererHasTarget As Boolean = False
    Protected Friend iBagSlotsCountW As Int16
    Protected Friend iBagSlotsCountH As Int16
    Protected Friend mBagTarget As cBag

    Protected Friend Sub InitISrenderer_test()
        bISrendererHasTarget = True
        Me.mBagTarget = New cBag(True, 25)
        SetBagDispPoint(50, 50)
        SetBagWH(5, 5)
    End Sub

    Protected Friend Sub DrawBagSlots()
        If bISrendererHasTarget Then
            For ix As Int16 = 0 To iBagSlotsCountW - 1
                For iy As Int16 = 0 To iBagSlotsCountH - 1
                    mGraph.DrawRectF(iISdispX + ix * 26, iISdispY + iy * 26, iISdispX + ix * 26 + 24, iISdispY + iy * 26 + 24, Color.FromArgb(128, Color.Black))
                Next
            Next
        End If
    End Sub

    Protected Friend Sub DrawBagGUI()
        If bISrendererHasTarget Then

        End If
    End Sub

    Protected Friend Sub DrawItems()
        If bISrendererHasTarget Then

        End If
    End Sub

    Protected Friend Sub SetBagWH(wslots As Int16, hslots As Int16)
        If bISrendererHasTarget Then
            If wslots * hslots = mBagTarget.iSlotsCount Then
                iBagSlotsCountW = wslots
                iBagSlotsCountH = hslots
            End If
        End If
    End Sub

    Protected Friend Sub SetBagDispPoint(x, y)
        iISdispX = x
        iISdispY = y
    End Sub

    Protected Friend Sub SetTargetBag(ByRef bag As cBag)
        mBagTarget = bag
    End Sub

End Class