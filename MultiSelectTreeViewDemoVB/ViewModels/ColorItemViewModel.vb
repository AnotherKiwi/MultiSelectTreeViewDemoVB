Imports System.Reflection

Namespace ViewModels
    <Obfuscation(Exclude:=True, ApplyToMembers:=False, Feature:="renaming")>
    Public Class ColorItemViewModel
        Inherits TreeItemViewModel

#Region "Constructor"

        Public Sub New(parent As TreeItemViewModel, lazyLoadChildren As Boolean)
            MyBase.New(parent, lazyLoadChildren)
            _color = Colors.Silver
        End Sub

#End Region 'Constructor

#Region "Public properties"

        Private _color As Color
        Public Property Color() As Color
            Get
                Return _color
            End Get
            Set
                If Value <> _color Then
                    _color = Value
                    OnPropertyChanged("Color")
                    OnPropertyChanged("BackgroundBrush")
                    OnPropertyChanged("ForegroundBrush")
                    DisplayName = _color.ToString()
                End If
            End Set
        End Property

        Public ReadOnly Property BackgroundBrush() As Brush
            Get
                Return New SolidColorBrush(_color)
            End Get
        End Property

        Public ReadOnly Property ForegroundBrush() As Brush
            Get
                Return If(IsDarkColor(_color), Brushes.White, Brushes.Black)
            End Get
        End Property

#End Region 'Public properties

#Region "Private methods"

        ''' <summary>
        ''' Computes the grey value of a _color.
        ''' </summary>
        ''' <param name="c"></param>
        ''' <returns></returns>
        Private Shared Function ToGray(c As Color) As Byte
            Return CType((c.R * 0.3 + c.G * 0.59 + c.B * 0.11), Byte)
        End Function

        ''' <summary>
        ''' Determines whether the _color is dark or light.
        ''' </summary>
        ''' <param name="c"></param>
        ''' <returns></returns>
        Private Shared Function IsDarkColor(c As Color) As Boolean
            Return ToGray(c) < &H90
        End Function

#End Region 'Private methods
    End Class
End Namespace