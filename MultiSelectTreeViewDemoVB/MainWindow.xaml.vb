Imports Catel.Windows
Imports MultiSelectTreeViewDemoVB.ViewModels

Namespace Views

    Partial Public Class MainWindow
        Inherits DataWindow

        Public Sub New()
            MyBase.New(DataWindowMode.[Custom])
            InitializeComponent()
            ShowSecondCheck_Checked(Nothing, Nothing)

            TheTreeView.HoverHighlighting = True
            TheTreeView.ItemIndent = 30

            ' Use the root node as the window's DataContext to allow data binding. The TreeView
            ' will use the Children property of the DataContext as list of root tree items. This
            ' property happens to be the same as each item DataTemplate uses to find its subitems.
            'Dim vm = New MainWindowViewModel
            'DataContext = vm

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
            Dim selection As Object() = New Object(TheTreeView.SelectedItems.Count - 1) {}
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

End Namespace
