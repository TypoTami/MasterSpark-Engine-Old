using System;
using System.Numerics;
using Raylib_cs;

namespace raylibTouhou
{
    class AdvancedBullet
    {
        private Vector2 Position;
        private Color colour;

        public AdvancedBullet(Vector2 position)
        {
            Position = position;

            int hex = Game.random.Next(0x300000f, 0x30f00ff);
            colour = Raylib.GetColor(hex);
        }

        private void Update()
        {
            // TODO
        }

        private void Draw()
        {
            Raylib.DrawCircleV(Position, 5.0f, colour);
        }
    }
}
