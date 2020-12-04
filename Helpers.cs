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

        public static bool InBetween(float lower, float upper, float number)
        {
            return (lower <= number && number <= upper);
        }
    }
}