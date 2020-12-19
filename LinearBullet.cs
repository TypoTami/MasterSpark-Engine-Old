using System;
using System.Numerics;
using Raylib_cs;

namespace raylibTouhou
{
    class LinearBullet
    {
        private Vector2 Position;
        private Vector2 Velocity;
        private float AngularVelocity;
        private float Angle;
        private Color colour;

        public LinearBullet(Vector2 position, float angle, float velocity = 0.0f, float angularVelocity = 0.0f)
        {
            Position = position;
            Velocity = new Vector2(
                (float)Math.Cos(angle + (Math.PI/2)) * velocity,
                (float)Math.Sin(angle + (Math.PI/2)) * velocity
            );
            AngularVelocity = angularVelocity;

            Angle = angle;

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
            Position = Vector2.Add(Position, Vector2.Multiply(Velocity, Program.TimeScalar));
            if (AngularVelocity != 0f)
            {
                float AngularVelocityCorrected = AngularVelocity * Program.TimeScalar;
                Angle += AngularVelocityCorrected;
                Velocity.X = (float)(Velocity.X * Math.Cos(AngularVelocityCorrected) - Velocity.Y * Math.Sin(AngularVelocityCorrected));
                Velocity.Y = (float)(Velocity.X * Math.Sin(AngularVelocityCorrected) + Velocity.Y * Math.Cos(AngularVelocityCorrected));
            }
        }

        private void Draw()
        {
            Vector2 TexOrigin = new Vector2(
                Position.X - (Game.BulletTexture.width / 2f) * 0.25f,
                Position.Y - (Game.BulletTexture.height / 2f) * 0.25f
            );
            Raylib.DrawTextureEx(Game.BulletTexture, TexOrigin, Angle * (float)(180/Math.PI), 0.25f, Color.WHITE);

            // Raylib.DrawCircleV(Position, 5.0f, colour);
            // Raylib.DrawText($"{Position}", Convert.ToInt32(Position.X)+5, Convert.ToInt32(Position.Y)+5, 5, Color.BLACK);
            // Raylib.DrawText($"{Velocity.X}\n{Velocity.Y}", 500, 500, 5, Color.RED);
        }
    }
}
