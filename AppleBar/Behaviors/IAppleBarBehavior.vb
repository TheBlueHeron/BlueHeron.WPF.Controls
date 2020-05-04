
''' <summary>
''' Interface definition for objects that define mouse behavior for the <see cref="AppleBar" /> and its <see cref="AppleBarItem">items</see>.
''' </summary>
Public Interface IAppleBarBehavior

	''' <summary>
	''' Intializes each element in the menu.
	''' </summary>
	''' <param name="parent">The menu containing the element.</param>
	''' <param name="element">The element to initialize.</param>
	Sub Initialize(parent As AppleBar, element As AppleBarItem)

	''' <summary>
	''' Fired on each element when the mouse enters an element's region.
	''' </summary>
	''' <param name="proximity">Indicates how close the mouse is to the current element.</param>
	''' <param name="element">The element of concern.</param>
	Sub ApplyMouseEnterBehavior(proximity As Integer, element As AppleBarItem)

	''' <summary>
	''' Fired on each element when the mouse leaves an element's region.
	''' </summary>
	''' <param name="element">The element of concern.</param>
	Sub ApplyMouseLeaveBehavior(element As AppleBarItem)

	''' <summary>
	''' Fired when the left mouse button is clicked on a particular element.
	''' </summary>
	''' <param name="selectedIndex">The index of the selected element.</param>
	''' <param name="element">The element of concern.</param>
	Sub ApplyMouseDownBehavior(selectedIndex As Integer, element As AppleBarItem)

	''' <summary>
	''' Fired when the left mouse button is lifted on a particular element.
	''' </summary>
	''' <param name="selectedIndex">The index of the selected element.</param>
	''' <param name="element">The element of concern.</param>
	Sub ApplyMouseUpBehavior(selectedIndex As Integer, element As AppleBarItem)

End Interface