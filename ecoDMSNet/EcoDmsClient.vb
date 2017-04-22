Imports System.IO
Imports System.Text.RegularExpressions
Imports RestSharp

Public Class EcoDmsClient
    Private ReadOnly _ecoDmsUser As String = String.Empty
    Private ReadOnly _ecodmsPassword As String = String.Empty
    Private ReadOnly _baseUri As String = String.Empty
    Private _currentArchiveId As Integer = - 1
    Private _restclient As RestClient = Nothing


    ''' <summary>
    ''' Creates a new instance of the ecoDMS .Net client
    ''' </summary>
    ''' <param name="baseUri">http://{host}:{port}</param>
    ''' <param name="user">The ecoDMS Username</param>
    ''' <param name="password">The ecoDMS Password</param>
    Public Sub New(baseUri As String, user As String, password As String)
        _ecoDmsUser = user
        _ecodmsPassword = password
        _baseUri = baseUri & "/api/"
    End Sub

    ''' <summary>
    ''' If the client is connectetd, this property contains the current ArchiveID
    ''' </summary>
    ''' <returns>ecoDMS current archive id </returns>
    Public ReadOnly Property CurrentArchiveId as Integer
        Get
            return _currentArchiveId
        End Get
    End Property

    ''' <summary>
    ''' Connect to the REST API using the given Archive id.
    ''' </summary>
    ''' <param name="archiveId">The ecoDMS Archive id</param>
    ''' <returns></returns>
    Public Function Connect(archiveId As Integer) As Boolean
        _restclient = New RestClient(_baseUri)
        _restclient.Authenticator = New Authenticators.HttpBasicAuthenticator(_ecoDmsUser, _ecodmsPassword)
        'First use Test to get a Cookie and Session ID
        Dim reqTest As New RestRequest("test")
        Dim respTest = _restclient.Execute(reqTest)

        If respTest.StatusCode <> Net.HttpStatusCode.OK Then
            Return False
        End If

        Dim reqLogToArc As New RestRequest(String.Format("connect/{0}", ArchiveID))
        Dim respLogToArc = _restclient.Execute(reqLogToArc)

        If respLogToArc.StatusCode = Net.HttpStatusCode.OK Then
            _currentArchiveId = ArchiveID
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Close the connection to the current Archive
    ''' </summary>
    ''' <returns></returns>
    Public Function Disconnect() As Boolean
        Dim reqDisc As New RestRequest("disconnect")
        Dim respDisc = _restclient.Execute(reqDisc)
        If respDisc.StatusCode = Net.HttpStatusCode.OK Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Returns a list of all avilable folders for the current user
    ''' </summary>
    ''' <returns>A list of folder object</returns>
    Public Function GetFolders() As List(Of Folder)
        If _currentArchiveId > - 1 Then
            Dim reqFolders As New RestRequest("folders")
            Dim folders As List(Of Folder) = _restclient.Execute (Of List(Of Folder))(reqFolders).Data
            Return folders
        Else
            Throw New NoArchiveOpened
        End If
    End Function

    ''' <summary>
    ''' Returns a list off all avilable status
    ''' </summary>
    ''' <returns>A list of Status object</returns>
    Public Function GetStatus() As List(Of Status)
        If _currentArchiveId > - 1 Then
            Dim reqStatus As New RestRequest("status")
            Dim status As List(Of Status) = _restclient.Execute (Of List(Of Status))(reqStatus).Data
            Return status
        Else
            Throw New NoArchiveOpened
        End If
    End Function

    ''' <summary>
    ''' Returns a list off all avilable ClassifyAttributes
    ''' </summary>
    ''' <returns>A Dictonary of Status(Key,Value)</returns>
    Public Function GetClassifyAttributes() As Dictionary(Of String,String)
        If _currentArchiveId > - 1 Then
            Dim reqClAt As New RestRequest("classifyAttributes")
            Dim ClAt As Dictionary(Of String,String) =
                    _restclient.Execute (Of Dictionary(Of String,String))(reqClAt).Data
            Return ClAt
        Else
            Throw New NoArchiveOpened
        End If
    End Function

    ''' <summary>
    ''' Returns a list off all avilable roles
    ''' </summary>
    ''' <returns>List of roles string</returns>
    Public Function GetRoles() As List(Of String)
        If _currentArchiveId > - 1 Then
            Dim reqRoles As New RestRequest("roles")
            Dim respRoles = _restclient.Execute (Of List(Of String))(reqRoles)

            Dim roles As List(Of String) = respRoles.Data

            Return roles
        Else
            Throw New NoArchiveOpened
        End If
    End Function

    ''' <summary>
    ''' Search documents based on the given search creteria.
    ''' You are able to specify multiple Filres in the creteria object. 
    ''' The filers will be traded AND (this and this and this) 
    ''' </summary>
    ''' <param name="criteria">The filter creterias</param>
    ''' <returns></returns>
    Public Function SearchDocuments(criteria As SearchCriteria) As List(Of Document)
        If _currentArchiveId > - 1 Then
            Dim reqDocs As New RestRequest("searchDocuments")
            reqDocs.Method = Method.POST
            reqDocs.RequestFormat = DataFormat.Json
            reqDocs.AddBody(criteria.FilterList.ToArray)

            Dim resp = _restclient.Execute (Of List(Of Document))(reqDocs)

            Dim documents As List(Of Document) = resp.Data
            Return documents
        Else
            Throw New NoArchiveOpened
        End If
    End Function

    ''' <summary>
    ''' Get all avilable types valid in this archive for classification
    ''' </summary>
    ''' <returns></returns>
    Public Function GetTypes() As List(Of DocumentType)
        If _currentArchiveId > - 1 Then
            Dim reqTypes As New RestRequest("types")
            Dim respTypes = _restclient.Execute (Of List(Of DocumentType))(reqTypes)

            Dim documentTypes As List(Of DocumentType) = respTypes.Data

            Return documentTypes
        Else
            Throw New NoArchiveOpened
        End If
    End Function


    ''' <summary>
    ''' Download the file from the archive corospodending to the goiven docid.
    ''' Attention: This action will consume one API client call!
    ''' </summary>
    ''' <param name="docid">id of the document to retive</param>
    ''' <param name="version">Optinal the version of the document, if its not specified the latest version will be recived</param>
    ''' <returns></returns>
    Public Function GetDocumentFile(docid As Integer, Optional version As Integer = - 1) As DocumentFile
        If _currentArchiveId > - 1 Then
            Dim reqString As String = String.Empty

            If Version > - 1 Then
                reqString = String.Format("document/{0}/version/{1}", docid, Version)
            Else
                reqString = String.Format("document/{0}", docid)
            End If

            Dim reqDocFile As New RestRequest(reqString)
            Dim respDocFile = _restclient.Execute(reqDocFile)
            If respDocFile.StatusCode = Net.HttpStatusCode.OK Then
                Dim df As New DocumentFile
                df.Docid = docid
                df.Version = version
                df.Data = respDocFile.RawBytes
                Dim fileNameHeader As String =
                        (From h in respDocFile.Headers Where h.Name = "Content-Disposition" Select h).SingleOrDefault().
                        Value
                Dim m As match = Regex.Match(fileNameHeader, ".*filename=(.*)")
                df.FileName = m.Groups(1).Value

                Return df
            Else
                Throw New FileNotFoundException(respDocFile.StatusDescription)
            End If
        Else
            Throw New NoArchiveOpened
        End If
    End Function

    ''' <summary>
    ''' Creates a new file in the ecoDMS system.
    ''' Attention: This action will consume one API client call!
    ''' </summary>
    ''' <param name="document">the document to create in the ecoDMS system</param>
    ''' <param name="versionControlled">Optinal:Specify if the  File is under version control. Default:false</param>
    ''' <returns></returns>
    Public Function UploadDocumentFile(document As DocumentFile, Optional versionControlled As Boolean = False) _
        As Integer
        If _currentArchiveId > - 1 Then
            Dim reqString As String = "uploadFile"

            If versionControlled Then
                reqString &= "/true"
            Else
                reqString &= "/false"
            End If

            Dim reqDocFile As New RestRequest(reqString)
            reqDocFile.Method = Method.POST
            reqDocFile.AddFileBytes("file", document.Data, document.FileName)

            Dim respDocFile = _restclient.Execute(reqDocFile)
            If respDocFile.StatusCode = Net.HttpStatusCode.Created Then
                Return CInt(respDocFile.Content)
            Else
                Throw New EcoDmsException(respDocFile.StatusDescription)
            End If
        Else
            Throw New NoArchiveOpened
        End If
    End Function

    ''' <summary>
    ''' Retrives the document info for a given docId from the ecoDMS system
    ''' </summary>
    ''' <param name="docId">the doid to get informations for</param>
    ''' <returns></returns>
    Public Function GetDocumentInfo(docId) As Document
        If _currentArchiveId > - 1 Then
            Dim reqDocs As New RestRequest(String.Format("documentInfo/{0}", docId))
            Dim resp = _restclient.Execute (Of List(Of Document))(reqDocs)
            If resp.StatusCode = Net.HttpStatusCode.OK Then
                Dim documents as Document = resp.Data(0)
                Return documents
            Else
                Throw New Exception(resp.StatusDescription)
            End If

        Else
            Throw New NoArchiveOpened
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <returns></returns>
    Public Function ClassifyDocument(doc As Document) As Boolean
        If _currentArchiveId > - 1 Then
            Dim reqDocs As New RestRequest("classifyDocument")
            reqDocs.Method = Method.POST
            reqDocs.RequestFormat = DataFormat.Json
            reqDocs.AddBody(doc)
            Dim resp = _restclient.Execute(reqDocs)
            If resp.StatusCode = Net.HttpStatusCode.OK Then
                Return True
            Else
                Throw New Exception(resp.StatusDescription)
            End If

        Else
            Throw New NoArchiveOpened
        End If
    End Function

    ''' <summary>
    ''' Creates a new folder in the ecoDMS arcive. 
    ''' If the parent folder is nothing, a main folder will be created
    ''' </summary>
    ''' <param name="folderName">The name of the folder</param>
    ''' <param name="externalKey">The external Key of the folder(Foreign Key)</param>
    ''' <param name="buzzwords">The buzzwords, maped to the folder</param>
    ''' <param name="parentFolderId"> The id of the parent folder (for example 2.4)</param>
    ''' <returns>Afolder object with the oid of the new created folder></returns>
    Public Function CreateFolder(folderName As String, Optional externalKey As String = "",
                                 Optional buzzwords As String = "", Optional parentFolderId As String = Nothing) _
        As Folder
        If _currentArchiveId > - 1 Then

            Dim requestString As String = "createFolder"
            Dim cFolder As New Folder

            If String.IsNullOrEmpty(parentFolderId) = False Then
                requestString &= "/parent/" & parentFolderId
                cFolder.mainFolder = False
            Else
                cFolder.mainFolder = True
            End If

            With cFolder
                .foldername = folderName
                .externalKey = externalKey
                .buzzwords = buzzwords
            End With

            Dim reqDocs As New RestRequest(requestString)
            reqDocs.Method = Method.POST
            reqDocs.RequestFormat = DataFormat.Json
            reqDocs.AddBody(cFolder)
            Dim resp = _restclient.Execute(reqDocs)
            If resp.StatusCode = Net.HttpStatusCode.ok Then
                cFolder.oId = resp.Content
                Return cFolder
            Else
                Throw New Exception(resp.StatusDescription)
            End If

        Else
            Throw New NoArchiveOpened
        End If
    End Function

    ''' <summary>
    ''' Get a tubnail of the given docid document. 
    ''' The maximum height is 500 (default).
    ''' </summary>
    ''' <param name="docid">The docid of the documnet you want to retrive the thumbnail</param>
    ''' <param name="page">The page number, default =1, if page not exist an error will thrown</param>
    ''' <param name="height">The height of the thubmnail, maximum is 500</param>
    ''' <returns>A Thumbnail object with the name and the data as a byte array</returns>
    Public Function GetThumbNail(docid As Integer, Optional page As Integer = 1, Optional height As Integer = 500) _
        As ThumbNail
        If _currentArchiveId > - 1 Then

            Dim reqDocFile As New RestRequest(String.Format("thumbnail/{0}/page/{1}/height/{2}", docid, page, height))
            Dim respDocFile = _restclient.Execute(reqDocFile)
            If respDocFile.StatusCode = Net.HttpStatusCode.OK Then
                Dim df As New ThumbNail
                df.Docid = docid
                df.Page = page
                df.Data = respDocFile.RawBytes
                Dim fileNameHeader As String =
                        (From h in respDocFile.Headers Where h.Name = "Content-Disposition" Select h).SingleOrDefault().
                        Value
                Dim m As match = Regex.Match(fileNameHeader, ".*filename=(.*)")
                df.FileName = m.Groups(1).Value
                Return df
            Else
                Throw New FileNotFoundException(respDocFile.StatusDescription)
            End If
        Else
            Throw New NoArchiveOpened
        End If
    End Function
End Class
