#include <iostream>
#include <cstdlib>
#include <ctime>
using namespace std;

int main()
{
    int cache[5], prop[5];
    int i, essais, maxEssais, bienPlaces;

    srand(time(0));

    for (i = 0; i < 5; i++)
        cache[i] = 1 + rand() % 9;

    maxEssais = 10;
    essais = 0;
    bienPlaces = 0;

    cout << "JEU DU MASTERMIND" << endl;
    cout << "Trouvez la combinaison de 5 numeros (1 a 9)" << endl;
    cout << "Vous avez " << maxEssais << " essais" << endl;
    cout << endl;

    while (essais < maxEssais && bienPlaces != 5)
    {
        cout << "Essai " << essais + 1 << " / " << maxEssais << " : ";
        for (i = 0; i < 5; i++)
        {
            cin >> prop[i];
            while (cin.fail() || prop[i] < 1 || prop[i] > 9)
            {
                cin.clear();
                cin.ignore(1000, '\n');
                cout << "Numero invalide (1 a 9), recommencez : ";
                cin >> prop[i];
            }
        }

        essais = essais + 1;

        bienPlaces = 0;
        for (i = 0; i < 5; i++)
            if (prop[i] == cache[i])
                bienPlaces = bienPlaces + 1;

        cout << bienPlaces << " numero(s) bien place(s)" << endl;
        cout << endl;
    }

    if (bienPlaces == 5)
        cout << "Bravo, vous avez gagne en " << essais << " essais !" << endl;
    else
    {
        cout << "Vous avez perdu ! La bonne combinaison etait : ";
        for (i = 0; i < 5; i++)
            cout << cache[i] << " ";
        cout << endl;
    }

    return 0;
}
