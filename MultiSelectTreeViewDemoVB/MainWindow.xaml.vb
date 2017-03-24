Imports MultiSelectTreeViewDemoVB.ViewModels

Partial Public Class MainWindow
    Inherits Window
    Public Sub New()
        InitializeComponent()
        ShowSecondCheck_Checked(Nothing, Nothing)

        '' Create some example nodes to play with
        'Dim rootNode As New TreeItemViewModel(Nothing, False) With {
        '    .DisplayName = "rootNode"
        '}
        'Dim node1 As New TreeItemViewModel(rootNode, False) With {
        '    .DisplayName = "element1 (editable)",
        '    .IsEditable = True
        '}
        'Dim node2 As New TreeItemViewModel(rootNode, False) With {
        '    .DisplayName = "element2"
        '}
        'Dim node11 As New TreeItemViewModel(node1, False) With {
        '    .DisplayName = "element11",
        '    .Remarks = "Look at me!"
        '}
        'Dim node12 As New TreeItemViewModel(node1, False) With {
        '    .DisplayName = "element12 (disabled)",
        '    .IsEnabled = False
        '}
        'Dim node13 As New TreeItemViewModel(node1, False) With {
        '    .DisplayName = "element13"
        '}
        'Dim node131 As New TreeItemViewModel(node13, False) With {
        '    .DisplayName = "element131"
        '}
        'Dim node132 As New TreeItemViewModel(node13, False) With {
        '    .DisplayName = "element132"
        '}
        'Dim node14 As New TreeItemViewModel(node1, False) With {
        '    .DisplayName = "element14 with colours"
        '}
        'Dim colorNode1 As New ColorItemViewModel(node14, False) With {
        '    .Color = Colors.Aqua,
        '    .IsEditable = True
        '}
        'Dim colorNode2 As New ColorItemViewModel(node14, False) With {
        '    .Color = Colors.ForestGreen
        '}
        'Dim colorNode3 As New ColorItemViewModel(node14, False) With {
        '    .Color = Colors.LightSalmon
        '}
        'Dim node15 As New TreeItemViewModel(node1, True) With {
        '    .DisplayName = "element15 (lazy loading)"
        '}

        '' Add them all to each other
        'rootNode.Children.Add(node1)
        'rootNode.Children.Add(node2)
        'node1.Children.Add(node11)
        'node1.Children.Add(node12)
        'node1.Children.Add(node13)
        'node13.Children.Add(node131)
        'node13.Children.Add(node132)
        'node1.Children.Add(node14)
        'node14.Children.Add(colorNode1)
        'node14.Children.Add(colorNode2)
        'node14.Children.Add(colorNode3)
        'node1.Children.Add(node15)

        ' Use the root node as the window's DataContext to allow data binding. The TreeView
        ' will use the Children property of the DataContext as list of root tree items. This
        ' property happens to be the same as each item DataTemplate uses to find its subitems.
        Dim vm = New MainWindowViewModel
        DataContext = vm

        '' Preset some node states
        'node1.IsSelected = True
        'node13.IsSelected = True
        'node14.IsExpanded = True


        TheTreeView.Visibility = Visibility.Visible
    End Sub

    Private Sub TheTreeView_PreviewSelectionChanged(sender As Object, e As PreviewSelectionChangedEventArgs)
        If LockSelectionCheck.IsChecked = True Then
            ' The current selection is locked by user request (Lock CheckBox is checked).
            ' Don't allow any changes to the selection at all.
            e.CancelThis = True
        Else
            ' Selection is not locked, apply other conditions.
            ' Require all selected items to be of the same type. If an item of another data
            ' type is already selected, don't include this new item in the selection.
            If e.Selecting AndAlso TheTreeView.SelectedItems.Count > 0 Then
                e.CancelThis = e.Item.[GetType]() <> TheTreeView.SelectedItems(0).[GetType]()
            End If
        End If

        'if (e.Selecting)
        '{
        '    System.Diagnostics.Debug.WriteLine("Preview: Selecting " + e.Item + (e.Cancel ? " - cancelled" : ""));
        '}
        'else
        '{
        '    System.Diagnostics.Debug.WriteLine("Preview: Deselecting " + e.Item + (e.Cancel ? " - cancelled" : ""));
        '}
    End Sub

    Private Sub ClearChildrenButton_Click(sender As Object, e As System.Windows.RoutedEventArgs)
        Dim selection As Object() = New Object(TheTreeView.SelectedItems.Count) {}
        TheTreeView.SelectedItems.CopyTo(selection, 0)
        For Each node As TreeItemViewModel In selection
            If node.Children IsNot Nothing Then
                node.Children.Clear()
            End If
        Next
    End Sub

    Private Sub AddChildButton_Click(sender As Object, e As System.Windows.RoutedEventArgs)
        For Each node As TreeItemViewModel In TheTreeView.SelectedItems
            If Not node.HasDummyChild Then
                node.Children.Add(New TreeItemViewModel(node, False) With {
                    .DisplayName = "newborn child"
                })
                node.IsExpanded = True
            End If
        Next
    End Sub

    Private Sub ExpandNodesButton_Click(sender As Object, e As System.Windows.RoutedEventArgs)
        For Each node As TreeItemViewModel In TheTreeView.SelectedItems
            node.IsExpanded = True
        Next
    End Sub

    Private Sub HideNodesButton_Click(sender As Object, e As System.Windows.RoutedEventArgs)
        For Each node As TreeItemViewModel In TheTreeView.SelectedItems.OfType(Of TreeItemViewModel)().ToArray()
            node.IsVisible = False
        Next
    End Sub

    Private Sub ShowNodesButton_Click(sender As Object, e As System.Windows.RoutedEventArgs)
        For Each node As TreeItemViewModel In TheTreeView.Items
            DoShowAll(node, Function(n) True)
        Next
    End Sub

    Private Sub DoShowAll(node As TreeItemViewModel, selector As Func(Of TreeItemViewModel, Boolean))
        node.IsVisible = selector(node)
        If node.Children IsNot Nothing Then
            For Each child In node.Children
                DoShowAll(child, selector)
            Next
        End If
    End Sub

    Private Sub SelectNoneButton_Click(sender As Object, e As System.Windows.RoutedEventArgs)
        For Each node As TreeItemViewModel In TheTreeView.Items
            DoSelectAll(node, Function(n) False)
        Next
    End Sub

    Private Sub SelectSomeButton_Click(sender As Object, e As System.Windows.RoutedEventArgs)
        Dim rnd As New Random()
        For Each node As TreeItemViewModel In TheTreeView.Items
            DoSelectAll(node, Function(n) rnd.[Next](0, 2) > 0)
        Next
    End Sub

    Private Sub SelectAllButton_Click(sender As Object, e As System.Windows.RoutedEventArgs)
        For Each node As TreeItemViewModel In TheTreeView.Items
            DoSelectAll(node, Function(n) True)
        Next
    End Sub

    Private Sub ToggleSelectButton_Click(sender As Object, e As System.Windows.RoutedEventArgs)
        For Each node As TreeItemViewModel In TheTreeView.Items
            DoSelectAll(node, Function(n) Not n.IsSelected)
        Next
    End Sub

    Private Sub DoSelectAll(node As TreeItemViewModel, selector As Func(Of TreeItemViewModel, Boolean))
        node.IsSelected = selector(node)
        If node.Children IsNot Nothing Then
            For Each child In node.Children
                DoSelectAll(child, selector)
            Next
        End If
    End Sub

    Private Sub ExpandMenuItem_Click(sender As Object, e As RoutedEventArgs)
        For Each node As TreeItemViewModel In TheTreeView.SelectedItems
            node.IsExpanded = True
        Next
    End Sub

    Private Sub RenameMenuItem_Click(sender As Object, e As RoutedEventArgs)
        For Each node As TreeItemViewModel In TheTreeView.SelectedItems
            node.IsEditing = True
            Exit For
        Next
    End Sub

    Private Sub DeleteMenuItem_Click(sender As Object, e As RoutedEventArgs)
        For Each node As TreeItemViewModel In TheTreeView.SelectedItems.Cast(Of TreeItemViewModel)().ToArray()
            node.Parent.Children.Remove(node)
        Next
    End Sub

    Private Sub ShowSecondCheck_Checked(sender As Object, e As RoutedEventArgs)
        If ShowSecondCheck.IsChecked = True Then
            If LastColumn.ActualWidth = 0 Then
                Width += FirstColumn.ActualWidth
            End If
            LastColumn.Width = New GridLength(1, GridUnitType.Star)
        Else
            If LastColumn.ActualWidth > 0 Then
                Width -= LastColumn.ActualWidth
            End If
            LastColumn.Width = New GridLength(0, GridUnitType.Pixel)
        End If
    End Sub
End Class
