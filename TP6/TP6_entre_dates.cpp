#include <iostream>
#include <string>
#include <cstdio>
using namespace std;

int bissextile(int a)
{
    return (a % 400 == 0) || ((a % 4 == 0) && (a % 100 != 0));
}

int joursMois(int m, int a)
{
    if (m < 1 || m > 12) return 0;
    if (m == 2) return bissextile(a) ? 29 : 28;
    if (m == 4 || m == 6 || m == 9 || m == 11) return 30;
    return 31;
}

int dateValide(int j, int m, int a)
{
    if (a < 1) return 0;
    if (j < 1 || j > joursMois(m, a)) return 0;
    return 1;
}

int lireDate(int &j, int &m, int &a)
{
    string ligne;
    int i, nb;

    getline(cin, ligne);
    for (i = 0; i < (int)ligne.size(); i++)
        if (ligne[i] == '/') ligne[i] = ' ';

    j = 0;
    m = 0;
    a = 0;
    nb = sscanf(ligne.c_str(), "%d %d %d", &j, &m, &a);

    return (nb == 3 && dateValide(j, m, a));
}

int nbJours(int j, int m, int a)
{
    int tot = 0, i;
    for (i = 1; i < a; i++)
        tot = tot + (bissextile(i) ? 366 : 365);
    for (i = 1; i < m; i++)
        tot = tot + joursMois(i, a);
    tot = tot + j;
    return tot;
}

int main()
{
    int j1, m1, a1, j2, m2, a2, d1, d2, diff;

    cout << "Nombre de jours separant deux dates" << endl;
    cout << "Date 1 (format jr/mois/annee) : ";
    while (!lireDate(j1, m1, a1))
        cout << "Date incorrecte, recommencez : ";

    cout << "Date 2 (format jr/mois/annee) : ";
    while (!lireDate(j2, m2, a2))
        cout << "Date incorrecte, recommencez : ";

    d1 = nbJours(j1, m1, a1);
    d2 = nbJours(j2, m2, a2);

    if (d1 > d2) diff = d1 - d2;
    else diff = d2 - d1;

    cout << "Nombre de jours separates par les deux dates : " << diff << endl;

    return 0;
}
