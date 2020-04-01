using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapeIntersection
{
    class Program
    {
        static void Main(string[] args)
        {
            var shapesToTest = TestCase();
            var intersections = FindIntersections(shapesToTest);

            Console.WriteLine(DictionaryToString(intersections));
            Console.Read();
        }
        public static Dictionary<int, List<int>> FindIntersections(List <Shape> _shapes){
            var aux = new  Dictionary<int, List<int>>();

            foreach(Shape shape in _shapes){
                var otherShapes = _shapes.Where((otherShape) => otherShape.ID != shape.ID).ToList();
                var intersectingShapes = otherShapes.Where(otherShape => CollisionDetector.AreIntersecting(shape,otherShape)).ToList();
                var listOfIDS = intersectingShapes.Select(otherShape => otherShape.ID).ToList();
                aux.Add(shape.ID,listOfIDS);
            }
            return aux;
        }


        //Auxiliary Console Logging
        public static string DictionaryToString(Dictionary<int, List<int>> _shapes){
            string s = string.Empty;
            foreach(var shape in _shapes){
                s += $"{shape.Key} -> (";
                foreach(var inter in shape.Value){
                    s += (shape.Value.First() == inter ? string.Empty : ",") + $" {inter} ";
                }
                s += ")" + (_shapes.Last().Key == shape.Key ? String.Empty : ",");
            }
            return s;
        }

        //Test Case 
        public static List<Shape> TestCase(){
            var aux = new List<Shape>(){
                new Rectangle(new Point(3,3),1,1),
                new Rectangle(new Point(2,2),3,3),
                new Circle(new Point(1,5),1),
                new Circle(new Point(3,8),1),
                new Circle(new Point(4,8),1),
                new Circle(new Point(8,7),1),
                new Rectangle(new Point(0,0),1,2),
                new Rectangle(new Point(0,0),2,1),
                new Circle(new Point(8,2),1),
                new Rectangle(new Point(6,0),4,4),
                new Circle(new Point(8,5),2),
            };
            return aux;
        }
    }

}
