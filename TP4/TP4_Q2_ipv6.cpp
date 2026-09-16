#include <iostream>
using namespace std;

int main()
{
    unsigned int g[8];
    int i, classe;
    bool valide, sepOk;
    char sep = ' ';

    cout << "Saisie d'une adresse IPv6 (8 groupes hexadecimaux separes par : )" << endl;

    do
    {
        valide = true;
        sepOk = true;
        cin.clear();

        cout << "Adresse : ";
        for (i = 0; i < 8; i++)
        {
            cin >> hex >> g[i];
            if (i < 7)
            {
                cin >> sep;
                if (sep != ':') sepOk = false;
            }
        }

        if (cin.fail())
        {
            cin.clear();
            cin.ignore(1000, '\n');
            cout << "Erreur : saisie incorrecte !" << endl;
            valide = false;
        }
        else if (!sepOk)
        {
            cin.ignore(1000, '\n');
            cout << "Erreur : format attendu g0:g1:g2:g3:g4:g5:g6:g7 !" << endl;
            valide = false;
        }
        else
        {
            for (i = 0; i < 8; i++)
            {
                if (g[i] > 0xFFFF)
                {
                    cout << "Erreur : chaque groupe doit etre compris entre 0 et FFFF !" << endl;
                    valide = false;
                }
            }

            if (g[0] == 0 && g[1] == 0 && g[2] == 0 && g[3] == 0 &&
                g[4] == 0 && g[5] == 0 && g[6] == 0 && g[7] == 0)
            {
                cout << "Erreur : adresse inexistante (::) !" << endl;
                valide = false;
            }
            else if (g[0] == 0 && g[1] == 0 && g[2] == 0 && g[3] == 0 &&
                     g[4] == 0 && g[5] == 0 && g[6] == 0 && g[7] == 1)
            {
                cout << "Erreur : adresse de bouclage (::1) inexploitable !" << endl;
                valide = false;
            }
            else if ((g[0] & 0xFF00) == 0xFF00)
            {
                cout << "Erreur : adresse multijoin (multicast) inexploitable !" << endl;
                valide = false;
            }
            else if (g[4] == 0 && g[5] == 0 && g[6] == 0 && g[7] == 0)
            {
                cout << "Erreur : adresse de reseau, identifiant d'hote nul, inexploitable !" << endl;
                valide = false;
            }
            else if (g[4] == 0xFFFF && g[5] == 0xFFFF && g[6] == 0xFFFF && g[7] == 0xFFFF)
            {
                cout << "Erreur : identifiant d'hote a tous les bits a 1, inexploitable !" << endl;
                valide = false;
            }
        }
    } while (!valide);

    if ((g[0] & 0xFFC0) == 0xFE80) classe = 1;
    else if ((g[0] & 0xE000) == 0x2000) classe = 2;
    else classe = 3;

    cout << endl;
    cout << "Adresse IPv6 : ";
    for (i = 0; i < 8; i++)
    {
        cout << hex << g[i];
        if (i < 7) cout << ":";
    }
    cout << dec << endl;

    if (classe == 1)
        cout << "Classe : Adresse de lien local (fe80::/10)" << endl;
    else if (classe == 2)
        cout << "Classe : Adresse unicast globale (2000::/3)" << endl;
    else
        cout << "Classe : Adresse privee ou reservee" << endl;

    cout << "Masque de sous-reseau (prefixe) : /64" << endl;
    cout << "Adresse de diffusion : inexistante en IPv6 (remplacee par le multijoin)" << endl;

    cout << "Adresse de reseau : ";
    for (i = 0; i < 4; i++)
    {
        cout << hex << g[i];
        if (i < 3) cout << ":";
    }
    cout << "::" << dec << endl;

    cout << "Plage d'hotes utilisables : ";
    for (i = 0; i < 4; i++)
    {
        cout << hex << g[i];
        if (i < 3) cout << ":";
    }
    cout << ":0000:0000:0000:0001 a ";
    for (i = 0; i < 4; i++)
    {
        cout << hex << g[i];
        if (i < 3) cout << ":";
    }
    cout << ":ffff:ffff:ffff:fffe" << dec << endl;

    return 0;
}
