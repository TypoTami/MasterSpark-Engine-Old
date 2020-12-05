using System;
using System.Numerics;
using Raylib_cs;

namespace raylibTouhou
{
    class LinearBullet
    {
        private Vector2 Position;
        private Vector2 Velocity;
        private Color colour;
        // private Texture2D Atlas;

        public LinearBullet(Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;

            int hex = Game.random.Next(0x000000f, 0x00f00ff);
            colour = Raylib.GetColor(hex);
        }

        public bool UpdateDraw()
        {
            Update();
            Draw();

            if ((Helpers.InBetweenFloat(Game.PlayAreaOrigin.X - 10, (Game.PlayAreaSize.X + Game.PlayAreaOrigin.X) + 10, Position.X)) &&
                (Helpers.InBetweenFloat(Game.PlayAreaOrigin.Y - 10, (Game.PlayAreaSize.Y + Game.PlayAreaOrigin.Y) + 10, Position.Y)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Update()
        {
            Position = Vector2.Add(Position, Velocity);
        }

        private void Draw()
        {
            Raylib.DrawCircleV(Position, 5.0f, colour);
            // Raylib.DrawText($"{Position}", Convert.ToInt32(Position.X)+5, Convert.ToInt32(Position.Y)+5, 5, Color.BLACK);
            // Raylib.DrawText($"{Velocity.X}\n{Velocity.Y}", 500, 500, 5, Color.RED);
        }
    }
}
