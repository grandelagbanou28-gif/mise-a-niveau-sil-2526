#include <iostream>
using namespace std;

int main()
{
    int an;
    bool bissextile;

    cout << "Donnez l'annee : ";
    cin >> an;

    bissextile = (an % 400 == 0) || ((an % 4 == 0) && (an % 100 != 0));

    if (bissextile)
        cout << "L'annee est bissextile" << endl;
    else
        cout << "L'annee n'est pas bissextile" << endl;

    return 0;
}
