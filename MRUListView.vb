Imports BlueHeron.MRU
Imports System.Windows.Controls

''' <summary>
''' Custom <see cref="ListView" /> control that maintains a list of MRU items
''' and stores them in the registry of the current user under the name of the current executing assembly.
''' </summary>
Public Class MRUListView
	Inherits Control

#Region " Objects and variables "

	Private m_MRUList As MruList

	Public Shared ReadOnly ItemsProperty As DependencyProperty = DependencyProperty.Register("Items", GetType(IEnumerable(Of MruItem)), GetType(MRUListView))
	Public Shared DeleteItem As New RoutedCommand("DeleteItem", GetType(MRUListView))
	Public Shared OpenItem As New RoutedCommand("OpenItem", GetType(MRUListView))

	Public Event ItemClicked As EventHandler(Of MruEventArgs)
	Public Event ItemSelected As EventHandler(Of MruEventArgs)

#End Region

#Region " Properties "

	Public Property Items As IEnumerable(Of MruItem)
		Get
			Return CType(GetValue(ItemsProperty), IEnumerable(Of MruItem))
		End Get
		Private Set(value As IEnumerable(Of MruItem))
			SetValue(ItemsProperty, value)
		End Set
	End Property

	Public ReadOnly Property MRUList As MruList
		Get
			Return m_MRUList
		End Get
	End Property

#End Region

#Region " Public methods and functions "

	Public Overridable Sub Add(newPath As String)

		m_MRUList.Add(newPath)

	End Sub

	Public Overridable Sub Clear()

		m_MRUList.Clear()

	End Sub

	Public Overridable Sub Delete(path As String)

		m_MRUList.Delete(path)

	End Sub

	Public Overridable Sub Load()

		m_MRUList.Load()

	End Sub

	Public Overridable Sub Save()

		m_MRUList.Save()

	End Sub

	Public Overrides Sub OnApplyTemplate()

		MyBase.OnApplyTemplate()
		Dim vw As ListView = CType(GetTemplateChild("PART_ItemsControl"), ListView)

		AddHandler vw.SelectionChanged, AddressOf OnSelectionChanged

	End Sub

#End Region

#Region " Private methods and functions "

	Private Sub OnSelectionChanged(sender As Object, e As SelectionChangedEventArgs)

		If e.AddedItems.Count > 0 Then
			Dim mi As MruItem = CType(e.AddedItems(0), MruItem)

			RaiseEvent ItemSelected(Me, New MruEventArgs(mi, ListAction.Select))
		End If

	End Sub

#End Region

#Region " Events "

	Public Overridable Sub OnDeleteItem(sender As Object, e As ExecutedRoutedEventArgs)

		Delete(CStr(e.Parameter))

	End Sub

	Public Overridable Sub OnCanDelete(sender As Object, e As CanExecuteRoutedEventArgs)

		e.CanExecute = True

	End Sub

	Public Overridable Sub OnOpenItem(sender As Object, e As ExecutedRoutedEventArgs)
		Dim p As String = CStr(e.Parameter)
		Dim mi As MruItem = m_MRUList.Entries.Single(Function(i) i.Path = p)

		RaiseEvent ItemClicked(Me, New MruEventArgs(mi, ListAction.Select))

	End Sub

	Public Overridable Sub OnCanOpen(sender As Object, e As CanExecuteRoutedEventArgs)

		e.CanExecute = True

	End Sub

	Private Sub m_MRUList_ListChanged(sender As Object, e As MRU.MruEventArgs)

		Items = Nothing
		Items = m_MRUList.Entries

	End Sub

#End Region

#Region " Construction "

	Shared Sub New()

		DefaultStyleKeyProperty.OverrideMetadata(GetType(MRUListView), New FrameworkPropertyMetadata(GetType(MRUListView)))

	End Sub

	Public Sub New()
		Dim bindingDelete = New CommandBinding(DeleteItem, AddressOf OnDeleteItem, AddressOf OnCanDelete)
		Dim bindingOpen = New CommandBinding(OpenItem, AddressOf OnOpenItem, AddressOf OnCanOpen)

		CommandBindings.Add(bindingDelete)
		CommandBindings.Add(bindingOpen)
		m_MRUList = New MruList
		AddHandler m_MRUList.ListChanged, AddressOf m_MRUList_ListChanged

	End Sub

#End Region

End Class