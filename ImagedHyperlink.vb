
''' <summary>
''' Hyperlink control, which consists of a small image, followed by a hyperlink.
''' </summary>
Public Class ImagedHyperlink
	Inherits Control

#Region " Objects and variables "

#Region " Dependency properties "

	Public Shared ReadOnly CommandProperty As DependencyProperty = DependencyProperty.Register("Command", GetType(ICommand), GetType(ImagedHyperlink))
	Public Shared ReadOnly CommandParameterProperty As DependencyProperty = DependencyProperty.Register("CommandParameter", GetType(Object), GetType(ImagedHyperlink))
	Public Shared ReadOnly ImageSourceProperty As DependencyProperty = DependencyProperty.Register("ImageSource", GetType(ImageSource), GetType(ImagedHyperlink))
	Public Shared ReadOnly ImageStyleProperty As DependencyProperty = DependencyProperty.Register("ImageStyle", GetType(Style), GetType(ImagedHyperlink))
	Public Shared ReadOnly LinkStyleProperty As DependencyProperty = DependencyProperty.Register("LinkStyle", GetType(Style), GetType(ImagedHyperlink))
	Public Shared ReadOnly TextProperty As DependencyProperty = DependencyProperty.Register("Text", GetType(String), GetType(ImagedHyperlink), New PropertyMetadata(String.Empty))

#End Region

#End Region

#Region " Properties "

	Public Property Command As ICommand
		Get
			Return CType(GetValue(CommandProperty), ICommand)
		End Get
		Set(value As ICommand)
			SetValue(CommandProperty, value)
		End Set
	End Property

	Public Property CommandParameter As Object
		Get
			Return CType(GetValue(CommandParameterProperty), Object)
		End Get
		Set(value As Object)
			SetValue(CommandParameterProperty, value)
		End Set
	End Property

	Public Property ImageSource As ImageSource
		Get
			Return CType(GetValue(ImageSourceProperty), ImageSource)
		End Get
		Set(value As ImageSource)
			SetValue(ImageSourceProperty, value)
		End Set
	End Property

	Public Property ImageStyle As Style
		Get
			Return CType(GetValue(ImageStyleProperty), Style)
		End Get
		Set(value As Style)
			SetValue(ImageStyleProperty, value)
		End Set
	End Property

	Public Property LinkStyle As Style
		Get
			Return CType(GetValue(LinkStyleProperty), Style)
		End Get
		Set(value As Style)
			SetValue(LinkStyleProperty, value)
		End Set
	End Property

	Public Property Text As String
		Get
			Return CStr(GetValue(TextProperty))
		End Get
		Set(value As String)
			SetValue(TextProperty, value)
		End Set
	End Property

#End Region

#Region " Construction "

	Shared Sub New()

		DefaultStyleKeyProperty.OverrideMetadata(GetType(ImagedHyperlink), New FrameworkPropertyMetadata(GetType(ImagedHyperlink)))

	End Sub

#End Region

End Class