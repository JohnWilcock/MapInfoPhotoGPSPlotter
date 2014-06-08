'/*****************************************************************************
'*      Author JWilcock 2014
'*      GPSPlotter
'*****************************************************************************


Imports System
Imports System.Windows.Forms
Imports System.Threading
Imports System.Xml
Imports System.Drawing
Imports System.IO
Imports System.Runtime.InteropServices
Imports MapInfo.MiPro.Interop
Imports System.Text.RegularExpressions
Imports System.Windows.Forms.DataVisualization.Charting



Namespace MapInfoPhotoGPSPlotter
    Partial Public Class Dlg

        Inherits UserControl
        ' some string in xml file 
        'Private Const STR_NAME As String = "Name"
        Private Const STR_ROOT As String = "root"
        Private Const STR_DIALOG As String = "Dialog"
        Private Const STR_NAMEDVIEWS As String = "MapInfoPhotoGPSPlotter"
        'Private Const STR_VIEWS As String = "Views"
        Private Const STR_PATH_DIALOG As String = "/MapInfoPhotoGPSPlotter/Dialog"
        Private Const STR_PATH_ROOT_FOLDER As String = "/MapInfoPhotoGPSPlotter/Views"
        Private Const STR_LEFT As String = "Left"
        Private Const STR_TOP As String = "Top"
        Private Const STR_WIDTH As String = "Width"
        Private Const STR_HEIGHT As String = "Height"

        ' The controller class which uses this dialog class ensures 
        ' * a single instance of this dialog class. However different 
        ' * running instance of MapInfo Professional will have their 
        ' * own copy of dll. To make sure that read/write from/to xml 
        ' * file which is going to be a single file on the disk, is 
        ' * smooth and we have the synchronized access to the xml file, 
        ' * the Mutexes will be used. 
        ' 

        ' Name of the mutex 
        Private sXMLFile As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\MapInfo\MapInfo\nviews.xml"
        Private dialogLeft As Integer, dialogTop As Integer, dialogWidth As Integer, dialogHeight As Integer
        ' flag indicating whether it is the first time the form is being loaded
        Dim firstLoad As Boolean = True
        Private _controller As Controller  ' represents the window that owns this dialog (main MI Pro window)

        Dim isMapper As Integer

        ''' <summary> 
        ''' Construction 
        ''' </summary> 
        ''' 
        Public Shared photoList As New List(Of Photo)
        Public tabfile As String = ""


        Public Sub New()
            InitializeComponent()
            'mut = New Mutex(False, mutexName)

            'check for blank file  - this is used as a flag to see if MapInfoPhotoGPSPlotter has been installed, if not it is autoregestered by the MBX
            CreateBlank()
            CreateSRID()

            ComboBox1.SelectedIndex = 0
            ComboBox2.SelectedIndex = 1
            Label3.Text = "WGS 84 - Latlon"
            CoordinateSystemPicker1.ChosenCoordSystemEPSG = "4326"
            InteropHelper.theDlg = Me
        End Sub

        ''' <summary>
        ''' Parameterised Construction
        ''' <param name="controller"></param>
        ''' </summary>
        Public Sub New(ByVal controller As Controller)
            Me.New()
            _controller = controller
        End Sub




