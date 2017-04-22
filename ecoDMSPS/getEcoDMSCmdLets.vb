Imports System.Management.Automation


<Cmdlet(VerbsCommon.Get,"ecoDMSFolders")>
Public Class GetEcoDmsCmdLets
      Inherits PSCmdlet

    Protected Overrides Sub ProcessRecord()
       WriteVerbose("Try to get Folders from ecoDMS")
         Dim eco As ecoDMSNet.EcoDmsClient = Me.SessionState.PSVariable.GetValue("ecoDMSClient") 
        If Not eco Is Nothing Then
            WriteObject(eco.GetFolders)
        Else
            Throw New Exception("No connection to ecoDMS, use Open-ecoDMSArchive cmdLet to connect first...")
        End If

   
    End Sub
End Class

<Cmdlet(VerbsCommon.Get,"ecoDMSRoles")>
Public Class GetEcoDmsRoles
      Inherits PSCmdlet

    Protected Overrides Sub ProcessRecord()
       WriteVerbose("Try to get Roles from ecoDMS")
         Dim eco As ecoDMSNet.EcoDmsClient = Me.SessionState.PSVariable.GetValue("ecoDMSClient") 
        If Not eco Is Nothing Then
            WriteObject(eco.GetRoles)
        Else
            Throw New Exception("No connection to ecoDMS, use Open-ecoDMSArchive cmdLet to connect first...")
        End If
    End Sub
End Class

<Cmdlet(VerbsCommon.Get,"ecoDMSStatus")>
Public Class GetEcoDmsStatus
      Inherits PSCmdlet

    Protected Overrides Sub ProcessRecord()
       WriteVerbose("Try to get Status from ecoDMS")
         Dim eco As ecoDMSNet.EcoDmsClient = Me.SessionState.PSVariable.GetValue("ecoDMSClient") 
        If Not eco Is Nothing Then
            WriteObject(eco.GetStatus)
        Else
            Throw New Exception("No connection to ecoDMS, use Open-ecoDMSArchive cmdLet to connect first...")
        End If
    End Sub
End Class

<Cmdlet(VerbsCommon.Get,"ecoDMSTypes")>
Public Class GetEcoDmsTypes
      Inherits PSCmdlet

    Protected Overrides Sub ProcessRecord()
       WriteVerbose("Try to get Types from ecoDMS")
         Dim eco As ecoDMSNet.EcoDmsClient = Me.SessionState.PSVariable.GetValue("ecoDMSClient") 
        If Not eco Is Nothing Then
            WriteObject(eco.GetTypes)
        Else
            Throw New Exception("No connection to ecoDMS, use Open-ecoDMSArchive cmdLet to connect first...")
        End If
    End Sub
End Class

<Cmdlet(VerbsCommon.Get,"ecoDMSAttributes")>
Public Class GetEcoDmsAttributes
      Inherits PSCmdlet

    Protected Overrides Sub ProcessRecord()
       WriteVerbose("Try to get Attributes from ecoDMS")
         Dim eco As ecoDMSNet.EcoDmsClient = Me.SessionState.PSVariable.GetValue("ecoDMSClient") 
        If Not eco Is Nothing Then
            WriteObject(eco.GetClassifyAttributes)
        Else
            Throw New Exception("No connection to ecoDMS, use Open-ecoDMSArchive cmdLet to connect first...")
        End If
    End Sub
End Class