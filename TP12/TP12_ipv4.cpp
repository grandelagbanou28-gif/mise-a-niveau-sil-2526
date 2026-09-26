#include <iostream>
using namespace std;

int main()
{
    int x, y, z, t, classe;
    bool valide;

    cout << "Saisie de l'adresse IPv4 (X.Y.Z.T)" << endl;

    do
    {
        valide = true;

        cout << "X : "; cin >> x;
        cout << "Y : "; cin >> y;
        cout << "Z : "; cin >> z;
        cout << "T : "; cin >> t;

        if (cin.fail())
        {
            cin.clear();
            cin.ignore(1000, '\n');
            cout << "Erreur : saisie incorrecte !" << endl;
            valide = false;
        }
        else if (x < 0 || x > 255 || y < 0 || y > 255 || z < 0 || z > 255 || t < 0 || t > 255)
        {
            cout << "Erreur : chaque octet doit etre compris entre 0 et 255 !" << endl;
            valide = false;
        }
        else if (x == 0)
        {
            cout << "Erreur : adresse inexistante (reseau 0.0.0.0) !" << endl;
            valide = false;
        }
        else if (x == 127)
        {
            cout << "Erreur : adresse de bouclage (loopback) inexploitable !" << endl;
            valide = false;
        }
        else if (x >= 224)
        {
            cout << "Erreur : adresse de classe D ou E inexploitable !" << endl;
            valide = false;
        }
        else
        {
            if (x <= 126) classe = 1;
            else if (x <= 191) classe = 2;
            else classe = 3;

            if (classe == 1 && y == 0 && z == 0 && t == 0)
            {
                cout << "Erreur : adresse de reseau inexploitable !" << endl;
                valide = false;
            }
            else if (classe == 1 && y == 255 && z == 255 && t == 255)
            {
                cout << "Erreur : adresse de diffusion inexploitable !" << endl;
                valide = false;
            }
            else if (classe == 2 && z == 0 && t == 0)
            {
                cout << "Erreur : adresse de reseau inexploitable !" << endl;
                valide = false;
            }
            else if (classe == 2 && z == 255 && t == 255)
            {
                cout << "Erreur : adresse de diffusion inexploitable !" << endl;
                valide = false;
            }
            else if (classe == 3 && t == 0)
            {
                cout << "Erreur : adresse de reseau inexploitable !" << endl;
                valide = false;
            }
            else if (classe == 3 && t == 255)
            {
                cout << "Erreur : adresse de diffusion inexploitable !" << endl;
                valide = false;
            }
        }
    } while (!valide);

    cout << endl;
    cout << "Adresse : " << x << "." << y << "." << z << "." << t << endl;

    if (classe == 1)
    {
        cout << "Classe : A" << endl;
        cout << "Masque de sous-reseau : 255.0.0.0" << endl;
        cout << "Adresse de diffusion : " << x << ".255.255.255" << endl;
        cout << "Plage d'hotes utilisables : " << x << ".0.0.1 a " << x << ".255.255.254" << endl;
    }
    else if (classe == 2)
    {
        cout << "Classe : B" << endl;
        cout << "Masque de sous-reseau : 255.255.0.0" << endl;
        cout << "Adresse de diffusion : " << x << "." << y << ".255.255" << endl;
        cout << "Plage d'hotes utilisables : " << x << "." << y << ".0.1 a "
             << x << "." << y << ".255.254" << endl;
    }
    else
    {
        cout << "Classe : C" << endl;
        cout << "Masque de sous-reseau : 255.255.255.0" << endl;
        cout << "Adresse de diffusion : " << x << "." << y << "." << z << ".255" << endl;
        cout << "Plage d'hotes utilisables : " << x << "." << y << "." << z << ".1 a "
             << x << "." << y << "." << z << ".254" << endl;
    }

    return 0;
}
