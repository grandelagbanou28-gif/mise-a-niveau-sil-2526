#include <iostream>
#include "point.h"
using namespace std;

int main()
{
    Point a, b, m;

    cout << "Saisie du point A :" << endl;
    a.saisir();
    cout << "Saisie du point B :" << endl;
    b.saisir();

    cout << "A = ";
    a.affiche();
    cout << "B = ";
    b.affiche();

    cout << "Distance AB = " << a.distance(b) << endl;

    m = a.milieu(b);
    cout << "Milieu de AB = ";
    m.affiche();

    cout << "Translation de A de (2 ; -3)" << endl;
    a.deplacer(2, -3);
    cout << "A apres deplacement = ";
    a.affiche();

    return 0;
}
