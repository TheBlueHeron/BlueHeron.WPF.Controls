Imports System.Windows.Controls

''' <summary>
''' Modal Dialog control.
''' </summary>
''' <remarks>Place this control at the bottom of any .xaml</remarks>
Public Class ModalDialog
	Inherits Control

#Region " Objects and variables "

	Private m_Overlay As Rectangle
	Private m_OverlayAnim As Animation.Storyboard

#Region " Routed events "

	Public Shared ReadOnly CancelClickEvent As RoutedEvent = EventManager.RegisterRoutedEvent("CancelClick", RoutingStrategy.Bubble, GetType(RoutedEventHandler), GetType(ModalDialog))
	Public Shared ReadOnly OKClickEvent As RoutedEvent = EventManager.RegisterRoutedEvent("OKClick", RoutingStrategy.Bubble, GetType(RoutedEventHandler), GetType(ModalDialog))

	Public Custom Event CancelClick As RoutedEventHandler
		AddHandler(ByVal value As RoutedEventHandler)
			MyBase.AddHandler(CancelClickEvent, value)
		End AddHandler
		RemoveHandler(ByVal value As RoutedEventHandler)
			MyBase.RemoveHandler(CancelClickEvent, value)
		End RemoveHandler
		RaiseEvent(sender As Object, e As RoutedEventArgs)
		End RaiseEvent
	End Event

	Public Custom Event OKClick As RoutedEventHandler
		AddHandler(ByVal value As RoutedEventHandler)
			MyBase.AddHandler(OKClickEvent, value)
		End AddHandler
		RemoveHandler(ByVal value As RoutedEventHandler)
			MyBase.RemoveHandler(OKClickEvent, value)
		End RemoveHandler
		RaiseEvent(sender As Object, e As RoutedEventArgs)
		End RaiseEvent
	End Event

#End Region

#Region " Dependency properties "

	Public Shared ReadOnly ContentProperty As DependencyProperty = DependencyProperty.Register("Content", GetType(Object), GetType(ModalDialog))
	Public Shared ReadOnly CornerRadiusProperty As DependencyProperty = DependencyProperty.Register("CornerRadius", GetType(CornerRadius), GetType(ModalDialog), New PropertyMetadata(New CornerRadius(6)))
	Public Shared ReadOnly DialogHeightProperty As DependencyProperty = DependencyProperty.Register("DialogHeight", GetType(Double), GetType(ModalDialog), New PropertyMetadata(200.0))
	Public Shared ReadOnly DialogWidthProperty As DependencyProperty = DependencyProperty.Register("DialogWidth", GetType(Double), GetType(ModalDialog), New PropertyMetadata(200.0))
	Public Shared ReadOnly HeaderBackgroundProperty As DependencyProperty = DependencyProperty.Register("HeaderBackground", GetType(Brush), GetType(ModalDialog), New PropertyMetadata(New SolidColorBrush(Colors.DarkBlue)))
	Public Shared ReadOnly HeaderImageProperty As DependencyProperty = DependencyProperty.Register("HeaderImage", GetType(ImageSource), GetType(ModalDialog))
	Public Shared ReadOnly HeaderTextProperty As DependencyProperty = DependencyProperty.Register("HeaderText", GetType(String), GetType(ModalDialog), New PropertyMetadata(GetType(ModalDialog).ToString))
	Public Shared ReadOnly HeaderTextStyleProperty As DependencyProperty = DependencyProperty.Register("HeaderTextStyle", GetType(Style), GetType(ModalDialog))
	Public Shared ReadOnly IsModalProperty As DependencyProperty = DependencyProperty.Register("IsModal", GetType(Boolean), GetType(ModalDialog), New PropertyMetadata(True))
	Public Shared ReadOnly OverlayBrushProperty As DependencyProperty = DependencyProperty.Register("OverlayBrush", GetType(Brush), GetType(ModalDialog), New PropertyMetadata(New SolidColorBrush(Colors.Black)))
	Public Shared ReadOnly OverlayOpacityProperty As DependencyProperty = DependencyProperty.Register("OverlayOpacity", GetType(Double), GetType(ModalDialog), New PropertyMetadata(0.67))

