Namespace ViewModels

    Public Class MainWindowViewModel

        Public Sub New()
            ' Create some example nodes to play with
            Dim rootNode As New TreeItemViewModel(Nothing, False) With {.DisplayName = "rootNode"}
            Dim node1 As New TreeItemViewModel(rootNode, False) With {.DisplayName = "element1 (editable)",
                                                                      .IsEditable = True}
            Dim node2 As New TreeItemViewModel(rootNode, False) With {.DisplayName = "element2"}
            Dim node11 As New TreeItemViewModel(node1, False) With {.DisplayName = "element11",
                                                                    .Remarks = "Look at me!"}
            Dim node12 As New TreeItemViewModel(node1, False) With {.DisplayName = "element12 (disabled)",
                                                                    .IsEnabled = False}
            Dim node13 As New TreeItemViewModel(node1, False) With {.DisplayName = "element13"}
            Dim node131 As New TreeItemViewModel(node13, False) With {.DisplayName = "element131"}
            Dim node132 As New TreeItemViewModel(node13, False) With {.DisplayName = "element132"}
            Dim node14 As New TreeItemViewModel(node1, False) With {.DisplayName = "element14 with colours"}
            Dim colorNode1 As New ColorItemViewModel(node14, False) With {.Color = Colors.Aqua,
                                                                          .IsEditable = True}
            Dim colorNode2 As New ColorItemViewModel(node14, False) With {.Color = Colors.ForestGreen}
            Dim colorNode3 As New ColorItemViewModel(node14, False) With {.Color = Colors.LightSalmon}
            Dim node15 As New TreeItemViewModel(node1, True) With {.DisplayName = "element15 (lazy loading)"}

            ' Add them all to each other
            rootNode.Children.Add(node1)
            rootNode.Children.Add(node2)
            node1.Children.Add(node11)
            node1.Children.Add(node12)
            node1.Children.Add(node13)
            node13.Children.Add(node131)
            node13.Children.Add(node132)
            node1.Children.Add(node14)
            node14.Children.Add(colorNode1)
            node14.Children.Add(colorNode2)
            node14.Children.Add(colorNode3)
            node1.Children.Add(node15)

            Me.RootNode = rootNode

            ' Preset some node states
            node1.IsSelected = True
            node13.IsSelected = True
            node14.IsExpanded = True

        End Sub

        Public Property RootNode As TreeItemViewModel
    End Class

End Namespace
