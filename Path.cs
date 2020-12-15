using System;
using System.Linq;
using System.Numerics;
using System.Geometry;
using Raylib_cs;

namespace raylibTouhou
{
    class Path
    {
        private Vector2[] OriginalPoints;
        private Vector2[] Points;
        private string[] SegmentTypes;
        public Vector2 Origin;
        public float Rotation;
        public float Scale;

        public Path(Vector2[] originalPointsInit, string[] segmentTypesInit, Vector2 originInit, float rotationInit = 0.0f, float scaleInit = 1.0f)
        {
            OriginalPoints = originalPointsInit;

            for (int i = 0; OriginalPoints.Length > i; i++) {
                OriginalPoints[i] = Vector2.Subtract(OriginalPoints[i], OriginalPoints[0]);
            }
            
            Points = OriginalPoints.ToArray();
            SegmentTypes = segmentTypesInit;

            Origin = originInit;
            Rotation = rotationInit;
            Scale = scaleInit;

            Update();
        }

        public Vector2 Position() {
            return new Vector2(0,0);
        }

        public void Update() {
            for (int i = 0; OriginalPoints.Length > i; i++) {
                Points[i] = Helpers.RotTransScale(Origin, OriginalPoints[i], Rotation, Scale);
            }
        }

        public void Draw()
        {
            int j = 0;
            for (int i = 0; SegmentTypes.Length > i; i++)
            {
                Bezier curve;
                if (SegmentTypes[i] == "LINE")
                {
                    Raylib.DrawLineEx(
                        Points[j],
                        Points[j+1],
                        4.0f,
                        Color.PURPLE
                    );
                    j++;
                }
                else if (SegmentTypes[i] == "BEZIER")
                {
                    curve = new Bezier(
                        Points[j],
                        Points[j+1],
                        Points[j+2],
                        Points[j+3]
                    );

                    Raylib.DrawLineEx(
                        Points[j],
                        Points[j+1],
                        4.0f,
                        Color.BLUE
                    );
                    Raylib.DrawLineEx(
                        Points[j+1],
                        Points[j+2],
                        4.0f,
                        Color.RED
                    );
                    Raylib.DrawLineEx(
                        Points[j+2],
                        Points[j+3],
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