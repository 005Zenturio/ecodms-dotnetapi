Imports System.IO

Public Class DocumentFile
    Public Property Docid As Integer
    Public Property Version As Integer
    Public Property Data As Byte()
    Public Property FileName As String
    Public Shared Function FromFile(fileName as String) As DocumentFile
        If My.Computer.FileSystem.FileExists(fileName) Then
            Dim doc As New DocumentFile
            doc.FileName = Path.GetFileName(fileName)
            doc.Data = My.Computer.FileSystem.ReadAllBytes(fileName)
         
            Return doc
        Else
            Throw New FileNotFoundException(fileName)
        End If
    End Function



End Class
