using System;
using System.Numerics;
using Raylib_cs;

namespace raylibTouhou
{
    class Bullet
    {
        private Vector2 Position;
        private Vector2 Velocity;
        // private Texture2D Atlas;

        public Bullet(Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;

            // Atlas = Raylib.LoadTexture("assets/bullets.png");
        }

        public bool UpdateDraw()
        {
            Update();
            Draw();
            // if ((Helpers.InBetween(Game.PlayAreaOrigin.X , Game.PlayAreaSize.X , Position.X) &&
            //      Helpers.InBetween(Game.PlayAreaOrigin.Y , Game.PlayAreaSize.Y , Position.X)) |
            //      (Velocity.X == 0 & Velocity.Y == 0))
            // {
            //     return true;
            // }
            // else
            // {
            //     return false;
            // }

            if ((Helpers.InBetween(Game.PlayAreaOrigin.X - 10, (Game.PlayAreaSize.X + Game.PlayAreaOrigin.X) + 10, Position.X)) &&
                (Helpers.InBetween(Game.PlayAreaOrigin.Y - 10, (Game.PlayAreaSize.Y + Game.PlayAreaOrigin.Y) + 10, Position.Y)))
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
            Raylib.DrawCircleV(Position, 5.0f, Color.BLACK);
            Raylib.DrawText($"{Position}", Convert.ToInt32(Position.X)+5, Convert.ToInt32(Position.Y)+5, 5, Color.BLACK);
        }
    }
}
