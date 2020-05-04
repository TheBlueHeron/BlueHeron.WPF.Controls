Imports System.Threading

''' <summary>
''' Behavior class to enable *-like resizing for gridview columns.
''' </summary>
''' <remarks>Translated from code found at: http://lazycowprojects.tumblr.com/post/7063214400/wpf-c-listview-column-width-auto </remarks>
Public Class GridViewColumnResize

#Region " Objects and variables "

#Region " DependencyProperties "

	Public Shared ReadOnly WidthProperty As DependencyProperty = DependencyProperty.RegisterAttached("Width", GetType(String), GetType(GridViewColumnResize), New PropertyMetadata(AddressOf OnSetWidthCallback))
	Public Shared ReadOnly GridViewColumnResizeBehaviorProperty As DependencyProperty = DependencyProperty.RegisterAttached("GridViewColumnResizeBehavior", GetType(GridViewColumnResizeBehavior), GetType(GridViewColumnResize), Nothing)
	Public Shared ReadOnly EnabledProperty As DependencyProperty = DependencyProperty.RegisterAttached("Enabled", GetType(Boolean), GetType(GridViewColumnResize), New PropertyMetadata(AddressOf OnSetEnabledCallback))
	Public Shared ReadOnly ListViewResizeBehaviorProperty As DependencyProperty = DependencyProperty.RegisterAttached("ListViewResizeBehaviorProperty", GetType(ListViewResizeBehavior), GetType(GridViewColumnResize), Nothing)

#End Region

#End Region

#Region " Public methods and functions "

	Public Shared Function GetWidth(obj As DependencyObject) As String

		Return CStr(obj.GetValue(WidthProperty))

	End Function

	Public Shared Sub SetWidth(obj As DependencyObject, value As String)

		obj.SetValue(WidthProperty, value)

	End Sub

	Public Shared Function GetEnabled(obj As DependencyObject) As Boolean

		Return CBool(obj.GetValue(EnabledProperty))

	End Function

	Public Shared Sub SetEnabled(obj As DependencyObject, value As Boolean)

		obj.SetValue(EnabledProperty, value)

	End Sub

#End Region

#Region " Private methods and functions "

	Private Shared Sub OnSetWidthCallback(dependencyObject As DependencyObject, e As DependencyPropertyChangedEventArgs)
		Dim element As GridViewColumn = CType(dependencyObject, GridViewColumn)

		If Not element Is Nothing Then
			Dim behavior As GridViewColumnResizeBehavior = GetOrCreateBehavior(element)

			behavior.Width = CStr(e.NewValue)
		End If

	End Sub

	Private Shared Sub OnSetEnabledCallback(dependencyObject As DependencyObject, e As DependencyPropertyChangedEventArgs)
		Dim element As ListView = CType(dependencyObject, ListView)

		If Not element Is Nothing Then
			Dim behavior As ListViewResizeBehavior = GetOrCreateBehavior(element)

			behavior.Enabled = CBool(e.NewValue)
		End If

	End Sub

	Private Shared Function GetOrCreateBehavior(element As ListView) As ListViewResizeBehavior
		Dim behavior As ListViewResizeBehavior = CType(element.GetValue(GridViewColumnResizeBehaviorProperty), ListViewResizeBehavior)

		If behavior Is Nothing Then
			behavior = New ListViewResizeBehavior(element)
			element.SetValue(ListViewResizeBehaviorProperty, behavior)
		End If

		Return behavior

	End Function

	Private Shared Function GetOrCreateBehavior(element As GridViewColumn) As GridViewColumnResizeBehavior
		Dim behavior As GridViewColumnResizeBehavior = CType(element.GetValue(GridViewColumnResizeBehaviorProperty), GridViewColumnResizeBehavior)

		If behavior Is Nothing Then
			behavior = New GridViewColumnResizeBehavior(element)
			element.SetValue(GridViewColumnResizeBehaviorProperty, behavior)
		End If

		Return behavior

	End Function

#End Region

