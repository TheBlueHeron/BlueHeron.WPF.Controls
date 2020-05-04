Imports System.Windows
Imports System.Windows.Input

Namespace Input

	''' <summary>
	''' Manages mouse events for a given <see cref="UIElement" />.
	''' </summary>
	''' <remarks>Adapted from the ICE Project (http://ICEdotNet.codeplex.com).</remarks>
	Public NotInheritable Class MouseManager
		Implements IDisposable

#Region " Objects and variables "

		Private m_Delta As Point
		Private m_Disposed As Boolean
		Private m_DoubleClick As Boolean
		Private ReadOnly m_Element As FrameworkElement
		Private m_HasMoved As Boolean
		Private m_IsAttached As Boolean
		Private m_LastDownTime As DateTime = Date.Now
		Private m_PrevPos As Point
		Private ReadOnly m_SpanDoubleClick As New TimeSpan(0, 0, 0, 0, 300)

#End Region

#Region " Properties "

		''' <summary>
		''' The <see cref="ContextMenu">context menu</see> that is attached to the <see cref="FrameworkElement">element</see>.
		''' </summary>
		''' <value><see cref="ContextMenu" /></value>
		''' <returns><see cref="ContextMenu" /></returns>
		Public Property ContextMenu As ContextMenu
			Get
				Return m_Element.ContextMenu
			End Get
			Set(value As ContextMenu)
				m_Element.ContextMenu = value
			End Set
		End Property

		''' <summary>
		''' The amount of current movement.
		''' </summary>
		''' <returns><see cref="Point" /></returns>
		Public ReadOnly Property Delta As Point
			Get
				Return m_Delta
			End Get
		End Property

		''' <summary>
		''' The <see cref="UIElement" /> for which mouse actions are controlled.
		''' </summary>
		''' <returns><see cref="UIElement" /></returns>
		Public ReadOnly Property Element As UIElement
			Get
				Return m_Element
			End Get
		End Property

		''' <summary>
		''' Gets or sets a boolean, determining whether the <see cref="MouseManager">MouseManager</see> should hook into the associated element's events.
		''' </summary>
		''' <value>Boolean</value>
		''' <returns>Boolean</returns>
		Public Property IsAttached As Boolean
			Get
				Return m_IsAttached
			End Get
			Set(value As Boolean)
				ToggleAttached(value)
			End Set
		End Property

#End Region

#Region " Event declarations "

		''' <summary>
		''' Occurs when the <see cref="FrameworkElement">element</see>'s context menu is about to close.
		''' </summary>
		Public Event ContextMenuClosing As ContextMenuEventHandler

		''' <summary>
		''' Occurs when the <see cref="FrameworkElement">element</see>'s context menu is about to open.
		''' </summary>
		Public Event ContextMenuOpening As ContextMenuEventHandler

		''' <summary>
		''' Occurs at the beginning of a drag operation
		''' </summary>
		Public Event DragOnLeftButtonDown As MouseEventHandler

		''' <summary>
		''' Occurs at the beginning of a drag operation
		''' </summary>
		Public Event DragOnRightButtonDown As MouseEventHandler

		''' <summary>
		''' Occurs at the end of a drag operation.
		''' </summary>
		Public Event DropOnLeftButtonDown As MouseButtonEventHandler

		''' <summary>
		''' Occurs at the end of a drag operation.
		''' </summary>
		Public Event DropOnRightButtonDown As MouseButtonEventHandler

		''' <summary>
		''' Occurs when two (left button) click events are performed within 0.3 seconds.
		''' </summary>
		Public Event DoubleClick As MouseButtonEventHandler

		''' <summary>
		''' Occurs when the left button is pressed down on the element.
		''' </summary>
		Public Event LeftButtonDown As MouseButtonEventHandler

		''' <summary>
		''' Occurs when a single click occurs on the element.
		''' </summary>
		Public Event LeftButtonClick As MouseButtonEventHandler

		''' <summary>
		''' Occurs when the mouse moves into the element's region.
		''' </summary>
		Public Event MouseEnter As MouseEventHandler

		''' <summary>
		''' Occurs when the mouse leaves the element's region.
		''' </summary>
		Public Event MouseLeave As MouseEventHandler

		''' <summary>
		''' Occurs when the mouse moves and the left button is down.
		''' </summary>
		Public Event MouseMovedOnLeftButtonDown As MouseEventHandler

		''' <summary>
		''' Occurs when the mouse moves and the right button is down.
		''' </summary>
		Public Event MouseMovedOnRightButtonDown As MouseEventHandler

		''' <summary>
		''' Occurs when the right button is pressed down on the element.
		''' </summary>
		Public Event RightButtonDown As MouseButtonEventHandler

		''' <summary>
		''' Occurs when a single click occurs on the element.
		''' </summary>
		Public Event RightButtonClick As MouseButtonEventHandler

		''' <summary>
		''' Occurs when the mouse wheel is rolled backward.
		''' </summary>
		Public Event WheelMouseDown As MouseWheelEventHandler

		''' <summary>
		''' Occurs when the mouse wheel is rolled forward.
		''' </summary>
		Public Event WheelMouseUp As MouseWheelEventHandler

#End Region

#Region " Public methods and functions "

		Public Sub CaptureMouse()

			m_Element.CaptureMouse()

		End Sub

		Public Sub ReleaseMouseCapture()

			m_Element.ReleaseMouseCapture()

		End Sub

#End Region

#Region " Private methods and functions "

		Private Sub Element_ContextMenuClosing(sender As Object, e As ContextMenuEventArgs)

			e.Handled = True
			RaiseEvent ContextMenuClosing(sender, e)

		End Sub

		Private Sub Element_ContextMenuOpening(sender As Object, e As ContextMenuEventArgs)

			e.Handled = True
			RaiseEvent ContextMenuOpening(sender, e)

		End Sub

		Private Sub Element_MouseEnter(sender As Object, e As MouseEventArgs)

			e.Handled = True
			m_Element.CaptureMouse()
			RaiseEvent MouseEnter(sender, e)

		End Sub

		Private Sub Element_MouseLeave(sender As Object, e As MouseEventArgs)

			e.Handled = True
			m_Element.ReleaseMouseCapture()
			RemoveHandler m_Element.MouseMove, AddressOf Element_MouseMove
			RaiseEvent MouseLeave(sender, e)

		End Sub

		''' <summary>
		''' Captures the mouse for the element and raises the <see cref="LeftButtonDown" /> event.
		''' </summary>
		''' <param name="sender">The <see cref="UIElement" /></param>
		''' <param name="e">The <see cref="MouseButtonEventArgs" /></param>
		Private Sub Element_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)

			'If Not m_Element.IsMouseCaptured Then
			'	m_Element.CaptureMouse()
			'End If
			m_DoubleClick = (Date.Now - m_LastDownTime <= m_SpanDoubleClick)
			m_LastDownTime = Date.Now
			m_HasMoved = False
			m_PrevPos = e.GetPosition(Nothing)
			e.Handled = True

			AddHandler m_Element.MouseMove, AddressOf Element_MouseMove

			RaiseEvent LeftButtonDown(sender, e)

		End Sub

		''' <summary>
		''' Transforms the button down event to a click.
		''' </summary>
		''' <param name="sender">The <see cref="UIElement" /></param>
		''' <param name="e">The <see cref="MouseButtonEventArgs" /></param>
		Private Sub Element_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

			m_Element.ReleaseMouseCapture()
			RemoveHandler m_Element.MouseMove, AddressOf Element_MouseMove
			m_Delta = New Point(0, 0)
			m_PrevPos = e.GetPosition(Nothing)
			e.Handled = True

			If m_HasMoved Then
				RaiseEvent DropOnLeftButtonDown(sender, e)
				m_HasMoved = False
			Else
				If m_DoubleClick Then
					RaiseEvent DoubleClick(m_Element, e)
				Else
					RaiseEvent LeftButtonClick(m_Element, e)
				End If
			End If

		End Sub

		''' <summary>
		''' Raises the <see cref="DragOnLeftButtonDown" /> and <see cref="MouseMovedOnLeftButtonDown" /> events.
		''' </summary>
		''' <param name="sender">The <see cref="UIElement" /></param>
		''' <param name="e">The <see cref="MouseButtonEventArgs" /></param>
		Private Sub Element_MouseMove(sender As Object, e As MouseEventArgs)
			Dim pos As Point = e.GetPosition(Nothing)

			UpdateDelta(pos)
			m_HasMoved = (m_Delta.X > 0 Or m_Delta.Y > 0)
			e.Handled = True
			If e.LeftButton = MouseButtonState.Pressed Then
				RaiseEvent DragOnLeftButtonDown(sender, e)
				RaiseEvent MouseMovedOnLeftButtonDown(sender, e)
			Else
				RaiseEvent DragOnRightButtonDown(sender, e)
				RaiseEvent MouseMovedOnRightButtonDown(sender, e)
			End If

		End Sub

		''' <summary>
		''' Captures the mouse for the element and raises the <see cref="RightButtonDown" /> event.
		''' </summary>
		''' <param name="sender">The <see cref="UIElement" /></param>
		''' <param name="e">The <see cref="MouseButtonEventArgs" /></param>
		Private Sub Element_MouseRightButtonDown(sender As Object, e As MouseButtonEventArgs)

			'If Not m_Element.IsMouseCaptured Then
			'	m_Element.CaptureMouse()
			'End If
			m_PrevPos = e.GetPosition(Nothing)
			m_HasMoved = False
			e.Handled = True

			AddHandler m_Element.MouseMove, AddressOf Element_MouseMove

			RaiseEvent RightButtonDown(sender, e)

		End Sub

		''' <summary>
		''' Transforms the button down event to a click.
		''' </summary>
		''' <param name="sender">The <see cref="UIElement" /></param>
		''' <param name="e">The <see cref="MouseButtonEventArgs" /></param>
		Private Sub Element_MouseRightButtonUp(sender As Object, e As MouseButtonEventArgs)

			e.Handled = True
			m_Element.ReleaseMouseCapture()
			RemoveHandler m_Element.MouseMove, AddressOf Element_MouseMove
			m_Delta = New Point(0, 0)
			m_PrevPos = e.GetPosition(Nothing)

			If m_HasMoved Then
				RaiseEvent DropOnRightButtonDown(sender, e)
				m_HasMoved = False
			Else
				RaiseEvent RightButtonClick(m_Element, e)
			End If

		End Sub

		''' <summary>
		''' Function is called when the browser receives an event from the mouse wheel.
		''' </summary>
		Private Sub Element_MouseWheel(sender As Object, e As MouseWheelEventArgs)

			e.Handled = True
			If e.Delta > 0 Then
				RaiseEvent WheelMouseUp(sender, e)
			Else
				RaiseEvent WheelMouseDown(sender, e)
			End If

		End Sub

		Private Sub ToggleAttached(attached As Boolean)

			If Not attached = m_IsAttached Then
				If attached Then
					With m_Element
						AddHandler .ContextMenuClosing, AddressOf Element_ContextMenuClosing
						AddHandler .ContextMenuOpening, AddressOf Element_ContextMenuOpening
						AddHandler .MouseEnter, AddressOf Element_MouseEnter
						AddHandler .MouseLeave, AddressOf Element_MouseLeave
						AddHandler .MouseLeftButtonDown, AddressOf Element_MouseLeftButtonDown
						AddHandler .MouseLeftButtonUp, AddressOf Element_MouseLeftButtonUp
						AddHandler .MouseRightButtonDown, AddressOf Element_MouseRightButtonDown
						AddHandler .MouseRightButtonUp, AddressOf Element_MouseRightButtonUp
						AddHandler .MouseWheel, AddressOf Element_MouseWheel
					End With
					m_HasMoved = False
					m_Delta = New Point(0, 0)
					m_PrevPos = m_Delta
				Else
					With m_Element
						RemoveHandler .ContextMenuClosing, AddressOf Element_ContextMenuClosing
						RemoveHandler .ContextMenuOpening, AddressOf Element_ContextMenuOpening
						RemoveHandler .MouseEnter, AddressOf Element_MouseEnter
						RemoveHandler .MouseLeave, AddressOf Element_MouseLeave
						RemoveHandler .MouseLeftButtonDown, AddressOf Element_MouseLeftButtonDown
						RemoveHandler .MouseLeftButtonUp, AddressOf Element_MouseLeftButtonUp
						RemoveHandler .MouseRightButtonDown, AddressOf Element_MouseRightButtonDown
						RemoveHandler .MouseRightButtonUp, AddressOf Element_MouseRightButtonUp
						RemoveHandler .MouseWheel, AddressOf Element_MouseWheel
					End With
				End If
				m_IsAttached = attached
			End If

		End Sub

		Private Sub UpdateDelta(pos As Point)

			m_Delta = New Point(pos.X - m_PrevPos.X, pos.Y - m_PrevPos.Y)
			m_PrevPos = pos

		End Sub

#End Region

#Region " Construction and destruction "

		''' <summary>
		''' Initializes a new instance of the MouseManager class.
		''' </summary>
		''' <param name="element">The <see cref="UIElement">UI element</see> to listen to</param>
		''' <remarks></remarks>
		Public Sub New(element As FrameworkElement, attach As Boolean)

			If element Is Nothing Then
				Throw New NullReferenceException("element")
			End If

			m_Element = element
			ToggleAttached(attach)	' calls ToggleAttached(...) to hook into events

		End Sub

		Private Sub Dispose(disposing As Boolean)

			If Not m_Disposed Then
				If disposing Then
					ToggleAttached(False)
				End If
			End If
			m_Disposed = True

		End Sub

#End Region

#Region "IDisposable Support"

		Public Sub Dispose() Implements IDisposable.Dispose

			Dispose(True)
			GC.SuppressFinalize(Me)

		End Sub

#End Region

	End Class

End Namespace