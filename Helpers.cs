using System;
using System.Numerics;
using Raylib_cs;

namespace raylibTouhou
{
    static class Helpers
    {
        static private Rectangle frameRect;
        static private Rectangle destRect;
        static private Vector2 origin = new Vector2(0, 0);

        public static void DrawFromAtlas(Texture2D atlas, Vector2 position, float scale, int totalFrames, bool flipped, int frame)
        {
            destRect.width = (atlas.width / 12) * scale;
            destRect.height = (atlas.height) * scale;
            destRect.x = position.X - destRect.width / 2;
            destRect.y = position.Y - destRect.height / 2; 

            frameRect.x = frame * (float)atlas.width / 12;
            frameRect.y = 0.0f;
            if (flipped == true) {
                frameRect.width = -(float)atlas.width / 12;
            } else {
                frameRect.width = +(float)atlas.width / 12;
            }
            frameRect.height = (float)atlas.height;

            Raylib.DrawTexturePro(atlas, frameRect, destRect, origin, 0.0f, Color.WHITE);
        }

        public static bool InBetweenFloat(float lower, float upper, float number)
        {
            return (lower <= number && number <= upper);
        }
        public static bool InBetweenInt(int lower, int upper, int number)
        {
            return (lower <= number && number <= upper);
        }

        public static Vector2 RotTransScale(Vector2 origin, Vector2 point, float rotation, float scale = 1.0f) {
            if (point.X == 0 && point.Y == 0) {
                return Vector2.Add (origin, point);
            }

            float length = Vector2.Distance(origin, Vector2.Add(origin, point)) * scale;

            float theta = (float)Math.Atan(point.Y/point.X);
            
            rotation += theta;

            float x = (float)Math.Cos(rotation) * length;
            float y = (float)Math.Sin(rotation) * length;

            point = Vector2.Add(origin, new Vector2(x, y));

            Raylib.DrawCircleV(origin, 5f, Color.GREEN);
            Raylib.DrawCircleLines(Convert.ToInt32(origin.X), Convert.ToInt32(origin.Y), length, Color.RED);

            Raylib.DrawCircleV(point, 5f, Color.DARKPURPLE);

            return point;
        }

        public static Vector2 Position(Vector2 from, Vector2 to, float position) {
            Vector2 point = Vector2.Multiply(Vector2.Subtract(to, from), position);
            return Vector2.Add(point, from);
        }
    }
}