#Region " Nested behaviors "

	''' <summary>
	''' GridViewColumn behavior class that gets attached to the GridViewColumn control.
	''' </summary>
	Public Class GridViewColumnResizeBehavior

		Private _element As GridViewColumn

		Public Sub New(element As GridViewColumn)

			_element = element

		End Sub

		Public Property Width As String

		Public ReadOnly Property IsStatic As Boolean
			Get
				Return (StaticWidth >= 0)
			End Get
		End Property

		Public ReadOnly Property StaticWidth As Double
			Get
				Dim result As Double

				Return If(Double.TryParse(Width, result), result, -1)
			End Get
		End Property

		Public ReadOnly Property Percentage As Double
			Get
				If Not IsStatic Then
					Return Multiplier * 100
				End If
				Return 0
			End Get
		End Property

		Public ReadOnly Property Multiplier As Double
			Get
				If Width = "*" OrElse Width = "1*" Then
					Return 1
				End If

				If Width.EndsWith("*") Then
					Dim perc As Double

					If Double.TryParse(Width.Substring(0, Width.Length - 1), perc) Then
						Return perc
					End If
				End If
				Return 1
			End Get
		End Property

		Public Sub SetWidth(allowedSpace As Double, totalPercentage As Double)

			If IsStatic Then
				_element.Width = StaticWidth
			Else
				Dim width As Double = allowedSpace * (Percentage / totalPercentage)

				_element.Width = width
			End If

		End Sub

	End Class

	''' <summary>
	''' ListViewResizeBehavior behavior class that gets attached to the ListView control.
	''' </summary>
	Public Class ListViewResizeBehavior

		Private Const Margin As Integer = 25
		Private Const RefreshTime As Long = Timeout.Infinite
		Private Const Delay As Long = 500
		Private _element As ListView
		Private _timer As Timer

		Public Sub New(element As ListView)

			If element Is Nothing Then
				Throw New ArgumentNullException("element")
			End If

			_element = element
			AddHandler element.Loaded, AddressOf OnLoaded

			' Action for resizing and re-enable the size lookup; this stops the columns from constantly resizing to improve performance
			Dim resizeAndEnableSize As Action = Sub() OnResizeAndEnableSize()

			_timer = New Timer(Function(x) Application.Current.Dispatcher.BeginInvoke(resizeAndEnableSize), Nothing, Delay, RefreshTime)

		End Sub

		Public Property Enabled As Boolean

		Private Sub OnLoaded(sender As Object, e As RoutedEventArgs)

			AddHandler _element.SizeChanged, AddressOf OnSizeChanged

		End Sub

		Private Sub OnResizeAndEnableSize()

			Resize()
			AddHandler _element.SizeChanged, AddressOf OnSizeChanged

		End Sub

		Private Sub OnSizeChanged(sender As Object, e As SizeChangedEventArgs)

			If e.WidthChanged Then
				RemoveHandler _element.SizeChanged, AddressOf OnSizeChanged
				_timer.Change(Delay, RefreshTime)
			End If

		End Sub

		Private Sub Resize()

			If Enabled Then
				Dim totalWidth As Double = _element.ActualWidth
				Dim gv As GridView = CType(_element.View, GridView)

				If Not gv Is Nothing Then
					Dim allowedSpace As Double = totalWidth - GetAllocatedSpace(gv)

					allowedSpace = allowedSpace - Margin
					Dim totalPercentage As Double = GridViewColumnResizeBehaviors(gv).Sum(Function(x) x.Percentage)

					For Each behavior As GridViewColumnResizeBehavior In GridViewColumnResizeBehaviors(gv)
						behavior.SetWidth(allowedSpace, totalPercentage)
					Next
				End If
			End If

		End Sub

		Private Shared Function GridViewColumnResizeBehaviors(gv As GridView) As IEnumerable(Of GridViewColumnResizeBehavior)
			Dim lst As New List(Of GridViewColumnResizeBehavior)

			For Each t As GridViewColumn In gv.Columns
				Dim gridViewColumnResizeBehavior As GridViewColumnResizeBehavior = CType(t.GetValue(GridViewColumnResizeBehaviorProperty), GridViewColumnResizeBehavior)

				If Not GridViewColumnResizeBehavior Is Nothing Then
					lst.Add(gridViewColumnResizeBehavior)
				End If
			Next

			Return lst

		End Function

		Private Shared Function GetAllocatedSpace(gv As GridView) As Double
			Dim totalWidth As Double = 0

			For Each t As GridViewColumn In gv.Columns
				Dim gridViewColumnResizeBehavior As GridViewColumnResizeBehavior = CType(t.GetValue(GridViewColumnResizeBehaviorProperty), GridViewColumnResizeBehavior)

				If Not gridViewColumnResizeBehavior Is Nothing Then
					If gridViewColumnResizeBehavior.IsStatic Then
						totalWidth += gridViewColumnResizeBehavior.StaticWidth
					End If
				Else
					totalWidth += t.ActualWidth
				End If
			Next

			Return totalWidth

		End Function

	End Class

#End Region

End Class