#Region "[DIALOG EVENT HANDLERS]"
        ''' <summary> 
        ''' Named View dialog Load event handler 
        ''' </summary> 
        ''' <param name="sender"></param> 
        ''' <param name="e"></param> 
        Private Sub NViewDlg_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            'CreateBlank()
            'set smaller font size
            Dim theFont As Font = New Font(Me.Font.FontFamily, 7, FontStyle.Regular)
            TextBox2.Font = theFont

            If firstLoad = True Then
                firstLoad = False

                If dialogWidth >= Me.MinimumSize.Width AndAlso dialogWidth <= Screen.PrimaryScreen.WorkingArea.Width Then
                    Me.Width = dialogWidth
                End If
                If dialogHeight >= Me.MinimumSize.Height AndAlso dialogHeight <= Screen.PrimaryScreen.WorkingArea.Height Then
                    Me.Height = dialogHeight
                End If
                If dialogLeft > -Me.Width AndAlso dialogLeft < Screen.PrimaryScreen.WorkingArea.Width Then
                    Me.Left = dialogLeft
                End If
                If dialogTop > -Me.Top AndAlso dialogTop < Screen.PrimaryScreen.WorkingArea.Height Then
                    Me.Top = dialogTop
                End If
            End If
        End Sub
        ' This call to the WIN32 API function SetFocus is used in NViewDlg_FormClosing below
        <DllImport("User32.dll")> _
        Private Shared Function SetFocus(ByVal hWnd As IntPtr)
        End Function




#End Region


        Public Sub CloseDockWindow()
            ''Write out the XML file that stores the Named Views info
            _controller.DockWindowClose()
        End Sub
        ''' <summary>
        ''' Set the dialog position and docking state 
        ''' </summary>
        Public Sub SetDockPosition()
            _controller.SetDockWindowPositionFromFile()
        End Sub




