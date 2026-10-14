Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Windows.Forms

Public Class Form1
    Inherits Form

    Private Shared ReadOnly BleuNuit As Color = Color.FromArgb(33, 35, 48)
    Private Shared ReadOnly Rouge As Color = Color.FromArgb(206, 69, 63)
    Private Shared ReadOnly RougeSombre As Color = Color.FromArgb(184, 58, 53)
    Private Shared ReadOnly Beige As Color = Color.FromArgb(203, 181, 140)
    Private Shared ReadOnly Fond As Color = Color.FromArgb(244, 245, 247)
    Private Shared ReadOnly Bordure As Color = Color.FromArgb(229, 231, 235)
    Private Shared ReadOnly Gris As Color = Color.FromArgb(107, 114, 128)
    Private Shared ReadOnly GrisClair As Color = Color.FromArgb(247, 248, 250)
    Private Shared ReadOnly SelectFond As Color = Color.FromArgb(252, 235, 234)

    Private Const TexteRecherche As String = "Rechercher un etudiant..."

    Private liste As New List(Of Etudiant)()
    Private listeAffichee As New List(Of Etudiant)()
    Private enRafraichissement As Boolean = False
    Private conteneur As New Panel()

    Private ep As New ErrorProvider()

    Private WithEvents txtNom As New TextBox()
    Private WithEvents txtPrenom As New TextBox()
    Private WithEvents txtTC As New TextBox()
    Private WithEvents txtSPE As New TextBox()
    Private WithEvents txtEPP As New TextBox()
    Private WithEvents txtRecherche As New TextBox()
    Private WithEvents btnAjouter As New Button()
    Private WithEvents btnModifier As New Button()
    Private WithEvents btnSupprimer As New Button()
    Private WithEvents btnMerite As New Button()
    Private WithEvents btnBulletin As New Button()
    Private WithEvents btnEnregistrer As New Button()
    Private WithEvents btnCharger As New Button()
    Private WithEvents btnQuitter As New Button()
    Private WithEvents dgv As New DataGridView()

    Private txtBulletin As New TextBox()
    Private lblStatut As New Label()
    Private lblTemps As New Label()
    Private lblEffectif As New Label()
    Private lblMoyenne As New Label()
    Private lblMeilleure As New Label()
    Private lblTaux As New Label()

    Public Sub New()
        Text = "Gestion des notes des etudiants - UATM/GASA FORMATION"
        Size = New Size(1000, 790)
        MinimumSize = New Size(1000, 790)
        StartPosition = FormStartPosition.CenterScreen
        BackColor = Fond
        Font = New Font("Segoe UI", 9.5F)

        ep.BlinkStyle = ErrorBlinkStyle.NeverBlink

        Dim bandeau As New Panel()
        bandeau.Dock = DockStyle.Top
        bandeau.Height = 62
        bandeau.BackColor = BleuNuit
        Dim titre As New Label()
        titre.Text = "Gestion des notes des etudiants"
        titre.ForeColor = Color.White
        titre.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        titre.Location = New Point(20, 7)
        titre.AutoSize = True
        Dim sousTitre As New Label()
        sousTitre.Text = "UATM-GASA Formation   |   Licence 3 SIL   |   Annee universitaire 2026-2027"
        sousTitre.ForeColor = Beige
        sousTitre.Location = New Point(22, 37)
        sousTitre.AutoSize = True
        Dim filet As New Panel()
        filet.BackColor = Rouge
        filet.Location = New Point(0, 58)
        filet.Size = New Size(bandeau.Width, 4)
        filet.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        bandeau.Controls.Add(titre)
        bandeau.Controls.Add(sousTitre)
        bandeau.Controls.Add(filet)
        Controls.Add(bandeau)

        conteneur.Size = New Size(944, 642)
        conteneur.BackColor = Fond
        Controls.Add(conteneur)

        Dim carte As New Panel()
        carte.Location = New Point(0, 0)
        carte.Size = New Size(944, 128)
        carte.BackColor = Color.White
        carte.BorderStyle = BorderStyle.FixedSingle

        carte.Controls.Add(Etiquette("Nom", 18, 14))
        ConfigurerChamp(txtNom, 18, 34, 210)
        carte.Controls.Add(txtNom)
        carte.Controls.Add(Etiquette("Prenom", 250, 14))
        ConfigurerChamp(txtPrenom, 250, 34, 210)
        carte.Controls.Add(txtPrenom)

        carte.Controls.Add(Etiquette("Tronc commun (coef. 3)", 18, 70))
        ConfigurerChamp(txtTC, 18, 90, 80)
        carte.Controls.Add(txtTC)
        carte.Controls.Add(Etiquette("Specialite (coef. 4)", 190, 70))
        ConfigurerChamp(txtSPE, 190, 90, 80)
        carte.Controls.Add(txtSPE)
        carte.Controls.Add(Etiquette("EPP (coef. 3)", 350, 70))
        ConfigurerChamp(txtEPP, 350, 90, 80)
        carte.Controls.Add(txtEPP)

        StyleBouton(btnAjouter, "Ajouter", 560, 46, 120, Rouge, Color.White, Rouge, RougeSombre)
        StyleBouton(btnModifier, "Modifier", 690, 46, 120, Color.White, BleuNuit, BleuNuit, GrisClair)
        StyleBouton(btnSupprimer, "Supprimer", 820, 46, 104, Color.White, Rouge, Rouge, SelectFond)
        carte.Controls.Add(btnAjouter)
        carte.Controls.Add(btnModifier)
        carte.Controls.Add(btnSupprimer)
        conteneur.Controls.Add(carte)

        StyleBouton(btnMerite, "Classer par ordre de merite", 0, 142, 210, BleuNuit, Color.White, BleuNuit, BleuNuit)
        StyleBouton(btnBulletin, "Bulletin (.txt)", 220, 142, 130, Color.White, BleuNuit, BleuNuit, GrisClair)
        StyleBouton(btnEnregistrer, "Enregistrer", 360, 142, 105, Color.White, BleuNuit, Bordure, GrisClair)
        StyleBouton(btnCharger, "Charger", 475, 142, 90, Color.White, BleuNuit, Bordure, GrisClair)
        StyleBouton(btnQuitter, "Quitter", 822, 142, 122, Color.White, Rouge, Rouge, SelectFond)
        conteneur.Controls.Add(btnMerite)
        conteneur.Controls.Add(btnBulletin)
        conteneur.Controls.Add(btnEnregistrer)
        conteneur.Controls.Add(btnCharger)
        conteneur.Controls.Add(btnQuitter)

        txtRecherche.Location = New Point(602, 145)
        txtRecherche.Size = New Size(200, 30)
        txtRecherche.Text = TexteRecherche
        txtRecherche.ForeColor = Gris
        conteneur.Controls.Add(txtRecherche)

        conteneur.Controls.Add(CreerCarteStat(0, "Effectif", Rouge, lblEffectif))
        conteneur.Controls.Add(CreerCarteStat(241, "Moyenne de la classe", BleuNuit, lblMoyenne))
        conteneur.Controls.Add(CreerCarteStat(482, "Meilleure moyenne", Beige, lblMeilleure))
        conteneur.Controls.Add(CreerCarteStat(723, "Taux de reussite", BleuNuit, lblTaux))

        dgv.Location = New Point(0, 268)
        dgv.Size = New Size(944, 252)
        ConfigurerGrille()
        conteneur.Controls.Add(dgv)

        Dim etiquetteBulletin As New Label()
        etiquetteBulletin.Text = "Bulletin de l'etudiant selectionne  (le bouton Bulletin l'exporte en fichier .txt)"
        etiquetteBulletin.ForeColor = Gris
        etiquetteBulletin.Location = New Point(0, 530)
        etiquetteBulletin.AutoSize = True
        conteneur.Controls.Add(etiquetteBulletin)

        txtBulletin.Location = New Point(0, 550)
        txtBulletin.Size = New Size(944, 88)
        txtBulletin.Multiline = True
        txtBulletin.ReadOnly = True
        txtBulletin.ScrollBars = ScrollBars.Vertical
        txtBulletin.WordWrap = False
        txtBulletin.BackColor = Color.White
        txtBulletin.Font = New Font("Consolas", 9.5F)
        conteneur.Controls.Add(txtBulletin)

        Dim barreStatut As New Panel()
        barreStatut.Dock = DockStyle.Bottom
        barreStatut.Height = 28
        barreStatut.BackColor = Color.White
        barreStatut.BorderStyle = BorderStyle.FixedSingle
        lblStatut.Text = "Pret"
        lblStatut.ForeColor = Gris
        lblStatut.Location = New Point(10, 5)
        lblStatut.AutoSize = True
        lblTemps.ForeColor = Gris
        lblTemps.Location = New Point(800, 5)
        lblTemps.Size = New Size(150, 18)
        lblTemps.TextAlign = ContentAlignment.MiddleRight
        lblTemps.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        barreStatut.Controls.Add(lblStatut)
        barreStatut.Controls.Add(lblTemps)
        Controls.Add(barreStatut)

        Dim tampon As BindingFlags = BindingFlags.Instance Or BindingFlags.NonPublic
        dgv.GetType().GetProperty("DoubleBuffered", tampon).SetValue(dgv, True)

        listeAffichee = New List(Of Etudiant)(liste)
        RafraichirGrille()
        lblTemps.Text = DateTime.Now.ToString("HH:mm")
        Redimensionnement(Me, EventArgs.Empty)
    End Sub

    Private Function Etiquette(texte As String, x As Integer, y As Integer) As Label
        Dim l As New Label()
        l.Text = texte
        l.ForeColor = Gris
        l.Location = New Point(x, y)
        l.AutoSize = True
        Return l
    End Function

    Private Sub ConfigurerChamp(t As TextBox, x As Integer, y As Integer, l As Integer)
        t.Location = New Point(x, y)
        t.Size = New Size(l, 28)
        t.BorderStyle = BorderStyle.FixedSingle
    End Sub

    Private Sub StyleBouton(b As Button, texte As String, x As Integer, y As Integer, l As Integer,
                            fond As Color, couleur As Color, bordure As Color, survol As Color)
        b.Text = texte
        b.Location = New Point(x, y)
        b.Size = New Size(l, 36)
        b.FlatStyle = FlatStyle.Flat
        b.FlatAppearance.BorderColor = bordure
        b.FlatAppearance.BorderSize = 1
        b.FlatAppearance.MouseOverBackColor = survol
        b.BackColor = fond
        b.ForeColor = couleur
        b.Cursor = Cursors.Hand
    End Sub

    Private Function CreerCarteStat(x As Integer, titre As String, accent As Color, ByRef valeur As Label) As Panel
        Dim p As New Panel()
        p.Location = New Point(x, 188)
        p.Size = New Size(229, 64)
        p.BackColor = Color.White
        p.BorderStyle = BorderStyle.FixedSingle
        Dim bande As New Panel()
        bande.BackColor = accent
        bande.Location = New Point(0, 0)
        bande.Size = New Size(4, 64)
        bande.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom
        Dim l As New Label()
        l.Text = titre.ToUpper()
        l.ForeColor = Gris
        l.Font = New Font("Segoe UI", 8)
        l.Location = New Point(16, 9)
        l.AutoSize = True
        valeur.ForeColor = BleuNuit
        valeur.Font = New Font("Segoe UI", 17, FontStyle.Bold)
        valeur.Location = New Point(15, 28)
        valeur.AutoSize = True
        valeur.Text = "--"
        p.Controls.Add(bande)
        p.Controls.Add(l)
        p.Controls.Add(valeur)
        Return p
    End Function

    Private Sub ConfigurerGrille()
        dgv.ReadOnly = True
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToResizeRows = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.MultiSelect = False
        dgv.RowHeadersVisible = False
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgv.BackgroundColor = Color.White
        dgv.BorderStyle = BorderStyle.FixedSingle
        dgv.GridColor = Bordure
        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersDefaultCellStyle.BackColor = BleuNuit
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        dgv.ColumnHeadersHeight = 34
        dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        dgv.DefaultCellStyle.BackColor = Color.White
        dgv.DefaultCellStyle.ForeColor = BleuNuit
        dgv.DefaultCellStyle.SelectionBackColor = SelectFond
        dgv.DefaultCellStyle.SelectionForeColor = BleuNuit
        dgv.AlternatingRowsDefaultCellStyle.BackColor = GrisClair
        dgv.RowTemplate.Height = 30

        dgv.Columns.Add("rang", "Rang")
        dgv.Columns.Add("nom", "Nom")
        dgv.Columns.Add("prenom", "Prenom")
        dgv.Columns.Add("tc", "Tronc commun")
        dgv.Columns.Add("spe", "Specialite")
        dgv.Columns.Add("epp", "EPP")
        dgv.Columns.Add("moyenne", "Moyenne")
        dgv.Columns.Add("mention", "Mention")
        dgv.Columns("rang").Width = 55
        dgv.Columns("nom").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgv.Columns("nom").FillWeight = 90
        dgv.Columns("prenom").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgv.Columns("prenom").FillWeight = 90
        dgv.Columns("tc").Width = 112
        dgv.Columns("spe").Width = 95
        dgv.Columns("epp").Width = 70
        dgv.Columns("moyenne").Width = 90
        dgv.Columns("mention").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgv.Columns("mention").FillWeight = 80
    End Sub

    Public Shared Function MentionDe(note As Double) As String
        If note >= 17 Then
            Return "Tres bien"
        ElseIf note >= 14 Then
            Return "Bien"
        ElseIf note >= 12 Then
            Return "Assez bien"
        ElseIf note >= 10 Then
            Return "Passable"
        Else
            Return "Insuffisant"
        End If
    End Function

    Private Sub Statut(message As String)
        lblStatut.Text = message
        lblTemps.Text = DateTime.Now.ToString("HH:mm")
    End Sub

    Private Sub MajStatistiques()
        lblEffectif.Text = liste.Count.ToString()
        If liste.Count = 0 Then
            lblMoyenne.Text = "--"
            lblMeilleure.Text = "--"
            lblTaux.Text = "0 %"
            Return
        End If
        Dim total As Double = 0
        Dim meilleure As Double = liste(0).Moyenne
        Dim admis As Integer = 0
        For Each et As Etudiant In liste
            total += et.Moyenne
            If et.Moyenne > meilleure Then meilleure = et.Moyenne
            If et.Admis Then admis += 1
        Next
        lblMoyenne.Text = (total / liste.Count).ToString("0.00")
        lblMeilleure.Text = meilleure.ToString("0.00")
        lblTaux.Text = CInt(admis * 100 / liste.Count).ToString() & " %"
    End Sub

    Private Sub RafraichirGrille()
        enRafraichissement = True
        dgv.Rows.Clear()
        For Each et As Etudiant In listeAffichee
            Dim rg As String = "-"
            If et.Rang > 0 Then rg = et.Rang.ToString()
            dgv.Rows.Add(rg, et.Nom, et.Prenom,
                         et.NoteTC.ToString("0.00"), et.NoteSPE.ToString("0.00"),
                         et.NoteEPP.ToString("0.00"), et.Moyenne.ToString("0.00"),
                         MentionDe(et.Moyenne))
        Next
        enRafraichissement = False
        MajStatistiques()
    End Sub

    Private Sub AppliquerFiltreEtAfficher()
        Dim q As String = txtRecherche.Text.Trim()
        If q = TexteRecherche OrElse q = "" Then
            listeAffichee = New List(Of Etudiant)(liste)
        Else
            listeAffichee = liste.FindAll(Function(et As Etudiant)
                                              Return et.Nom.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                                                     et.Prenom.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0
                                          End Function)
        End If
        RafraichirGrille()
    End Sub

    Private Function EtudiantSelectionne() As Etudiant
        If dgv.CurrentRow Is Nothing Then Return Nothing
        Dim i As Integer = dgv.CurrentRow.Index
        If i < 0 OrElse i >= listeAffichee.Count Then Return Nothing
        Return listeAffichee(i)
    End Function

    Private Function LireSaisie(ByRef et As Etudiant) As Boolean
        ep.Clear()
        Dim ok As Boolean = True
        If txtNom.Text.Trim() = "" Then
            ep.SetError(txtNom, "Nom obligatoire")
            ok = False
        End If
        If txtPrenom.Text.Trim() = "" Then
            ep.SetError(txtPrenom, "Prenom obligatoire")
            ok = False
        End If

        Dim tc As Double = 0
        Dim spe As Double = 0
        Dim epp As Double = 0
        If Not Double.TryParse(txtTC.Text, tc) Then
            ep.SetError(txtTC, "Nombre attendu entre 0 et 20")
            ok = False
        ElseIf tc < 0 OrElse tc > 20 Then
            ep.SetError(txtTC, "Note entre 0 et 20")
            ok = False
        End If
        If Not Double.TryParse(txtSPE.Text, spe) Then
            ep.SetError(txtSPE, "Nombre attendu entre 0 et 20")
            ok = False
        ElseIf spe < 0 OrElse spe > 20 Then
            ep.SetError(txtSPE, "Note entre 0 et 20")
            ok = False
        End If
        If Not Double.TryParse(txtEPP.Text, epp) Then
            ep.SetError(txtEPP, "Nombre attendu entre 0 et 20")
            ok = False
        ElseIf epp < 0 OrElse epp > 20 Then
            ep.SetError(txtEPP, "Note entre 0 et 20")
            ok = False
        End If

        If Not ok Then
            Statut("Saisie incomplete : corrigez les champs signales")
            Return False
        End If

        et = New Etudiant()
        et.Nom = txtNom.Text.Trim().ToUpper()
        et.Prenom = txtPrenom.Text.Trim()
        et.NoteTC = tc
        et.NoteSPE = spe
        et.NoteEPP = epp
        Return True
    End Function

    Private Sub ViderSaisie()
        txtNom.Clear()
        txtPrenom.Clear()
        txtTC.Clear()
        txtSPE.Clear()
        txtEPP.Clear()
        ep.Clear()
    End Sub

    Private Sub ChampsEntree(sender As Object, e As KeyPressEventArgs) Handles txtNom.KeyPress, txtPrenom.KeyPress,
        txtTC.KeyPress, txtSPE.KeyPress, txtEPP.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            e.Handled = True
            btnAjouter.PerformClick()
        End If
    End Sub

    Private Sub AjouterClic(sender As Object, e As EventArgs) Handles btnAjouter.Click
        Dim et As Etudiant = Nothing
        If Not LireSaisie(et) Then Return
        liste.Add(et)
        txtRecherche.Text = TexteRecherche
        AppliquerFiltreEtAfficher()
        ViderSaisie()
        txtNom.Focus()
        Statut(et.Nom & " " & et.Prenom & " ajoute(e)")
    End Sub

    Private Sub ModifierClic(sender As Object, e As EventArgs) Handles btnModifier.Click
        Dim cible As Etudiant = EtudiantSelectionne()
        If cible Is Nothing Then
            MessageBox.Show("Selectionnez d'abord un etudiant dans le tableau.",
                            "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim et As Etudiant = Nothing
        If Not LireSaisie(et) Then Return
        cible.Nom = et.Nom
        cible.Prenom = et.Prenom
        cible.NoteTC = et.NoteTC
        cible.NoteSPE = et.NoteSPE
        cible.NoteEPP = et.NoteEPP
        AppliquerFiltreEtAfficher()
        ViderSaisie()
        Statut("Fiche de " & cible.Nom & " " & cible.Prenom & " modifiee")
    End Sub

    Private Sub SupprimerClic(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        Dim cible As Etudiant = EtudiantSelectionne()
        If cible Is Nothing Then
            MessageBox.Show("Selectionnez d'abord un etudiant dans le tableau.",
                            "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim reponse As DialogResult = MessageBox.Show("Supprimer " & cible.Nom & " " & cible.Prenom & " ?",
                                                      "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reponse <> DialogResult.Yes Then Return
        liste.Remove(cible)
        AppliquerFiltreEtAfficher()
        ViderSaisie()
        txtBulletin.Clear()
        Statut("Etudiant supprime")
    End Sub

    Private Sub MeriteClic(sender As Object, e As EventArgs) Handles btnMerite.Click
        If liste.Count = 0 Then
            Statut("Aucun etudiant enregistre")
            Return
        End If
        liste.Sort(Function(a As Etudiant, b As Etudiant) b.Moyenne.CompareTo(a.Moyenne))
        Dim r As Integer = 1
        For Each et As Etudiant In liste
            et.Rang = r
            r += 1
        Next
        AppliquerFiltreEtAfficher()
        Statut("Classement par ordre de merite effectue")
    End Sub

    Private Sub SelectionChangee(sender As Object, e As EventArgs) Handles dgv.SelectionChanged
        If enRafraichissement Then Return
        Dim et As Etudiant = EtudiantSelectionne()
        If et Is Nothing Then Return
        txtNom.Text = et.Nom
        txtPrenom.Text = et.Prenom
        txtTC.Text = et.NoteTC.ToString("0.00")
        txtSPE.Text = et.NoteSPE.ToString("0.00")
        txtEPP.Text = et.NoteEPP.ToString("0.00")
        txtBulletin.Text = ConstruireBulletin(et)
    End Sub

    Private Function ConstruireBulletin(et As Etudiant) As String
        Dim s As String = ""
        s &= "BULLETIN DE NOTES - UATM/GASA FORMATION" & vbCrLf
        s &= "Annee universitaire 2026-2027" & vbCrLf
        s &= "==================================================================" & vbCrLf
        s &= "Etudiant : " & et.Nom & " " & et.Prenom & vbCrLf
        s &= "------------------------------------------------------------------" & vbCrLf
        s &= String.Format("{0,-30}{1,8}{2,8}   {3}", "Matiere", "Note", "Coef", "Appreciation") & vbCrLf
        s &= String.Format("{0,-30}{1,8}{2,8}   {3}", "Tronc commun", et.NoteTC.ToString("0.00"), 3, MentionDe(et.NoteTC)) & vbCrLf
        s &= String.Format("{0,-30}{1,8}{2,8}   {3}", "Specialite", et.NoteSPE.ToString("0.00"), 4, MentionDe(et.NoteSPE)) & vbCrLf
        s &= String.Format("{0,-30}{1,8}{2,8}   {3}", "EPP", et.NoteEPP.ToString("0.00"), 3, MentionDe(et.NoteEPP)) & vbCrLf
        s &= "------------------------------------------------------------------" & vbCrLf
        s &= "Moyenne generale : " & et.Moyenne.ToString("0.00") & " / 20   (" & MentionDe(et.Moyenne) & ")" & vbCrLf
        If et.Rang > 0 Then
            s &= "Rang : " & et.Rang & " sur " & liste.Count & vbCrLf
        Else
            s &= "Rang : classement non effectue" & vbCrLf
        End If
        s &= "Decision : " & et.Decision() & vbCrLf
        Return s
    End Function

    Private Sub BulletinClic(sender As Object, e As EventArgs) Handles btnBulletin.Click
        Dim et As Etudiant = EtudiantSelectionne()
        If et Is Nothing Then
            MessageBox.Show("Selectionnez d'abord un etudiant dans le tableau.",
                            "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        txtBulletin.Text = ConstruireBulletin(et)
        Using sfd As New SaveFileDialog()
            sfd.Filter = "Bulletin texte (*.txt)|*.txt"
            sfd.FileName = "bulletin_" & et.Nom & "_" & et.Prenom & ".txt"
            If sfd.ShowDialog() = DialogResult.OK Then
                File.WriteAllText(sfd.FileName, txtBulletin.Text)
                Statut("Bulletin exporte : " & Path.GetFileName(sfd.FileName))
            End If
        End Using
    End Sub

    Private Sub EnregistrerClic(sender As Object, e As EventArgs) Handles btnEnregistrer.Click
        If liste.Count = 0 Then
            Statut("Aucun etudiant a enregistrer")
            Return
        End If
        Using sfd As New SaveFileDialog()
            sfd.Filter = "Fichier CSV (*.csv)|*.csv"
            sfd.FileName = "notes_uatm.csv"
            If sfd.ShowDialog() <> DialogResult.OK Then Return
            Dim lignes As New List(Of String)()
            lignes.Add("Nom;Prenom;Tronc commun;Specialite;EPP")
            For Each et As Etudiant In liste
                lignes.Add(et.Nom & ";" & et.Prenom & ";" &
                           et.NoteTC.ToString(CultureInfo.InvariantCulture) & ";" &
                           et.NoteSPE.ToString(CultureInfo.InvariantCulture) & ";" &
                           et.NoteEPP.ToString(CultureInfo.InvariantCulture))
            Next
            File.WriteAllLines(sfd.FileName, lignes)
        End Using
        Statut(liste.Count & " etudiant(s) enregistre(s) en CSV")
    End Sub

    Private Sub ChargerClic(sender As Object, e As EventArgs) Handles btnCharger.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Fichier CSV (*.csv)|*.csv"
            If ofd.ShowDialog() <> DialogResult.OK Then Return
            Dim charges As Integer = 0
            Dim lignes As String() = File.ReadAllLines(ofd.FileName)
            For i As Integer = 0 To lignes.Length - 1
                If i = 0 AndAlso lignes(i).StartsWith("Nom") Then Continue For
                Dim parties As String() = lignes(i).Split(";"c)
                If parties.Length < 5 Then Continue For
                Dim tc As Double
                Dim spe As Double
                Dim epp As Double
                If Not Double.TryParse(parties(2), NumberStyles.Float, CultureInfo.InvariantCulture, tc) Then Continue For
                If Not Double.TryParse(parties(3), NumberStyles.Float, CultureInfo.InvariantCulture, spe) Then Continue For
                If Not Double.TryParse(parties(4), NumberStyles.Float, CultureInfo.InvariantCulture, epp) Then Continue For
                If tc < 0 OrElse tc > 20 OrElse spe < 0 OrElse spe > 20 OrElse epp < 0 OrElse epp > 20 Then Continue For
                Dim et As New Etudiant()
                et.Nom = parties(0).Trim().ToUpper()
                et.Prenom = parties(1).Trim()
                et.NoteTC = tc
                et.NoteSPE = spe
                et.NoteEPP = epp
                liste.Add(et)
                charges += 1
            Next
            If charges = 0 Then
                Statut("Aucune donnee valide dans le fichier")
            Else
                liste.Sort(Function(a As Etudiant, b As Etudiant) b.Moyenne.CompareTo(a.Moyenne))
                Dim r As Integer = 1
                For Each et As Etudiant In liste
                    et.Rang = r
                    r += 1
                Next
                AppliquerFiltreEtAfficher()
                Statut(charges & " etudiant(s) charge(s) depuis le CSV")
            End If
        End Using
    End Sub

    Private Sub RechercheSaisie(sender As Object, e As EventArgs) Handles txtRecherche.TextChanged
        AppliquerFiltreEtAfficher()
    End Sub

    Private Sub RechercheEntree(sender As Object, e As EventArgs) Handles txtRecherche.GotFocus
        If txtRecherche.Text = TexteRecherche Then txtRecherche.Text = ""
        txtRecherche.ForeColor = BleuNuit
    End Sub

    Private Sub RechercheSortie(sender As Object, e As EventArgs) Handles txtRecherche.LostFocus
        If txtRecherche.Text.Trim() = "" Then
            txtRecherche.Text = TexteRecherche
            txtRecherche.ForeColor = Gris
        End If
    End Sub

    Private Sub QuitterClic(sender As Object, e As EventArgs) Handles btnQuitter.Click
        Close()
    End Sub

    Private Sub Fermeture(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If liste.Count > 0 Then
            Dim reponse As DialogResult = MessageBox.Show("Quitter l'application ?",
                                                          "GestionNotes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reponse <> DialogResult.Yes Then e.Cancel = True
        End If
    End Sub
    Private Sub Redimensionnement(sender As Object, e As EventArgs) Handles MyBase.Resize
        If conteneur Is Nothing Then Return
        Dim x As Integer = (ClientSize.Width - conteneur.Width) \ 2
        If x < 0 Then x = 0
        Dim placeHaute As Integer = 62
        Dim placeBasse As Integer = ClientSize.Height - 28
        Dim y As Integer = placeHaute + (placeBasse - placeHaute - conteneur.Height) \ 2
        If y < placeHaute Then y = placeHaute
        conteneur.Location = New Point(x, y)
    End Sub

End Class
