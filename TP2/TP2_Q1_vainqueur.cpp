#include <iostream>
using namespace std;

int main()
{
    int dosse, dossmt;
    double tc, mt;

    cout << "Dossard du concurrent : ";
    cin >> dosse;
    cout << "Temps : ";
    cin >> tc;

    dossmt = dosse;
    mt = tc;

    while (dosse != 0)
    {
        if (tc < mt)
        {
            mt = tc;
            dossmt = dosse;
        }
        cout << "Dossard du concurrent : ";
        cin >> dosse;
        cout << "Temps : ";
        cin >> tc;
    }

    cout << "Le vainqueur est : " << dossmt << " son temps est de : " << mt << endl;

    return 0;
}
