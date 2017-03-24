Imports System.Reflection

Namespace Helpers

    ''' <summary>
    '''     Provides a configurable DataTemplateSelector.
    ''' </summary>
    ''' <remarks>
    '''     Translated from MultiSelectTreeView Demo.
    ''' </remarks>
    <Obfuscation(Exclude:=True, ApplyToMembers:=True, Feature:="renaming")>
    Public Class TypeTemplateSelector

        Inherits DataTemplateSelector

        ''' <summary>
        ''' Gets or sets the list of template definitions.
        ''' </summary>
        Public Property TemplateDefinitions() As List(Of TypeTemplateDefinition)

        ''' <summary>
        ''' Initialises a new instance of the TypeTemplateSelector class.
        ''' </summary>
        Public Sub New()
            TemplateDefinitions = New List(Of TypeTemplateDefinition)()
        End Sub

        ''' <summary>
        ''' Selects a DataTemplate for the item, based on the item's type.
        ''' </summary>
        ''' <param name="item">Item to select the template for.</param>
        ''' <param name="container">Unused.</param>
        ''' <returns></returns>
        Public Overrides Function SelectTemplate(item As Object, container As DependencyObject) As DataTemplate
            For Each def In TemplateDefinitions
                If def.Type.IsInstanceOfType(item) Then
                    Return def.Template
                End If
            Next
            Return Nothing
        End Function
    End Class

    ''' <summary>
    '''     Defines a template to be selected for a type.
    ''' </summary>
    ''' <remarks>
    '''     Translated from MultiSelectTreeView Demo.
    ''' </remarks>
    <Obfuscation(Exclude:=True, ApplyToMembers:=True, Feature:="renaming")>
    Public Class TypeTemplateDefinition

        ''' <summary>
        ''' Gets or sets the item type to define the template for.
        ''' </summary>
        Public Property Type() As Type

        ''' <summary>
        ''' Gets or sets the DataTemplate to select for this item type.
        ''' </summary>
        Public Property Template() As DataTemplate
    End Class

End Namespace