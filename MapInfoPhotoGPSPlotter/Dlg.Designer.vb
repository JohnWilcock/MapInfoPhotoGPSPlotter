Imports System.io
Imports System.Reflection

Namespace MapInfoPhotoGPSPlotter
    Partial Class Dlg
        ''' <summary> 
        ''' Required designer variable. 
        ''' </summary> 
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary> 
        ''' Clean up any resources being used. 
        ''' </summary> 
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param> 
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"

        ''' <summary> 
        ''' Required method for Designer support - do not modify 
        ''' the contents of this method with the code editor. 
        ''' </summary> 
        Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Dlg))
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer()
            Me.TabControl1 = New System.Windows.Forms.TabControl()
            Me.TabPage1 = New System.Windows.Forms.TabPage()
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.Button1 = New System.Windows.Forms.Button()
            Me.CheckBox2 = New System.Windows.Forms.CheckBox()
            Me.Button2 = New System.Windows.Forms.Button()
            Me.CheckBox1 = New System.Windows.Forms.CheckBox()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.TextBox1 = New System.Windows.Forms.TextBox()
            Me.ComboBox2 = New System.Windows.Forms.ComboBox()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.ComboBox1 = New System.Windows.Forms.ComboBox()
            Me.TabPage2 = New System.Windows.Forms.TabPage()
            Me.TextBox2 = New System.Windows.Forms.TextBox()
            Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
            Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
            Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
            Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
            Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
            Me.TableLayoutPanel5 = New System.Windows.Forms.TableLayoutPanel()
            Me.TableLayoutPanel6 = New System.Windows.Forms.TableLayoutPanel()
            Me.TableLayoutPanel7 = New System.Windows.Forms.TableLayoutPanel()
            Me.TableLayoutPanel8 = New System.Windows.Forms.TableLayoutPanel()
            Me.CoordinateSystemPicker1 = New CoordinateSystemPicker()
            Me.ToolStripContainer1.ContentPanel.SuspendLayout()
            Me.ToolStripContainer1.TopToolStripPanel.SuspendLayout()
            Me.ToolStripContainer1.SuspendLayout()
            Me.TabControl1.SuspendLayout()
            Me.TabPage1.SuspendLayout()
            Me.GroupBox1.SuspendLayout()
            Me.TabPage2.SuspendLayout()
            Me.ToolStrip1.SuspendLayout()
            Me.TableLayoutPanel1.SuspendLayout()
            Me.TableLayoutPanel2.SuspendLayout()
            Me.TableLayoutPanel3.SuspendLayout()
            Me.TableLayoutPanel4.SuspendLayout()
            Me.TableLayoutPanel5.SuspendLayout()
            Me.TableLayoutPanel6.SuspendLayout()
            Me.TableLayoutPanel7.SuspendLayout()
            Me.TableLayoutPanel8.SuspendLayout()
            Me.SuspendLayout()
            '
            'ToolStripContainer1
            '
            '
            'ToolStripContainer1.ContentPanel
            '
            Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.TabControl1)
            resources.ApplyResources(Me.ToolStripContainer1.ContentPanel, "ToolStripContainer1.ContentPanel")
            resources.ApplyResources(Me.ToolStripContainer1, "ToolStripContainer1")
            Me.ToolStripContainer1.Name = "ToolStripContainer1"
            '
            'ToolStripContainer1.TopToolStripPanel
            '
            Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.ToolStrip1)
            '
            'TabControl1
            '
            Me.TabControl1.Controls.Add(Me.TabPage1)
            Me.TabControl1.Controls.Add(Me.TabPage2)
            resources.ApplyResources(Me.TabControl1, "TabControl1")
            Me.TabControl1.Name = "TabControl1"
            Me.TabControl1.SelectedIndex = 0
            '
            'TabPage1
            '
            Me.TabPage1.Controls.Add(Me.GroupBox1)
            resources.ApplyResources(Me.TabPage1, "TabPage1")
            Me.TabPage1.Name = "TabPage1"
            Me.TabPage1.UseVisualStyleBackColor = True
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.TableLayoutPanel1)
            resources.ApplyResources(Me.GroupBox1, "GroupBox1")
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.TabStop = False
            '
            'Label3
            '
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.Name = "Label3"
            '
            'Button1
            '
            resources.ApplyResources(Me.Button1, "Button1")
            Me.Button1.Name = "Button1"
            Me.Button1.UseVisualStyleBackColor = True
            '
            'CheckBox2
            '
            resources.ApplyResources(Me.CheckBox2, "CheckBox2")
            Me.CheckBox2.Name = "CheckBox2"
            Me.CheckBox2.UseVisualStyleBackColor = True
            '
            'Button2
            '
            resources.ApplyResources(Me.Button2, "Button2")
            Me.Button2.Name = "Button2"
            Me.Button2.UseVisualStyleBackColor = True
            '
            'CheckBox1
            '
            resources.ApplyResources(Me.CheckBox1, "CheckBox1")
            Me.CheckBox1.Name = "CheckBox1"
            Me.CheckBox1.UseVisualStyleBackColor = True
            '
            'Label4
            '
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.Name = "Label4"
            '
            'TextBox1
            '
            resources.ApplyResources(Me.TextBox1, "TextBox1")
            Me.TextBox1.Name = "TextBox1"
            '
            'ComboBox2
            '
            resources.ApplyResources(Me.ComboBox2, "ComboBox2")
            Me.ComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.ComboBox2.FormattingEnabled = True
            Me.ComboBox2.Items.AddRange(New Object() {resources.GetString("ComboBox2.Items"), resources.GetString("ComboBox2.Items1"), resources.GetString("ComboBox2.Items2")})
            Me.ComboBox2.Name = "ComboBox2"
            '
            'Label2
            '
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.Name = "Label2"
            '
            'Label1
            '
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'ComboBox1
            '
            resources.ApplyResources(Me.ComboBox1, "ComboBox1")
            Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.ComboBox1.FormattingEnabled = True
            Me.ComboBox1.Items.AddRange(New Object() {resources.GetString("ComboBox1.Items"), resources.GetString("ComboBox1.Items1"), resources.GetString("ComboBox1.Items2")})
            Me.ComboBox1.Name = "ComboBox1"
            '
            'TabPage2
            '
            Me.TabPage2.Controls.Add(Me.TextBox2)
            resources.ApplyResources(Me.TabPage2, "TabPage2")
            Me.TabPage2.Name = "TabPage2"
            Me.TabPage2.UseVisualStyleBackColor = True
            '
            'TextBox2
            '
            resources.ApplyResources(Me.TextBox2, "TextBox2")
            Me.TextBox2.Name = "TextBox2"
            '
            'ToolStrip1
            '
            resources.ApplyResources(Me.ToolStrip1, "ToolStrip1")
            Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.ToolStripButton2})
            Me.ToolStrip1.Name = "ToolStrip1"
            '
            'ToolStripButton1
            '
            resources.ApplyResources(Me.ToolStripButton1, "ToolStripButton1")
            Me.ToolStripButton1.Name = "ToolStripButton1"
            '
            'ToolStripButton2
            '
            resources.ApplyResources(Me.ToolStripButton2, "ToolStripButton2")
            Me.ToolStripButton2.Name = "ToolStripButton2"
            '
            'TableLayoutPanel1
            '
            resources.ApplyResources(Me.TableLayoutPanel1, "TableLayoutPanel1")
            Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel7, 0, 2)
            Me.TableLayoutPanel1.Controls.Add(Me.CoordinateSystemPicker1, 0, 3)
            Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel4, 0, 1)
            Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 0, 0)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            '
            'TableLayoutPanel2
            '
            resources.ApplyResources(Me.TableLayoutPanel2, "TableLayoutPanel2")
            Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel3, 0, 0)
            Me.TableLayoutPanel2.Controls.Add(Me.CheckBox1, 0, 1)
            Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
            '
            'TableLayoutPanel3
            '
            resources.ApplyResources(Me.TableLayoutPanel3, "TableLayoutPanel3")
            Me.TableLayoutPanel3.Controls.Add(Me.ComboBox1, 1, 0)
            Me.TableLayoutPanel3.Controls.Add(Me.Label1, 0, 0)
            Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
            '
            'TableLayoutPanel4
            '
            resources.ApplyResources(Me.TableLayoutPanel4, "TableLayoutPanel4")
            Me.TableLayoutPanel4.Controls.Add(Me.TableLayoutPanel6, 0, 1)
            Me.TableLayoutPanel4.Controls.Add(Me.TableLayoutPanel5, 0, 0)
            Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
            '
            'TableLayoutPanel5
            '
            resources.ApplyResources(Me.TableLayoutPanel5, "TableLayoutPanel5")
            Me.TableLayoutPanel5.Controls.Add(Me.Label2, 0, 0)
            Me.TableLayoutPanel5.Controls.Add(Me.ComboBox2, 1, 0)
            Me.TableLayoutPanel5.Name = "TableLayoutPanel5"
            '
            'TableLayoutPanel6
            '
            resources.ApplyResources(Me.TableLayoutPanel6, "TableLayoutPanel6")
            Me.TableLayoutPanel6.Controls.Add(Me.Label4, 0, 0)
            Me.TableLayoutPanel6.Controls.Add(Me.Button2, 2, 0)
            Me.TableLayoutPanel6.Controls.Add(Me.TextBox1, 1, 0)
            Me.TableLayoutPanel6.Name = "TableLayoutPanel6"
            '
            'TableLayoutPanel7
            '
            resources.ApplyResources(Me.TableLayoutPanel7, "TableLayoutPanel7")
            Me.TableLayoutPanel7.Controls.Add(Me.TableLayoutPanel8, 0, 1)
            Me.TableLayoutPanel7.Controls.Add(Me.CheckBox2, 0, 0)
            Me.TableLayoutPanel7.Name = "TableLayoutPanel7"
            '
            'TableLayoutPanel8
            '
            resources.ApplyResources(Me.TableLayoutPanel8, "TableLayoutPanel8")
            Me.TableLayoutPanel8.Controls.Add(Me.Button1, 1, 0)
            Me.TableLayoutPanel8.Controls.Add(Me.Label3, 0, 0)
            Me.TableLayoutPanel8.Name = "TableLayoutPanel8"
            '
            'CoordinateSystemPicker1
            '
            resources.ApplyResources(Me.CoordinateSystemPicker1, "CoordinateSystemPicker1")
            Me.CoordinateSystemPicker1.ChosenCoordMIString = Nothing
            Me.CoordinateSystemPicker1.ChosenCoordSystemEPSG = Nothing
            Me.CoordinateSystemPicker1.ChosenCoordSystemWKT = Nothing
            Me.CoordinateSystemPicker1.Name = "CoordinateSystemPicker1"
            '
            'Dlg
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.ToolStripContainer1)
            Me.MinimumSize = New System.Drawing.Size(150, 200)
            Me.Name = "Dlg"
            Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
            Me.ToolStripContainer1.TopToolStripPanel.ResumeLayout(False)
            Me.ToolStripContainer1.TopToolStripPanel.PerformLayout()
            Me.ToolStripContainer1.ResumeLayout(False)
            Me.ToolStripContainer1.PerformLayout()
            Me.TabControl1.ResumeLayout(False)
            Me.TabPage1.ResumeLayout(False)
            Me.GroupBox1.ResumeLayout(False)
            Me.TabPage2.ResumeLayout(False)
            Me.TabPage2.PerformLayout()
            Me.ToolStrip1.ResumeLayout(False)
            Me.ToolStrip1.PerformLayout()
            Me.TableLayoutPanel1.ResumeLayout(False)
            Me.TableLayoutPanel2.ResumeLayout(False)
            Me.TableLayoutPanel2.PerformLayout()
            Me.TableLayoutPanel3.ResumeLayout(False)
            Me.TableLayoutPanel3.PerformLayout()
            Me.TableLayoutPanel4.ResumeLayout(False)
            Me.TableLayoutPanel5.ResumeLayout(False)
            Me.TableLayoutPanel5.PerformLayout()
            Me.TableLayoutPanel6.ResumeLayout(False)
            Me.TableLayoutPanel6.PerformLayout()
            Me.TableLayoutPanel7.ResumeLayout(False)
            Me.TableLayoutPanel7.PerformLayout()
            Me.TableLayoutPanel8.ResumeLayout(False)
            Me.TableLayoutPanel8.PerformLayout()
            Me.ResumeLayout(False)

End Sub
        Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
        Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
        Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
        Friend WithEvents ToolStripButton2 As System.Windows.Forms.ToolStripButton
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
        Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
        Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
        Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
        Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
        Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
        Friend WithEvents CoordinateSystemPicker1 As CoordinateSystemPicker
        Friend WithEvents Button2 As System.Windows.Forms.Button
        Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Button1 As System.Windows.Forms.Button
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents TableLayoutPanel5 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents TableLayoutPanel7 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents TableLayoutPanel8 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents TableLayoutPanel6 As System.Windows.Forms.TableLayoutPanel

#End Region




    End Class
End Namespace