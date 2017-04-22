Imports System.Management.Automation

<Cmdlet(VerbsCommon.Get,"ecoDMSDocument")>
Public Class GetEcoDmsDocument
    Inherits PSCmdlet

    <Parameter(Mandatory := True,
               ValueFromPipelineByPropertyName := True,
               ValueFromPipeline := True,
               Position := 0,
               HelpMessage := "The docid of the file to download"), [Alias]("id")>
    Public Property Docid As Integer
    
    <Parameter(Mandatory := False,
               ValueFromPipelineByPropertyName := True,
               ValueFromPipeline := True,
               Position := 1,
               HelpMessage := "The download location of the file"), [Alias]("Path")>
    Public Property DestinationPath As String = String.Empty

        <Parameter(Mandatory := False,
               ValueFromPipelineByPropertyName := True,
               ValueFromPipeline := True,
               Position := 2,
               HelpMessage := "The Optional Filename to use for download"), [Alias]("FileName")>
    Public Property AlternateFileName As String =  String.Empty
    
     <Parameter(Mandatory := False,
               ValueFromPipelineByPropertyName := True,
               ValueFromPipeline := True,
               Position := 3,
               HelpMessage := "The Version of the file to download"), [Alias]("Ver")>
    Public Property Version As Integer = -1


Protected Overrides Sub ProcessRecord()
        WriteVerbose("Try to download the Document from ecoDMS based on the specified docid")
        Dim eco As ecoDMSNet.EcoDmsClient = Me.SessionState.PSVariable.GetValue("ecoDMSClient")
        If Not eco Is Nothing Then
            
            Dim doc = eco.GetDocumentFile(Docid,Version)
         

            If My.Computer.FileSystem.DirectoryExists(DestinationPath) Then
                Dim fn As String = doc.FileName

                   If String.IsNullOrEmpty(AlternateFileName) = False Then
               fn = AlternateFileName
            End If
                My.Computer.FileSystem.WriteAllBytes(IO.Path.Combine(DestinationPath,fn),doc.Data,False)
            End If

            WriteObject(doc)
        Else
            Throw New Exception("No connection to ecoDMS, use Open-ecoDMSArchive cmdLet to connect first...")
        End If
    End Sub
    End Class


<Cmdlet(VerbsCommon.[New], "ecoDMSDocument")>
Public Class NewEcoDmsDocument
    Inherits PSCmdlet

    <Parameter(Mandatory := True,
               ValueFromPipelineByPropertyName := True,
               ValueFromPipeline := True,
               Position := 0,
               HelpMessage := "The file to upload to ecoDMS"), [Alias]("FilePath")>
    Public Property File As String

    <Parameter(Mandatory := False,
               ValueFromPipelineByPropertyName := True,
               ValueFromPipeline := True,
               Position := 1,
               HelpMessage := "Specifies if the file is under version controll or not"), [Alias]("Path")>
    Public Property VersionControlled As SwitchParameter

    Protected Overrides Sub ProcessRecord()
        WriteVerbose("Try to upload the Document to ecoDMS based on the specified file")
        Dim eco As ecoDMSNet.EcoDmsClient = Me.SessionState.PSVariable.GetValue("ecoDMSClient")
        If Not eco Is Nothing Then
            Dim doc = ecoDMSNet.DocumentFile.FromFile(File)
            doc.Docid = eco.UploadDocumentFile(doc, VersionControlled.ToBool)
            WriteObject(doc.Docid)
        Else
            Throw New Exception("No connection to ecoDMS, use Open-ecoDMSArchive cmdLet to connect first...")
        End If
    End Sub
End Class


