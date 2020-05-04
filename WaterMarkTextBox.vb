Imports System.Windows.Controls

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Public Class WaterMarkTextBox
	Inherits Control

#Region " Objects and variables "

	Private m_Command As RoutedCommand
	Private txtSearch As TextBox

#Region " Dependency properties "

	Public Shared ReadOnly ImageSourceProperty As DependencyProperty = DependencyProperty.Register("ImageSource", GetType(ImageSource), GetType(WaterMarkTextBox))
	Public Shared ReadOnly TextProperty As DependencyProperty = DependencyProperty.Register("Text", GetType(String), GetType(WaterMarkTextBox))
	Public Shared ReadOnly WaterMarkProperty As DependencyProperty = DependencyProperty.Register("WaterMark", GetType(String), GetType(WaterMarkTextBox))

#End Region

#End Region

#Region " Properties "

	''' <summary>
	''' 
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Property Command As RoutedCommand
		Get
			Return m_Command
		End Get
		Set(value As RoutedCommand)
			m_Command = value
		End Set
	End Property

	''' <summary>
	''' 
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Property ImageSource As ImageSource
		Get
			Return CType(GetValue(ImageSourceProperty), ImageSource)
		End Get
		Set(value As ImageSource)
			SetValue(ImageSourceProperty, value)
		End Set
	End Property

	''' <summary>
	''' 
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Property Text As String
		Get
			Return CStr(GetValue(TextProperty))
		End Get
		Set(value As String)
			SetValue(TextProperty, value)
		End Set
	End Property

	''' <summary>
	''' 
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Property WaterMark As String
		Get
			Return CStr(GetValue(WaterMarkProperty))
		End Get
		Set(value As String)
			SetValue(WaterMarkProperty, value)
		End Set
	End Property

#End Region

#Region " Public methods and functions "

	Public Sub Execute()

		If Not m_Command Is Nothing AndAlso Not String.IsNullOrEmpty(txtSearch.Text) AndAlso m_Command.CanExecute(txtSearch.Text, txtSearch) Then
			m_Command.Execute(txtSearch.Text, txtSearch)
		End If

	End Sub

	Public Overrides Sub OnApplyTemplate()

		MyBase.OnApplyTemplate()
		txtSearch = CType(GetTemplateChild("txtSearch"), TextBox)
		AddHandler CType(GetTemplateChild("imgSearch"), Image).MouseDown, AddressOf OnImageMouseDown

	End Sub

#End Region

#Region " Events "

	Protected Overridable Sub OnImageMouseDown(sender As Object, e As MouseEventArgs)

		Execute()

	End Sub

#End Region

#Region " Construction "

	Shared Sub New()

		DefaultStyleKeyProperty.OverrideMetadata(GetType(WaterMarkTextBox), New FrameworkPropertyMetadata(GetType(WaterMarkTextBox)))

	End Sub

#End Region

End Class