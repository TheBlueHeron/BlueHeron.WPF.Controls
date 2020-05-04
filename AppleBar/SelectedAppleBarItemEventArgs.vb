
''' <summary>
''' Event arguments for the MenuIndexChanged and MenuItemClicked events of the <see cref="AppleBar" />.
''' </summary>
Public Class SelectedAppleBarItemEventArgs
	Inherits EventArgs

#Region " Objects and variables "

	Private m_Index As Integer
	Private m_Item As AppleBarItem

#End Region

#Region " Properties "

	''' <summary>
	''' The index of the <see cref="AppleBarItem" /> that raised the event.
	''' </summary>
	''' <returns>Integer</returns>
	Public ReadOnly Property Index As Integer
		Get
			Return m_Index
		End Get
	End Property

	''' <summary>
	''' The <see cref="AppleBarItem" /> that raised the event.
	''' </summary>
	''' <returns><see cref="AppleBarItem" /></returns>
	Public ReadOnly Property Item As AppleBarItem
		Get
			Return m_Item
		End Get
	End Property

#End Region

#Region " Construction and Destruction "

	Public Sub New(item As AppleBarItem, index As Integer)

		m_Index = index
		m_Item = item

	End Sub

#End Region

End Class