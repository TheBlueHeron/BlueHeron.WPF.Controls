
''' <summary>
''' Extends <see cref="FrameworkElement">FrameworkElements</see> that are on a <see cref="Canvas">canvas</see> with the ability to be dragged around.
''' </summary>
''' <remarks>See: http://stackoverflow.com/questions/294220/dragging-an-image-in-wpf </remarks>
Public Class DraggableExtender
	Inherits DependencyObject

#Region " Objects and variables "

	Private Shared m_IsDragging As Boolean = False
	Private Shared m_Offset As Point ' the offset from the top, left of the item being dragged and the original mouse down

#End Region

#Region " Attached properties "

	' The exposed dependency property, accessible as: DraggableExtender.CanDrag="True/False"
	Public Shared ReadOnly CanDragProperty As DependencyProperty = DependencyProperty.RegisterAttached("CanDrag", GetType(Boolean), GetType(DraggableExtender), New UIPropertyMetadata(False, AddressOf OnChangeCanDragProperty))

	' shared setter
	Public Shared Sub SetCanDrag(element As UIElement, o As Boolean)

		element.SetValue(CanDragProperty, o)

	End Sub

	' shared getter
	Public Shared Function GetCanDrag(element As UIElement) As Boolean

		Return CBool(element.GetValue(CanDragProperty))

	End Function

#End Region

#Region " Private methods and functions "

	' This is triggered when the CanDrag property is set. We'll simply check the element is a UI element and that it is within a canvas. If it is, we'll hook into the mouse events
	Private Shared Sub OnChangeCanDragProperty(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
		Dim element As UIElement = CType(d, UIElement)

		If Not element Is Nothing Then

			If Not e.NewValue Is e.OldValue Then
				If CBool(e.NewValue) Then
					AddHandler element.PreviewMouseDown, AddressOf Element_PreviewMouseDown
					AddHandler element.PreviewMouseUp, AddressOf Element_PreviewMouseUp
					AddHandler element.PreviewMouseMove, AddressOf Element_PreviewMouseMove
				Else
					RemoveHandler element.PreviewMouseDown, AddressOf Element_PreviewMouseDown
					RemoveHandler element.PreviewMouseUp, AddressOf Element_PreviewMouseUp
					RemoveHandler element.PreviewMouseMove, AddressOf Element_PreviewMouseMove
				End If
			End If
		End If

	End Sub

	' This is triggered when the mouse button is pressed on the element being hooked
	Private Shared Sub Element_PreviewMouseDown(sender As Object, e As Global.System.Windows.Input.MouseButtonEventArgs)
		Dim element As FrameworkElement = CType(sender, FrameworkElement) ' ensure it's a framework element because access to the visual tree is necessary

		If Not element Is Nothing Then
			m_IsDragging = True	' start dragging and get the offset of the mouse relative to the element
			m_Offset = e.GetPosition(element)
		End If

	End Sub

	' This is triggered when the mouse is moved over the element
	Private Shared Sub Element_PreviewMouseMove(sender As Object, e As MouseEventArgs)

		' If we're not dragging, don't bother - also validate the element
		If m_IsDragging Then
			Dim element As FrameworkElement = CType(sender, FrameworkElement)

			If Not element Is Nothing Then
				Dim canvas As Canvas = CType(element.Parent, Canvas)
				If Not canvas Is Nothing Then
					Dim mousePoint As Point = e.GetPosition(canvas)	' Get the position of the mouse relative to the canvas

					mousePoint.Offset(-m_Offset.X, -m_Offset.Y)	' Offset the mouse position by the original offset position

					element.SetValue(canvas.LeftProperty, mousePoint.X)	' Move the element on the canvas
					element.SetValue(canvas.TopProperty, mousePoint.Y)
				End If
			End If
		End If

	End Sub

	' this is triggered when the mouse is released
	Private Shared Sub Element_PreviewMouseUp(sender As Object, e As MouseButtonEventArgs)

		m_IsDragging = False

	End Sub

#End Region

End Class