#include <iostream>
using namespace std;

int joursMois(int mois, int an)
{
    if (mois == 1 || mois == 3 || mois == 5 || mois == 7 ||
        mois == 8 || mois == 10 || mois == 12)
        return 31;
    else if (mois == 4 || mois == 6 || mois == 9 || mois == 11)
        return 30;
    else if (mois == 2)
    {
        if ((an % 400 == 0) || ((an % 4 == 0) && (an % 100 != 0)))
            return 29;
        else
            return 28;
    }
    else
        return 0;
}

int main()
{
    int an, mois, jour;
    bool valide;

    cout << "Donnez l'annee : ";
    cin >> an;
    cout << "Donnez le mois : ";
    cin >> mois;
    cout << "Donnez le jour : ";
    cin >> jour;

    valide = (mois >= 1 && mois <= 12) && (jour >= 1 && jour <= joursMois(mois, an));

    if (!valide)
    {
        cout << "La date " << jour << "/" << mois << "/" << an << " n'est pas valide" << endl;
        return 1;
    }

    cout << "La date est valide" << endl;

    jour = jour + 1;

    if (jour > joursMois(mois, an))
    {
        jour = 1;
        mois = mois + 1;
        if (mois > 12)
        {
            mois = 1;
            an = an + 1;
        }
    }

    cout << "Le lendemain : " << jour << "/" << mois << "/" << an << endl;

    return 0;
}
