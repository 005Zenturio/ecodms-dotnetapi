Imports System.Management.Automation

<Cmdlet(VerbsCommon.[New],"ecoDMSFolder")>
Public Class NewEcoDmsFolder
     Inherits PSCmdlet

    <Parameter(Mandatory := True,
               ValueFromPipelineByPropertyName := True,
               ValueFromPipeline := True,
               Position := 0,
               HelpMessage := "The Name of the forder to create"), [Alias]("Name")>
    Public Property FolderName As String 
    
    <Parameter(Mandatory := False,
               ValueFromPipelineByPropertyName := True,
               ValueFromPipeline := True,
               Position := 1,
               HelpMessage := "The external key (foreign key) of the folder"), [Alias]("Key")>
    Public Property ExternalKey As String = String.Empty

        <Parameter(Mandatory := False,
               ValueFromPipelineByPropertyName := True,
               ValueFromPipeline := True,
               Position := 2,
               HelpMessage := "The Optional buzzwords to use for folder"), [Alias]("bw")>
    Public Property Buzzwords As String =  String.Empty
    
     <Parameter(Mandatory := False,
               ValueFromPipelineByPropertyName := True,
               ValueFromPipeline := True,
               Position := 3,
               HelpMessage := "The parent folder of the folder to create"), [Alias]("pfId")>
    Public Property ParentFolderId As String = Nothing
    
Protected Overrides Sub ProcessRecord()
        WriteVerbose("Try to create a new folder in the ecoDMS system based on the specified foldername")
        Dim eco As ecoDMSNet.EcoDmsClient = Me.SessionState.PSVariable.GetValue("ecoDMSClient")
        If Not eco Is Nothing Then
            
            Dim nfo = eco.CreateFolder(FolderName,ExternalKey,Buzzwords,ParentFolderId)

            WriteObject(nfo)
        Else
            Throw New Exception("No connection to ecoDMS, use Open-ecoDMSArchive cmdLet to connect first...")
        End If
    End Sub
End Class
