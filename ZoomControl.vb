
''' <summary>
''' <see cref="ContentControl" /> extended with zooming and panning capabilities.
''' </summary>
''' <remarks>Adapted from code by Waaahsabi (http://65.39.148.34/script/Membership/View.aspx?mid=4223859).</remarks>
Public Class ZoomControl
	Inherits ContentControl

#Region " Objects and variables "

	Private m_ClickPoint As Point
	Private m_ControlGrid As Grid
	Private m_CurrentOffset As Point
	Private m_Grid As Grid
	Private m_InitialZoom As Double = 1.0
	Private m_MaxZoom As Double = 1000
	Private m_MinZoom As Double = 0.001
	Private m_Zoom As Double = 1.0

#End Region

#Region " Properties "

	Public Property IncludeBubbledEvents As Boolean = False

	Public Property InitialZoom As Double
		Get
			Return m_InitialZoom
		End Get
		Set(value As Double)
			m_InitialZoom = Math.Min(MaxZoom, Math.Max(MinZoom, value))
			m_Zoom = m_InitialZoom
		End Set
	End Property

	Public Property MaxZoom As Double
		Get
			Return m_MaxZoom
		End Get
		Set(value As Double)
			m_MaxZoom = Math.Min(1000, Math.Max(MinZoom, value))
		End Set
	End Property

	Public Property MinZoom As Double
		Get
			Return m_MinZoom
		End Get
		Set(value As Double)
			m_MinZoom = Math.Min(MaxZoom, Math.Max(0.001, value))
		End Set
	End Property

	Public Property PanEnabled As Boolean = True

	Public ReadOnly Property Transform As MatrixTransform
		Get
			Return CType(m_Grid.RenderTransform, MatrixTransform)
		End Get
	End Property

	Public ReadOnly Property Zoom As Double
		Get
			Return Transform.Matrix.M11	' m11 and m22 are always the same value
		End Get
	End Property

	Public Property ZoomEnabled As Boolean = True
	Public Property ZoomStep As Double = 0.1

#End Region

#Region " Public methods and functions "

	Public Overrides Sub OnApplyTemplate()

		m_ControlGrid = CType(GetTemplateChild("ControlGrid"), Grid)
		If m_ControlGrid Is Nothing Then
			Throw New NullReferenceException("ControlGrid")
		End If
		m_Grid = CType(GetTemplateChild("TransformationGrid"), Grid)
		If m_Grid Is Nothing Then
			Throw New NullReferenceException("TransformationGrid")
		End If
		If InitialZoom < MinZoom Then
			InitialZoom = MinZoom
		End If

		m_Grid.RenderTransform = New MatrixTransform(New Matrix(InitialZoom, 0, 0, InitialZoom, 0, 0))

		MyBase.OnApplyTemplate()

		m_CurrentOffset = New Point(0, 0)
		m_ClickPoint = New Point(ActualWidth, ActualHeight)

	End Sub

	Public Sub ResetZoomAndPan()

		m_Grid.RenderTransform = New MatrixTransform(New Matrix(1, 0, 0, 1, 0, 0))

	End Sub

	Public Sub ZoomIn(position As Point)

		DoZoom(position, True)

	End Sub

	Public Sub ZoomOut(position As Point)

		DoZoom(position, False)

	End Sub

#End Region

#Region " Private methods and functions "

	Private Sub DoZoom(position As Point, zoomIn As Boolean)

		If zoomIn Then
			m_Zoom += ZoomStep
		Else
			m_Zoom -= ZoomStep
		End If
		m_Zoom = Math.Max(m_MinZoom, Math.Min(m_MaxZoom, m_Zoom))

		Dim mousePos As Point = position
		Dim relativeTransform As MatrixTransform = CType(m_Grid.TransformToVisual(Me), MatrixTransform)
		Dim deltaX As Double = mousePos.X - relativeTransform.Matrix.OffsetX
		Dim deltaY As Double = mousePos.Y - relativeTransform.Matrix.OffsetY
		Dim m11 As Double = m_Zoom
		Dim m22 As Double = m_Zoom
		Dim offsetX As Double = Transform.Matrix.OffsetX + deltaX * (1 - m_Zoom / Transform.Matrix.M11)
		Dim offsetY As Double = Transform.Matrix.OffsetY + deltaY * (1 - m_Zoom / Transform.Matrix.M22)
		Dim updatedGrid As New Matrix(m11, Transform.Matrix.M12, Transform.Matrix.M21, m22, offsetX, offsetY)

		m_Grid.RenderTransform = New MatrixTransform(updatedGrid)

	End Sub

	Private Function ShouldHandle(source As Object) As Boolean

		Return IncludeBubbledEvents OrElse (source Is m_ControlGrid) OrElse (source Is Me)

	End Function

	Protected Overrides Sub OnMouseMove(e As Global.System.Windows.Input.MouseEventArgs)

		If e.RightButton = MouseButtonState.Pressed AndAlso PanEnabled AndAlso ShouldHandle(e.OriginalSource) Then
			Dim newPoint As Point = e.GetPosition(Nothing)
			Dim delta As Point = New Point(m_ClickPoint.X - newPoint.X, m_ClickPoint.Y - newPoint.Y)
			Dim updatedGrid As New Matrix(Transform.Matrix.M11, Transform.Matrix.M12, Transform.Matrix.M21, Transform.Matrix.M22, m_CurrentOffset.X - delta.X, m_CurrentOffset.Y - delta.Y)

			m_Grid.RenderTransform = New MatrixTransform(updatedGrid)
		End If

	End Sub

	Protected Overrides Sub OnMouseRightButtonDown(e As Global.System.Windows.Input.MouseButtonEventArgs)

		If PanEnabled AndAlso ShouldHandle(e.OriginalSource) Then
			e.Handled = True
			m_ClickPoint = e.GetPosition(Nothing)
			m_CurrentOffset = New Point(Transform.Matrix.OffsetX, Transform.Matrix.OffsetY)
		End If

	End Sub

	Protected Overrides Sub OnMouseWheel(e As Global.System.Windows.Input.MouseWheelEventArgs)

		If ZoomEnabled AndAlso ShouldHandle(e.OriginalSource) Then
			e.Handled = True
			If e.Delta > 0 Then
				ZoomIn(m_ClickPoint)
			ElseIf e.Delta < 0 Then
				ZoomOut(m_ClickPoint)
			End If
		End If

	End Sub

#End Region

#Region " Construction and Destruction "

	Public Sub New()

		DefaultStyleKey = GetType(ZoomControl)

	End Sub

#End Region

End Class