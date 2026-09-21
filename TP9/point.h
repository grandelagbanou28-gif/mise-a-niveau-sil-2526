#ifndef POINT_H
#define POINT_H

class Point
{
private:
    double x, y;

public:
    Point();
    Point(double x, double y);
    Point(const Point &p);

    void deplacer(double dx, double dy);
    void affiche();
    void saisir();
    double distance(Point p);
    Point milieu(Point p);

    double getX() const;
    double getY() const;
};

#endif
