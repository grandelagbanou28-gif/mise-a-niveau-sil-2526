Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms

Public Class Etudiant
    Public Nom As String = ""
    Public Prenom As String = ""
    Public NoteTC As Double = 0
    Public NoteSPE As Double = 0
    Public NoteEPP As Double = 0
    Public Rang As Integer = 0

    Public Function Moyenne() As Double
        Return (NoteTC * 3 + NoteSPE * 4 + NoteEPP * 3) / 10
    End Function

    Public Function Decision() As String
        If Moyenne() >= 10 Then
            Return "admis(e)"
        Else
            Return "non admis(e)"
        End If
    End Function
End Class

Public Class Form1
    Inherits Form

    Private liste As New List(Of Etudiant)()

    Private txtNom As New TextBox()
    Private txtPrenom As New TextBox()
    Private txtTC As New TextBox()
    Private txtSPE As New TextBox()
    Private txtEPP As New TextBox()
    Private btnAjouter As New Button()
    Private btnClassement As New Button()
    Private btnBulletin As New Button()
    Private btnQuitter As New Button()
    Private dgv As New DataGridView()
    Private txtBulletin As New TextBox()

    Public Sub New()
        Text = "Gestion des notes des etudiants - UATM/GASA FORMATION"
        Size = New Size(940, 700)
        StartPosition = FormStartPosition.CenterScreen
        Font = New Font("Segoe UI", 9.5F)

        AjouterLabel("Nom :", 20, 28)
        Placer(txtNom, 75, 24, 170)
        AjouterLabel("Prenom :", 265, 28)
        Placer(txtPrenom, 330, 24, 170)

        AjouterLabel("Tronc commun (coef. 3) :", 20, 68)
        Placer(txtTC, 175, 64, 70)
        AjouterLabel("Specialite (coef. 4) :", 275, 68)
        Placer(txtSPE, 420, 64, 70)
        AjouterLabel("EPP (coef. 3) :", 520, 68)
        Placer(txtEPP, 630, 64, 70)

        ConfigurerBouton(btnAjouter, "Ajouter", 20, 105, 130)
        ConfigurerBouton(btnClassement, "Afficher par ordre de merite", 165, 105, 230)
        ConfigurerBouton(btnBulletin, "Editer le bulletin", 410, 105, 170)
        ConfigurerBouton(btnQuitter, "Quitter", 760, 105, 110)

        dgv.Location = New Point(20, 155)
        dgv.Size = New Size(850, 300)
        dgv.ReadOnly = True
        dgv.AllowUserToAddRows = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.MultiSelect = False
        dgv.RowHeadersVisible = False
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgv.BackgroundColor = Color.White
        dgv.Columns.Add("rang", "Rang")
        dgv.Columns.Add("nom", "Nom")
        dgv.Columns.Add("prenom", "Prenom")
        dgv.Columns.Add("tc", "Tronc commun")
        dgv.Columns.Add("spe", "Specialite")
        dgv.Columns.Add("epp", "EPP")
        dgv.Columns.Add("moyenne", "Moyenne generale")
        dgv.Columns.Add("mention", "Mention")
        Controls.Add(dgv)

        AjouterLabel("Bulletin de l'etudiant selectionne :", 20, 470)
        txtBulletin.Location = New Point(20, 495)
        txtBulletin.Size = New Size(850, 140)
        txtBulletin.Multiline = True
        txtBulletin.ReadOnly = True
        txtBulletin.ScrollBars = ScrollBars.Vertical
        txtBulletin.Font = New Font("Consolas", 10)
        Controls.Add(txtBulletin)

        AddHandler btnAjouter.Click, AddressOf BtnAjouterClick
        AddHandler btnClassement.Click, AddressOf BtnClassementClick
        AddHandler btnBulletin.Click, AddressOf BtnBulletinClick
        AddHandler btnQuitter.Click, AddressOf BtnQuitterClick
    End Sub

    Private Sub AjouterLabel(texte As String, x As Integer, y As Integer)
        Dim lbl As New Label()
        lbl.Text = texte
        lbl.Location = New Point(x, y)
        lbl.AutoSize = True
        Controls.Add(lbl)
    End Sub

    Private Sub Placer(txt As TextBox, x As Integer, y As Integer, l As Integer)
        txt.Location = New Point(x, y)
        txt.Width = l
        Controls.Add(txt)
    End Sub

    Private Sub ConfigurerBouton(btn As Button, texte As String, x As Integer, y As Integer, l As Integer)
        btn.Text = texte
        btn.Location = New Point(x, y)
        btn.Size = New Size(l, 35)
        Controls.Add(btn)
    End Sub

    Private Function Mention(note As Double) As String
        If note >= 10 Then
            Return "valide"
        Else
            Return "non valide"
        End If
    End Function

    Private Sub AfficherListe()
        dgv.Rows.Clear()
        For Each et As Etudiant In liste
            Dim rg As String = "-"
            If et.Rang > 0 Then rg = et.Rang.ToString()
            dgv.Rows.Add(rg, et.Nom, et.Prenom,
                         et.NoteTC.ToString("0.00"), et.NoteSPE.ToString("0.00"),
                         et.NoteEPP.ToString("0.00"), et.Moyenne().ToString("0.00"),
                         Mention(et.Moyenne()))
        Next
    End Sub

    Private Sub BtnAjouterClick(sender As Object, e As EventArgs)
        If txtNom.Text.Trim() = "" OrElse txtPrenom.Text.Trim() = "" Then
            MessageBox.Show("Veuillez saisir le nom et le prenom de l'etudiant.",
                            "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim tc, spe, epp As Double
        If Not Double.TryParse(txtTC.Text, tc) OrElse
           Not Double.TryParse(txtSPE.Text, spe) OrElse
           Not Double.TryParse(txtEPP.Text, epp) Then
            MessageBox.Show("Les notes doivent etre des nombres.",
                            "Erreur de saisie", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If tc < 0 OrElse tc > 20 OrElse spe < 0 OrElse spe > 20 OrElse epp < 0 OrElse epp > 20 Then
            MessageBox.Show("Les notes doivent etre comprises entre 0 et 20.",
                            "Erreur de saisie", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim et As New Etudiant()
        et.Nom = txtNom.Text.Trim().ToUpper()
        et.Prenom = txtPrenom.Text.Trim()
        et.NoteTC = tc
        et.NoteSPE = spe
        et.NoteEPP = epp
        liste.Add(et)

        AfficherListe()

        txtNom.Clear()
        txtPrenom.Clear()
        txtTC.Clear()
        txtSPE.Clear()
        txtEPP.Clear()
        txtNom.Focus()
    End Sub

    Private Sub BtnClassementClick(sender As Object, e As EventArgs)
        If liste.Count = 0 Then
            MessageBox.Show("Aucun etudiant enregistre.", "Information",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        liste.Sort(Function(a As Etudiant, b As Etudiant) b.Moyenne().CompareTo(a.Moyenne()))

        Dim r As Integer = 1
        For Each et As Etudiant In liste
            et.Rang = r
            r += 1
        Next

        AfficherListe()
        txtBulletin.Text = "Classement par ordre de merite effectue."
    End Sub

    Private Sub BtnBulletinClick(sender As Object, e As EventArgs)
        If dgv.CurrentRow Is Nothing Then
            MessageBox.Show("Selectionnez un etudiant dans le tableau puis recliquez sur le bulletin.",
                            "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim i As Integer = dgv.CurrentRow.Index
        If i < 0 OrElse i >= liste.Count Then Return

        Dim et As Etudiant = liste(i)
        Dim s As String = ""
        s &= "BULLETIN - UATM/GASA FORMATION" & vbCrLf
        s &= "Etudiant : " & et.Nom & " " & et.Prenom & vbCrLf
        s &= "------------------------------------------------------------------" & vbCrLf
        s &= "Tronc commun (coefficient 3) : " & et.NoteTC.ToString("0.00") & "  [" & Mention(et.NoteTC) & "]" & vbCrLf
        s &= "Specialite  (coefficient 4) : " & et.NoteSPE.ToString("0.00") & "  [" & Mention(et.NoteSPE) & "]" & vbCrLf
        s &= "EPP         (coefficient 3) : " & et.NoteEPP.ToString("0.00") & "  [" & Mention(et.NoteEPP) & "]" & vbCrLf
        s &= "------------------------------------------------------------------" & vbCrLf
        s &= "Moyenne generale : " & et.Moyenne().ToString("0.00") & " / 20" & vbCrLf

        If et.Rang > 0 Then
            s &= "Rang : " & et.Rang & " eme sur " & liste.Count & vbCrLf
        Else
            s &= "Rang : classement non effectue" & vbCrLf
        End If

        s &= "Decision : " & et.Decision() & vbCrLf

        txtBulletin.Text = s
    End Sub

    Private Sub BtnQuitterClick(sender As Object, e As EventArgs)
        Close()
    End Sub
End Class
