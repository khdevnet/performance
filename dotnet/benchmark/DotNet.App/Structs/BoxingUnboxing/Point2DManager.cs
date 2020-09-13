using System;
using System.Collections.Generic;

namespace DotNet.App.Structs.BoxingUnboxing
{
    public class Point2DManager<TPoint2D>
    {
        private readonly List<TPoint2D> points;

        public Point2DManager(List<TPoint2D> points)
        {
            this.points = points;
        }

        public bool Contains(TPoint2D point2D)
        {
            return points.Contains(point2D);
        }

        public bool ContainsCustom<TP>(TP point2D)
        where TP : IEquatable<TPoint2D>
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (point2D.Equals(points[i])) return true;
            }
            return false;
        }
    }

    public struct Point2D
    {
        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
    }

    public struct Point2DCustomEquals : IEquatable<Point2DCustomEquals>
    {
        public Point2DCustomEquals(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public static bool operator ==(Point2DCustomEquals a, Point2DCustomEquals b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Point2DCustomEquals a, Point2DCustomEquals b)
        {
            return !(a == b);
        }

        public bool Equals(Point2DCustomEquals other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point2DCustomEquals)) return false;
            Point2DCustomEquals other = (Point2DCustomEquals)obj;
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }
    }

    public static class Ext
    {
        public static void D(this Point2DCustomEquals thиs)
        {
            var s = thиs.X;
        }
    }
}
