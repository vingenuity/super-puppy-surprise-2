using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpaceHaste.Primitives
{
    public class LineManager
    {
        public static List<Point> Points;
        public LineManager()
        {
            Points = new List<Point>();
        }
        public static void AddLine(Point point)
        {
            Points.Add(point);
        }
        public static void Clear()
        {
            Points = new List<Point>();
        }
        public static void RemoveLine(Point point)
        {
            Points.Remove(point);
        }
    }
}