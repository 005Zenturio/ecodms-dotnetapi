Public Class DocumentType
    Public Property Id As Integer
    Public Property Name As String
    Public Property Frist As TypeFrist
    
    Public Class TypeFrist
        Public Property Jahre As Integer
        Public Property Monate As Integer
        Public Property Tage As Integer

        Public Overrides Function ToString() As String
            Return String.Format("Years:{0}, Months:{1}, Days:{2}",Jahre, Monate, Tage)
        End Function

    End Class


End Class
