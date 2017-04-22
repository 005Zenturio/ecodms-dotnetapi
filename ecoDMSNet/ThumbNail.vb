Imports System.IO

Public Class ThumbNail
    Public Property DocId As Integer
    Public Property Data As Byte()
    Public Property FileName As String
    Public Property Page As Integer
    Public Property Height As Integer

    Public Function SaveToFile(file As String) As Boolean
        Try
         My.Computer.FileSystem.WriteAllBytes(file,Data,False)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GetImage() As System.drawing.image

        If Data.Count > 0 then
            Return System.Drawing.Image.FromStream(New memorystream(Data))
        Else
            Return Nothing
        End If
    End Function

End Class
