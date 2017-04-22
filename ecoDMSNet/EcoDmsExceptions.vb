Public Class NoArchiveOpened
        Inherits Exception
    
        Public Sub New()
            MyBase.New("You have to open a Archive first, to do this opperation!")
        End Sub

End Class

Public Class EcoDmsException
        Inherits Exception
    
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

End Class