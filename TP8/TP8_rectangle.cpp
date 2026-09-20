#include <iostream>
using namespace std;

class Rectangle
{
private:
    double largeur, hauteur;

public:
    Rectangle(double l, double h)
    {
        largeur = l;
        hauteur = h;
    }

    double perimetre()
    {
        return 2 * (largeur + hauteur);
    }

    double surface()
    {
        return largeur * hauteur;
    }

    void afficher()
    {
        cout << "Largeur   = " << largeur << endl;
        cout << "Hauteur   = " << hauteur << endl;
        cout << "Perimetre = " << perimetre() << endl;
        cout << "Surface   = " << surface() << endl;
    }

    double getLargeur()
    {
        return largeur;
    }

    void setLargeur(double l)
    {
        largeur = l;
    }

    double getHauteur()
    {
        return hauteur;
    }

    void setHauteur(double h)
    {
        hauteur = h;
    }
};

int main()
{
    double l, h;
    Rectangle r1(5, 3);

    cout << "Rectangle construit avec (5 ; 3) :" << endl;
    r1.afficher();
    cout << endl;

    cout << "Nouvelle largeur : ";
    cin >> l;
    cout << "Nouvelle hauteur : ";
    cin >> h;

    r1.setLargeur(l);
    r1.setHauteur(h);

    cout << endl;
    cout << "Apres modification :" << endl;
    r1.afficher();
    cout << endl;
    cout << "Lecture avec getLargeur/getHauteur : " << r1.getLargeur()
         << " ; " << r1.getHauteur() << endl;

    return 0;
}
