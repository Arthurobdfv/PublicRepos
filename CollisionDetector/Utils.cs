using System;
using System.Linq;
using System.Collections.Generic;

public class Point {
    public float x;
    public float y;

    public Point(float _x = 0 , float _y = 0)
    {
        x = _x;
        y = _y;
    }

    public static float Distance(Point a, Point b){
        var vector = a - b;
        return (float)Math.Sqrt(vector.x*vector.x + vector.y*vector.y);
    }

    public static Point operator + (Point a, Point b){
        return new Point(a.x + b.x, a.y + b.y);
    }
    public static Point operator - (Point a, Point b){
        return new Point(a.x - b.x, a.y-b.y);
    }
}

public class Line {
    public Point p1,p2;

    public Line(Point _p1, Point _p2){
        p1 = _p1;
        p2 = _p2;
    }

    public float DistanceFromLine(Point _p){
        float dx = p2.x - p1.x;
        float dy = p2.y - p1.y;

        float t = ((_p.x - p1.x) * dx + (_p.y - p1.y) * dy) /
        (dx * dx + dy * dy);

        Point closest = new Point();

        if (t < 0)
        {
            closest = new Point(p1.x, p1.y);
            dx = _p.x - p1.x;
            dy = _p.y - p1.y;
        }
        else if (t > 1)
        {
            closest = new Point(p2.x, p2.y);
            dx = _p.x - p2.x;
            dy = _p.y - p2.y;
        }
        else
        {
            closest = new Point(p1.x + t * dx, p1.y + t * dy);
            dx = _p.x - closest.x;
            dy = _p.y - closest.y;
        }
        return (float)Math.Sqrt(dx * dx + dy * dy);
    }

    public static bool LinesIntersect(Line l1, Line l2){
        var p1 = l1.p1;
        var p2 = l1.p2;
        var p3 = l2.p1;
        var p4 = l2.p2;

        float dx12 = l1.p2.x - l1.p1.x;
        float dy12 = l1.p2.y - l1.p1.y;
        float dx34 = l2.p2.x - l2.p1.x;
        float dy34 = l2.p2.y - l2.p1.y;

        float denominator = (dy12 * dx34 - dx12 * dy34);

        float t1 = ((p1.x - p3.x) * dy34 + (p3.y - p1.y) * dx34) / denominator;
        if (float.IsInfinity(t1))
        {
            return false;
        }

        float t2 = ((p3.x - p1.x) * dy12 + (p1.y - p3.y) * dx12) / -denominator;

        return ((t1 >= 0) && (t1 <= 1) && (t2 >= 0) && (t2 <= 1));
    }
}

public abstract class Shape{
    private static int id = 0;

    private static int GetNewID() => ++id;
    private int m_id;
    private Point m_pivot;

    public int ID {
        get{ return m_id; }
        private set{
            m_id = value;
        }
    }

    public Point Pivot {
        get{ return m_pivot; }
        set{
            m_pivot = value;
        }
    }

    public Shape(Point _p){
        this.ID = GetNewID();
        this.Pivot = _p;
    }
    public abstract bool Contains(Point _p);
}

public class Circle : Shape {
    private float m_radius;

    public float Radius{
        get { return m_radius; }
        set { 
            m_radius = value;
        }
    }

    public Circle(Point _center, float _radius) : base(_center){
        Radius = _radius;
    }

    public override bool Contains(Point _p)
    {
        float distance = Point.Distance(Pivot,_p);
        return distance < Radius;
    }
}

public class Rectangle : Shape {
    private float m_height;
    private float m_width;

    public float Height{
        get{ return m_height; }
        set{ m_height = value; }
    }

    public float Width{
        get{ return m_width; }
        set{ m_width = value; }
    }

    public Rectangle(Point _pivot, float _width, float _height) : base(_pivot){
        Height = _height;
        Width = _width;
    }

    public override bool Contains(Point _p)
    {
        bool insideX = _p.x <= Pivot.x ? false : _p.x >= (Pivot.x + Width) ? false : true;
        bool insideY = _p.y <= Pivot.y ? false : _p.y >= (Pivot.y + Height) ? false : true;
    
        return (insideX && insideY);
    }

    public Point[] GetPoints(){
        Point[] aux = new Point[4]{
            Pivot,
            new Point(Pivot.x + Width, Pivot.y),
            new Point(Pivot.x + Width, Pivot.y + Height),
            new Point(Pivot.x, Pivot.y + Height)
        };
        return aux;
    }

    public Line[] GetLines(){
        var points = GetPoints();
        var aux = new Line[4]{
            new Line(points[0],points[1]),
            new Line(points[1],points[2]),
            new Line(points[2],points[3]),
            new Line(points[3],points[0])
        };
        return aux;
    }

}
    public class CollisionDetector {

        private static bool RectangleIntersectCircle(Rectangle a, Circle b){
                var aLines = a.GetLines(); 
                foreach(var l in aLines){
                    if(l.DistanceFromLine(b.Pivot) < b.Radius) return true;
                }
                return false;
        }
        public static bool AreIntersecting(Shape a, Shape b){
            if(a.Contains(b.Pivot) || b.Contains(a.Pivot)) return true;
            if(a is Rectangle){
                var aRectangle = a as Rectangle;
                foreach(var vertex in aRectangle.GetPoints()){
                    if(b.Contains(vertex)) return true;
                }
                if(b is Circle){
                    return RectangleIntersectCircle(aRectangle,b as Circle);
                }
                if(b is Rectangle){
                    var bRectangle = b as Rectangle;
                    foreach(Line la in aRectangle.GetLines()){
                        foreach(Line lb in bRectangle.GetLines()){
                            if(Line.LinesIntersect(la,lb)) return true;
                        }
                    }
                }
            }
            if(a is Circle){
                Circle aCircle = a as Circle;
                if(b is Circle){
                    return Point.Distance(a.Pivot,b.Pivot) < aCircle.Radius + (b as Circle).Radius;
                }
                if(b is Rectangle){
                    return RectangleIntersectCircle((Rectangle)b,(Circle)a);
                } 
            }
            return false;
        }
    }
