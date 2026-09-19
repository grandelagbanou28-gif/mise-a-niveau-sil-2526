#include <iostream>
using namespace std;

int main()
{
    int n, i, h, maxi, t[15];

    cout << "Nombre N d'entiers (entre 4 et 15) : ";
    cin >> n;
    while (cin.fail() || n < 4 || n > 15)
    {
        cin.clear();
        cin.ignore(1000, '\n');
        cout << "N incorrect, recommencez : ";
        cin >> n;
    }

    cout << "Saisissez " << n << " entiers entre 0 et 9 :" << endl;
    for (i = 0; i < n; i++)
    {
        cout << "T[" << i << "] = ";
        cin >> t[i];
        while (cin.fail() || t[i] < 0 || t[i] > 9)
        {
            cin.clear();
            cin.ignore(1000, '\n');
            cout << "Valeur incorrecte, recommencez : ";
            cin >> t[i];
        }
    }

    maxi = t[0];
    for (i = 1; i < n; i++)
        if (t[i] > maxi) maxi = t[i];

    for (h = maxi; h >= 1; h--)
    {
        for (i = 0; i < n; i++)
        {
            if (t[i] >= h) cout << "*";
            else cout << " ";
            cout << "   ";
        }
        cout << endl;
    }

    for (i = 0; i < n; i++)
    {
        if (i < 10) cout << i << "   ";
        else cout << i << "  ";
    }
    cout << endl;

    return 0;
}
