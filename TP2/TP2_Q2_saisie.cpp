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
    int dosse, dossmt;
    double tc, mt;

    dosse = lireDossard();
    tc = lireTemps();

    dossmt = dosse;
    mt = tc;

    while (dosse != 0)
    {
        if (tc < mt)
        {
            mt = tc;
            dossmt = dosse;
        }
        dosse = lireDossard();
        if (dosse != 0)
            tc = lireTemps();
    }

    cout << "Le vainqueur est : " << dossmt << " son temps est de : " << mt << endl;

    return 0;
}
