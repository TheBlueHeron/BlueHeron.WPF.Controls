Imports System.Windows
Imports System.Windows.Media.Animation

''' <summary>
''' Toolbar control that mimicks the Apple taskbar.
''' </summary>
Public Class AppleBar
	Inherits ItemsControl

#Region " Objects and variables "

	Private ReadOnly m_Generator As AppleBarItemContainerGenerator

	''' <summary>
	''' Delegate for the MenuIndexChanged and MenuItemClicked events.
	''' </summary>
	''' <param name="sender">Represents the object that fired the event.</param>
	''' <param name="e">Event data for the event.</param>
	Public Delegate Sub MenuIndexChangedHandler(sender As Object, e As SelectedAppleBarItemEventArgs)

	''' <summary>
	''' Event fired when the mouse moves over a new menu item.
	''' </summary>
	Public Event MenuIndexChanged As MenuIndexChangedHandler

	''' <summary>
	''' Event fired when a menu item is clicked.
	''' </summary>
	Public Event MenuItemClicked As MenuIndexChangedHandler

#Region " Dependency properties "

	Public Shared ReadOnly BehaviorProperty As DependencyProperty = DependencyProperty.RegisterAttached("Behavior", GetType(IAppleBarBehavior), GetType(AppleBar), New PropertyMetadata(New DefaultAppleBarBehavior(), AddressOf OnBehaviorChanged))
	Public Shared ReadOnly CornerRadiusProperty As DependencyProperty = DependencyProperty.Register("CornerRadius", GetType(CornerRadius), GetType(AppleBar), New PropertyMetadata(Nothing))
	Public Shared Shadows ReadOnly ItemContainerStyleProperty As DependencyProperty = DependencyProperty.Register("ItemContainerStyle", GetType(Style), GetType(AppleBar), New PropertyMetadata(Nothing, AddressOf OnItemContainerStylePropertyChanged))

#End Region

#End Region

#Region " Properties "

	Public Property Behavior As IAppleBarBehavior
		Get
			Return CType(GetValue(BehaviorProperty), IAppleBarBehavior)
		End Get
		Set(value As IAppleBarBehavior)
			SetValue(BehaviorProperty, value)
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

	Public Shadows Property ItemContainerStyle As Style
		Get
			Return CType(GetValue(ItemContainerStyleProperty), Style)
		End Get
		Set(value As Style)
			SetValue(ItemContainerStyleProperty, value)
		End Set
	End Property

#End Region

#Region " Public methods and functions "

	Friend Shared Function GetGenerator(control As AppleBar) As AppleBarItemContainerGenerator
		Dim menu As AppleBar = CType(control, AppleBar)

		If Not menu Is Nothing Then
			Return menu.m_Generator
		End If

		Return Nothing

	End Function

#End Region

