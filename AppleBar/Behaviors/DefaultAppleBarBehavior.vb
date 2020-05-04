Imports System.Windows
Imports System.Windows.Media.Animation

''' <summary>
''' Default handler for <see cref="AppleBar" /> behavior.
''' </summary>
Public Class DefaultAppleBarBehavior
	Implements IAppleBarBehavior

#Region " Objects and variables "

	Private m_MouseCaptured As Boolean
	Private m_Sizes As Double() = New Double() {1.0, 0.9, 0.8, 0.7}

#End Region

#Region " Properties "

	Public Property IsBounceEnabled As Boolean = True
	Public Property MaxItemHeight As Double = 40
	Public Property MaxItemWidth As Double = 200

#End Region

#Region " Public methods and functions "

	Public Overridable Sub ApplyMouseDownBehavior(selectedIndex As Integer, element As AppleBarItem) Implements IAppleBarBehavior.ApplyMouseDownBehavior

		If IsBounceEnabled Then
			m_MouseCaptured = element.CaptureMouse()

			Dim da As New DoubleAnimationUsingKeyFrames
			Dim k1 As New SplineDoubleKeyFrame With {.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(100)), .Value = MaxItemHeight * 0.3}
			Dim sb As New Storyboard

			da.KeyFrames.Add(k1)
			Storyboard.SetTarget(da, element)
			Storyboard.SetTargetProperty(da, New PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)"))
			sb.Children.Add(da)
			sb.Begin()
		End If

	End Sub

	Public Overridable Sub ApplyMouseEnterBehavior(proximity As Integer, element As AppleBarItem) Implements IAppleBarBehavior.ApplyMouseEnterBehavior

		If proximity >= m_Sizes.Length OrElse proximity < 0 Then
			proximity = m_Sizes.Length - 1
		End If

		Dim speed As TimeSpan = TimeSpan.FromMilliseconds(100)
		Dim daWidth As New DoubleAnimation With {.To = m_Sizes(proximity) * MaxItemWidth, .Duration = New Duration(speed)}
		Dim daHeight As New DoubleAnimation With {.To = m_Sizes(proximity) * MaxItemHeight, .Duration = New Duration(speed)}
		Dim sb As New Storyboard()

		Storyboard.SetTarget(daWidth, element)
		Storyboard.SetTarget(daHeight, element)
		Storyboard.SetTargetProperty(daHeight, New PropertyPath("Height"))
		Storyboard.SetTargetProperty(daWidth, New PropertyPath("Width"))
		sb.Children.Add(daWidth)
		sb.Children.Add(daHeight)
		Try
			sb.Begin()
		Catch ex As Exception

		End Try

	End Sub

	Public Overridable Sub ApplyMouseLeaveBehavior(element As AppleBarItem) Implements IAppleBarBehavior.ApplyMouseLeaveBehavior
		' do nothing
	End Sub

	Public Overridable Sub ApplyMouseUpBehavior(selectedIndex As Integer, element As AppleBarItem) Implements IAppleBarBehavior.ApplyMouseUpBehavior

		If m_MouseCaptured AndAlso IsBounceEnabled Then
			Dim da As New DoubleAnimationUsingKeyFrames()
			Dim k2 As New SplineDoubleKeyFrame With {.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(100)), .Value = 0}
			Dim k3 As New SplineDoubleKeyFrame With {.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200)), .Value = Me.MaxItemHeight * 0.1}
			Dim k4 As New SplineDoubleKeyFrame With {.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(250)), .Value = 0}
			Dim sb As New Storyboard

			da.KeyFrames.Add(k2)
			da.KeyFrames.Add(k3)
			da.KeyFrames.Add(k4)

			Storyboard.SetTarget(da, element)
			Storyboard.SetTargetProperty(da, New PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)"))
			sb.Children.Add(da)
			sb.Begin()

			element.ReleaseMouseCapture()
			m_MouseCaptured = False
		End If

	End Sub

	Public Overridable Sub Initialize(parent As AppleBar, element As AppleBarItem) Implements IAppleBarBehavior.Initialize

		element.Height = MaxItemHeight * m_Sizes(m_Sizes.Length - 1)
		element.Width = MaxItemWidth * m_Sizes(m_Sizes.Length - 1)

	End Sub

#End Region

#Region " Construction and Destruction "

	Public Sub New()
	End Sub

#End Region

End Class