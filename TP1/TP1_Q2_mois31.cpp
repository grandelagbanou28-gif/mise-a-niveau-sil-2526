#include <iostream>
using namespace std;

int main()
{
    int an, mois;
    bool bissextile, mois31;

    cout << "Donnez l'annee : ";
    cin >> an;

    bissextile = (an % 400 == 0) || ((an % 4 == 0) && (an % 100 != 0));

    if (bissextile)
        cout << "L'annee est bissextile" << endl;
    else
        cout << "L'annee n'est pas bissextile" << endl;

    cout << "Donnez le mois (1 a 12) : ";
    cin >> mois;

    mois31 = (mois == 1 || mois == 3 || mois == 5 || mois == 7 ||
              mois == 8 || mois == 10 || mois == 12);

    if (mois31)
        cout << "Ce mois contient 31 jours" << endl;
    else
        cout << "Ce mois ne contient pas 31 jours" << endl;

    return 0;
}
