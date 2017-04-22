Imports System.Management.Automation

<Cmdlet(VerbsCommon.Search,"ecoDMSDocuments")>
Public Class SearchecoDmsDocuments
      Inherits PSCmdlet

        <Parameter(Mandatory:=True,
                   ValueFromPipelineByPropertyName:=True,
                   ValueFromPipeline:=True,
                   Position:=0,
                   HelpMessage:="List of Search creterias (docid:>:0)"),[Alias]("fi")>
        Public Property Filter As List(Of String)
    
       Protected Overrides Sub ProcessRecord()
        WriteVerbose("Try to get Documents from ecoDMS based on the search creteria")
         Dim eco As ecoDMSNet.EcoDmsClient = Me.SessionState.PSVariable.GetValue("ecoDMSClient") 
        If Not eco Is Nothing Then

             Dim sc As new ecoDMSNet.SearchCriteria

            For each f In Filter

              Dim x =  f.Split(":")

                sc.AddFilter(x(0),x(1),x(2))
            Next

            WriteObject(eco.SearchDocuments(sc))
        Else
            Throw New Exception("No connection to ecoDMS, use Open-ecoDMSArchive cmdLet to connect first...")
        End If
    End Sub
End Class
