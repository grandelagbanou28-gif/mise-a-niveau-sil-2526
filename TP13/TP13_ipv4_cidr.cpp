#include <iostream>
using namespace std;

void afficherAdresse(unsigned int a)
{
    cout << ((a >> 24) & 255) << "." << ((a >> 16) & 255) << "."
         << ((a >> 8) & 255) << "." << (a & 255);
}

int main()
{
    int x, y, z, t, n, classe, bitsClasse;
    unsigned int ip, masque, reseau, diffusion, reseauPrincipal, dernReseau, dernDiff;
    long long nbSousReseaux, nbHotes, taille;
    bool valide;

    cout << "Saisie d'une adresse IPv4 avec masque en notation CIDR (X.Y.Z.T/N)" << endl;

    do
    {
        valide = true;

        cout << "X : "; cin >> x;
        cout << "Y : "; cin >> y;
        cout << "Z : "; cin >> z;
        cout << "T : "; cin >> t;
        cout << "N : "; cin >> n;

        if (cin.fail())
        {
            cin.clear();
            cin.ignore(1000, '\n');
            cout << "Erreur : saisie incorrecte !" << endl;
            continue;
        }

        if (x < 0 || x > 255 || y < 0 || y > 255 || z < 0 || z > 255 || t < 0 || t > 255)
        {
            cout << "Erreur : chaque octet doit etre compris entre 0 et 255 !" << endl;
            valide = false;
            continue;
        }

        if (x == 0)
        {
            cout << "Erreur : adresse inexistante (reseau 0.0.0.0) !" << endl;
            valide = false;
            continue;
        }

        if (x == 127)
        {
            cout << "Erreur : adresse de bouclage (loopback) inexploitable !" << endl;
            valide = false;
            continue;
        }

        if (x >= 224)
        {
            cout << "Erreur : adresse de classe D ou E inexploitable !" << endl;
            valide = false;
            continue;
        }

        if (x <= 126)
        {
            classe = 1;
            bitsClasse = 8;
        }
        else if (x <= 191)
        {
            classe = 2;
            bitsClasse = 16;
        }
        else
        {
            classe = 3;
            bitsClasse = 24;
        }

        if (n < bitsClasse)
        {
            cout << "Erreur : le masque /" << n
                 << " est plus court que le masque naturel de la classe !" << endl;
            valide = false;
            continue;
        }

        if (n > 30)
        {
            cout << "Erreur : un masque /" << n
                 << " ne laisse pas de place aux hotes !" << endl;
            valide = false;
            continue;
        }

        ip = ((unsigned int)x << 24) | ((unsigned int)y << 16)
           | ((unsigned int)z << 8) | (unsigned int)t;
        masque = 0xFFFFFFFFu << (32 - n);
        reseau = ip & masque;
        diffusion = reseau | ~masque;

        if (ip == reseau)
        {
            cout << "Erreur : c'est l'adresse du sous-reseau, inexploitable !" << endl;
            valide = false;
        }
        else if (ip == diffusion)
        {
            cout << "Erreur : c'est l'adresse de diffusion, inexploitable !" << endl;
            valide = false;
        }

    } while (!valide);

    ip = ((unsigned int)x << 24) | ((unsigned int)y << 16)
       | ((unsigned int)z << 8) | (unsigned int)t;
    masque = 0xFFFFFFFFu << (32 - n);
    reseau = ip & masque;
    diffusion = reseau | ~masque;
    reseauPrincipal = ip & (0xFFFFFFFFu << (32 - bitsClasse));

    taille = 1LL << (32 - n);
    nbHotes = taille - 2;
    nbSousReseaux = 1LL << (n - bitsClasse);

    cout << endl;
    cout << "Adresse : " << x << "." << y << "." << z << "." << t << "/" << n << endl;

    if (classe == 1) cout << "Classe : A" << endl;
    else if (classe == 2) cout << "Classe : B" << endl;
    else cout << "Classe : C" << endl;

    cout << "Masque de sous-reseau : ";
    afficherAdresse(masque);
    cout << endl;

    cout << "Nombre de sous-reseaux : " << nbSousReseaux << endl;
    cout << "Nombre d'hotes par sous-reseau : " << nbHotes << endl;

    cout << endl;
    cout << "Premier sous-reseau :" << endl;
    cout << "  Adresse du sous-reseau : ";
    afficherAdresse(reseauPrincipal);
    cout << endl;
    cout << "  Adresse de diffusion : ";
    afficherAdresse(reseauPrincipal | ~masque);
    cout << endl;
    cout << "  Plage d'hotes utilisables : ";
    afficherAdresse(reseauPrincipal + 1);
    cout << " a ";
    afficherAdresse((reseauPrincipal | ~masque) - 1);
    cout << endl;

    dernReseau = reseauPrincipal + (unsigned int)((nbSousReseaux - 1) * taille);
    dernDiff = dernReseau | ~masque;

    cout << endl;
    cout << "Dernier sous-reseau :" << endl;
    cout << "  Adresse du sous-reseau : ";
    afficherAdresse(dernReseau);
    cout << endl;
    cout << "  Adresse de diffusion : ";
    afficherAdresse(dernDiff);
    cout << endl;
    cout << "  Plage d'hotes utilisables : ";
    afficherAdresse(dernReseau + 1);
    cout << " a ";
    afficherAdresse(dernDiff - 1);
    cout << endl;

    return 0;
}
