Imports System.Windows
Imports System.Windows.Media.Animation

''' <summary>
''' Templated 'Loading...' indicator control.
''' </summary>
''' <remarks>
''' Adapted from code by Jevgenij Pankov (http://www.codeproject.com/script/Membership/View.aspx?mid=43107).
''' See http://www.codeproject.com/KB/silverlight/LoadingIndicator.aspx .
''' </remarks>
<TemplatePart(Name:="PART_AnimationElementContainer", Type:=GetType(Canvas)), TemplatePart(Name:="PART_AnimationElement", Type:=GetType(FrameworkElement))>
Public Class LoadingIndicator
	Inherits Control

#Region " Objects and variables "

	Private Const _DefaultDurationInSeconds As Double = 1
	Private Const _DefaultCount As Integer = 12
	Private Const _EndOpacity As Double = 0.1
	Private Const PART_AnimationElement As String = "PART_AnimationElement"
	Private Const PART_AnimationElementContainer As String = "PART_AnimationElementContainer"

	Private m_AnimationElement As FrameworkElement
	Private m_AnimationElementContainer As Canvas
	Private m_InnerRadius As Double
	Private ReadOnly m_StoryBoards As List(Of Storyboard)

#End Region

#Region " Properties "

#Region " AnimationElementTemplate "

	''' <summary>
	''' Identifies the <see cref="LoadingIndicator.AnimationElementTemplate" /> dependency property.
	''' </summary>
	Public Shared ReadOnly AnimationElementTemplateProperty As DependencyProperty = DependencyProperty.Register("AnimationElementTemplate", GetType(DataTemplate), GetType(LoadingIndicator), New PropertyMetadata(Nothing))

	''' <summary>
	''' Gets or sets AnimationElementTemplate.
	''' </summary>
	Public Property AnimationElementTemplate As DataTemplate
		Get
			Return CType(GetValue(AnimationElementTemplateProperty), DataTemplate)
		End Get
		Set(value As DataTemplate)
			SetValue(AnimationElementTemplateProperty, value)
		End Set
	End Property

#End Region

#Region " ControlVisibility "

	''' <summary>
	''' Identifies the <see cref="LoadingIndicator.ControlVisibility" /> dependency property.
	''' </summary>
	Private Shared ReadOnly ControlVisibilityProperty As DependencyProperty = DependencyProperty.Register("ControlVisibility", GetType(Visibility), GetType(LoadingIndicator), New PropertyMetadata(Visibility.Visible, New PropertyChangedCallback(AddressOf OnControlVisibilityPropertyChanged)))

	''' <summary>
	''' Gets or sets Control Visibility.
	''' </summary>
	Private Property ControlVisibility As Boolean
		Get
			Return CBool(GetValue(ControlVisibilityProperty))
		End Get
		Set(value As Boolean)
			SetValue(ControlVisibilityProperty, value)
		End Set
	End Property

	''' <summary>
	''' Stop animation when control becomes collapsed and create it anew - when visible.
	''' </summary>
	''' <param name="d">LoadingIndicator object whose ControlVisibility property is changed.</param>
	''' <param name="e">DependencyPropertyChangedEventArgs which contains the old and new values.</param>
	Private Shared Sub OnControlVisibilityPropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
		Dim ctl As LoadingIndicator = CType(d, LoadingIndicator)
		Dim visibility As Visibility = CType(e.NewValue, Visibility)

		If Not ctl.m_AnimationElementContainer Is Nothing Then
			If visibility = visibility.Collapsed Then
				ctl.StopAnimation()
			Else
				ctl.StartAnimation()
			End If
		End If

	End Sub

#End Region

#Region " Count "

	''' <summary>
	''' Identifies the <see cref="LoadingIndicator.Count" /> dependency property.
	''' </summary>
	Public Shared ReadOnly CountProperty As DependencyProperty = DependencyProperty.Register("Count", GetType(Integer), GetType(LoadingIndicator), New PropertyMetadata(_DefaultCount, New PropertyChangedCallback(AddressOf OnCountPropertyChanged)))

	''' <summary>
	''' Gets or sets Count of animated elements.
	''' </summary>
	Public Property Count As Integer
		Get
			Return CInt(GetValue(CountProperty))
		End Get
		Set(value As Integer)
			SetValue(CountProperty, value)
		End Set
	End Property

	''' <summary>
	''' Check Count property and redraw control with new parameters.
	''' </summary>
	''' <param name="d">LoadingIndicator object whose Count property is changed.</param>
	''' <param name="e">DependencyPropertyChangedEventArgs which contains the old and new values.</param>
	Private Shared Sub OnCountPropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
		Dim ctl As LoadingIndicator = CType(d, LoadingIndicator)
		Dim count As Integer = CInt(e.NewValue)

		If count <= 0 Then
			ctl.Count = _DefaultCount
		End If
		ctl.CreateAnimation()

	End Sub

#End Region

#Region " Duration "

	''' <summary>
	''' Identifies the <see cref="LoadingIndicator.Duration" /> dependency property.
	''' </summary>
	Public Shared ReadOnly DurationProperty As DependencyProperty = DependencyProperty.Register("Duration", GetType(TimeSpan), GetType(LoadingIndicator), New PropertyMetadata(TimeSpan.FromSeconds(_DefaultDurationInSeconds), New PropertyChangedCallback(AddressOf OnDurationPropertyChanged)))

	''' <summary>
	''' Gets or sets the duration of one animation cycle.
	''' </summary>
	Public Property Duration As TimeSpan
		Get
			Return CType(GetValue(DurationProperty), TimeSpan)
		End Get
		Set(value As TimeSpan)
			SetValue(DurationProperty, value)
		End Set
	End Property

	''' <summary>
	''' Called when the <see cref="Duration" /> property is changed.
	''' </summary>
	''' <param name="d">LoadingIndicator object whose <see cref="Duration" /> property is changed.</param>
	''' <param name="e">DependencyPropertyChangedEventArgs which contains the old and new values.</param>
	Private Shared Sub OnDurationPropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
		Dim ctl As LoadingIndicator = CType(d, LoadingIndicator)

		ctl.CreateAnimation()

	End Sub

