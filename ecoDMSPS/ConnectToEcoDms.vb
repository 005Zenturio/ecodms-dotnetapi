Imports System.Management.Automation


<Cmdlet(VerbsCommon.Open,"ecoDMSArchive")>
Public Class ConnectToEcoDms
        Inherits PSCmdlet

        <Parameter(Mandatory:=True,
                   ValueFromPipelineByPropertyName:=True,
                   ValueFromPipeline:=True,
                   Position:=0,
                   HelpMessage:="The base url to connect to(http://[host]:[port]"),[Alias]("url")>
        Public Property BaseUri As String

          <Parameter(Mandatory:=True,
                   ValueFromPipelineByPropertyName:=True,
                   ValueFromPipeline:=True,
                   Position:=2,
                   HelpMessage:="Helpmsg"),[Alias]("User")>
        Public Property UserName As String

         <Parameter(Mandatory:=True,
                   ValueFromPipelineByPropertyName:=True,
                   ValueFromPipeline:=True,
                   Position:=3,
                   HelpMessage:="dfsdfsdfdsfdsf"),[Alias]("pw")>
        Public Property Password As String
    
       <Parameter(Mandatory:=True,
                   ValueFromPipelineByPropertyName:=True,
                   ValueFromPipeline:=True,
                   Position:=4,
                   HelpMessage:="dfsdfsdfdsfdsf"),[Alias]("aid")>
        Public Property ArchiveId As Integer
    
    Protected Overrides Sub ProcessRecord()
       WriteVerbose(String.Format("try to connect to ecoDMS on {0} with user {1} and password ******.",BaseUri,UserName))

        If Not Me.SessionState.PSVariable.GetValue("ecoDMSClient") Is Nothing Then
            Throw New Exception("Allredy connected to a server, you have to use disconnect first!")
        End If

        Dim eco As New ecoDMSNet.EcoDmsClient(BaseUri,UserName,Password)
        If eco.Connect(ArchiveId) Then
                Me.SessionState.PSVariable.Set("ecoDMSClient",eco)
        Else
            Throw New Exception("Connection failed!")
        End If
    End Sub


End Class

<Cmdlet(VerbsCommon.Close,"ecoDMSArchive")>
Public Class DisconnectFromEcoDms
        Inherits PSCmdlet
    Protected Overrides Sub ProcessRecord()
       Dim eco As ecoDMSNet.EcoDmsClient = Me.SessionState.PSVariable.GetValue("ecoDMSClient")
        If Not eco Is Nothing Then
            WriteVerbose(String.Format("try to disconnect from ecoDMS archive with id:{0}.",eco.CurrentArchiveId))
            eco.Disconnect
            eco = Nothing
            Me.SessionState.PSVariable.Remove("ecoDMSClient")
        Else
            Throw New Exception("No connection to ecoDMS, use Open-ecoDMSArchive cmdLet to connect first...")
        End If
    End Sub
End Class