
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Security.Permissions

'''based on source here:
''' https://code.google.com/p/bizhawk/source/browse/trunk/BizHawk.Util/FolderBrowserDialogEx.cs?r=4318
''' 


''' <summary>
''' Component wrapping access to the Browse For Folder common dialog box.
''' Call the ShowDialog() method to bring the dialog box up.
''' </summary>
Namespace MapInfoPhotoGPSPlotter
    Public NotInheritable Class ModifiedFolderBrowser
        Inherits Component
        Private Const MAX_PATH As Integer = 260

        Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr

        ' Root node of the tree view.
        Private m_startLocation As FolderID = FolderID.Desktop

        ' Browse info options.
        Private publicOptions As Integer = CInt(Win32API.Shell32.BffStyles.RestrictToFilesystem) Or CInt(Win32API.Shell32.BffStyles.RestrictToDomain)

        Private Const privateOptions As Integer = CInt(Win32API.Shell32.BffStyles.NewDialogStyle Or Win32API.Shell32.BffStyles.ShowTextBox)

        ' Description text to show.
        Public Description As String = "Please select a folder below:"

        ''' <summary>
        ''' Enum of CSIDLs identifying standard shell folders.
        ''' </summary>
        Public Enum FolderID
            Desktop = &H0
            Printers = &H4
            MyDocuments = &H5
            Favorites = &H6
            Recent = &H8
            SendTo = &H9
            StartMenu = &HB
            MyComputer = &H11
            NetworkNeighborhood = &H12
            Templates = &H15
            MyPictures = &H27
            NetAndDialUpConnections = &H31
        End Enum

        ''' <summary>
        ''' Helper function that returns the IMalloc interface used by the shell.
        ''' </summary>
        Private Shared Function GetSHMalloc() As Win32API.IMalloc
            Dim malloc As Win32API.IMalloc
            Win32API.Shell32.SHGetMalloc(malloc)
            Return malloc
        End Function

        ''' <summary>
        ''' Shows the folder browser dialog box.
        ''' </summary>
        Public Function ShowDialog() As DialogResult
            Return ShowDialog(Nothing)
        End Function


        Private Function callback(hwnd As IntPtr, uMsg As UInteger, lParam As IntPtr, lpData As IntPtr) As Integer
            Select Case uMsg
                Case 1
                    If True Then
                        Dim str As IntPtr = Marshal.StringToHGlobalUni(SelectedPath)
                        SendMessage(hwnd, (&H400 + 103), 1, str.ToInt32())
                        Marshal.FreeHGlobal(str)
                        Exit Select
                    End If

            End Select
            Return 0
        End Function


        ''' <summary>
        ''' Shows the folder browser dialog box with the specified owner window.
        ''' </summary>
        Public Function ShowDialog(owner As IWin32Window) As DialogResult
            Dim pidlRoot As IntPtr = IntPtr.Zero

            ' Get/find an owner HWND for this dialog.
            Dim hWndOwner As IntPtr

            If owner IsNot Nothing Then
                hWndOwner = owner.Handle
            Else
                hWndOwner = Win32API.GetActiveWindow()
            End If

            ' Get the IDL for the specific startLocation.
            Win32API.Shell32.SHGetSpecialFolderLocation(hWndOwner, CInt(m_startLocation), pidlRoot)

            If pidlRoot = IntPtr.Zero Then
                Return DialogResult.Cancel
            End If

            Dim mergedOptions As Integer = CInt(publicOptions) Or CInt(privateOptions)

            If (mergedOptions And CInt(Win32API.Shell32.BffStyles.NewDialogStyle)) <> 0 Then
                If System.Threading.ApartmentState.MTA = Application.OleRequired() Then
                    mergedOptions = mergedOptions And (Not CInt(Win32API.Shell32.BffStyles.NewDialogStyle))
                End If
            End If

            Dim pidlRet As IntPtr = IntPtr.Zero

            Try
                ' Construct a BROWSEINFO.
                Dim bi As New Win32API.Shell32.BROWSEINFO()
                Dim buffer As IntPtr = Marshal.AllocHGlobal(MAX_PATH)

                bi.pidlRoot = pidlRoot
                bi.hwndOwner = hWndOwner
                bi.pszDisplayName = buffer
                bi.lpszTitle = Description
                bi.ulFlags = mergedOptions
                bi.lpfn = New Win32API.Shell32.BFFCALLBACK(AddressOf callback)
                ' The rest of the fields are initialized to zero by the constructor.
                ' bi.lParam = IntPtr.Zero;    bi.iImage = 0;

                ' Show the dialog.
                pidlRet = Win32API.Shell32.SHBrowseForFolder(bi)

                ' Free the buffer you've allocated on the global heap.
                Marshal.FreeHGlobal(buffer)

                If pidlRet = IntPtr.Zero Then
                    ' User clicked Cancel.
                    Return DialogResult.Cancel
                End If

                ' Then retrieve the path from the IDList.
                Dim sb As New StringBuilder(MAX_PATH)
                If 0 = Win32API.Shell32.SHGetPathFromIDList(pidlRet, sb) Then
                    Return DialogResult.Cancel

                    ' Convert to a string.
                End If
            Finally
                Dim malloc As Win32API.IMalloc = GetSHMalloc()
                malloc.Free(pidlRoot)

                If pidlRet <> IntPtr.Zero Then
                    malloc.Free(pidlRet)
                End If
            End Try

            Return DialogResult.OK
        End Function

        ''' <summary>
        ''' Helper function used to set and reset bits in the publicOptions bitfield.
        ''' </summary>
        Private Sub SetOptionField(mask As Integer, turnOn As Boolean)
            If turnOn Then
                publicOptions = publicOptions Or mask
            Else
                publicOptions = publicOptions And Not mask
            End If
        End Sub

        ''' <summary>
        ''' Only return file system directories. If the user selects folders
        ''' that are not part of the file system, the OK button is unavailable.
        ''' </summary>
        <Category("Navigation")> _
        <Description("Only return file system directories. If the user selects folders " & "that are not part of the file system, the OK button is unavailable.")> _
        <DefaultValue(True)> _
        Public Property OnlyFilesystem() As Boolean
            Get
                Return (publicOptions And CInt(Win32API.Shell32.BffStyles.RestrictToFilesystem)) <> 0
            End Get
            Set(value As Boolean)
                SetOptionField(CInt(Win32API.Shell32.BffStyles.RestrictToFilesystem), value)
            End Set
        End Property

        ''' <summary>
        ''' Location of the root folder from which to start browsing. Only the specified
        ''' folder and any folders beneath it in the namespace hierarchy  appear
        ''' in the dialog box.
        ''' </summary>
        <Category("Navigation")> _
        <Description("Location of the root folder from which to start browsing. Only the specified " & "folder and any folders beneath it in the namespace hierarchy appear " & "in the dialog box.")> _
        <DefaultValue(GetType(FolderID), "0")> _
        Public Property StartLocation() As FolderID
            Get
                Return m_startLocation
            End Get
            Set(value As FolderID)
                Dim permission As New UIPermission(UIPermissionWindow.AllWindows) '.Demand()
                m_startLocation = value
            End Set
        End Property

        Public SelectedPath As String

    End Class





    Friend Class Win32API
        ' C# representation of the IMalloc interface.
        <InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000002-0000-0000-C000-000000000046")> _
        Public Interface IMalloc
            <PreserveSig> _
            Function Alloc(<[In]> cb As Integer) As IntPtr

            <PreserveSig> _
            Function Realloc(<[In]> pv As IntPtr, <[In]> cb As Integer) As IntPtr

            <PreserveSig> _
            Sub Free(<[In]> pv As IntPtr)

            <PreserveSig> _
            Function GetSize(<[In]> pv As IntPtr) As Integer

            <PreserveSig> _
            Function DidAlloc(pv As IntPtr) As Integer

            <PreserveSig> _
            Sub HeapMinimize()
        End Interface

        <DllImport("User32.DLL")> _
        Public Shared Function GetActiveWindow() As IntPtr
        End Function

        Public Class Shell32
            ' Styles used in the BROWSEINFO.ulFlags field.
            <Flags> _
            Public Enum BffStyles
                RestrictToFilesystem = &H1
                ' BIF_RETURNONLYFSDIRS
                RestrictToDomain = &H2
                ' BIF_DONTGOBELOWDOMAIN
                RestrictToSubfolders = &H8
                ' BIF_RETURNFSANCESTORS
                ShowTextBox = &H10
                ' BIF_EDITBOX
                ValidateSelection = &H20
                ' BIF_VALIDATE
                NewDialogStyle = &H40
                ' BIF_NEWDIALOGSTYLE
                BrowseForComputer = &H1000
                ' BIF_BROWSEFORCOMPUTER
                BrowseForPrinter = &H2000
                ' BIF_BROWSEFORPRINTER
                BrowseForEverything = &H4000
                ' BIF_BROWSEINCLUDEFILES
            End Enum

            ' Delegate type used in BROWSEINFO.lpfn field.
            Public Delegate Function BFFCALLBACK(hwnd As IntPtr, uMsg As UInteger, lParam As IntPtr, lpData As IntPtr) As Integer

            <StructLayout(LayoutKind.Sequential, Pack:=8)> _
            Public Structure BROWSEINFO
                Public hwndOwner As IntPtr
                Public pidlRoot As IntPtr
                Public pszDisplayName As IntPtr
                <MarshalAs(UnmanagedType.LPTStr)> _
                Public lpszTitle As String
                Public ulFlags As Integer
                <MarshalAs(UnmanagedType.FunctionPtr)> _
                Public lpfn As BFFCALLBACK
                Public lParam As IntPtr
                Public iImage As Integer
            End Structure

            <DllImport("Shell32.DLL")> _
            Public Shared Function SHGetMalloc(ByRef ppMalloc As IMalloc) As Integer
            End Function

            <DllImport("Shell32.DLL")> _
            Public Shared Function SHGetSpecialFolderLocation(hwndOwner As IntPtr, nFolder As Integer, ByRef ppidl As IntPtr) As Integer
            End Function

            <DllImport("Shell32.DLL")> _
            Public Shared Function SHGetPathFromIDList(pidl As IntPtr, Path As StringBuilder) As Integer
            End Function

            <DllImport("Shell32.DLL", CharSet:=CharSet.Auto)> _
            Public Shared Function SHBrowseForFolder(ByRef bi As BROWSEINFO) As IntPtr
            End Function
        End Class

    End Class
End Namespace