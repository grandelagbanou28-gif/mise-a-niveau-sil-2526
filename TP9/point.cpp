#include <iostream>
#include <cmath>
#include "point.h"
using namespace std;

Point::Point()
{
    x = 0;
    y = 0;
}

Point::Point(double x, double y)
{
    this->x = x;
    this->y = y;
}

Point::Point(const Point &p)
{
    x = p.x;
    y = p.y;
}

void Point::deplacer(double dx, double dy)
{
    x = x + dx;
    y = y + dy;
}

void Point::affiche()
{
    cout << "( " << x << " ; " << y << " )" << endl;
}

void Point::saisir()
{
    cout << "x = ";
    cin >> x;
    cout << "y = ";
    cin >> y;
}

double Point::distance(Point p)
{
    double dx, dy;
    dx = x - p.x;
    dy = y - p.y;
    return sqrt(dx * dx + dy * dy);
}

Point Point::milieu(Point p)
{
    return Point((x + p.x) / 2, ( y + p.y) / 2);
}

double Point::getX() const
{
    return x;
}

double Point::getY() const
{
    return y;
}
