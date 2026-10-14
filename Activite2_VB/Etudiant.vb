Public Class Etudiant
    Public Property Nom As String = ""
    Public Property Prenom As String = ""
    Public Property NoteTC As Double = 0
    Public Property NoteSPE As Double = 0
    Public Property NoteEPP As Double = 0
    Public Property Rang As Integer = 0

    Public ReadOnly Property Moyenne As Double
        Get
            Return (NoteTC * 3 + NoteSPE * 4 + NoteEPP * 3) / 10
        End Get
    End Property

    Public ReadOnly Property Admis As Boolean
        Get
            Return Moyenne >= 10
        End Get
    End Property

    Public Function Decision() As String
        If Admis Then
            Return "admis(e)"
        Else
            Return "non admis(e)"
        End If
    End Function
End Class
