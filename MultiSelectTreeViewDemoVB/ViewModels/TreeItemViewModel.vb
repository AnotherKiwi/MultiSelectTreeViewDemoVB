Imports System.Reflection
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Runtime.CompilerServices

Namespace ViewModels
    ''' <summary>
    ''' Sample base class for tree items view models. All specialised tree item view model classes
    ''' should inherit from this class.
    ''' </summary>
    <Obfuscation(Exclude:=True, ApplyToMembers:=False, Feature:="renaming")>
    Public Class TreeItemViewModel
        Implements INotifyPropertyChanged

#Region "Fields"
        Private Shared ReadOnly DummyChild As New TreeItemViewModel()
#End Region 'Fields

#Region "Constructor"

        Public Sub New(_parent As TreeItemViewModel, lazyLoadChildren As Boolean)
            Me._parent = _parent

            _children = New ObservableCollection(Of TreeItemViewModel)()

            If lazyLoadChildren Then
                _children.Add(DummyChild)
            End If
        End Sub

        ' This is used to create the DummyChild instance.
        Public Sub New()
        End Sub

#End Region 'Constructor

#Region "Public properties"

        Private ReadOnly _children As ObservableCollection(Of TreeItemViewModel)
        ''' <summary>
        ''' Returns the logical child items of this object.
        ''' </summary>
        Public ReadOnly Property Children() As ObservableCollection(Of TreeItemViewModel)
            Get
                Return _children
            End Get
        End Property

        ''' <summary>
        ''' Returns true if this object's Children have not yet been populated.
        ''' </summary>
        Public ReadOnly Property HasDummyChild() As Boolean
            Get
                Return _children.Count = 1 AndAlso _children(0) Is DummyChild
            End Get
        End Property

        Private _isExpanded As Boolean
        ''' <summary>
        ''' Gets/sets whether the TreeViewItem 
        ''' associated with this object is expanded.
        ''' </summary>
        Public Property IsExpanded() As Boolean
            Get
                Return _isExpanded
            End Get
            Set
                If Value <> _isExpanded Then
                    _isExpanded = Value
                    OnPropertyChanged()

                    ' Expand all the way up to the root.
                    If _isExpanded AndAlso _parent IsNot Nothing Then
                        _parent._isExpanded = True
                    End If

                    ' Lazy load the child items, if necessary.
                    If _isExpanded AndAlso HasDummyChild Then
                        _children.Remove(DummyChild)
                        LoadChildren()
                    End If
                End If
            End Set
        End Property

        ''' <summary>
        ''' Invoked when the child items need to be loaded on demand.
        ''' Subclasses can override this to populate the Children collection.
        ''' </summary>
        Protected Overridable Sub LoadChildren()
            Dim i As Integer = 0
            While i < 100
                _children.Add(New TreeItemViewModel(Me, True) With {.DisplayName = "subnode " & i.ToString})
                i += 1
            End While
        End Sub

        Private _isSelected As Boolean
        ''' <summary>
        ''' Gets/sets whether the TreeViewItem 
        ''' associated with this object is selected.
        ''' </summary>
        Public Property IsSelected() As Boolean
            Get
                Return _isSelected
            End Get
            Set
                If Value <> _isSelected Then
                    _isSelected = Value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        Private _isEditable As Boolean
        Public Property IsEditable() As Boolean
            Get
                Return _isEditable
            End Get
            Set
                If Value <> _isEditable Then
                    _isEditable = Value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        Private _isEditing As Boolean
        Public Property IsEditing() As Boolean
            Get
                Return _isEditing
            End Get
            Set
                If Value <> _isEditing Then
                    _isEditing = Value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        Private _isEnabled As Boolean = True
        Public Property IsEnabled() As Boolean
            Get
                Return _isEnabled
            End Get
            Set
                If Value <> _isEnabled Then
                    _isEnabled = Value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        Private _isVisible As Boolean = True
        Public Property IsVisible() As Boolean
            Get
                Return _isVisible
            End Get
            Set
                If Value <> _isVisible Then
                    _isVisible = Value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        Private _remarks As String
        Public Property Remarks() As String
            Get
                Return _remarks
            End Get
            Set
                If Value <> _remarks Then
                    _remarks = Value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        Private ReadOnly _parent As TreeItemViewModel
        Public ReadOnly Property Parent() As TreeItemViewModel
            Get
                Return _parent
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return "{Node " + DisplayName + "}"
        End Function

#End Region 'Public properties

#Region "ViewModelBase"

        ''' <summary>
        ''' Returns the user-friendly name of this object.
        ''' Child classes can set this property to a new value,
        ''' or override it to determine the value on-demand.
        ''' </summary>
        Private _displayName As String
        Public Overridable Property DisplayName() As String
            Get
                Return _displayName
            End Get
            Set
                If Value <> _displayName Then
                    _displayName = Value
                    OnPropertyChanged()
                End If
            End Set
        End Property

#End Region 'ViewModelBase

#Region "INotifyPropertyChanged Members"

        ''' <summary>
        '''     Raised when a property on this object has a new value.
        ''' </summary>
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged


        ''' <summary>
        '''     Raises this object's PropertyChanged event.
        ''' </summary>
        ''' <param name="propertyName">The property that has a new value.</param>
        Protected Overridable Sub OnPropertyChanged(<CallerMemberName> Optional propertyName As String = Nothing)

            Dim handler As PropertyChangedEventHandler = Me.PropertyChangedEvent
            If handler IsNot Nothing Then
                Dim e = New PropertyChangedEventArgs(propertyName)
                handler(Me, e)
            End If
        End Sub

#End Region ' INotifyPropertyChanged Members

    End Class
End Namespace