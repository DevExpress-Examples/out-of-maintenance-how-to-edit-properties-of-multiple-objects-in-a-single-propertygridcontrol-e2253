Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraVerticalGrid.Events

Namespace CutomProperties
	Partial Public Class Form1
		Inherits Form
		Private propertyStore As New List(Of PropertyDescriptor)()
		Public Sub New()
			InitializeComponent()
			propertyStore.Add(New CustomPropertyDescriptor(Me, "Form's Size", "Size"))
			propertyStore.Add(New CustomPropertyDescriptor(Me, "Form's Title", "Text"))
			propertyStore.Add(New CustomPropertyDescriptor(Me.propertyGridControl1, "PropertyGridControl's RecordWidth", "RecordWidth"))
			propertyStore.Add(New CustomPropertyDescriptor(Me.button1, "Button's Visibility", "Visible"))
			Me.propertyGridControl1.SelectedObject = propertyStore
		End Sub

		Private Sub propertyGridControl1_CustomPropertyDescriptors(ByVal sender As Object, ByVal e As CustomPropertyDescriptorsEventArgs) Handles propertyGridControl1.CustomPropertyDescriptors
			If e.Source Is propertyStore Then
				Dim rootProperties As New PropertyDescriptorCollection(Nothing)
				For Each pd As PropertyDescriptor In propertyStore
					rootProperties.Add(pd)
				Next pd
				e.Properties = rootProperties
			End If
		End Sub
	End Class
	Friend Class CustomPropertyDescriptor
		Inherits PropertyDescriptor
		Private name_Renamed As String
		Private sourcePropertyDescriptor_Renamed As PropertyDescriptor
		Private source_Renamed As Object

		Public Sub New(ByVal source As Object, ByVal name As String, ByVal targetPath As String)
			MyBase.New(name, Nothing)
			Me.name_Renamed = name
			Me.source_Renamed = source
			Me.sourcePropertyDescriptor_Renamed = TypeDescriptor.GetProperties(source)(targetPath)
			If SourcePropertyDescriptor Is Nothing Then
				Throw New Exception("Can't bind to the source with the " & targetPath & " property")
			End If
		End Sub

		Public Overrides ReadOnly Property Name() As String
			Get
				Return name_Renamed
			End Get
		End Property
		Public Overrides ReadOnly Property ComponentType() As Type
			Get
				Return SourcePropertyDescriptor.ComponentType
			End Get
		End Property
		Public Overrides ReadOnly Property IsReadOnly() As Boolean
			Get
				Return SourcePropertyDescriptor.IsReadOnly
			End Get
		End Property
		Public Overrides ReadOnly Property PropertyType() As Type
			Get
				Return SourcePropertyDescriptor.PropertyType
			End Get
		End Property

		Private ReadOnly Property SourcePropertyDescriptor() As PropertyDescriptor
			Get
				Return sourcePropertyDescriptor_Renamed
			End Get
		End Property
		Private ReadOnly Property Source() As Object
			Get
				Return source_Renamed
			End Get
		End Property

		Public Overrides Function GetValue(ByVal component As Object) As Object
			Return SourcePropertyDescriptor.GetValue(Source)
		End Function
		Public Overrides Function CanResetValue(ByVal component As Object) As Boolean
			Return SourcePropertyDescriptor.CanResetValue(Source)
		End Function
		Public Overrides Sub ResetValue(ByVal component As Object)
			SourcePropertyDescriptor.ResetValue(Source)
		End Sub
		Public Overrides Sub SetValue(ByVal component As Object, ByVal value As Object)
			SourcePropertyDescriptor.SetValue(Source, value)
		End Sub
		Public Overrides Function ShouldSerializeValue(ByVal component As Object) As Boolean
			Return SourcePropertyDescriptor.ShouldSerializeValue(Source)
		End Function
	End Class
End Namespace
