Imports System.Collections.ObjectModel

''' <summary>
''' Representation of the ItemContainerGenerator. Contains utilities for mapping items of an <see cref="ItemsControl" /> to a generated container.
''' </summary>
Public Class AppleBarItemContainerGenerator

#Region " Objects and variables "

	Private ReadOnly m_ItemsContainer As IDictionary(Of DependencyObject, Object)
	Private m_ItemsHost As Panel

#End Region

#Region " Properties "

	Friend ReadOnly Property ItemsHost As Panel
		Get
			If m_ItemsHost Is Nothing Then
				If m_ItemsContainer.Count = 0 Then
					Return Nothing
				End If
				Dim container As DependencyObject = m_ItemsContainer.First().Key

				m_ItemsHost = CType(VisualTreeHelper.GetParent(container), Panel)
			End If
			Return m_ItemsHost
		End Get
	End Property

#End Region

#Region " Public methods and functions "

	Friend Sub ClearContainerForItemOverride(element As DependencyObject, item As Object)

		m_ItemsContainer.Remove(element)

	End Sub

	''' <summary>
	''' Gets an item container from a specified index.
	''' </summary>
	''' <param name="index">The index for which to obtain the container.</param>
	''' <returns>A container if one can be found. Returns nothing/null otherwise.</returns>
	Public Function ContainerFromIndex(index As Integer) As DependencyObject
		Dim host As Panel = ItemsHost

		If host Is Nothing OrElse host.Children Is Nothing OrElse index < 0 OrElse index >= host.Children.Count Then
			Return Nothing
		End If

		Return host.Children(index)

	End Function

	''' <summary>
	''' Gets a read-only collection of containers.
	''' </summary>
	''' <returns>Returns a ReadOnlyCollection of containers.</returns>
	Public Function GetContainerList() As ReadOnlyCollection(Of DependencyObject)
		Dim containers As List(Of DependencyObject) = m_ItemsContainer.Keys.ToList

		Return New ReadOnlyCollection(Of DependencyObject)(containers)

	End Function

	''' <summary>
	''' Gets an index from a specified container.
	''' </summary>
	''' <param name="container"></param>
	''' <returns></returns>
	Public Function IndexFromContainer(container As DependencyObject) As Integer

		If container Is Nothing Then
			Throw New ArgumentException("container")
		End If

		Dim element As UIElement = CType(container, UIElement)
		If element Is Nothing Then
			Return -1
		End If

		Dim host As Panel = ItemsHost

		If host Is Nothing OrElse host.Children Is Nothing Then
			Return -1
		End If

		Return host.Children.IndexOf(element)

	End Function

	Friend Sub PrepareContainerForItemOverride(element As DependencyObject, item As Object, parentItemContainerStyle As Style)
		Dim control As Control = CType(element, Control)

		m_ItemsContainer(element) = item
		If Not parentItemContainerStyle Is Nothing AndAlso Not control Is Nothing AndAlso control.Style Is Nothing Then
			control.SetValue(control.StyleProperty, parentItemContainerStyle)
		End If

	End Sub

	Friend Sub UpdateItemContainerStyle(itemContainerStyle As Style)

		If itemContainerStyle Is Nothing Then
			Return
		End If

		Dim host As Panel = ItemsHost

		If host Is Nothing OrElse host.Children Is Nothing Then
			Return
		End If

		For Each element As UIElement In host.Children
			Dim obj As FrameworkElement = CType(element, FrameworkElement)

			If obj.Style Is Nothing Then
				obj.Style = itemContainerStyle
			End If
		Next

	End Sub

#End Region

#Region " Construction and Destruction "

	''' <summary>
	''' Creates a new instance of an <see cref="AppleBarItemContainerGenerator" />.
	''' </summary>
	Public Sub New()

		m_ItemsContainer = New Dictionary(Of DependencyObject, Object)

	End Sub

#End Region

End Class