<Cmdlet(VerbsCommon.Set, "ecoDMSClassifyDocument")>
Public Class SetEcoDmsClassifyDocument
    Inherits PSCmdlet

    <Parameter(Mandatory := True,
               ValueFromPipelineByPropertyName := True,
               ValueFromPipeline := True,
               Position := 0,
               HelpMessage := "The docid of the file to classify"), [Alias]("id")>
    Public Property Docid As Integer

    <Parameter(Mandatory := False,
               ValueFromPipelineByPropertyName := True,
               ValueFromPipeline := True,
               Position := 1,
               HelpMessage := "The main folder of the document"), [Alias]("mf")>
    Public Property MainFolder As String

        <Parameter(Mandatory := False,
               ValueFromPipelineByPropertyName := True,
               ValueFromPipeline := True,
               Position := 2,
               HelpMessage := "The  folder of the document"), [Alias]("f")>
    Public Property Folder As String

        <Parameter(Mandatory := False,
               ValueFromPipelineByPropertyName := True,
               ValueFromPipeline := True,
               Position := 3,
               HelpMessage := "The status of the document"), [Alias]("s")>
    Public Property Status As String

        <Parameter(Mandatory := False,
               ValueFromPipelineByPropertyName := True,
               ValueFromPipeline := True,
               Position := 4,
               HelpMessage := "The main folder of the document"), [Alias]("da")>
    Public Property Docart As String

        <Parameter(Mandatory := False,
               ValueFromPipelineByPropertyName := True,
               ValueFromPipeline := True,
               Position := 5,
               HelpMessage := "The comment of the document"), [Alias]("bemerkung")>
    Public Property Comment As String

        <Parameter(Mandatory := False,
               ValueFromPipelineByPropertyName := True,
               ValueFromPipeline := True,
               Position := 6,
               HelpMessage := "The resubmission date (Wiedervorlage) of the document"), [Alias]("resubmission")>
    Public Property Defdate As String

       <Parameter(Mandatory:=True,
                   ValueFromPipelineByPropertyName:=True,
                   ValueFromPipeline:=True,
                   Position:=7,
                   HelpMessage:="List of non-default classification attributes (namne:value)"),[Alias]("nda")>
        Public Property ndefClassAttributes As List(Of String)



    Protected Overrides Sub ProcessRecord()
        WriteVerbose("Try toclassify the Document in ecoDMS based on the specified attributes")
        Dim eco As ecoDMSNet.EcoDmsClient = Me.SessionState.PSVariable.GetValue("ecoDMSClient")
        If Not eco Is Nothing Then
         Dim  updocinfo = eco.GetDocumentInfo(Docid)
        If String.IsNullOrEmpty(Folder) = False then  updocinfo.ClassifyAttributes(ecoDMSNet.DefaultClassifyAttributes.ECO_CLASSIFY_FOLDER) = Folder
        If String.IsNullOrEmpty(MainFolder) = False then  updocinfo.ClassifyAttributes(ecoDMSNet.DefaultClassifyAttributes.ECO_CLASSIFY_MAINFOLDER) = MainFolder
        If String.IsNullOrEmpty(Status) = False then  updocinfo.ClassifyAttributes(ecoDMSNet.DefaultClassifyAttributes.ECO_CLASSIFY_STATUS) = Status
        If String.IsNullOrEmpty(Docart) = False then  updocinfo.ClassifyAttributes(ecoDMSNet.DefaultClassifyAttributes.ECO_CLASSIFY_DOCTYPE) = Docart
        If String.IsNullOrEmpty(Comment) = False then  updocinfo.ClassifyAttributes(ecoDMSNet.DefaultClassifyAttributes.ECO_CLASSIFY_BEMERKUNG) = Comment
        If String.IsNullOrEmpty(defdate) = False then  updocinfo.ClassifyAttributes(ecoDMSNet.DefaultClassifyAttributes.ECO_CLASSIFY_RESUBMISSION) = defdate
        If not ndefClassAttributes Is Nothing Then
            For Each x In From f In ndefClassAttributes Select f.Split(":")
                updocinfo.ClassifyAttributes.Add(x(0),x(1))
            Next
        End If
        WriteObject(eco.ClassifyDocument(updocinfo))
        Else
            Throw New Exception("No connection to ecoDMS, use Open-ecoDMSArchive cmdLet to connect first...")
        End If
    End Sub
End Class

<Cmdlet(VerbsCommon.Get, "ecoDMSClassifiecation")>
Public Class GetEcoDmsClassification
    Inherits PSCmdlet

    <Parameter(Mandatory := True,
               ValueFromPipelineByPropertyName := True,
               ValueFromPipeline := True,
               Position := 0,
               HelpMessage := "The docid of the file to get the classifiecation"), [Alias]("id")>
    Public Property Docid As Integer

    Protected Overrides Sub ProcessRecord()
        WriteVerbose("Try to get the Document's classification from ecoDMS based on the specified docID")
        Dim eco As ecoDMSNet.EcoDmsClient = Me.SessionState.PSVariable.GetValue("ecoDMSClient")
        If Not eco Is Nothing Then
         dim   doccla = eco.GetDocumentInfo(Docid)
            WriteObject(doccla)
        Else
            Throw New Exception("No connection to ecoDMS, use Open-ecoDMSArchive cmdLet to connect first...")
        End If
    End Sub
End Class