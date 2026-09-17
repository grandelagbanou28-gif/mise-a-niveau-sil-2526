#include <iostream>
#include <string>
using namespace std;

int main()
{
    int L, H, r, c, i;
    string p1 = "GRANDEL", p2 = "AGBANOU";

    cout << "Longueur L (entre 10 et 30) : ";
    cin >> L;
    while (cin.fail() || L < 10 || L > 30)
    {
        cin.clear();
        cin.ignore(1000, '\n');
        cout << "Longueur incorrecte, recommencez : ";
        cin >> L;
    }

    cout << "Hauteur H (entre 15 et 25) : ";
    cin >> H;
    while (cin.fail() || H < 15 || H > 25)
    {
        cin.clear();
        cin.ignore(1000, '\n');
        cout << "Hauteur incorrecte, recommencez : ";
        cin >> H;
    }

    for (c = 0; c < L; c++)
    {
        if (c < (int)p1.size()) cout << p1[c];
        else cout << "-";
    }
    cout << endl;

    for (r = 2; r < H; r++)
    {
        c = L - (r - 1) * (L - 1) / (H - 1);
        for (i = 1; i < c; i++) cout << " ";
        cout << "*" << endl;
    }

    for (c = 0; c < L; c++)
    {
        if (c < (int)p2.size()) cout << p2[c];
        else cout << "-";
    }
    cout << endl;

    return 0;
}