#Region "[HELPERS]"

        Public Shared Sub CreateBlank()
            'create blank tab is it does not exist
            Dim MIfolderDir As String = InteropServices.MapInfoApplication.Eval("GetFolderPath$(-3)")
            If Not System.IO.File.Exists(MIfolderDir & "\MapInfoPhotoGPSPlotter.tab") Then
                InteropServices.MapInfoApplication.Do("Create Table blank(blank Char(30)) file " & Chr(34) & MIfolderDir & "\MapInfoPhotoGPSPlotter.tab" & Chr(34))
                InteropServices.MapInfoApplication.Do("open table " & Chr(34) & MIfolderDir & "\MapInfoPhotoGPSPlotter.tab" & Chr(34) & " as MapInfoPhotoGPSPlotter hide readonly")
                InteropServices.MapInfoApplication.Do("create map for blank CoordSys Earth Projection 8, 79, " & Chr(34) & "m" & Chr(34) & ", -2, 49, 0.9996012717, 400000, -100000 Bounds (-7845061.1011, -15524202.1641) (8645061.1011, 4470074.53373)")
                InteropServices.MapInfoApplication.Do("close table blank")
            End If
        End Sub

        Public Shared Sub CreateSRID()
            'create blank tab is it does not exist
            Dim MIfolderDir As String = InteropServices.MapInfoApplication.Eval("GetFolderPath$(-3)")
            If Not System.IO.File.Exists(MIfolderDir & "\SRID.csv") Then
                My.Computer.FileSystem.WriteAllText(MIfolderDir & "\" & "SRID.csv", My.Resources.SRID, False)
            End If
        End Sub

#End Region



        'functions called from MapBasic ***************************************

        Public Shared Function About() As Boolean
            Dim AB As New AboutBox1
            AB.ShowDialog()
            Return True
        End Function






        '**********************************************************************



        Private Sub Button4_Click(sender As Object, e As EventArgs)
            Dim AB As New AboutBox1
            AB.ShowDialog()
        End Sub

        Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
            TabPage2.Show()
            getGPSFiles()
            createTabFile(getFileLocation)
        End Sub

        Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
            TabPage2.Show()
            getGPSFolder()
            createTabFile(getFileLocation)
        End Sub


        Sub addMapper(ByVal fileLocation As String, ByVal tablename As String)

            Select Case ComboBox1.SelectedIndex
                Case 0
                    'new
                    InteropServices.MapInfoApplication.Do(" Map from " & tablename)
                Case 1
                    'front
                    'is front win a mapper ?
                    If InteropServices.MapInfoApplication.Eval("windowinfo(frontwindow(),3)") <> 1 Then
                        Exit Sub
                    End If
                    InteropServices.MapInfoApplication.Do("add Map window frontwindow() layer " & tablename)
                Case 2
                    'none
            End Select



        End Sub

        Sub increaseIncrement(ByVal currentValue As Integer, ByVal maxVal As Integer)
            Dim ProgressValue As Double = Math.Floor((currentValue / maxVal) * 100)
            If ProgressValue > 100 Then
                Exit Sub
            Else
                ProgressBar1.Value = ProgressValue
            End If

        End Sub


        Sub getGPSFiles()
            clear()

            'set file open dialog
            Dim OFD As New OpenFileDialog
            Dim thePhoto As Bitmap
            Dim count As Integer = 1
            OFD.Filter = "Image Files (*.jpg)|*.jpg|All Files (*.*)|*.*"
            OFD.FilterIndex = 1
            OFD.Title = "Open Image File/s"
            OFD.Multiselect = True

            If OFD.ShowDialog = Windows.Forms.DialogResult.OK Then

                'split filesnames out
                Dim allFiles() As String
                allFiles = OFD.FileNames

                For Each item As String In allFiles

                    'check for gps info
                    If isGPS(Bitmap.FromFile(item), item) Then
                        'proccessing text update
                        Label5.Text = "Processing " & count & "/" & allFiles.Length & " ..." & Path.GetFileNameWithoutExtension(item)

                        'get info and place into photo class > put in list
                        thePhoto = Bitmap.FromFile(item)
                        photoList.Add(imageInfo(thePhoto, item))
                        thePhoto.Dispose()

                        'prog bar update
                        increaseIncrement(count, allFiles.Length)
                        count = count + 1
                    End If
                Next

            End If
        End Sub


        Sub getGPSFolder()
            clear()
            Dim MBFD As New FolderBrowserDialog
            Dim thePhoto As Bitmap
            If MBFD.ShowDialog = Windows.Forms.DialogResult.OK Then

            Else
                Exit Sub
            End If


            'cycle through files
            Dim fileEntries As String() = Directory.GetFiles(MBFD.SelectedPath)
            ' Process the list of files found in the directory. 
            For Each fileName As String In fileEntries
                If fileName.Substring(fileName.Length - 3, 3).ToUpper = "JPG" Then
                    If isGPS(Bitmap.FromFile(fileName), fileName) Then
                        'get info and place into photo class > put in list
                        thePhoto = Bitmap.FromFile(fileName)
                        photoList.Add(imageInfo(thePhoto, fileName))
                        thePhoto.Dispose()

                    End If
                End If

            Next

        End Sub

        Sub clear()
            photoList.Clear()
        End Sub

        Function isGPS(ByVal photo As Bitmap, ByVal filename As String) As Boolean
            Dim byte_property_id As Byte()
            If photo.PropertyIdList.Length = 0 Then Return False
            byte_property_id = photo.GetPropertyItem(2).Value
            'Latitude degrees minutes and seconds (rational)
            Dim _1 As Double = System.BitConverter.ToInt32(byte_property_id, 0) / System.BitConverter.ToInt32(byte_property_id, 4)
            Dim _2 As Double = System.BitConverter.ToInt32(byte_property_id, 8) / System.BitConverter.ToInt32(byte_property_id, 12)
            Dim _3 As Double = System.BitConverter.ToInt32(byte_property_id, 16) / System.BitConverter.ToInt32(byte_property_id, 20)

            If _1 + _2 + _3 > 0 Then
                Return True
            Else
                TextBox2.Text = TextBox2.Text & vbNewLine & filename & " No GPS info"
                Return False
            End If
        End Function

        Function imageInfo(ByVal thePhoto As Bitmap, ByVal filepath As String) As Photo
            imageInfo = New Photo

            Dim byte_property_id As Byte()
            Dim ascii_string_property_id As String
            Dim prop_type As String
            Dim selected_image As System.Drawing.Bitmap
            Dim property_ids() As Integer
            Dim scan_property As Integer
            Dim counter As Integer
            Dim degrees As Double
            Dim minutes As Double
            Dim seconds As Double

            selected_image = thePhoto
            property_ids = selected_image.PropertyIdList

            imageInfo.filename = Path.GetFileNameWithoutExtension(filepath)
            imageInfo.filepath = filepath
            imageInfo.localX = 0
            imageInfo.localY = 0

            For Each scan_property In property_ids
                counter = counter + 1
                byte_property_id = selected_image.GetPropertyItem(scan_property).Value
                prop_type = selected_image.GetPropertyItem(scan_property).Type
                'MsgBox(scan_property.ToString)
                If scan_property = 1 Then
                    'Latitude North or South
                    ascii_string_property_id = System.Text.Encoding.ASCII.GetString(byte_property_id)
                    imageInfo.NS = ascii_string_property_id


                ElseIf scan_property = 2 Then
                    'Latitude degrees minutes and seconds (rational)
                    degrees = System.BitConverter.ToInt32(byte_property_id, 0) / System.BitConverter.ToInt32(byte_property_id, 4)
                    minutes = System.BitConverter.ToInt32(byte_property_id, 8) / System.BitConverter.ToInt32(byte_property_id, 12)
                    seconds = System.BitConverter.ToInt32(byte_property_id, 16) / System.BitConverter.ToInt32(byte_property_id, 20)

                    imageInfo.dlat = degrees + (minutes / 60) + (seconds / 3600)
                    imageInfo.lat = degrees & " " & minutes & " " & seconds

                    If Asc(imageInfo.NS) = 83 Then imageInfo.dlat = (imageInfo.dlat * -1)

                ElseIf scan_property = 3 Then
                    'Longitude East or West
                    ascii_string_property_id = System.Text.Encoding.ASCII.GetString(byte_property_id)
                    imageInfo.EW = ascii_string_property_id


                ElseIf scan_property = 4 Then
                    'Longitude degrees minutes and seconds (rational)
                    degrees = System.BitConverter.ToInt32(byte_property_id, 0) / System.BitConverter.ToInt32(byte_property_id, 4)
                    minutes = System.BitConverter.ToInt32(byte_property_id, 8) / System.BitConverter.ToInt32(byte_property_id, 12)
                    seconds = System.BitConverter.ToInt32(byte_property_id, 16) / System.BitConverter.ToInt32(byte_property_id, 20)

                    imageInfo.dlon = degrees + (minutes / 60) + (seconds / 3600)
                    imageInfo.lon = degrees & " " & minutes & " " & seconds

                    If Asc(imageInfo.EW) = 87 Then
                        imageInfo.dlon = (imageInfo.dlon * -1)
                    End If
                    'MsgBox(Asc(imageInfo.EW))
                    'ElseIf scan_property = 18 Then
                    ' 'Datum used at GPS acquisition (ascii)
                    '   ascii_string_property_id = System.Text.Encoding.ASCII.GetString(byte_property_id)
                    '  MsgBox("GPS Datum= " & ascii_string_property_id)

                ElseIf scan_property = 17 Then '24?
                    imageInfo.picDirection = System.BitConverter.ToInt32(byte_property_id, 0) / System.BitConverter.ToInt32(byte_property_id, 4)

                ElseIf scan_property = 306 Then 'GetPropertyItem(&H132)
                    'datetime
                    Dim sdate As String = System.Text.Encoding.UTF8.GetString(byte_property_id).Trim()
                    Dim secondhalf As String = sdate.Substring(sdate.IndexOf(" "), (sdate.Length - sdate.IndexOf(" ")))
                    Dim firsthalf As String = sdate.Substring(0, 10)
                    firsthalf = firsthalf.Replace(":", "-")
                    sdate = firsthalf & secondhalf
                    imageInfo.picDate = DateTime.Parse(sdate)

                End If

            Next

        End Function

        Function getFileLocation() As String
            If ComboBox2.Text = "Cosmetic Layer" Then
                Return "Cosmetic"
            End If

            Dim SFA As New SaveFileDialog

            'new file
            If ComboBox2.SelectedIndex = 2 Then
                'if auto new file
                getFileLocation = Path.Combine(InteropServices.MapInfoApplication.Eval("GetFolderPath$(5)"), "GPS.tab")

                Dim count As Integer = 1
                While File.Exists(getFileLocation)
                    getFileLocation = Path.Combine(InteropServices.MapInfoApplication.Eval("GetFolderPath$(5)"), "GPS_" & count & ".tab")
                    count = count + 1
                End While

                Return getFileLocation
            End If

            If TextBox1.Text = "" Then
                'no file specified

                If SFA.ShowDialog() = DialogResult.OK Then
                    Return SFA.FileName
                Else
                    Return "-1"
                End If

            Else
                'file specified
                If Directory.Exists(Path.GetDirectoryName(TextBox1.Text)) Then
                    Return TextBox1.Text
                Else
                    SFA.Title = "invalid new file specified - please select another"
                    If SFA.ShowDialog() = DialogResult.OK Then
                        Return SFA.FileName
                    Else
                        Return "-1"
                    End If

                End If
            End If


        End Function

        Sub createTabFile(ByVal fileLocation As String)
            If fileLocation = "-1" Then
                TextBox2.Text = TextBox2.Text & vbCrLf & "No valid output file location specified"
                Exit Sub
            End If

            Dim tableName As String = ""
            Dim pointString As String = ""
            Dim points As Double()
            InteropServices.MapInfoApplication.Do("dim RotatedObject as symbol")

            'get coord system
            'default wgs 84
            Dim coord As String = InteropServices.MapInfoApplication.Eval("EPSGToCoordSysString$(" & Chr(34) & "EPSG:" & CoordinateSystemPicker1.ChosenCoordSystemEPSG & Chr(34) & ")")

            Select Case ComboBox2.SelectedIndex
                Case 1, 2

                    'new file
                    'get a free table name
                    tableName = Path.GetFileNameWithoutExtension(fileLocation)
                    'check for existing table of same name
                    Dim count As Integer = 1
                    While tableNameExists(tableName)
                        tableName = tableName & count
                        count = count + 1
                    End While


                    'create file
                    InteropServices.MapInfoApplication.Do("Create Table " & tableName & " ( FileName char(100),lat char(20),lon char(20),decimal_lat float, decimal_lon float,localX float,localY float,path char(250),direction float,PhotoDate char(50) ) file " & Chr(34) & fileLocation & Chr(34))
                    InteropServices.MapInfoApplication.Do("Create map for " & tableName & " " & coord)


                    'add points
                    For Each item As Photo In photoList

                        'add object to map
                        'MakeFontSymbol( ) 
                        InteropServices.MapInfoApplication.Do("Set Style Symbol MakeFontSymbol(135,RGB( 255, 1, 1 ),24," & Chr(34) & "Wingdings 3" & Chr(34) & ",32," & item.picDirection & ")")

                        'set coord
                        'InteropServices.MapInfoApplication.Do("Set CoordSys Earth")
                        InteropServices.MapInfoApplication.Do("Set " & coord)
                        points = convertCoords(item.dlon, item.dlat)
                        item.localX = points(0)
                        item.localY = points(1)


                        'add point
                        InteropServices.MapInfoApplication.Do("Insert Into " & tableName & " (Obj, FileName, lat,lon,decimal_lat,decimal_lon,localX,localY,path,direction,Photodate) Values(CreatePoint(" & item.localX & ", " & item.localY & ")," & Chr(34) & item.filename & Chr(34) & "," & Chr(34) & item.lat & Chr(34) & "," & Chr(34) & item.lon & Chr(34) & "," & item.dlat & "," & item.dlon & "," & item.localX & "," & item.localY & "," & Chr(34) & item.filepath & Chr(34) & "," & item.picDirection & "," & Chr(34) & item.picDate & Chr(34) & ")")
                        TextBox2.Text = TextBox2.Text & vbNewLine & "plotting ..." & item.filename & ": " & item.dlon & ", " & item.dlat
                        TextBox2.Text = TextBox2.Text & vbNewLine & item.filename & " Done"
                    Next

                    'save table
                    InteropServices.MapInfoApplication.Do("Commit Table " & tableName)

                Case 0
                    'cosmetic
                    'check for mapper win
                    'is front win a mapper ?
                    If InteropServices.MapInfoApplication.Eval("windowinfo(frontwindow(),3)") <> 1 Then
                        TextBox2.Text = TextBox2.Text & vbNewLine & " no front mapper win for cosmetic layer"
                        TabPage2.Show()
                        Exit Sub
                    End If

                    'get cosmeticN name
                    Dim cosmeticN As String = InteropServices.MapInfoApplication.Eval("windowinfo(frontwindow(),10)")

                    'get mapper coord - and convert and set to this
                    coord = InteropServices.MapInfoApplication.Eval("mapperinfo(frontwindow(),17)")
                    'set coord picker to this
                    Dim a As String = "CoordSysStringToEPSG(" & coord & ")"
                    InteropServices.MapInfoApplication.Do("dim cs as string")
                    InteropServices.MapInfoApplication.Do("cs = CoordSysStringToEPSG(cs)")
                    CoordinateSystemPicker1.ChosenCoordSystemEPSG = InteropServices.MapInfoApplication.Eval("cs")


                    TextBox2.Text = TextBox2.Text & vbNewLine & "mapper = " & CoordinateSystemPicker1.ChosenCoordSystemEPSG
                    CoordinateSystemPicker1.ChosenCoordSystem = InteropServices.MapInfoApplication.Eval("CoordSysName$(EPSGToCoordSysString$(" & Chr(34) & "EPSG:" & CoordinateSystemPicker1.ChosenCoordSystemEPSG & Chr(34) & "))")
                    TextBox2.Text = TextBox2.Text & vbNewLine & "converting coord system to cosmetic mapper = " & CoordinateSystemPicker1.ChosenCoordSystem

                    'add graphic objects only
                    For Each item As Photo In photoList
                        'MakeFontSymbol( ) 
                        InteropServices.MapInfoApplication.Do("Set Style Symbol MakeFontSymbol(135,RGB( 255, 1, 1 ),24," & Chr(34) & "Wingdings 3" & Chr(34) & ",32," & item.picDirection & ")")

                        'set coord
                        InteropServices.MapInfoApplication.Do("Set " & coord)
                        points = convertCoords(item.dlon, item.dlat)
                        item.localX = points(0)
                        item.localY = points(1)

                        InteropServices.MapInfoApplication.Do("Insert Into " & cosmeticN & " (Obj) Values(CreatePoint(" & item.localX & ", " & item.localY & "))")
                        TextBox2.Text = TextBox2.Text & vbNewLine & item.filename & " Done (obj only)"
                    Next

                Case 2
                    'new auto

            End Select
            addMapper(fileLocation, tableName)

            'zoom to points
            If CheckBox1.Checked Then
                InteropServices.MapInfoApplication.Do("Set Map Zoom Entire Layer 1") 'will always be top layer
            End If
        End Sub

        Function createPointString(ByVal x As Double, ByVal y As Double) As String
            createPointString = "CreatePoint(" & x & ", " & y & ")"
        End Function


        Function convertCoords(ByVal GridX As Double, ByVal GridY As Double) As Double()
            Dim otherCoodSystem As ProjNet.CoordinateSystems.ICoordinateSystem = SridReader.GetCSbyID(CoordinateSystemPicker1.ChosenCoordSystemEPSG)
            Dim WGS84 As ProjNet.CoordinateSystems.ICoordinateSystem = SridReader.GetCSbyID(4326)

            Dim ctfac As New ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory()
            Dim trans As ProjNet.CoordinateSystems.Transformations.ICoordinateTransformation = ctfac.CreateFromCoordinateSystems(WGS84, otherCoodSystem)

            'converts coords
            Dim fromPoint As Double() = New Double() {GridX, GridY}
            convertCoords = trans.MathTransform.Transform(fromPoint)

        End Function


        Function tableNameExists(ByVal tableName As String) As Boolean
            For i As Integer = 1 To InteropServices.MapInfoApplication.Eval("numtables()")
                If InteropServices.MapInfoApplication.Eval("tableinfo(" & i & ",1)") = tableName Then
                    Return True
                End If
            Next
            Return False
        End Function


        Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
            'new file
            Dim SFA As New SaveFileDialog
            If SFA.ShowDialog() = DialogResult.OK Then

                If SFA.FileName.Substring(SFA.FileName.Length - 4, 4).ToUpper <> ".TAB" Then
                    Path.Combine(SFA.FileName, ".tab")
                End If

            End If
            TextBox1.Text = SFA.FileName
        End Sub




        Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
            If ComboBox2.SelectedIndex = 0 Then 'cosmetic
                ComboBox1.SelectedIndex = 1
                ComboBox1.Enabled = False
                CheckBox2.Enabled = False

            Else
                ComboBox1.Enabled = True
                CheckBox2.Enabled = True
            End If

            If ComboBox2.SelectedIndex = 1 Then
                TextBox1.Visible = True
                Label4.Visible = True
                Button2.Visible = True
            Else
                TextBox1.Visible = False
                Label4.Visible = False
                Button2.Visible = False

            End If
        End Sub

        Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
            If ComboBox1.SelectedIndex = 1 Then 'front mapper
                CheckBox1.Enabled = True
            Else
                CheckBox1.Enabled = False
            End If
        End Sub

        Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
            CoordinateSystemPicker1.Visible = True
        End Sub

        Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
            If Button1.Enabled = True Then
                CheckBox2.Checked = False
                Button1.Enabled = False
                Label3.Visible = False
                CoordinateSystemPicker1.ChosenCoordSystemEPSG = "4326"
                Label3.Text = "WGS 84 - Latlon"
            Else
                CheckBox2.Checked = True
                Button1.Enabled = True
                Label3.Visible = True
            End If
        End Sub

    End Class




    Public Class Photo
        'all properties extracted per photo
        Private _dlat As Double
        Public Property dlat As Double
            Get
                Return _dlat
            End Get
            Set(ByVal value As Double)
                _dlat = value
            End Set
        End Property

        Private _lat As String
        Public Property lat As String
            Get
                Return _lat
            End Get
            Set(ByVal value As String)
                _lat = value
            End Set
        End Property

        Private _NS As String
        Public Property NS As String
            Get
                Return _NS
            End Get
            Set(ByVal value As String)
                _NS = value
            End Set
        End Property

        Private _EW As String
        Public Property EW As String
            Get
                Return _EW
            End Get
            Set(ByVal value As String)
                _EW = value
            End Set
        End Property

        Private _lon As String
        Public Property lon As String
            Get
                Return _lon
            End Get
            Set(ByVal value As String)
                _lon = value
            End Set
        End Property

        Private _dlon As Double
        Public Property dlon As Double
            Get
                Return _dlon
            End Get
            Set(ByVal value As Double)
                _dlon = value
            End Set
        End Property

        Private _localX As String
        Public Property localX As String
            Get
                Return _localX
            End Get
            Set(ByVal value As String)
                _localX = value
            End Set
        End Property

        Private _localY As String
        Public Property localY As String
            Get
                Return _localY
            End Get
            Set(ByVal value As String)
                _localY = value
            End Set
        End Property

        Private _filename As String
        Public Property filename As String
            Get
                Return _filename
            End Get
            Set(ByVal value As String)
                _filename = value
            End Set
        End Property

        Private _filepath As String
        Public Property filepath As String
            Get
                Return _filepath
            End Get
            Set(ByVal value As String)
                _filepath = value
            End Set
        End Property

        Private _picDate As Date
        Public Property picDate As Date
            Get
                Return _picDate
            End Get
            Set(ByVal value As Date)
                _picDate = value
            End Set
        End Property

        Private _picDirection As Double
        Public Property picDirection As Double
            Get
                Return _picDirection
            End Get
            Set(ByVal value As Double)
                _picDirection = value
            End Set
        End Property
    End Class


End Namespace