Public Class SearchCriteria
    Public Property FilterList As New List(Of Filter)
    
    Public Sub AddFilter(classifyAttribut As String,searchOperator As String,searchValue As String) 
        FilterList.Add( New Filter With {.ClassifyAttribut=classifyAttribut,.SearchOperator=searchOperator,.SearchValue=searchValue})
    End Sub
    
    Public Class Filter 
     Public Property classifyAttribut as string  
     Public Property searchOperator As String
     Public Property searchValue As String   
  End Class

End Class
