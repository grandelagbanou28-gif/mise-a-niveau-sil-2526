#include <iostream>
using namespace std;

int lireDossard()
{
    double x;
    cout << "Dossard (0 pour terminer) : ";
    cin >> x;
    while (cin.fail() || x < 0 || x != int(x))
    {
        cin.clear();
        cin.ignore(1000, '\n');
        cout << "Dossard incorrect, recommencez : ";
        cin >> x;
    }
    return int(x);
}

double lireTemps()
{
    double t;
    cout << "Temps : ";
    cin >> t;
    while (cin.fail() || t <= 0)
    {
        cin.clear();
        cin.ignore(1000, '\n');
        cout << "Temps incorrect, recommencez : ";
        cin >> t;
    }
    return t;
}

int main()
{
    int dosse, dossmt, doss2;
    double tc, mt, t2;

    dosse = lireDossard();
    tc = lireTemps();

    dossmt = dosse;
    mt = tc;
    doss2 = -1;
    t2 = 0;

    while (dosse != 0)
    {
        if (tc < mt)
        {
            t2 = mt;
            doss2 = dossmt;
            mt = tc;
            dossmt = dosse;
        }
        else if (doss2 == -1 || tc < t2)
        {
            t2 = tc;
            doss2 = dosse;
        }

        dosse = lireDossard();
        if (dosse != 0)
            tc = lireTemps();
    }

    cout << "Le vainqueur est : " << dossmt << " son temps est de : " << mt << endl;

    if (doss2 == -1)
        cout << "Il n'y a pas de deuxieme" << endl;
    else
        cout << "Le deuxieme est : " << doss2 << " son temps est de : " << t2 << endl;

    return 0;
}