#End Region

#End Region

#Region " Public methods and functions "

	''' <summary>
	''' Builds the visual tree when a new template is applied.
	''' </summary>
	Public Overrides Sub OnApplyTemplate()

		MyBase.OnApplyTemplate()
		m_AnimationElementContainer = CType(GetTemplateChild(PART_AnimationElementContainer), Canvas)

		If m_AnimationElementContainer Is Nothing Then
			Throw New NotImplementedException("Template PART_AnimationElementContainer is required to display LoadingIndicator.")
		End If
		m_AnimationElement = CType(AnimationElementTemplate.LoadContent(), FrameworkElement)
		If m_AnimationElement Is Nothing Then
			Throw New NotImplementedException("Template PART_AnimationElement is required to display LoadingIndicator.")
		End If
		' calculate inner radius of the indicator
		Dim outerRadius As Double = Math.Min(Width, Height)

		m_InnerRadius = outerRadius / 2 - Math.Max(m_AnimationElement.Width, m_AnimationElement.Height)

		CreateAnimation()

	End Sub

	''' <summary>
	''' Starts the animation.
	''' </summary>
	Public Sub StartAnimation()

		If m_StoryBoards.Count = 0 Then
			CreateAnimation()
		Else
			m_StoryBoards.ForEach(Sub(x) x.Begin())
		End If

	End Sub

	''' <summary>
	''' Stops the animation.
	''' </summary>
	Public Sub StopAnimation()

		m_StoryBoards.ForEach(Sub(x) x.Stop())

	End Sub

#End Region

#Region " Private methods and functions "

	''' <summary>
	''' Copy the base animation element <see cref="Count" /> times and start the animation.
	''' </summary>
	Private Sub CreateAnimation()

		If Not m_AnimationElementContainer Is Nothing Then
			Dim binding As New Data.Binding() With {.Source = Me, .Path = New PropertyPath("Visibility")}
			Dim angle As Double = 360.0 / Count
			Dim width As Double = m_AnimationElement.Width
			Dim x As Double = (Me.Width - width) / 2
			Dim y As Double = Height / 2 + m_InnerRadius

			m_AnimationElementContainer.Children.Clear()
			m_StoryBoards.Clear()

			For i As Integer = 0 To Count - 1
				' Copy base element
				Dim element As FrameworkElement = CType(AnimationElementTemplate.LoadContent(), FrameworkElement)

				element.Opacity = 0

				Dim tt As New TranslateTransform() With {.X = x, .Y = y}
				Dim rt As New RotateTransform() With {.Angle = i * angle + 180, .CenterX = (width / 2), .CenterY = -m_InnerRadius}
				Dim tg As New TransformGroup()

				tg.Children.Add(rt)
				tg.Children.Add(tt)
				element.RenderTransform = tg

				m_AnimationElementContainer.Children.Add(element)

				Dim Animation As New DoubleAnimation() With {.From = m_AnimationElement.Opacity, .To = _EndOpacity, .Duration = Duration, .RepeatBehavior = RepeatBehavior.Forever, .BeginTime = TimeSpan.FromMilliseconds((Duration.TotalMilliseconds / Count) * i)}

				Storyboard.SetTargetProperty(Animation, New PropertyPath("Opacity"))
				Storyboard.SetTarget(Animation, element)

				Dim sb As New Storyboard()
				sb.Children.Add(Animation)
				sb.Begin()

				m_StoryBoards.Add(sb)
			Next

			' bind ControlVisibilityProperty to the Visibility property in order to catch the missing OnVisibilityChanged event
			SetBinding(LoadingIndicator.ControlVisibilityProperty, binding)
		End If

	End Sub

#End Region

#Region " Construction "

	''' <summary>
	''' Initializes a new instance of a <see cref="LoadingIndicator"/> object.
	''' </summary>
	Public Sub New()

		DefaultStyleKey = GetType(LoadingIndicator)
		m_StoryBoards = New List(Of Storyboard)

	End Sub

#End Region

End Class