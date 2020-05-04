
''' <summary>
''' Represents a menu item in an <see cref="AppleBar" />.
''' </summary>
Public Class AppleBarItem
	Inherits ContentControl

#Region " Objects and variables "

#Region " Dependency properties "

	Public Shared ReadOnly CommandNameProperty As DependencyProperty = DependencyProperty.Register("CommandName", GetType(String), GetType(AppleBarItem), New PropertyMetadata(String.Empty))
	Public Shared ReadOnly CornerRadiusProperty As DependencyProperty = DependencyProperty.Register("CornerRadius", GetType(CornerRadius), GetType(AppleBarItem), New PropertyMetadata(Nothing))

#End Region

#End Region

#Region " Properties "

	Public Property CommandName As String
		Get
			Return CStr(GetValue(CommandNameProperty))
		End Get
		Set(value As String)
			SetValue(CommandNameProperty, value)
		End Set
	End Property

	Public Property CornerRadius As CornerRadius
		Get
			Return CType(GetValue(CornerRadiusProperty), CornerRadius)
		End Get
		Set(value As CornerRadius)
			SetValue(CornerRadiusProperty, value)
		End Set
	End Property

	Public Property ParentBar As AppleBar

#End Region

#Region " Events "

	Protected Overridable Sub AppleBarItem_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)
		Dim index As Integer = AppleBar.GetGenerator(ParentBar).IndexFromContainer(Me)

		ParentBar.OnItemMouseUp(index)

	End Sub

	Protected Overridable Sub AppleBarItem_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
		Dim index As Integer = AppleBar.GetGenerator(ParentBar).IndexFromContainer(Me)

		ParentBar.OnItemMouseDown(index)

	End Sub

	Protected Overridable Sub AppleBarItem_MouseEnter(sender As Object, e As MouseEventArgs)
		Dim index As Integer = AppleBar.GetGenerator(ParentBar).IndexFromContainer(Me)

		ParentBar.OnMouseEnter(index)

	End Sub

	Protected Overridable Sub AppleBarItem_MouseLeave(sender As Object, e As MouseEventArgs)
		Dim index As Integer = AppleBar.GetGenerator(ParentBar).IndexFromContainer(Me)

		ParentBar.OnItemMouseLeave(index)

	End Sub

#End Region

#Region " Construction and Destruction "

	''' <summary>
	''' Initializes a new instance of an <see cref="AppleBarItem" />.
	''' </summary>
	Public Sub New()

		Me.DefaultStyleKey = GetType(AppleBarItem)
		AddHandler MouseLeftButtonDown, AddressOf AppleBarItem_MouseLeftButtonDown
		AddHandler MouseLeftButtonUp, AddressOf AppleBarItem_MouseLeftButtonUp
		AddHandler MouseEnter, AddressOf AppleBarItem_MouseEnter
		AddHandler MouseLeave, AddressOf AppleBarItem_MouseLeave

	End Sub

#End Region

End Class