Imports System.Collections.Generic
Imports System.Text
Imports ProjNet.CoordinateSystems
Imports ProjNet.Converters
Imports MapInfo.MiPro.Interop
Imports MapInfoPhotoGPSPlotter.InteropHelper

Public Class CoordinateSystemPicker

    Public ChosenCoordSystem As String
    Public WithEvents ChosenCoordSystemEPSG As String
    Public WithEvents ChosenCoordSystemWKT As String
    Public WithEvents ChosenCoordMIString As String
    Public ChosenICoordSystem As ICoordinateSystem

    Public WKTList As New List(Of SridReader.WKTstring)

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        'set to wgs 84 epsg 4326
        ChosenCoordSystemEPSG = "4326"
        ChosenCoordSystem = "WGS 84 - Latlon"

    End Sub

    Private Sub CoordinateSystemPicker_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Sub SearchCSbyID(id As Integer)
        ListBox1.Items.Clear()
        WKTList.Clear()

        For Each wkt As SridReader.WKTstring In SridReader.GetSRIDs()
            If wkt.WKID = id Then
                'We found it!
                ListBox1.Items.Add(wkt.WKID & vbTab & wkt.WKTName)
                WKTList.Add(wkt)
            End If
        Next
    End Sub


    Public Sub SearchCSbyName(name As String)
        ListBox1.Items.Clear()
        WKTList.Clear()

        For Each wkt As SridReader.WKTstring In SridReader.GetSRIDs()
            If wkt.WKTName.ToUpper.Contains(name.ToUpper) Then
                'We found it!
                ListBox1.Items.Add(wkt.WKID & vbTab & wkt.WKTName)
                WKTList.Add(wkt)
            End If
        Next
    End Sub

    Public Sub ShowAll()
        ListBox1.Items.Clear()
        WKTList.Clear()

        For Each wkt As SridReader.WKTstring In SridReader.GetSRIDs()
            ListBox1.Items.Add(wkt.WKID & vbTab & wkt.WKTName)
            WKTList.Add(wkt)
        Next
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If ListBox1.SelectedIndex < 0 Then
            MsgBox("No CRS selected, Highlight a coordinate system in the list")
            Exit Sub
        End If

        ChosenCoordSystem = WKTList(ListBox1.SelectedIndex).WKTName
        ChosenCoordSystemEPSG = WKTList(ListBox1.SelectedIndex).WKID
        ChosenCoordSystemWKT = WKTList(ListBox1.SelectedIndex).WKT
        theDlg.Label3.Text = ChosenCoordSystem
        'Me.Dispose()
        'Me.Parent.Controls.Remove(Me)
        Me.Visible = False
    End Sub

    Sub changeSel()
        ChosenCoordSystem = WKTList(ListBox1.SelectedIndex).WKTName
        ChosenCoordSystemEPSG = WKTList(ListBox1.SelectedIndex).WKID
        ChosenCoordSystemWKT = WKTList(ListBox1.SelectedIndex).WKT
        theDlg.Label3.Text = ChosenCoordSystem
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        SearchCSbyName(TextBox2.Text)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SearchCSbyID(TextBox1.Text)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ShowAll()
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If TextBox2.Text <> "" Then
            SearchCSbyName(TextBox2.Text)
        End If
        ShowAll()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" Then
            SearchCSbyID(TextBox1.Text)
        Else
            ShowAll()
        End If

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        changeSel()
    End Sub
End Class




Public Class SridReader

    'Private sridFile As System.IO.StreamReader = System.IO.File.OpenText(Global.My.Resources.Resources.SRID)

    Shared MIfolderDir As String = InteropServices.MapInfoApplication.Eval("GetFolderPath$(-3)")
    Shared filename As String = MIfolderDir & "\SRID.csv"

    'Change this to point to the SRID.CSV file.
    Public Structure WKTstring
        ''' <summary>Well-known ID</summary>
        Public WKID As Integer
        ''' <summary>Well-known Text</summary>
        Public WKT As String
        ''' <summary>Well-known name</summary>
        Public WKTName As String
    End Structure
    ''' <summary>Enumerates all SRID's in the SRID.csv file.</summary>
    ''' <returns>Enumerator</returns>
    Public Shared Iterator Function GetSRIDs() As IEnumerable(Of WKTstring)
        Using sr As System.IO.StreamReader = System.IO.File.OpenText(filename)
            'Using sr As System.IO.StreamReader = System.IO.File.OpenText(Global.My.Resources.Resources.SRID)
            While Not sr.EndOfStream
                Dim line As String = sr.ReadLine()
                Dim split As Integer = line.IndexOf(";"c)
                If split > -1 Then
                    Dim wkt As New WKTstring()
                    wkt.WKID = Integer.Parse(line.Substring(0, split))
                    wkt.WKT = line.Substring(split + 1)
                    wkt.WKTName = line.Split(",")(0).Split(";")(1).Substring(7)
                    Yield wkt
                End If
            End While
            sr.Close()
        End Using
    End Function
    ''' <summary>Gets a coordinate system from the SRID.csv file</summary>
    ''' <param name="id">EPSG ID</param>
    ''' <returns>Coordinate system, or null if SRID was not found.</returns>
    Public Shared Function GetCSbyID(id As Integer) As ICoordinateSystem
        For Each wkt As SridReader.WKTstring In SridReader.GetSRIDs()
            If wkt.WKID = id Then
                'We found it!
                Return TryCast(ProjNet.Converters.WellKnownText.CoordinateSystemWktReader.Parse(wkt.WKT), ICoordinateSystem)
            End If
        Next
        Return Nothing
    End Function
End Class

