using System;
using System.Numerics;
using System.Geometry;
using Raylib_cs;

namespace raylibTouhou
{
    class Path
    {
        private Vector2[] Points;
        private string[] SegmentTypes;

        public Path(Vector2[] points, string[] segmentTypes)
        {
            Points = points;
            SegmentTypes = segmentTypes;
        }
        public void Draw(Vector2 origin)
        {
            int j = 0;
            for (int i = 0; SegmentTypes.Length - 0 > i; i++)
            {
                Bezier curve;
                if (SegmentTypes[i] == "LINE")
                {
                    Raylib.DrawLineEx(
                        Vector2.Add(Points[j], origin),
                        Vector2.Add(Points[j+1], origin),
                        4.0f,
                        Color.PURPLE
                    );
                    j++;
                }
                else if (SegmentTypes[i] == "BEZIER")
                {
                    curve = new Bezier(
                        Vector2.Add(Points[j], origin),
                        Vector2.Add(Points[j+1], origin),
                        Vector2.Add(Points[j+2], origin),
                        Vector2.Add(Points[j+3], origin)
                    );

                    Raylib.DrawLineEx(
                        Vector2.Add(Points[j], origin),
                        Vector2.Add(Points[j+1], origin),
                        4.0f,
                        Color.BLUE
                    );
                    Raylib.DrawLineEx(
                        Vector2.Add(Points[j+1], origin),
                        Vector2.Add(Points[j+2], origin),
                        4.0f,
                        Color.RED
                    );
                    Raylib.DrawLineEx(
                        Vector2.Add(Points[j+2], origin),
                        Vector2.Add(Points[j+3], origin),
                        4.0f,
                        Color.BLUE
                    );

                    for (int k = 0; 20 > k; k++)
                    {
                        Raylib.DrawCircleV(curve.Position(k/20f), 2.5f, Color.DARKPURPLE);
                    }

                    j += 3;
                }
            }
        }
    }
}