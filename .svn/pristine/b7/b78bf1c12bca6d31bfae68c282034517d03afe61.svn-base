Imports System.Windows.Controls.Primitives

''' <summary>
''' Numeric UpDown control, used for user input of numeric values.
''' </summary>
Public Class NumericUpDown
	Inherits Control

#Region " Objects and variables "

#Region " Fields "

	Private box As TextBox
	Private _rememberedValue As Integer = 10
	Private _Updated As Boolean
	Public Delegate Sub NumericUpDownChangedEventHandler(ByVal sender As Object, ByVal e As NumericUpdownChangedEventArgs)

#End Region

#Region " Dependency properties "

	Public Shared MaximumProperty As DependencyProperty = DependencyProperty.Register("Maximum", GetType(Integer), GetType(NumericUpDown), New PropertyMetadata(10, New PropertyChangedCallback(AddressOf MaximumValueChanged)))
	Public Shared MinimumProperty As DependencyProperty = DependencyProperty.Register("Minimum", GetType(Integer), GetType(NumericUpDown), New PropertyMetadata(1, New PropertyChangedCallback(AddressOf MinimumValueChanged)))
	Public Shared ValueProperty As DependencyProperty = DependencyProperty.Register("Value", GetType(Integer), GetType(NumericUpDown), New PropertyMetadata(10, New PropertyChangedCallback(AddressOf ValueChanged)))

#End Region

#Region " Events "

	Public Event NumericUpDownChanged As NumericUpDownChangedEventHandler

#End Region

#End Region

#Region " Properties "

	''' <summary>
	''' Maximum value of the control.
	''' </summary>
	''' <value>Integer</value>
	''' <returns>Integer</returns>
	Public Property Maximum() As Integer
		Get
			Return CInt(GetValue(MaximumProperty))
		End Get
		Set(ByVal value As Integer)
			SetValue(MaximumProperty, value)
		End Set
	End Property

	''' <summary>
	''' Minimum value of the control.
	''' </summary>
	''' <value>Integer</value>
	''' <returns>Integer</returns>
	Public Property Minimum() As Integer
		Get
			Return CInt(GetValue(MinimumProperty))
		End Get
		Set(ByVal value As Integer)
			SetValue(MinimumProperty, value)
		End Set
	End Property

	''' <summary>
	''' Current value of the control.
	''' </summary>
	''' <value>Integer</value>
	''' <returns>Integer</returns>
	Public Property Value() As Integer
		Get
			Return CInt(GetValue(ValueProperty))
		End Get
		Set(ByVal value As Integer)
			SetValue(ValueProperty, value)
		End Set
	End Property

#End Region

#Region " Public methods and functions "

	Public Sub UpdateValue(ByVal val As Integer)

		If Not Value = val Then
			_Updated = False
			Value = val	' if all is well, the _Updated field will be set to true

			If _Updated Then
				box.Text = CStr(val)

				RaiseEvent NumericUpDownChanged(Me, New NumericUpdownChangedEventArgs(Value))
			End If
		End If

	End Sub

#End Region

#Region " Private methods and functions "

	Private Shared Sub MaximumValueChanged(ByVal sender As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
		Dim nud As NumericUpDown = CType(sender, NumericUpDown)
		Dim val As Integer = CInt(e.NewValue)

		If Not nud Is Nothing Then
			If val <= nud.Minimum Then
				nud.Maximum = CInt(e.OldValue)
			End If
			nud.Value = Math.Min(nud.Value, nud.Maximum)
		End If

	End Sub

	Private Shared Sub MinimumValueChanged(ByVal sender As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
		Dim nud As NumericUpDown = CType(sender, NumericUpDown)
		Dim val As Integer = CInt(e.NewValue)

		If Not nud Is Nothing Then
			If val >= nud.Maximum Then
				nud.Minimum = CInt(e.OldValue)
			End If
			nud.Value = Math.Max(nud.Value, nud.Minimum)
		End If

	End Sub

	Public Overrides Sub OnApplyTemplate()

		MyBase.OnApplyTemplate()

		box = CType(MyBase.GetTemplateChild("NumericTextBox"), TextBox)
		Dim btnUp As Button = CType(MyBase.GetTemplateChild("ValUp"), Button)
		Dim btnDown As Button = CType(MyBase.GetTemplateChild("ValDown"), Button)

		If box Is Nothing Then Return
		If btnUp Is Nothing Then Return
		If btnDown Is Nothing Then Return

		box.Text = CStr(_rememberedValue)

		AddHandler box.TextChanged, AddressOf txt_TextChanged
		AddHandler box.KeyDown, AddressOf txt_KeyDown
		AddHandler btnUp.Click, AddressOf btnUp_Click
		AddHandler btnDown.Click, AddressOf btnDown_Click

	End Sub

	Private Sub RememberValue(val As Integer)

		_rememberedValue = val

	End Sub

	Private Shared Sub ValueChanged(ByVal sender As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
		Dim nud As NumericUpDown = CType(sender, NumericUpDown)
		Dim val As Integer = CInt(e.NewValue)

		If Not nud Is Nothing Then
			nud.Value = Math.Max(Math.Min(val, nud.Maximum), nud.Minimum)
		End If
		nud._Updated = (Not nud.Value = CInt(e.OldValue))
		If nud._Updated Then
			nud.RememberValue(nud.Value)
		End If

	End Sub

#End Region

#Region " Events "

	Private Sub txt_TextChanged(ByVal sender As Object, ByVal e As TextChangedEventArgs)
		Dim result As Integer

		If Integer.TryParse(box.Text, result) Then
			UpdateValue(result)
		End If

	End Sub

	Private Sub txt_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)

		If ((e.Key >= Key.D0 And e.Key <= Key.D9) OrElse (e.Key >= Key.NumPad0 And e.Key <= Key.NumPad9) OrElse e.Key = Key.Back) Then
			e.Handled = False
		Else
			e.Handled = True
		End If

	End Sub

	Private Sub btnUp_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)

		UpdateValue(Value + 1)

	End Sub

	Private Sub btnDown_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)

		UpdateValue(Value - 1)

	End Sub

#End Region

#Region " Construction "

	Public Sub New()

		Me.DefaultStyleKey = GetType(NumericUpDown)
		_Updated = False

	End Sub

#End Region

End Class