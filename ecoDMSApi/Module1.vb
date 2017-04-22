
Module Module1

    Sub Main()

        Dim ecoClient As New ecoDMSNet.EcoDmsClient("http://192.168.1.20:8180","ecodms","ecodms")

        ecoClient.Connect(1)
        Console.WriteLine("Folders---")
        For each f As ecoDMSNet.Folder in ecoClient.GetFolders
            Console.WriteLine("{0}=>{1}",f.OId,f.Foldername)
        Next

        Dim sc As new ecoDMSNet.SearchCriteria

        sc.AddFilter("docid",">","0")

            Console.WriteLine("SearchDocuments---")
        Dim docs = ecoClient.SearchDocuments(sc)

        For each d In docs
               Console.WriteLine("{0}=>{1}",d.DocId,d.ClassifyAttributes("bemerkung"))

        Next
            Console.WriteLine("Roles---")
          Dim roles = ecoClient.GetRoles

        For each d In roles
               Console.WriteLine("{0}=>{1}",d,d)

        Next
            Console.WriteLine("Status---")
         Dim status = ecoClient.GetStatus

        For each d In status
               Console.WriteLine("{0}=>{1}",d.Id,d.Name)

        Next
            Console.WriteLine("Types---")
         Dim types = ecoClient.GetTypes

        For each d In types
               Console.WriteLine("{0}=>{1}",d.Id,d.Name)

        Next

            Console.WriteLine("Attributes---")
              Dim atr = ecoClient.GetClassifyAttributes

        For each d In atr
               Console.WriteLine("{0}=>{1}",d.Key,d.Value)

        Next

    '    Dim doc = ecoClient.GetDocumentFile(99)
        '  My.Computer.FileSystem.WriteAllBytes(doc.FileName,doc.Data,False)


     '   Dim updoc = ecoDMSNet.DocumentFile.FromFile("M:\Dropbox\IncadeaDE\Dokumente\Dokumentationen\automotive.dvm\Funktionsbeschreibung_dvm_1.5.docx")

      'Dim updocnr As Integer = ecoClient.UploadDocumentFile(updoc,
      '      Console.WriteLine("ClassifyDocument---")
      '  Dim updocinfo = ecoClient.GetDocumentInfo(3)
      '         Console.WriteLine("{0}=>{1}",updocinfo.DocId,updocinfo.ClassifyAttributes(ecoDMSNet.DefaultClassifyAttributes.ECO_CLASSIFY_BEMERKUNG))
      '      updocinfo.ClassifyAttributes(ecoDMSNet.DefaultClassifyAttributes.ECO_CLASSIFY_FOLDER) ="4.3"
      '      updocinfo.ClassifyAttributes(ecoDMSNet.DefaultClassifyAttributes.ECO_CLASSIFY_MAINFOLDER) = "4"
      '      updocinfo.ClassifyAttributes(ecoDMSNet.DefaultClassifyAttributes.ECO_CLASSIFY_STATUS) = "3"
      '  ecoClient.ClassifyDocument(updocinfo)

      ' '      Console.WriteLine("Create Main Folder---")
      ' 'Dim fo =  ecoClient.CreateFolder("RootFolderTest")
       '   Console.WriteLine("{0}=>{1}",fo.oId,fo.foldername)

       ' ecoClient.CreateFolder(parentFolderId:=)
       ' ecoClient.CreateFolder("Test 2","1260.120","Test, Bla, Blub",fo.oId)
       ' ecoClient.CreateFolder("Test 3","5245.120","Test, Bla, Blub",fo.oId)

     Dim tu =   ecoClient.GetThumbNail(3)
        My.Computer.FileSystem.WriteAllBytes(tu.FileName,tu.Data,False)    
        ecoClient.Disconnect
        'Dim client As New RestClient("http://192.168.1.20:8180")
        'Dim request As New RestRequest("/api/test")
        'client.Authenticator = New RestSharp.Authenticators.HttpBasicAuthenticator("ecodms","ecodms")
        'Dim resp =client.Execute(request)
        'Console.WriteLine(resp.Content)
        'Dim request1 As New RestRequest("api/connect/1")
        'Dim resp1 =client.Execute(request1)
        '   Console.WriteLine(resp1.Content)

        '    Dim requestFolders As New RestRequest("api/folders")
        'Dim folders As List(Of Folder)  =client.Execute(Of List(Of Folder))(requestFolders).Data


        '  Dim request2 As New RestRequest("api/disconnect")
        'Dim resp2 =client.Execute(request2)


           Console.ReadLine
    End Sub

End Module