#Region " Private methods and functions "

	''' <summary>
	''' Removes an <see cref="AppleBarItem" /> from the control.
	''' </summary>
	''' <param name="element">The <see cref="AppleBarItem" /> element of concern.</param>
	''' <param name="item">The item contained by the <see cref="AppleBarItem" />.</param>
	Protected Overrides Sub ClearContainerForItemOverride(element As DependencyObject, item As Object)
		Dim menuItem As AppleBarItem = CType(element, AppleBarItem)

		If Not menuItem Is Nothing Then
			menuItem.ParentBar = Nothing
		End If

		m_Generator.ClearContainerForItemOverride(element, item)
		MyBase.ClearContainerForItemOverride(element, item)

	End Sub

	''' <summary>
	''' Returns a new <see cref="AppleBarItem" /> instance.
	''' </summary>
	''' <returns><see cref="AppleBarItem" /></returns>
	Protected Overrides Function GetContainerForItemOverride() As DependencyObject

		Return New AppleBarItem()

	End Function

	''' <summary>
	''' Determine if an item is an <see cref="AppleBarItem" />.
	''' </summary>
	''' <param name="item">The object to test.</param>
	''' <returns>A boolean indicating if the item is an <see cref="AppleBarItem" />.</returns>
	Protected Overrides Function IsItemItsOwnContainerOverride(item As Object) As Boolean

		Return (TypeOf (item) Is AppleBarItem)

	End Function

	Public Overrides Sub OnApplyTemplate()

		MyBase.OnApplyTemplate()
		Dim elBorder As DependencyObject = Me.GetTemplateChild("brdOuter")

		If Not elBorder Is Nothing Then
			Dim brd As Border = CType(elBorder, Border)

			brd.Opacity = 0.65
			AddHandler brd.MouseEnter, AddressOf ToolBar_MouseEnter
			AddHandler brd.MouseLeave, AddressOf Toolbar_MouseLeave
		End If

	End Sub

	''' <summary>
	''' Associates the element with a parent container and registers the item with the generator.
	''' </summary>
	''' <param name="element">The <see cref="AppleBarItem" /> element of concern.</param>
	''' <param name="item">The item contained by the <see cref="AppleBarItem" />.</param>
	Protected Overrides Sub PrepareContainerForItemOverride(element As DependencyObject, item As Object)
		Dim menuItem As AppleBarItem = CType(element, AppleBarItem)

		If Not menuItem Is Nothing Then
			' associate the parent
			menuItem.ParentBar = Me
			Me.Behavior.Initialize(Me, menuItem)
		End If

		MyBase.PrepareContainerForItemOverride(element, item)
		m_Generator.PrepareContainerForItemOverride(element, item, ItemContainerStyle)

	End Sub

#End Region

#Region " Events "

	Protected Overridable Sub ToolBar_MouseEnter(sender As Object, e As MouseEventArgs)

		CType(CType(sender, Border).Resources("ToolBarEnterAnimation"), Storyboard).Begin()

	End Sub

	Protected Overridable Sub Toolbar_MouseLeave(sender As Object, e As MouseEventArgs)

		OnMouseEnter(-1)
		CType(CType(sender, Border).Resources("ToolBarLeaveAnimation"), Storyboard).Begin()

	End Sub

	Private Shared Sub OnBehaviorChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
		Dim b As AppleBar = CType(d, AppleBar)

		If Not b Is Nothing Then
			For Each item As AppleBarItem In b.m_Generator.GetContainerList()
				b.Behavior.Initialize(b, item)
			Next
		End If

	End Sub

	Private Shared Sub OnItemContainerStylePropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
		Dim b As AppleBar = CType(d, AppleBar)
		Dim value As Style = CType(e.NewValue, Style)
		Dim generator As AppleBarItemContainerGenerator = AppleBar.GetGenerator(b)

		generator.UpdateItemContainerStyle(value)

	End Sub

	Friend Sub OnItemMouseDown(selectedIndex As Integer)
		Dim content As AppleBarItem = CType(m_Generator.ContainerFromIndex(selectedIndex), AppleBarItem)

		Me.Behavior.ApplyMouseDownBehavior(selectedIndex, content)

	End Sub

	Friend Sub OnItemMouseUp(selectedIndex As Integer)
		Dim content As AppleBarItem = CType(m_Generator.ContainerFromIndex(selectedIndex), AppleBarItem)

		Me.Behavior.ApplyMouseUpBehavior(selectedIndex, content)

		Dim menuArgs As New SelectedAppleBarItemEventArgs(content, selectedIndex)

		RaiseEvent MenuItemClicked(Me, menuArgs)

	End Sub

	Friend Shadows Sub OnMouseEnter(selectedIndex As Integer)

		For i As Integer = 0 To Me.Items.Count - 1
			Dim content As AppleBarItem = CType(m_Generator.ContainerFromIndex(i), AppleBarItem)

			If selectedIndex = -1 Then
				Me.Behavior.ApplyMouseEnterBehavior(-1, content)
				Continue For
			End If

			Me.Behavior.ApplyMouseEnterBehavior(Math.Abs(i - selectedIndex), content)
		Next

		If Not selectedIndex = -1 Then
			Dim content As AppleBarItem = CType(m_Generator.ContainerFromIndex(selectedIndex), AppleBarItem)
			Dim menuArgs As New SelectedAppleBarItemEventArgs(content, selectedIndex)

			RaiseEvent MenuIndexChanged(Me, menuArgs)
		End If

	End Sub

	Friend Sub OnItemMouseLeave(selectedIndex As Integer)

		If selectedIndex > -1 Then
			Dim content As AppleBarItem = CType(m_Generator.ContainerFromIndex(selectedIndex), AppleBarItem)

			Me.Behavior.ApplyMouseLeaveBehavior(content)
		End If

	End Sub

#End Region

#Region " Construction and Destruction "

	Public Sub New()

		m_Generator = New AppleBarItemContainerGenerator
		DefaultStyleKey = GetType(AppleBar)

	End Sub

#End Region

End Class