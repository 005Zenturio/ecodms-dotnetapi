# ecodms-dotnetapi
dotnet api and powershell module implementation for ecodms

To use the module in powershell, you have to load the module first.

1.) start the powershell.exe

2.) import the module like: Import-Module .\ecoDMSPS.dll -Verbose

3.) To connect to EcoDMS use the Open-ecoDMSArchive cmdlet:

    Open-ecoDMSArchive -url http://192.168.1.20:8180 -UserName ecodms -Password ecodms -ArchiveId 1	

The following commands are avilable:

    Create a new folder                                       : New-ecoDMSFolder 

    Download a document(Attention: one API call will be used) : Get-ecoDMSDocument

    Upload a Document(Attention: one API call will be used)	  : New-ecoDMSDocument

    Classify a document                                       : Set-ecoDMSClassifyDocument

    Get the document't classification                         : Get-ecoDMSClassifiecation

    Search for documents in ecoDMS                            : Search-ecoDMSDocuments
  
    Connect to a ecoDMS archive                               : Open-ecoDMSArchive

    Dissconect from a ecoDMS archive                          : Close-ecoDMSArchive

    List all folders in the archive                           : Get-ecoDMSFolders

    List all roles in the current archive                     : Get-ecoDMSRoles

    List all status in the current archive                    : Get-ecoDMSStatus

    List all types in the current archive                     : Get-ecoDMSTypes

    List all known and non-default classification atrtributes : Get-ecoDMSAttributes

    The parameters of each command can be displayd with get-help <command>.

3th party libaries used in this project:

restsharp -> http://restsharp.org/ || https://github.com/restsharp/RestSharp/blob/master/LICENSE.txt