#End Region

#End Region

#Region " Properties "

	''' <summary>
	''' Content of the dialog body.
	''' </summary>
	''' <value><see cref="Object" /></value>
	''' <returns><see cref="Object" /></returns>
	Public Property Content As Object
		Get
			Return CType(GetValue(ContentProperty), Object)
		End Get
		Set(value As Object)
			SetValue(ContentProperty, value)
		End Set
	End Property

	''' <summary>
	''' Radius of the dialog's corners.
	''' </summary>
	''' <value><see cref="CornerRadius" /></value>
	''' <returns><see cref="CornerRadius" /></returns>
	Public Property CornerRadius As CornerRadius
		Get
			Return CType(GetValue(CornerRadiusProperty), CornerRadius)
		End Get
		Set(value As CornerRadius)
			SetValue(CornerRadiusProperty, value)
		End Set
	End Property

	''' <summary>
	''' Height of the dialog.
	''' </summary>
	''' <value>Double</value>
	''' <returns>Double</returns>
	''' <remarks>The height of the overlay is set through the control's <see cref="Height" /> property.</remarks>
	Public Property DialogHeight As Double
		Get
			Return CDbl(GetValue(DialogHeightProperty))
		End Get
		Set(value As Double)
			SetValue(DialogHeightProperty, value)
		End Set
	End Property

	''' <summary>
	''' Width of the dialog.
	''' </summary>
	''' <value>Double</value>
	''' <returns>Double</returns>
	''' <remarks>The width of the overlay is set through the control's <see cref="Width" /> property.</remarks>
	Public Property DialogWidth As Double
		Get
			Return CDbl(GetValue(DialogWidthProperty))
		End Get
		Set(value As Double)
			SetValue(DialogWidthProperty, value)
		End Set
	End Property

	''' <summary>
	''' Background brush for the caption bar of the dialog.
	''' </summary>
	''' <value><see cref="Brush" /></value>
	''' <returns><see cref="Brush" /></returns>
	Public Property HeaderBackground As Brush
		Get
			Return CType(GetValue(HeaderBackgroundProperty), Brush)
		End Get
		Set(value As Brush)
			SetValue(HeaderBackgroundProperty, value)
		End Set
	End Property

	''' <summary>
	''' Source for the (16x16) image in the caption bar of the dialog.
	''' </summary>
	''' <value><see cref="ImageSource" /></value>
	''' <returns><see cref="ImageSource" /></returns>
	Public Property HeaderImage As ImageSource
		Get
			Return CType(GetValue(HeaderImageProperty), ImageSource)
		End Get
		Set(value As ImageSource)
			SetValue(HeaderImageProperty, value)
		End Set
	End Property

	''' <summary>
	''' Text content for the caption bar of the dialog.
	''' </summary>
	''' <value>String</value>
	''' <returns>String</returns>
	Public Property HeaderText As String
		Get
			Return CStr(GetValue(HeaderTextProperty))
		End Get
		Set(value As String)
			SetValue(HeaderTextProperty, value)
		End Set
	End Property

	''' <summary>
	''' Text style for the caption bar of the dialog.
	''' </summary>
	''' <value><see cref="Style" /></value>
	''' <returns><see cref="Style" /></returns>
	Public Property HeaderTextStyle As Style
		Get
			Return CType(GetValue(HeaderTextStyleProperty), Style)
		End Get
		Set(value As Style)
			SetValue(HeaderTextStyleProperty, value)
		End Set
	End Property

	''' <summary>
	''' Gets or sets a boolean, determining whether this dialog blocks the rest of the application.
	''' </summary>
	''' <value>Boolean</value>
	''' <returns>Boolean</returns>
	Public Property IsModal As Boolean
		Get
			Return CBool(GetValue(IsModalProperty))
		End Get
		Set(value As Boolean)
			SetValue(IsModalProperty, value)
		End Set
	End Property

	''' <summary>
	''' Returns whether the dialog is currently visible.
	''' </summary>
	''' <returns>Boolean</returns>
	Public ReadOnly Property IsOpen As Boolean
		Get
			Return (Visibility = Windows.Visibility.Visible)
		End Get
	End Property

	''' <summary>
	''' <see cref="Brush" /> to fill the overlay that blocks the controls behind the modal dialog.
	''' </summary>
	''' <value><see cref="Brush" /></value>
	''' <returns><see cref="Brush" /></returns>
	Public Property OverlayBrush As Brush
		Get
			Return CType(GetValue(OverlayBrushProperty), Brush)
		End Get
		Set(value As Brush)
			SetValue(OverlayBrushProperty, value)
		End Set
	End Property

	''' <summary>
	''' Opacity of the overlay that blocks the controls behind the modal dialog.
	''' </summary>
	''' <value>Double</value>
	''' <returns>Double</returns>
	Public Property OverlayOpacity As Double
		Get
			Return CDbl(GetValue(OverlayOpacityProperty))
		End Get
		Set(value As Double)
			SetValue(OverlayOpacityProperty, value)
		End Set
	End Property

#End Region

#Region " Public methods and functions "

	Public Overrides Sub OnApplyTemplate()

		MyBase.OnApplyTemplate()

		Dim btnCancel As DependencyObject = GetTemplateChild("btnCancel")
		Dim btnOK As DependencyObject = GetTemplateChild("btnOK")

		If Not btnCancel Is Nothing Then
			AddHandler CType(btnCancel, Button).Click, AddressOf CancelButton_Click
		End If
		If Not btnOK Is Nothing Then
			AddHandler CType(btnOK, Button).Click, AddressOf OKButton_Click
		End If

		m_OverlayAnim = CType(Template.Resources("OverlayAnimation"), Animation.Storyboard)
		m_Overlay = CType(GetTemplateChild("ModalOverlay"), Rectangle)

		' trying to set animation 'To' property here, because binding Animation values inside a ControlTemplate is not supported.
		' see: http://msdn.microsoft.com/en-us/library/ms742868.aspx
		'Dim da As Animation.DoubleAnimation = CType(m_OverlayAnim.Children(1), Animation.DoubleAnimation)

		'da.To = OverlayOpacity ' error, it's already frozen...

		If Visibility = Windows.Visibility.Hidden Then
			Visibility = Windows.Visibility.Collapsed
		End If

	End Sub

	Public Sub Hide()

		Visibility = Windows.Visibility.Collapsed
		If IsModal Then
			HideOverlay()
		End If

	End Sub

	Public Sub Show()

		Visibility = Windows.Visibility.Visible
		If IsModal Then
			ShowOverlay()
		End If

	End Sub

#End Region

#Region " Private methods and functions "

	Private Sub ShowOverlay()

		If Not m_Overlay Is Nothing Then
			m_OverlayAnim.Begin(m_Overlay)
		End If

	End Sub

	Private Sub HideOverlay()

		m_OverlayAnim.Stop(m_Overlay)
		m_Overlay.SetCurrentValue(Rectangle.OpacityProperty, 0.0)

	End Sub

#End Region

#Region " Events "

	Private Sub CancelButton_Click(sender As Object, e As RoutedEventArgs)

		MyBase.RaiseEvent(New RoutedEventArgs(CancelClickEvent))

	End Sub

	Private Sub OKButton_Click(sender As Object, e As RoutedEventArgs)

		MyBase.RaiseEvent(New RoutedEventArgs(OKClickEvent))

	End Sub

#End Region

#Region " Construction "

	Shared Sub New()

		DefaultStyleKeyProperty.OverrideMetadata(GetType(ModalDialog), New FrameworkPropertyMetadata(GetType(ModalDialog)))

	End Sub

	Public Sub New()
	End Sub

#End Region

End Class