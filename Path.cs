using System;
using System.Linq;
using System.Collections.Generic;
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
        public float Scale; // Scale should not be changed after it is set?
        public List<float> SegmentLengths = new List<float>();

        public Path(Vector2[] originalPointsInit, string[] segmentTypesInit, Vector2 originInit, float rotationInit = 0.0f, float scaleInit = 1.0f)
        {
            OriginalPoints = originalPointsInit;

            for (int i = 0; OriginalPoints.Length > i; i++)
            {
                OriginalPoints[i] = Vector2.Subtract(OriginalPoints[i], OriginalPoints[0]);
            }

            Points = OriginalPoints.ToArray();
            SegmentTypes = segmentTypesInit;

            Origin = originInit;
            Rotation = rotationInit;
            Scale = scaleInit;

            for (int i = 0; SegmentTypes.Length > i; i++)
            {
                if (SegmentTypes[i] == "LINE")
                {
                    SegmentLengths.Add(GetLine(i).Length);
                    // Console.WriteLine($"{i}: LINE\t {SegmentTypes[i]}\t {GetLine(i).Length}");
                }
                else if (SegmentTypes[i] == "BEZIER")
                {
                    SegmentLengths.Add(GetBezier(i).Length);
                    // Console.WriteLine($"{i}: CURVE\t {SegmentTypes[i]}\t {GetBezier(i).Length}");
                }
            }

            Update();
        }

        public Vector2 Position(float t)
        {
            float totalLength = SegmentLengths.Sum();
            float sumOf = 0.0f;
            int segmentIndex = 0;
            for (int i = SegmentLengths.Count - 1; i >= 0; i--)
            {
                sumOf += SegmentLengths[i];
                // Console.WriteLine($"{i}: {SegmentLengths[i]}\t {totalLength}\t {sumOf}\t {totalLength - sumOf}");
                if (t > totalLength - sumOf)
                {
                    t -= totalLength - sumOf;
                    t /= SegmentLengths[i];
                    segmentIndex = i;
                    break;
                }
            }

            if (SegmentTypes[segmentIndex] == "LINE")
            {
                return GetLine(segmentIndex).Position(t);
            }
            else if (SegmentTypes[segmentIndex] == "BEZIER")
            {
                return GetBezier(segmentIndex).Position(t);
            }
            else
            {
                return new Vector2(0, 0);
            }

        }

        public void Update()
        {
            for (int i = 0; OriginalPoints.Length > i; i++)
            {
                Points[i] = Helpers.RotTransScale(Origin, OriginalPoints[i], Rotation, Scale);
            }
        }

        private Bezier GetBezier(int index)
        {
            int j = 0;
            for (int i = 0; index > i; i++)
            {
                if (SegmentTypes[i] == "BEZIER")
                {
                    j += 3;
                }
                else
                {
                    j++;
                }
            }

            return new Bezier(
                Points[j],
                Points[j + 1],
                Points[j + 2],
                Points[j + 3]
            );
        }

        private Line GetLine(int index)
        {
            int j = 0;
            for (int i = 0; index > i; i++)
            {
                if (SegmentTypes[i] == "LINE")
                {
                    j++;
                }
                else
                {
                    j += 3;
                }
            }

            return new Line(
                Points[j],
                Points[j + 1]
            );
        }

        public void Draw()
        {
            int j = 0;
            for (int i = 0; SegmentTypes.Length > i; i++)
            {
                if (SegmentTypes[i] == "LINE")
                {
                    Line line = GetLine(i);

                    Raylib.DrawLineEx(
                        line.P1,
                        line.P2,
                        4.0f,
                        Color.PURPLE
                    );
                    j++;
                }
                else if (SegmentTypes[i] == "BEZIER")
                {
                    Raylib.DrawLineEx(
                        Points[j],
                        Points[j + 1],
                        4.0f,
                        Color.BLUE
                    );
                    Raylib.DrawLineEx(
                        Points[j + 1],
                        Points[j + 2],
                        4.0f,
                        Color.RED
                    );
                    Raylib.DrawLineEx(
                        Points[j + 2],
                        Points[j + 3],
                        4.0f,
                        Color.BLUE
                    );

                    Bezier curve = GetBezier(i);

                    for (int k = 0; 20 > k; k++)
                    {
                        Raylib.DrawCircleV(curve.Position(k / 20f), 2.5f, Color.DARKPURPLE);
                    }

                    j += 3;
                }
            }
        }
    }
}