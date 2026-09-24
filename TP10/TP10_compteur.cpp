#include <iostream>
using namespace std;

class Compteur
{
private:
    int valeur;

public:
    Compteur()
    {
        valeur = 0;
    }

    void incrementer()
    {
        valeur = valeur + 1;
    }

    void decrementer()
    {
        if (valeur > 0)
            valeur = valeur - 1;
    }

    int getValeur()
    {
        return valeur;
    }
};

int main()
{
    int i;
    Compteur c;

    cout << c.getValeur() << " ";

    for (i = 0; i < 10; i++)
        c.incrementer();
    cout << c.getValeur() << " ";

    for (i = 0; i < 20; i++)
        c.decrementer();
    cout << c.getValeur() << endl;

    return 0;
}
