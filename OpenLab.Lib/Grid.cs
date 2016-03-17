using System;
using System.Drawing;

namespace OpenLab.Lib
{
    public static class Grid
    {
        public static Point NearestNode(Point Point, Size Size)
        {
            var halfSize = new Size(Size.Width / 2, Size.Height / 2);
            return NearestNode(Point + halfSize) - halfSize;
        }

        public static Point NearestNode(Point Point)
        {
            return new Point(NearestNode(Point.X), NearestNode(Point.Y));
        }

        public static Size NearestNode(Size Size)
        {
            return new Size(NearestNode(Size.Width), NearestNode(Size.Height));
        }

        private static int NearestNode(int Number)
        {
            return (int)Math.Round(Number / 10.0) * 10;
        }
    }
}
