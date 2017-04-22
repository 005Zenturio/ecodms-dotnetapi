Public Class Document
    Public Property docId As Integer
    Public Property clDocId As Integer
    Public Property archiveName As Integer
    Public Property classifyAttributes As Dictionary(Of String, String)
    Public Property editRoles As List(Of String)
    Public Property readRoles As List(Of String)

End Class

  Public Structure DefaultClassifyAttributes
      Public Const ECO_CLASSIFY_MAINFOLDER As String = "mainfolder"
      Public Const ECO_CLASSIFY_FOLDER as String = "folder"
      Public Const ECO_CLASSIFY_STATUS as String = "status"
      Public Const ECO_CLASSIFY_DOCTYPE as String = "docart"
      Public Const ECO_CLASSIFY_BEMERKUNG as String = "bemerkung"
      Public Const ECO_CLASSIFY_DOCID as String = "docid"
      Public Const ECO_CLASSIFY_REVISION as String = "revision"
      Public Const ECO_CLASSIFY_FULLTEXT as String = "fulltext"
      Public Const ECO_CLASSIFY_RESUBMISSION as String = "defdate"
      Public Const ECO_CLASSIFY_DATE as String = "cdate"
      Public Const ECO_CLASSIFY_MODIFIED_BY as String = "changeid"
      Public Const ECO_CLASSIFY_MODIFIED as String = "ctimestamp"
    End Structure