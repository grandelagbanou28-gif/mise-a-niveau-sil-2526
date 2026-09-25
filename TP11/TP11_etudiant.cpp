#include <iostream>
#include <string>
using namespace std;

class Etudiant
{
private:
    string nom, prenom;
    float tabnotes[10];

public:
    virtual void saisie()
    {
        int i;
        cout << "Nom : ";
        cin >> nom;
        cout << "Prenom : ";
        cin >> prenom;
        cout << "Les 10 notes :" << endl;
        for (i = 0; i < 10; i++)
        {
            cout << "Note " << i + 1 << " : ";
            cin >> tabnotes[i];
            while (cin.fail() || tabnotes[i] < 0 || tabnotes[i] > 20)
            {
                cin.clear();
                cin.ignore(1000, '\n');
                cout << "Note incorrecte (0 a 20), recommencez : ";
                cin >> tabnotes[i];
            }
        }
    }

    virtual void affichage()
    {
        int i;
        cout << "Nom : " << nom << endl;
        cout << "Prenom : " << prenom << endl;
        cout << "Notes : ";
        for (i = 0; i < 10; i++)
            cout << tabnotes[i] << " ";
        cout << endl;
        cout << "Moyenne : " << moyenne() << endl;
    }

    float moyenne()
    {
        int i;
        float s = 0;
        for (i = 0; i < 10; i++)
            s = s + tabnotes[i];
        return s / 10;
    }

    virtual int admis()
    {
        if (moyenne() >= 10) return 1;
        else return 0;
    }

    virtual int exaequo(Etudiant E)
    {
        if (moyenne() == E.moyenne()) return 1;
        else return 0;
    }
};

class Etudiant_en_Maitrise : public Etudiant
{
private:
    float note_memoire;

public:
    using Etudiant::exaequo;

    void saisie()
    {
        Etudiant::saisie();
        cout << "Note du memoire : ";
        cin >> note_memoire;
        while (cin.fail() || note_memoire < 0 || note_memoire > 20)
        {
            cin.clear();
            cin.ignore(1000, '\n');
            cout << "Note incorrecte (0 a 20), recommencez : ";
            cin >> note_memoire;
        }
    }

    void affichage()
    {
        Etudiant::affichage();
        cout << "Note du memoire : " << note_memoire << endl;
    }

    int admis()
    {
        if (moyenne() >= 10 && note_memoire >= 10) return 1;
        else return 0;
    }

    int exaequo(Etudiant_en_Maitrise E)
    {
        if (moyenne() == E.moyenne() && note_memoire == E.note_memoire) return 1;
        else return 0;
    }
};

int main()
{
    Etudiant e1, e2;
    Etudiant_en_Maitrise m1;

    cout << "=== Etudiant 1 ===" << endl;
    e1.saisie();
    e1.affichage();
    cout << "Admis : " << e1.admis() << endl << endl;

    cout << "=== Etudiant 2 ===" << endl;
    e2.saisie();
    e2.affichage();
    cout << "Admis : " << e2.admis() << endl << endl;

    cout << "Ex aequo entre etudiant 1 et etudiant 2 : " << e1.exaequo(e2) << endl << endl;

    cout << "=== Etudiant en maitrise ===" << endl;
    m1.saisie();
    m1.affichage();
    cout << "Admis : " << m1.admis() << endl;

    return 0;
}
